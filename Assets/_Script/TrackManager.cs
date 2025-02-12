using UnityEngine;
using System.Collections.Generic;
using System;

public class TrackManager : MonoBehaviour
{
    public Track trackPrefabs;

    [Range(0f, 50f)] public float scrollspeed = 10f;
    [Range(1, 100)] public int trackCount = 3;
    // public float trackThreshold = 10f; //트랙 삭제 z축

    private List<Track> trackList = new List<Track>();  // 생성한 트랙들 보관
    private Transform camTransform;
    void Start()
    {
        // 메인 카메라 Transform을 미리 받아온다. 
        camTransform = Camera.main.transform;
        SpawnInitialTrack();
    }

    
    void Update()
    {
       RepositionTrack();
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

}
