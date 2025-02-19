using System.Collections.Generic;
using UnityEngine;

// 싱글의 파생형
// TYPE ( 2Blocks, Composited )
public class ObstacleTripleComposited : ObstacleTriple
{
    protected List<Vector3> spawnedPos  = new List<Vector3>();
    // override 기각, 무시
    // 부모에 있는 SetLanePosition은 무시,
    // 현재 나의 SetLanePosition을 사용
    // Enumerate : 나열하다
    
    public List<Obstacle> compositedPrefabs;

    private void Start()
    {
        SpawnComposited();
    }


    public void SpawnComposited()
    {
        // 2개의 싱글 장애물 스폰
        // spawnedPos (1번, 2번) 위치에 생성

        // spawnedPos -> 루프문 -> Instantiate -> 프리팹

        //compositedPrefabs
        foreach(var p in spawnedPos)
        {
            int rnd = Random.Range(0, compositedPrefabs.Count);
            Obstacle prefab = compositedPrefabs[rnd];

            
            var o = Instantiate(prefab, p, Quaternion.identity, transform);
            Vector3 localpos = o.transform.localPosition;
            o.transform.localPosition = new Vector3(localpos.x, 0f, 0f);
        };
    }

    public override void SetLanePosition(int lane, float zpos, TrackManager tm)
    {
        spawnedPos.Clear();
        // lane 0 => 0, 1의 중심
        // lane 1 => 1, 2의 중심 => 나중에

        lane = Mathf.Clamp(lane, 0, tm.laneList.Count-1);
        Vector3 lanepos0 = tm.laneList[0].position;
        Vector3 lanepos1 = tm.laneList[1].position;
        Vector3 lanepos2 = tm.laneList[2].position;

        spawnedPos.Add(lanepos0);
        spawnedPos.Add(lanepos1);
        spawnedPos.Add(lanepos2);

        // 위치와 회전 설정
        Vector3 pos = new Vector3(lanepos1.x, lanepos1.y, zpos);
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}
