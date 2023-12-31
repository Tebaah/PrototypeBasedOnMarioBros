using Godot;
using System;


public partial class EnemyController : CharacterBody2D
{
	// Variables
	[Export]public float speed = 100.0f; // Velocidad de movimiento
	[Export]public float _destiny; // Ubicacion en pixel debe ser simepre menor a la position
	
	private Vector2 _initialPosition;
	private bool _isMovingtoTheLeft = true;

	private AnimatedSprite2D _animationController;
	private AudioStreamPlayer2D _audioController;

	// Metodos
	public override void _Ready()
	{
		_initialPosition = Position;
		_animationController = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_audioController = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
	}

	
	public override void _PhysicsProcess(double delta)
	{
		// Asignamos la velocidad
		float move = speed * (float)delta;

		// Si esta en movimiento verifica
		if(_isMovingtoTheLeft)
		{
			MoveLocalX(-move);
			_animationController.Play("walk");
			_animationController.FlipH = false;

			// Verificacion de la posicion y cambia el movimento a la izquierda a false al completar
			if(Position.X <= _destiny)
			{
				Position = new Vector2(_destiny,Position.Y);
				_isMovingtoTheLeft = false;
			}
		}
		else
		{
			MoveLocalX(move);
			_animationController.FlipH = true;

			// Verificacion de la posicion y cambia el movimiento a la izquierda a true al completar
			if(Position.X >= _initialPosition.X)
			{
				Position = new Vector2(_initialPosition.X, Position.Y);
				_isMovingtoTheLeft = true;
			}
		}
	}

	private void OnEnemyBodyEntered(CharacterBody2D body)
	{
		if(body.Name == "Player")
		{
			// Activamos audio y destruimos el objeto
			_audioController.Play();
			Die(0.1f);
		}
	}

	// Funcion para destruir el objeto luego de un tiempo
	private async void Die(float delay)
	{
		await ToSignal(GetTree().CreateTimer(delay), "timeout");
		QueueFree();
	}
}
