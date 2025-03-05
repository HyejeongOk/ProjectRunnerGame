using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator
{
    private List<Lane> lanePatterns = new List<Lane>();
    private RandomGenerator randomGenerator = new RandomGenerator();

    // 할당량 채우면 교체하라 ()
    private Vector2 limitQuota;
    private int _currentQuota;

    private int laneCount;
    [HideInInspector] public Lane currentPattern;

    // 생성자 (Construct) : 클래스 최초 호출
    public LaneGenerator(int lanecount, Vector2 quota, List<LanepatternPool> pools) 
    {
        laneCount = lanecount;
        limitQuota = quota;

        lanePatterns.Add(new LaneWaveStraight());
        lanePatterns.Add(new LaneWave());
        lanePatterns.Add(new LaneZigzag());

        
        foreach( LanepatternPool p in pools)
            randomGenerator.AddItem(p);

        SwitchPattern();

        // Factory
    }

 

    void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;
        // for (int i = 0; i<count; i++)
        // {
            // 시작 위치
            //Gizmos.color = Color.green;
            //Gizmos.DrawSphere(transform.position, 0.5f);

            // 끝 위치
            //Gizmos.color = Color.cyan;
            //Gizmos.DrawCube(targetPos, Vector3.one * 1f);

            //count 만큼 배치 =>반복문
            // i++, 5 
            // 0f : 시작, 0.2f, 0.4f, 0.6, 0.8 1f : 끝
            // i / count => 0f, 1/5=>0.2, 2/5=>0.4, 3/5=>0.6... 5/5=1
            // count = 1 -> 중심에 -> 0.5f

            // float t = (float)i/(count-1);
            // Vector3 v = Vector3.Lerp(transform.position, transform.position + transform.forward * offsetZ, t);
            // // 3.14 => 180도 , 2PI = 360도
            // // Sin => -1f ~ 1f => 음수를 양수로 전환
            // float s = Mathf.Abs(Mathf.Sin(t * Mathf.PI * frequency)) * amplitude;
            // v = new Vector3(v.x, v.y + s, v.z);
            // Gizmos.DrawCube(v, Vector3.one * 0.5f);
        // }
    }

    public LaneData GetNextLane()
    {  
        _currentQuota++;

        if(_currentQuota >= Random.Range((int)limitQuota.x, limitQuota.y))
            SwitchPattern();

        if (currentPattern == null)
            return new LaneData(-1);

        return currentPattern.GetNextLane();
    }

    public void SwitchPattern(int index = -1)
    {
        // string <-> enum 변환이 가능하다
        LaneType laneType = (LaneType) randomGenerator.GetRandom().GetItem() ;

        // -1 의미? 랜덤으로 발생
        // 0,1 의미? 0,1의 패턴을 정확하게 지명
        //var i = index == -1 ? Random.Range(0, lanePatterns.Count) : Mathf.Clamp(index, 0, lanePatterns.Count-1);

        // if( index == -1 )
        // {
        //     index = Random.Range(0, lanePatterns.Count);
       
        // }

        // else
        // {
        //     //1. 범위를 벗어나면, 나가라
        //     //2. 범위 안으로 제한하라 => Clamp

        //     index = Mathf.Clamp(index, 0, lanePatterns.Count-1);
        // }

            Lane lanePattern = lanePatterns.Find(f => f.laneType == laneType);
            currentPattern = lanePattern;
            currentPattern?.Initialize(laneCount);
           
            _currentQuota = 0;
    }
}
