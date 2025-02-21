using DG.Tweening;
using UnityEngine;

public class CollectableCoin : Collectable
{
    // 해당 코인 증가량
    public uint Add = 1;

    public override void SetLanePosition(int lane, float zpos, TrackManager tm)
    {
        // Lane 위치
        lane = Mathf.Clamp(lane, 0, tm.laneList.Count-1);
        Transform laneTransform = tm.laneList[lane];
        Vector3 pos = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    Sequence _seqInfo;
    public override void Collect()
    {
        GameManager.coins += Add;

        // _seqInfo = DOTween.Sequence();
        
        // _seqInfo.Append(tmcoin.transform.DOScale(1.2f, duration * 0.1f));
        // _seqInfo.Append(tmInfomation.transform.DOScale(1f, duration * 0.4f));
        // _seqInfo.AppendInterval(duration*0.2f);
        // _seqInfo.Append(tmInfomation.transform.DOScale(0f, duration * 0.1f));

        // this를 쓰명 CollectableCoin만, gameObject를 쓰면 전부 다 지워짐
        Destroy(gameObject);
    }
}
