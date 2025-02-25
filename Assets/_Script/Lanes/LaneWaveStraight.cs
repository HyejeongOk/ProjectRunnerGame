public class LaneWaveStraight : Lane
{
    public string Name => "StraightPattern";
    // // 외부 노출용
    // public int MaxLane {get {return _maxLane ; } set {_maxLane = value;} }  
    // // 데이터 보관용
    // public int _maxLane; 
    
    private int currentLane;

    public void Initialize(int maxlane)
    {
        currentLane = UnityEngine.Random.Range(0,maxlane);
    }

    public int GetNextLane()
    {
        // count 만큼 나란히 배치
        return currentLane;
    }


    // public float offsetZ;
    // public int count;  // 현재 패턴에서 최대한 코인을 발생할 개수
}   
