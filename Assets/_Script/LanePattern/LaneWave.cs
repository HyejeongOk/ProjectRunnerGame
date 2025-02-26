
using UnityEngine;

public class LaneWave : Lane
{
    public string Name => "WavePattern";
    private LaneData data;

    public float amplitude = 2.5f;  // 진폭 (Amplitude)
    public float frequency = 1f;  // 주기 (Frequency)

    private float elapsed = 0f;
   
    // // 외부 노출용
    // public int MaxLane {get {return _maxLane ; } set {_maxLane = value;} }  
    // // 데이터 보관용
    // public int _maxLane; 
    // private int currentLane;
    
    public void Initialize(int maxlane)
    {
        data.maxlane = maxlane;

        System.Random random = new System.Random();
        data.currentLane = UnityEngine.Random.Range(0, maxlane);

        elapsed = 0f;
    }

    public LaneData GetNextLane()
    {
        data.currentY = Mathf.Abs(Mathf.Sin(elapsed * Mathf.PI * frequency)) * amplitude;
        elapsed += 0.1f;

        return data;
    }

}   
