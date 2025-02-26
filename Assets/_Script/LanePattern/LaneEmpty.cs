

public class LaneEmpty : Lane
{
    public string Name => "EmptyPattern";
      
    private LaneData data;

    public void Initialize(int maxlane)
    {
        data.maxlane = maxlane;
        
        data.currentLane = -1;  // 비어있다 의미 ( 0 대신 )
    }

    public LaneData GetNextLane()
    {
        // count 만큼 나란히 배치
        return data;
    }
}   
