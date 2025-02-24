using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class CollectableCoin : Collectable
{
   // public Transform pivot;

   

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

    public override void Collect()
    {
        GameManager.coins += Add;

        transform.SetParent(null);

        transform.DOScale(1.2f, 0.25f)
            .OnComplete(() => transform.DOScale(0f, 0.2f)
            .OnComplete(() => Destroy(gameObject)));
        Destroy(gameObject);
    }
}
