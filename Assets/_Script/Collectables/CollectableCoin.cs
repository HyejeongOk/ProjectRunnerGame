using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class CollectableCoin : Collectable
{
   [SerializeField] Transform pivot;
   [SerializeField] ParticleSystem particle;
   

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


        // transform.DOScale(1.2f, 0.25f)
        //     .OnComplete(() => transform.DOScale(0f, 0.2f)
        //     .OnComplete(() => Destroy(gameObject)));
        // Destroy(gameObject);
        
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        // 1 transform , 2 pivot , 3 particle => world 좌표로 분리
        // 코인이 사라질 때, Track 종속이 아닌, World로 바꾼다
        // (Local => World)
        transform.SetParent(null);
        
        pivot.gameObject.SetActive(false);
        particle.Play();

        yield return new WaitUntil(() => particle.isPlaying == false);

        Destroy(gameObject);
    }
}
