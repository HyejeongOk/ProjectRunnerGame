using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TrackManager : MonoBehaviour
{
    [Space(20)]
    [SerializeField] Track trackPrefabs;
    [SerializeField] PlayerControl playerPrefab;

    [Space(20)]
    [Range(0f, 50f)] public float scrollspeed = 10f;
    [Range(1, 100)] public int trackCount = 3;
    // public float trackThreshold = 10f; //트랙 삭제 z축

    [Space(20)]
    [SerializeField] Material CurvedMaterial;
    // public Vector2 CurvedValue;

    // 주기, 진폭
    [Range(0f, 0.5f), SerializeField] public float CurvedFrequencyX;  // 주기
    [Range(0f, 10f), SerializeField] public float CurvedAmplitudeX; //진폭

    [Range(0f, 0.5f), SerializeField] public float CurvedFrequencyY;  // 주기
    [Range(0f, 10f), SerializeField] public float CurvedAmplitudeY; //진폭


    private List<Track> trackList = new List<Track>();  // 생성한 트랙들 보관
    private Transform camTransform;

    // 상태 정보
    [HideInInspector] public List<Transform> laneList;  // 현재 트랙의 라인 정보를 전달


    // 캐시 데이터
    private int _curveAmount = Shader.PropertyToID("_CurveAmount");

    IEnumerator Start()
    {
        // 메인 카메라 Transform을 미리 받아온다. 
        camTransform = Camera.main.transform;

        SpawnInitialTrack();
        SpawnPlayer();

        Debug.Log("3");
        yield return new WaitForSeconds(1f);
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        GameManager.IsPlaying = true;
    }

    
    void Update()
    {
        if(GameManager.IsPlaying == false) return;

       RepositionTrack();

       //float sin = Mathf.Sin(Time.time);

       BendTrack();
    }

    // 초기 트랙 생성 (한번만 실행)
    void SpawnInitialTrack()
    {

        // Track track = Instantiate(trackPrefabs, position, Quaternion.identity, transform);
        // track.name = "Track_0";

        // trackList.Add(track);

        // 초기값 = 카메라의 z좌표
        Vector3 position = new Vector3(0f, 0f, camTransform.position.z) ;
        // 초기값; 종료조건; 증감;
        for(int i = 0; i < trackCount; i++)
        {   
            Track next = SpawnNextTrack(position, $"Track_{i}");
            // 첫번째 ExitPoint에 두번째 EntryPoint 접합
            // Track track = Instantiate(trackPrefabs, position, Quaternion.identity, transform);
            // track.name = $"Track_{i}";
            // track.trackmgr = this;
            // trackList.Add(track);

            position = next.ExitPoint.position;
        }
    }

    void ScrollTrack()
    {
        // 트랙 스크롤
        // Vector3.back (0, 0, -1f)

        // foreach ( 전달받을 값 in 리스트)
        foreach(Track t in trackList)
        {
            if( t != null )
                t.transform.position += Vector3.back * scrollspeed * Time.deltaTime;
        }
    }

    Track SpawnNextTrack(Vector3 position, string trackname)
    {
        // 첫번째 ExitPoint에 두번째 EntryPoint 접합
            Track Next = Instantiate(trackPrefabs, position, Quaternion.identity, transform);
            
            Next.name = trackname;
            Next.trackmgr = this;
            
            laneList  = Next.lanelist;


            trackList.Add(Next);
            return Next;
            
    }
    
    // 트랙 재배치
    void RepositionTrack()
    {
        if(trackList.Count <= 0) return;

        // 언제 재배치 하나? 덥 : z축 < 0f -> 삭제 -> 리스트의 마지막에 생성
        // Track_0 => trackList[0]
        // trackList[0] => 트랙의 첫번째 값 가져오기
        // trackList[trackList.Count-1] => 트랙의 마지막 값 가져오기
        if (trackList[0].ExitPoint.position.z < camTransform.position.z)
        {
            Track track = trackList[trackList.Count-1];
            SpawnNextTrack(track.ExitPoint.position, trackList[0].name);

            Destroy(trackList[0].gameObject);
            trackList.RemoveAt(0);
            
        }
    }

    void BendTrack()
    {
        if (scrollspeed <= 0f) return;

        // 0f ~ 1f => -1f ~ 1f

       // 0 -> *2 -1 -1
       // 1 -> *2 -1 1
       // 0.5 -> *2 1 0

       float rndX = Mathf.PerlinNoise1D(Time.time * CurvedFrequencyX) *2f - 1f;
       rndX = rndX * CurvedAmplitudeX;
       float rndY = Mathf.PerlinNoise1D(Time.time * CurvedFrequencyY) *2f - 1f;
       rndY = rndY * CurvedAmplitudeY;
       
       CurvedMaterial.SetVector(_curveAmount, new Vector4(rndX, rndY, 0f, 0f));
    }

    // z 값에 해당하는 트랙을 가져오기
    public Track GetTrackByZ(float z)
    {
        // 해당하는 트랙 찾아서 반환
        foreach(var t in trackList)
        {
            if( z > t.EntryPoint.position.z && z <= t.ExitPoint.position.z)
                return t;
        }
        return null;
    }

    public void StopScrollTrack()
    {
        scrollspeed = 0f;
    }

     void SpawnPlayer()
    {
        PlayerControl player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.trackMgr = this;
    }
}
