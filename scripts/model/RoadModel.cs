using System;
using System.Collections.Generic;

public static class Ids
{
    private static int currentId = 0;
    public static int Get()
    {
        return currentId++;
    }
}

public class RoadSegmentModel
{
    public readonly int Id = Ids.Get();
    public bool HasBarrier { get; set; }
    public double Position { get; set; }
    public double NextPosition { get; set; }

    public RoadSegmentModel(bool hasBarrier = true, double position = 0, double nextPosition = 0)
    {
        HasBarrier = hasBarrier;
        this.Position = position;
        NextPosition = nextPosition;
    }

    public void Update(double delta, double velocity)
    {
        NextPosition = Position;
        var distDelta = delta * velocity;
        Position += distDelta;
        NextPosition = Position + distDelta;
    }
}

public class RoadModel
{
    public event Action<RoadSegmentModel> OnAddSegment;
    public event Action<RoadSegmentModel> OnRemoveSegment;
    public List<RoadSegmentModel> Segments = new List<RoadSegmentModel>();
    public double Velocity = 5;
    public double FrontDistance = -80;
    public double BackDistance = 20;
    public double SegmentSize = 10;
    public int SegmentCount = 0;
    public int BarrierFrequency = 2;
    public double BarrierHeight = 1.25;

    public void DeleteBackSegments()
    {
        var continueDeleting = Segments.Count > 0;

        while (continueDeleting)
        {
            var segment = Segments[0];

            if (segment.Position > BackDistance)
            {
                OnRemoveSegment?.Invoke(segment);
                Segments.RemoveAt(0);
            }
            else
            {
                continueDeleting = false;
            }
        }
    }

    public void AddFrontSegments()
    {
        var backDistance = BackDistance;

        if (Segments.Count > 0)
        {
            var segment = Segments[Segments.Count - 1];
            backDistance = segment.Position - SegmentSize;
        }

        for (var pos = backDistance; pos >= FrontDistance; pos -= SegmentSize)
        {
            SegmentCount++;

            var hasBarrier = SegmentCount % BarrierFrequency == 0;
            var segment = new RoadSegmentModel(hasBarrier, pos, pos);

            Segments.Add(segment);
            OnAddSegment?.Invoke(segment);
        }
    }

    public void UpdateSegments(double delta)
    {
        foreach (var segment in Segments)
        {
            segment.Update(delta, Velocity);
        }
    }

    public void Update(double delta)
    {
        DeleteBackSegments();
        AddFrontSegments();
        UpdateSegments(delta);
    }

    public RoadSegmentModel GetSegmentById(int id)
    {
        foreach (var segment in Segments)
        {
            if (segment.Id == id)
            {
                return segment;
            }
        }
        return null;
    }

    public RoadSegmentModel GetClosestBarrier(double to)
    {
        for (var i = Segments.Count - 1; i >= 0; i--)
        {
            var segment = Segments[i];
            if (segment.HasBarrier)
            {
                if (segment.Position > -(SegmentSize * BarrierFrequency) + to)
                {
                    return segment;
                }
            }
        }
        return null;
    }
}
