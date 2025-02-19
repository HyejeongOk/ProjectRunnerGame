using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// 싱글의 파생형
// TYPE ( 2Blocks, Composited )
public class ObstacleTriple : Obstacle
{
    

    // override 기각, 무시
    // 부모에 있는 SetLanePosition은 무시,
    // 현재 나의 SetLanePosition을 사용
    // Enumerate : 나열하다

    public override void SetLanePosition(int lane, float zpos, TrackManager tm)
    {
        // lane 0 => 0, 1의 중심
        // lane 1 => 1, 2의 중심 => 나중에
        Vector3 lanepos1 = tm.laneList[1].position;
        

        // 위치와 회전 설정
        Vector3 pos = new Vector3(lanepos1.x, tm.laneList[lane].position.y, zpos);
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}
