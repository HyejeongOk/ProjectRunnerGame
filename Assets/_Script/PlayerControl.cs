using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float horzspeed;
    [HideInInspector] public TrackManager trackMgr;

    public int currentLane = 1;

    void Update()
    {
        // -1, 0, 1

        if (Input.GetButtonDown("Left"))  // 왼쪽 이동
        {
            currentLane -= 1;
            currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count-1);
            
            Transform l = trackMgr.laneList[currentLane];

            transform.position = new Vector3(l.position.x, transform.position.y, transform.position.z);
        }

        else if (Input.GetButtonDown("Right")) // 오른쪽 이동
        {
            currentLane += 1;
             currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count-1);

             Transform l = trackMgr.laneList[currentLane];
             transform.position = new Vector3(l.position.x, transform.position.y, transform.position.z);
        }

        // transform.position += Vector3.right * horz * horzspeed * Time.deltaTime;
    }
}
