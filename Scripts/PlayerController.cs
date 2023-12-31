using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	// Variables
	[Export]public float moveSpeed = 150.0f;
	[Export]public float jumpVelocity = 350.0f;

	// Obtener la gravedad del sistema 
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private AnimatedSprite2D _animatedController;
	private AudioStreamPlayer2D _audioJump;
	[Export] public Label life;
	private int _life = 1;
	[Export] public Label score;
	private int _score;

	// Metodos
	public override void _Ready()
	{
		_animatedController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_audioJump = GetNode<AudioStreamPlayer2D>("jump");
		score.Text = $"Score: {_score}";
		life.Text = $"Life: {_life}";
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
		if(Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = -jumpVelocity;
			_animatedController.Play("jump");
			_audioJump.Play();
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

		// Cuando toco un enemigo
		if(body.IsInGroup("Enemies"))
		{
			_life -= 1;
			life.Text = $"Life: {_life}";

			if(_life == 0)
			{
				GD.Print("Me mato un enemigo");
			}			
		}

		// Cuando toco una moneda
		if(body.IsInGroup("Coins"))
		{
			_score += 1;
			score.Text = $"Score: {_score}";
		}
	}


	// Funciona para destruir el personaje cuando cae
	private void Dead()
	{
		if(Position.Y >= 136)
		{
			// Accion cuando caigo por el borde
			GD.Print("Me mori !!!!");
		}
	}
}
