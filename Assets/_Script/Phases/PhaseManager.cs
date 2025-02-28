using UnityEngine;
using CustomInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PhaseManager : MonoBehaviour
{
    [HorizontalLine("기본속성"), HideField] public bool _l0;
    [SerializeField] float updateInterval = 1f;

    [HorizontalLine("Phase 속성"), HideField] public bool _l1;
    [SerializeField] List<Phase> mileageList = new List<Phase>();

    private TrackManager trackMgr;
    private ObstacleManager obstacleMgr;
    private IngameUI uiIngame;

    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();

        obstacleMgr = FindFirstObjectByType<ObstacleManager>();

        uiIngame = FindFirstObjectByType<IngameUI>();

        GetFinishline();

        uiIngame.SetMileage(mileageList);

        yield return new WaitUntil( ()=> GameManager.IsPlaying );
        StartCoroutine(IntervalUpdate());
    }

    IEnumerator IntervalUpdate()
    {
        if (mileageList == null || mileageList.Count <= 0)
            yield break;

        int i = 0;

        while(true)
        {
            Phase phase = mileageList[i];
            // 특정 마일리지 마다 호출 ( 이벤트 시스템 )
            if(GameManager.mileage >= phase.Mileage)
            {
                SetPhase(phase);
                i++;
            }

            // Count 같다라는 의미 => 끝 도착
            if(i == mileageList.Count)
            {
                GameClear(phase);
                yield break;
            }
                   
            yield return new WaitForSeconds(updateInterval);
        }
    }

   void GetFinishline()
    {
        Phase phaseEnd = mileageList.LastOrDefault();       

        GameManager.mileageFinish = phaseEnd.Mileage;
    }



    void SetPhase(Phase phase)
    {
        uiIngame?.SetPhase(phase);
        trackMgr?.SetPhase(phase);
        obstacleMgr?.SetPhase(phase);
    }

    void GameClear(Phase phase)
    {
        SetPhase(phase);

        GameManager.IsPlaying = false;
        GameManager.IsGameOver = true;
    }
}