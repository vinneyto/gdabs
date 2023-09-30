using Godot;
using System;

public partial class RoadSegment : Node3D
{
	private Node3D barrier;

	private bool barrierVisible = true;

	public bool BarrierVisible
	{
		get => barrierVisible;
		set
		{
			if (barrierVisible != value)
			{
				barrierVisible = value;

				if (barrierVisible)
				{
					barrier.Show();
				}
				else
				{
					barrier.Hide();
				}
			}
		}
	}

	public override void _Ready()
	{
		barrier = GetNode<Node3D>("Barrier");
	}

	public void Update(RoadSegmentModel model, double barrierHeight)
	{
		BarrierVisible = model.HasBarrier;
		Position = new Vector3(0, 0, (float)model.Position);

		barrier.Position = new Vector3(0, (float)barrierHeight - 1, 0);
	}
}
