using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public Transform EntryPoint;
    public Transform ExitPoint;
    [HideInInspector] public List<Transform> lanelist;

    [HideInInspector] public TrackManager trackmgr;

    void LateUpdate()
    {
        Scroll();
    }

    void Scroll()
    {
        if(trackmgr == null) return;

        transform.position += Vector3.back * trackmgr.scrollspeed * Time.smoothDeltaTime;
        
    // Time.deltaTime => 매 프레임 당 1번 호출될 때 간격(Interval Time)
    // Time.fixedDeltaTime => 0.02 간격
    // Time.smoothDeltaTime => deltaTime 평균 => 값이 고르게 나온다

    // fixedDelta < delta < smoothDelta
    
    }
}
