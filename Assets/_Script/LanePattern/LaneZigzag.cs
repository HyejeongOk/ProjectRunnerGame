using UnityEngine;

public class LaneZigzag : Lane
{
    public LaneType laneType => LaneType.ZIGZAG;

    private LaneData data;

    public void Initialize(int maxlane)
    {
        data.maxlane = maxlane;
    }

    private int elapsed;

    public LaneData GetNextLane()
    {
        data.currentLane = (int)Mathf.PingPong(elapsed++, data.maxlane -1 );
        return data;
    }

}
