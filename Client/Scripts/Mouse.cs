using Godot;
using System;
using Color = System.Drawing.Color;

public class Mouse : Control
{
	public bool drawing = false;
	public Line2D line;
	private Vector2 start;

	public override void _Ready()
	{
		Console.WriteLine("Ready");
	}

	public void OnAttackDeclared()
	{
		drawing = true;
		start = GetGlobalMousePosition();
		line = new Line2D();
	}

	public void OnAttackCancelled()
	{
		drawing = false;
		line.ClearPoints();
	}

	public override void _Process(float delta)
	{
		Update();
	}

	public override void _Draw()
	{
		if (!drawing) return;
		Vector2 end = GetGlobalMousePosition();
		DrawLine(start, end, new Godot.Color(255, 0, 0), 10, true);
		Console.WriteLine($"Point {start} to Point {end}");
	}
}
