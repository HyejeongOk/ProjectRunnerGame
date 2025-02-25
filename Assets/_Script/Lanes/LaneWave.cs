
public class LaneWave : Lane
{
    public string Name => "WavePattern";

    // // 외부 노출용
    // public int MaxLane {get {return _maxLane ; } set {_maxLane = value;} }  
    // // 데이터 보관용
    // public int _maxLane; 
    // private int currentLane;
    
    public void Initialize(int maxlane)
    {
    }

    public int GetNextLane()
    {
        return -1;
    }

    public float amplitude;  // 진폭 (Amplitude)
    public float frequency;  // 주기 (Frequency)
    public float offsetZ;
    public int count;
}   
