using System;

public sealed class PlayerWallet : Component {
	[Property] PlayerHud Hud { get; set; }

	[Property] GameObject Entities { get; set; }
	[Property] GameObject MasterPack { get; set; }

	[Property] SoundEvent CollectSound { get; set; }

	[Property] SoundEvent Inhale { get; set; }

	[Property] float SalaryFrequency { get; set; }
	[Property] int SalaryAmount { get; set; }

	public int Money = 500;
	float SalaryTimer = 0f;

	protected override void OnFixedUpdate() {
		SalaryTimer += Time.Delta;

		if (SalaryTimer >= SalaryFrequency) {
			SalaryTimer -= SalaryFrequency;
			Money += SalaryAmount;
		}

		if (Input.Pressed("ToggleShop") || Input.Pressed("AltToggleShop")) {
			foreach (GameObject obj in Scene.GetAllObjects(false)) {
				if (obj.Tags.Has("shop")) {
					obj.Enabled = !obj.Enabled;
					break;
				}
			}
		}

		if (Input.Pressed("use") || Input.Pressed("attack1")) {
			bool collectedFromPrinter = false;
			bool smoked = false;

			foreach (GameObject obj in Scene.GetAllObjects(true)) {
				float distance = obj.Transform.Position.Distance(GameObject.Transform.Position);

				if (distance > 100f) { continue; }

				if (obj.Tags.Has("printer")) {
					PrinterScript printer = obj.Components.Get<PrinterScript>();

					if (printer != null) {
						Money += printer.Money;
						printer.Money = 0;
					}

					if (obj.Children.Count > 0) {
						GameObject screen = obj.Children.First();

						if (screen != null) {
							PrinterHud screenScript = screen.Components.Get<PrinterHud>();

							if (screenScript != null) {
								screenScript.Money = 0;
							}
						}
					}

					collectedFromPrinter = true;
				}

				if (obj.Tags.Has("weed_plant")) {
					WeedPlantScript script = obj.Components.Get<WeedPlantScript>();

					if (script != null && script.GrowStage == 4) {
						script.Harvest();

						if (Entities != null && MasterPack != null) {
							Vector3 targetPosition = obj.Transform.Position;
							targetPosition += Vector3.Up * 50f;
							Transform targetTransform = new( targetPosition );

							MasterPack.Clone(targetTransform, Entities);
						}
					}
				}

				if (obj.Tags.Has("pack")) {
					if (Hud != null) {
						Hud.Dose();
						smoked = true;
						obj.Destroy();
					}
				}
			}
		
			if (collectedFromPrinter && CollectSound != null) {
				Sound.Play( CollectSound );
			}

			if (smoked && Inhale != null) {
				Sound.Play( Inhale );
			}
		}

		if (Input.Pressed("attack2")) {
			// This should be done with an interface, but I can't be bothered
			foreach (GameObject obj in Scene.GetAllObjects(true)) {
				if (obj.Tags.Has( "printer" )) {
					PrinterScript printer = obj.Components.Get<PrinterScript>();

					if (printer != null) { printer.ResetRotation(); }

					foreach (GameObject child in obj.Children) {
						PrinterHud screen = child.Components.Get<PrinterHud>();

						if (screen != null) { screen.ResetRotation(); }
					}
				}

				if (obj.Tags.Has( "weed_plant" )) {
					WeedPlantScript script = obj.Components.Get<WeedPlantScript>();

					if (script != null) { script.ResetRotation(); }

					foreach (GameObject child in obj.Children) {
						LeafScript leaf = child.Components.Get<LeafScript>();

						if (leaf != null) { leaf.ResetRotation(); }
					}
				}
			}
		}
	}
}
