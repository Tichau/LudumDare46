using Godot;
using System;

public class PlayerController : Node2D
{
	private int speed = 50;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Hello from C# to Godot :)");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		float y = 0f;
		float x = 0f;

		if (Input.IsKeyPressed((int)KeyList.Up))
		{
			y = -this.speed * delta;
		}
		if (Input.IsKeyPressed((int)KeyList.Down))
		{
			y = this.speed * delta;
		}
		if (Input.IsKeyPressed((int)KeyList.Left))
		{
			x = -this.speed * delta;
		}
		if (Input.IsKeyPressed((int)KeyList.Right))
		{
			x = this.speed * delta;
		}

		this.Translate(new Vector2(x, y));
	}
}
