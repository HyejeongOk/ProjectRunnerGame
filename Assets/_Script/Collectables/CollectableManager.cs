using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.Rendering;


// Serializable : 인스펙터 노출을 위한 내부 작업들은 한다
[System.Serializable]
public class CollectablePool : RandomItem
{
    public Collectable collectable;

    public override object GetItem()
    {
        return collectable;
    }

    // object > Object

}

[System.Serializable]
public class LanepatternPool : RandomItem
{
    public string patternName;
    public override object GetItem()
    {
        return patternName;
    }
} 

public class CollectableManager : MonoBehaviour
{
    [Space(20)]
    public List<CollectablePool> collectablepools;
    private RandomGenerator collectableGenerator = new RandomGenerator();

    public List<LanepatternPool> lanepatternPools;
    private LaneGenerator laneGenerator;

    [Space(20)]
    [SerializeField] float spawnZpos = 60f;
    [SerializeField, AsRange(0, 100)] Vector2 spawnInterval; // 개별 스폰 간격
    
    [SerializeField, AsRange(0, 30)] Vector2 spawnQuota; // 한 패턴에서 최대 찍을 개수

    private TrackManager trackMgr;

    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙 관리자 없음");
            yield break;   // return과 동일 : 함수 완전 탈출
        } 

        yield return new WaitForEndOfFrame();

        // 아이템들 프리팹과 랜덤비중 등록
        foreach(var pool in collectablepools)
            collectableGenerator.AddItem(pool);

        // 레인의 패턴과 랜덤비중 등록
       laneGenerator = new LaneGenerator(trackMgr.laneList.Count, spawnQuota, lanepatternPools);

        // yield return new WaitForEndOfFrame();  //  지연 : 1프라임만 지연
        // yield return new WaitForSeconds(2f); // 지연 : 2초 지연
        yield return new WaitUntil( () => GameManager.IsPlaying == true);

        StartCoroutine(InfiniteSpawn());

    }

    public void SpawnCollectable()
   {
        (LaneData lanedata, Collectable prefab) = RandomLanePrefab();

        // Z 위치
        // 현재 해당 트랙의 자식으로 넣는다
        // 현재 해당 트랙 ?
        Track t = trackMgr.GetTrackByZ(spawnZpos);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }

        if(prefab != null && lanedata.currentLane != -1)
        {
            Collectable o = Instantiate(prefab, t.CollectableRoot);
            o.SetLanePosition(lanedata.currentLane, lanedata.currentY, spawnZpos, trackMgr);
        }
   }

   IEnumerator InfiniteSpawn()
   {
        double lastMileage = 0f;
        while(true)
        {   
            yield return new WaitUntil( () => GameManager.IsPlaying);

            // 1m 거리 간격 이상일 때만 장애물을 생성한다.
            // 5m - 0m = 5 > 1m 성립 => lastMileage = 5m
            // 5.5m - 5m = 0.5m > 1m 패스
            // 6.2m - 5m = 1.2m > 1m 성립 => lastMileage = 6.2m
            
            if(GameManager.mileage - lastMileage > Random.Range(spawnInterval.x, spawnInterval.y))
            {
                SpawnCollectable();
                lastMileage = GameManager.mileage;
            }
        }
   } 

    // TEMPCODE
   (LaneData, Collectable) RandomLanePrefab()
   {
        // 랜덤1 : Lane을 랜덤 생성
        // int rndLane = Random.Range(0, trackMgr.laneList.Count);

        LaneData lane = laneGenerator.GetNextLane();
        Collectable prefab = collectableGenerator.GetRandom().GetItem() as Collectable;

        if(prefab == null) 
            return (lane, null);

        return (lane, prefab);
   }
}
