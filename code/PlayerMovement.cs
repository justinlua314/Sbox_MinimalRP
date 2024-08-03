using System;

public sealed class PlayerMovement : Component {
	CharacterController controller;

	[Property] public Angles EyeAngles { get; set; }

	[Property] float WalkSpeed { get; set; } = 110f;
	[Property] float RunSpeed { get; set; } = 320f;

	Vector3 Gravity = new( 0, 0, 1300 );
	public Vector3 WishVelocity;

	private bool IsRunning;

	public bool Sitting = false;

	public bool MoveFrozen = false;
	public bool LookFrozen = false;

	protected override void OnStart() {
		controller = Components.Get<CharacterController>();
	}

	protected override void OnUpdate() {
		if ( !LookFrozen ) {
			var ee = EyeAngles;
			ee += Input.AnalogLook * 0.5f;
			ee.roll = 0;
			ee.pitch = Math.Max( Math.Min( ee.pitch, 90 ), -90 );

			EyeAngles = ee;

			Scene.Camera.Transform.Rotation = EyeAngles.ToRotation();

			var targetAngle = new Angles( 0, EyeAngles.yaw, 0 ).ToRotation();

			if (!Sitting) { Transform.Rotation = targetAngle; }
		}

		IsRunning = Input.Down( "Run" );
	}

	void BuildWishVelocity() {
		var rot = EyeAngles.ToRotation();

		WishVelocity = rot * Input.AnalogMove;
		WishVelocity = WishVelocity.WithZ( 0 );

		if (!WishVelocity.IsNearZeroLength) { WishVelocity = WishVelocity.Normal; }

		if (Input.Down("Run")) {
			WishVelocity *= RunSpeed;
		} else {
			WishVelocity *= WalkSpeed;
		}
	}

	protected override void OnFixedUpdate() {
		if ( MoveFrozen ) { return; }

		BuildWishVelocity();

		if (controller.IsOnGround && Input.Down("jump")) {
			controller.Punch( Vector3.Up * 450f );
		}

		if (controller.IsOnGround) {
			controller.Velocity = controller.Velocity.WithZ( 0 );
			controller.Accelerate(WishVelocity);
			controller.ApplyFriction( 8.0f );
		} else {
			controller.Velocity -= Gravity * Time.Delta * 0.5f;
			controller.Accelerate(WishVelocity.ClampLength(50));
			controller.ApplyFriction( 0.1f );
		}

		controller.Move();
	}
}
