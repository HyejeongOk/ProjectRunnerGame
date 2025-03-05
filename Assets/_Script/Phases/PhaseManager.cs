using UnityEngine;
using CustomInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PhaseManager : MonoBehaviour
{
    [HorizontalLine("기본속성"), HideField] public bool _l0;
    [SerializeField] float updateInterval = 1f;

    [HorizontalLine("Phase Data 속성"), HideField] public bool _l1;
    [SerializeField, Foldout] List<PhaseSO> phaseList = new List<PhaseSO>();

    private TrackManager trackMgr;
    private ObstacleManager obstacleMgr;
    private CollectableManager colMgr;
    private IngameUI uiIngame;

    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        obstacleMgr = FindFirstObjectByType<ObstacleManager>();
        colMgr = FindFirstObjectByType<CollectableManager>();
        uiIngame = FindFirstObjectByType<IngameUI>();

        GetFinishline();

        uiIngame.SetMileage(phaseList);

        yield return new WaitUntil( ()=> GameManager.IsGameOver == false && GameManager.IsPlaying == true );
        StartCoroutine(IntervalUpdate());
    }

    IEnumerator IntervalUpdate()
    {
        if (phaseList == null || phaseList.Count <= 0)
            yield break;

        int i = 0;

        while(true)
        {
            PhaseSO phase = phaseList[i];
            // 특정 마일리지 마다 호출 ( 이벤트 시스템 )
            if(GameManager.mileage >= phase.Mileage)
            {
                SetPhase(phase);
                i++;
            }

            // Count 같다라는 의미 => 끝 도착
            if(i >= phaseList.Count)
            {
                GameClear(phase);
                yield break;
            }
                   
            yield return new WaitForSeconds(updateInterval);
        }
    }

   void GetFinishline()
    {
        PhaseSO phaseEnd = phaseList.LastOrDefault();       

        GameManager.mileageFinish = phaseEnd.Mileage;
    }



    void SetPhase(PhaseSO phase)
    {
        uiIngame?.SetPhase(phase);
        trackMgr?.SetPhase(phase);
        obstacleMgr?.SetPhase(phase);
        colMgr?.SetPhase(phase);
    }

    void GameClear(PhaseSO phase)
    {
        SetPhase(phase);

        GameManager.IsPlaying = false;
        GameManager.IsGameOver = true;
    }
}