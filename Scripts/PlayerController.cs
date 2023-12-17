using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	[Export]public float moveSpeed = 150.0f;
	[Export]public float jumpVelocity = 350.0f;

	// Obtener la gravedad del sistema 
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private AnimatedSprite2D _animatedController;
	private AudioStreamPlayer2D _audioController;

	public override void _Ready()
	{
		_animatedController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_audioController = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
	}

    public override void _Process(double delta)
    {
        Dead();
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
			_audioController.Play();
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

		UpdateSpriteRender(velocity.X, velocity.Y);

		Velocity = velocity;
		MoveAndSlide();
	}

	private void UpdateSpriteRender(float velocityX, float velocityY)
	{
		bool walking = velocityX != 0;
		bool jumping = velocityY != 0;

		if(walking)
		{
			_animatedController.Play("walk");
			_animatedController.FlipH = velocityX < 0;
		}
		else
		{
			_animatedController.Play("stand");
		}

		if(jumping)
		{
			_animatedController.Play("jump");			
		}

	}

	private void OnPlayerBodyEntered(CharacterBody2D body)
	{
		if(body.IsInGroup("Enemies"))
		{
			// Accion cuando un enemigo me toca
			GD.Print("Me mato un enemigo");
		}
		if(body.IsInGroup("Coins"))
		{
			GD.Print("+1 moneda");
		}
	}

	private void Dead()
	{
		if(Position.Y >= 136)
		{
			// Accion cuando caigo por el borde
			GD.Print("Me mori !!!!");
		}
	}
}
