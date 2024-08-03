public sealed class LeafScript : Component {
	Rotation DefaultRotation;

	public void ResetRotation() { GameObject.Transform.Rotation = DefaultRotation; }

	protected override void OnStart() {
		DefaultRotation = GameObject.Transform.Rotation;
	}
}
