using Godot;
using System;

public partial class CoinController : CharacterBody2D
{

	private AudioStreamPlayer2D _audioController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioController = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnCoinBodyEntered(CharacterBody2D body)
	{	
		if(body.Name == "Player")
		{	
			_audioController.Play();
	
			//QueueFree();
		}

	}
}
