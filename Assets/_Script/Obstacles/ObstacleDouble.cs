using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// 싱글의 파생형
// TYPE ( 2Blocks, Composited )
public class ObstacleDouble : Obstacle
{
    // public : 전체 공개
    // private : 비공개 (본인만 공개)
    // protected : 본인과 자식에게만 공개

    protected List<Vector3> spawnedPos  = new List<Vector3>();

    // override 기각, 무시
    // 부모에 있는 SetLanePosition은 무시,
    // 현재 나의 SetLanePosition을 사용
    // Enumerate : 나열하다

    public override void SetLanePosition(int lane, float zpos, TrackManager tm)
    {
        spawnedPos.Clear();
        // lane 0 => 0, 1의 중심
        // lane 1 => 1, 2의 중심 => 나중에

        lane = Mathf.Clamp(lane, 0, tm.laneList.Count-1);
        Vector3 lanepos0 = tm.laneList[0].position;
        Vector3 lanepos1 = tm.laneList[1].position;
        Vector3 lanepos2 = tm.laneList[2].position;

        float posX = 0f;

        // 내부에서 자체 랜덤으로 Lane 결정
        int rndLane = Random.Range(0, tm.laneList.Count-1);
        if(rndLane == 0)
        {
            posX = (lanepos0.x + lanepos1.x) / 2;
            spawnedPos.Add(lanepos0);
            spawnedPos.Add(lanepos1);
        }

        else if(rndLane == 1)
        {
            posX = (lanepos1.x + lanepos2.x) / 2;
            spawnedPos.Add(lanepos1);
            spawnedPos.Add(lanepos2);
        }

        // 위치와 회전 설정
        Vector3 pos = new Vector3(posX, tm.laneList[lane].position.y, zpos);
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}
