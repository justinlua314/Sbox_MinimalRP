public sealed class SeedScript : Component, Component.ITriggerListener {
	public void OnTriggerEnter(Collider other) {
		if (other.Tags.Has( "weed_plant" )) {
			WeedPlantScript script = other.GameObject.Components.Get<WeedPlantScript>();

			if (script != null) {
				if (script.Plant()) {
					GameObject.Destroy();
				}
			}
		}
	}
}
