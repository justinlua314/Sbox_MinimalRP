public sealed class WeedPlantScript : Component {
	[Property] List<Model> StageModels { get; set; }

	[Property] float GrowTime { get; set; } = 1f;

	ModelRenderer ModelRender;
	Rotation DefaultRotation;

	public int GrowStage = 0;
	float GrowTimer = 0f;
	bool Growing = false;

	void NextStage() {
		GrowStage++;

		if (GrowStage == StageModels.Count) {
			GrowStage--;
		}

		if (GrowStage == StageModels.Count - 1) {
			foreach (GameObject leaf in GameObject.Children) {
				leaf.Enabled = true;
			}
		}

		if (ModelRender != null) {
			ModelRender.Model = StageModels[GrowStage];
		}
	}

	public bool Plant() {
		if (Growing) { return false; }

		Growing = true;
		return true;
	}

	public void Harvest() {
		if (GrowStage != StageModels.Count - 1) { return; }

		foreach (GameObject leaf in GameObject.Children) {
			leaf.Enabled = false;
		}

		if (ModelRender != null) {
			ModelRender.Model = StageModels[0];
		}

		GrowStage = 0;
		Growing = false;
	}

	public void ResetRotation() { GameObject.Transform.Rotation = DefaultRotation; }

	protected override void OnStart() {
		ModelRender = GameObject.Components.Get<ModelRenderer>();
		DefaultRotation = GameObject.Transform.Rotation;
		GrowTimer = GrowTime;
	}

	protected override void OnFixedUpdate() {
		if (!Growing || GrowStage == 4) { return; }

		GrowTimer -= Time.Delta;

		if (GrowTimer <= 0f) {
			GrowTimer += GrowTime;
			NextStage();
		}
	}
}
