using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using DG.Tweening;

[System.Serializable]
public class ObstaclePool : RandomItem
{
    public List<Obstacle> obstacleList;

    public override object GetItem()
    {
        if(obstacleList == null || obstacleList.Count <= 0)
            return null;

        return obstacleList[Random.Range(0, obstacleList.Count)];
    }
}

public class ObstacleManager : MonoBehaviour
{
    [Space(20)]
    //public List<ObstaclePool> obstaclePools;

    // [Space(20)]
    // [SerializeField] List<Obstacle> obstacleSingle;
    // [SerializeField] List<Obstacle> obstacleDouble;
    // [SerializeField] List<Obstacle> obstacleTriple;
    [SerializeField] float spawnZpos = 60f;
    [SerializeField, ReadOnly] Vector2 spawnInterval;
    [SerializeField, ReadOnly] ObstacleSO data;

    private TrackManager trackMgr;
    private RandomGenerator randomGenerator = new RandomGenerator();

    // Coroutine 방식 : Function, Method, Subroutine
    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙 관리자 없음");
            yield break;   // return과 동일 : 함수 완전 탈출
        } 

        

        // yield return new WaitForEndOfFrame();  //  지연 : 1프라임만 지연
        // yield return new WaitForSeconds(2f); // 지연 : 2초 지연
        yield return new WaitUntil( () => GameManager.IsPlaying == true);

        StartCoroutine(InfiniteSpawn());

        
        //SpawnObstacle();
    }

    //장애물 생성 (lane = 0, 1, 2)
    public void SpawnObstacle()
   {
        if( data == null)
            return;

        (int lane, Obstacle prefab) = RandomLanePrefab();

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
            Obstacle o = Instantiate(prefab, t.ObstacleRoot);
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
                SpawnObstacle();
                lastMileage = GameManager.mileage;
            }
            
            // // if (GameManager.IsPlaying == false)
            //     // yield return null;
            //     // yield break;

            // 시간 기반
            // yield return new WaitForSeconds(spawnInterval);
        }
   } 

    // TEMPCODE
   (int, Obstacle) RandomLanePrefab()
   {
        // 랜덤1 : Lane을 랜덤 생성
        int rndLane = Random.Range(0, trackMgr.laneList.Count);
        // // 랜덤2 : Prefab 타입 랜덤 생성
        // int rndType = Random.Range((int)ObstacleType.Single, (int)ObstacleType._MAX_);
        
        // List<Obstacle> obstacles = rndType switch { 
        //     (int)ObstacleType.Single => obstacleSingle,
        //     (int)ObstacleType.Double => obstacleDouble,
        //     (int)ObstacleType.Triple => obstacleTriple,
        //     _ => null
        //     };
        // if (obstacles.Count <= 0 )
        //     return (-1, null);

        // Obstacle prefab = obstacles[Random.Range(0, obstacles.Count)];

        // GetRandom() => ObstaclePool 가져온다
        // GetItem() => Pool 안에서 RandomItem 가져온다
        // as Obstacle => RandomItem을 Obstacle로 형변환
        Obstacle prefab = randomGenerator.GetRandom().GetItem() as Obstacle;

        if(prefab == null) 
            return (-1, null);

        return (rndLane, prefab);
   }

   public void SetPhase(PhaseSO phase, float duration = 1f)
   {
        if(phase.obstacleData == null)
        {
            ClearObstacles();
            return; 
        }

        data = phase.obstacleData;

        // 랜덤 초기화
        randomGenerator.Clear();

        // Obstacles Pools에 있는 모든 값을 랜덤생성기에 등록
        foreach(var pool in data.pools)
            randomGenerator.AddItem(pool);

        // 장애물 interval적용 
        DOVirtual.Vector2(spawnInterval, data.interval, duration, i => spawnInterval = i).SetEase(Ease.InOutSine);
   }

   public void ClearObstacles()
   {
        randomGenerator.Clear();
        data = null;
   }
}
