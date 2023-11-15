using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	[Export]public float moveSpeed = 150.0f;
	[Export]public float jumpVelocity = 350.0f;

	// Obtener la gravedad del sistema 
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private AnimatedSprite2D _animatedController;

	public override void _Ready()
	{
		_animatedController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Gravedad personaje
		if(!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		// Salto personaje
		if(Input.IsActionPressed("jump") && IsOnFloor())
		{
			velocity.Y = -jumpVelocity;
			_animatedController.Play("jump");
		}

		// Movimiento personaje izquierda y derecha
		velocity.X = 0;
		if(Input.IsActionPressed("left"))
		{
			velocity.X = -moveSpeed;
		}
		else if(Input.IsActionPressed("right"))
		{
			velocity.X = moveSpeed;
		}

		UpdateSpriteRender(velocity.X);
		UpdateSpriteRenderJump(velocity.Y);

		Velocity = velocity;
		MoveAndSlide();
	}

	private void UpdateSpriteRender(float velocityX)
	{
		bool walking = velocityX != 0;

		if(walking)
		{
			_animatedController.Play("walk");
			_animatedController.FlipH = velocityX < 0;
		}
		else
		{
			_animatedController.Play("stand");
		}
	}
		private void UpdateSpriteRenderJump(float velocityY)
	{
		bool jumping = velocityY != 0;

		if(jumping)
		{
			_animatedController.Play("jump");			
		}

	}

	private void OnPlayerBodyEntered(CharacterBody2D body)
	{
		if(body.Name == "Enemy")
		{
			GD.Print("Toque un enemigo");
		}
	}
}
