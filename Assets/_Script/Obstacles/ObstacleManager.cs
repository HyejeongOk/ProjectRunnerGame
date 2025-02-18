using System.Collections;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] Obstacle obstaclePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnInterval = 1f;

    private TrackManager trackMgr;

    // Coroutine 방식 : Function, Method, Subroutine
    IEnumerator Start()
    {
        TrackManager[] tm = FindObjectsByType<TrackManager>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        if (tm == null || tm.Length <= 0)
        {
            Debug.LogError($"트랙 관리자 없음");
            yield break;   // return과 동일 : 함수 완전 탈출
        } 

        trackMgr = tm[0];

        // yield return new WaitForEndOfFrame();  //  지연 : 1프라임만 지연
        // yield return new WaitForSeconds(2f); // 지연 : 2초 지연
        yield return new WaitUntil( () => GameManager.IsPlaying == true);

        StartCoroutine(InfiniteSpawn());

        
        //SpawnObstacle();
    }

    //장애물 생성 (lane = 0, 1, 2)
    public void SpawnObstacle(int lane)
   {
        // Lane 위치
        lane = Mathf.Clamp(lane, 0, trackMgr.laneList.Count-1);
        Transform laneTransform = trackMgr.laneList[lane];
        Vector3 pos = new Vector3(laneTransform.position.x, laneTransform.position.y, spawnPoint.position.z);
        
        // Z 위치
        // 현재 해당 트랙의 자식으로 넣는다
        // 현재 해당 트랙 ?
        Track t = trackMgr.GetTrackByZ(spawnPoint.position.z);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }

        Obstacle o = Instantiate(obstaclePrefab, pos, Quaternion.identity, t.ObstacleRoot);
   }

   IEnumerator InfiniteSpawn()
   {
        while(true)
        {   yield return new WaitUntil( () => GameManager.IsPlaying);
        
            // if (GameManager.IsPlaying == false)
                // yield return null;
                // yield break;

            SpawnObstacle(Random.Range(0,trackMgr.laneList.Count));
            yield return new WaitForSeconds(spawnInterval);
        }
   } 
}
