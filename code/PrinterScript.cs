public sealed class PrinterScript : Component {
	public int Money = 0;
	public int Speed = 0;

	float PrintTimer = 0;

	Rotation DefaultRotation;

	public void ResetPrinter() {
		Money = 0;
	}

	public void SetSpeed(int speed) {
		Speed = speed;
		PrintTimer = speed;
	}

	public void SetColor(Color color) {
		ModelRenderer model = GameObject.Components.Get<ModelRenderer>();

		if (model != null) {
			model.Tint = color;
		}
	}

	public void ResetRotation() { GameObject.Transform.Rotation = DefaultRotation; }

	protected override void OnStart() {
		PrintTimer = Speed;
		DefaultRotation = GameObject.Transform.Rotation;
	}

	protected override void OnFixedUpdate() {
		PrintTimer -= Time.Delta;

		if (PrintTimer <= 0) {
			PrintTimer += Speed;
			Money += 10;

			foreach (GameObject child in GameObject.Children) {
				PrinterHud screen = child.Components.Get<PrinterHud>();

				if (screen != null) {
					screen.Money = Money;
					break;
				}
			}
		}
	}
}
