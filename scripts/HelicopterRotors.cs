using Godot;
using System;

public partial class HelicopterRotors : Node3D
{
	private Node3D mainRotor;
	private Node3D tailRotor;

	private float rotationVelocity = 15f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainRotor = GetNode<Node3D>("MainRotor_2");
		tailRotor = GetNode<Node3D>("TailRotor_1");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		mainRotor.RotateY(-(float)delta * rotationVelocity);
		tailRotor.RotateX(-(float)delta * rotationVelocity);
	}
}
