using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;


// Serializable : 인스펙터 노출을 위한 내부 작업들은 한다
[System.Serializable]
public class CollectablePool : RandomItem
{
    public Collectable collectable;

    public override Object GetItem()
    {
        return collectable;
    }
}

public class CollectableManager : MonoBehaviour
{
    [Space(20)]
    public List<CollectablePool> collectablepools;

    [Space(20)]
    [SerializeField] float spawnZpos = 60f;
    [SerializeField, AsRange(0, 100)] Vector2 spawnInterval;

    private TrackManager trackMgr;
    private RandomGenerator randomGenerator = new RandomGenerator();

    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙 관리자 없음");
            yield break;   // return과 동일 : 함수 완전 탈출
        } 

        // collectable Pools에 있는 모든 값을 랜덤생성기에 등록
        foreach(var pool in collectablepools)
            randomGenerator.AddItem(pool);

        // yield return new WaitForEndOfFrame();  //  지연 : 1프라임만 지연
        // yield return new WaitForSeconds(2f); // 지연 : 2초 지연
        yield return new WaitUntil( () => GameManager.IsPlaying == true);

        StartCoroutine(InfiniteSpawn());

    }

    //장애물 생성 (lane = 0, 1, 2)
    public void SpawnCollectable()
   {
        (int lane, Collectable prefab) = RandomLanePrefab();

        // Z 위치
        // 현재 해당 트랙의 자식으로 넣는다
        // 현재 해당 트랙 ?
        Track t = trackMgr.GetTrackByZ(spawnZpos);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }

        if(prefab != null)
        {
            Collectable o = Instantiate(prefab, t.CollectableRoot);
            o.SetLanePosition(lane, spawnZpos, trackMgr);
        }
   }

   IEnumerator InfiniteSpawn()
   {
        double lastMileage = 0;
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
   (int, Collectable) RandomLanePrefab()
   {
        // 랜덤1 : Lane을 랜덤 생성
        int rndLane = Random.Range(0, trackMgr.laneList.Count);
    

        Collectable prefab = randomGenerator.GetRandom().GetItem() as Collectable;

        if(prefab == null) 
            return (-1, null);

        return (rndLane, prefab);
   }
}
