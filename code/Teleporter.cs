public sealed class Teleporter : Component, Component.ITriggerListener {
	[Property] Vector3 TeleportLocation { get; set; }

	public void OnTriggerEnter( Collider other ) {
		Log.Info( "Called" );

		if (other.Tags.Has( "player" )) {
			other.Transform.Position = TeleportLocation;
			other.Transform.ClearInterpolation();
		}
	}
}
