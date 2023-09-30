using Godot;
using Godot.Collections;
using System;

public partial class Road : Node3D
{
	protected PackedScene roadSegmentScene;

	protected RoadModel roadModel;

	protected Dictionary<int, RoadSegment> segments = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		roadSegmentScene = GD.Load<PackedScene>("res://scenes/RoadSegment.tscn");

		roadModel = new RoadModel();
		roadModel.OnAddSegment += OnAddSegment;
		roadModel.OnRemoveSegment += OnRemoveSegment;
	}

	protected void OnAddSegment(RoadSegmentModel model)
	{
		segments[model.Id] = roadSegmentScene.Instantiate<RoadSegment>();

		AddChild(segments[model.Id]);

		GD.Print("add segment");
	}

	protected void OnRemoveSegment(RoadSegmentModel model)
	{
		segments[model.Id].QueueFree();

		segments.Remove(model.Id);

		GD.Print("remove segment");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		roadModel.Update(delta);

		foreach (var item in segments)
		{
			var model = roadModel.GetSegmentById(item.Key);

			item.Value.Update(model, roadModel.BarrierHeight);
		}
	}
}
