using Godot;
using System;
using System.Threading;

public partial class CoinController : CharacterBody2D
{
	// Variables
	private AudioStreamPlayer2D _audioController;

	// Metodos
	public override void _Ready()
	{
		_audioController = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer");

	}

	
	public override void _Process(double delta)
	{
	}

	private void OnCoinBodyEntered(CharacterBody2D body)
	{			
		if (body.Name == "Player")
		{	
			// Activamos audio y destruimos objeto - collision characterbody debe estar disable
			_audioController.Play();
			Visible = false;
			Die(0.25f);
		}

	}
	
	// Funcion para destruir el objeto luego de un tiempo
	private async void Die(float delay)
	{
		await ToSignal(GetTree().CreateTimer(delay), "timeout");
		QueueFree();
	}
}
