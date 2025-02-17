using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using Deform;

public class PlayerControl : MonoBehaviour
{
    // 속성 : 인스펙터 노출
    [SerializeField] Transform pivot;
    [SerializeField] SquashAndStretchDeformer deformLeft, deformRight, deformJumpUp, deformJumpDown, deformSlide;


    [SerializeField] float moveDuration = 0.5f; // 이동에 걸리는 시간
    [SerializeField] Ease moveEase;

    [Space(20)]
    [SerializeField] float jumpDuration = 0.5f;    // 점프 지속 시간
    [SerializeField] float jumpHeight = 3f;      // 점프 높이
    [SerializeField] Ease jumpEase;

    [SerializeField] float[] jumpIntervals = { 0.25f, 0.5f, 0.75f, 0.25f }; // 점프 시퀀스 타이밍 조절

    [Space(20)]
    [SerializeField] float slideDuration = 0.5f;  // 슬라이드 지속 시간


    // 다른 클래스에 공개는 하지만 인스펙터 노출 안함
    [HideInInspector] public TrackManager trackMgr;


    // 내부 사용 : 인스펙터 노출 안함
    private int currentLane = 1;
    private Vector3 targetpos;

    private bool isMoving , isJumping;

    void Update()
    {
        if (pivot == null)
            return;

        if (Input.GetButtonDown("Left") && currentLane > 0)
            HandleDirection(-1);

        if (Input.GetButtonDown("Right") && currentLane < trackMgr.laneList.Count-1 )
            HandleDirection(1);
        
        if (Input.GetButton("Jump"))
            HandleJump(); 

        if (Input.GetButton("Slide"))
            HandleSlide();         
    }



    private Sequence _seqMove;
    // direction -1 이면 왼쪽 , +1 이면 오른쪽
    void HandleDirection(int direction)
    {
        if ( isJumping == true ) return;

        isMoving = true;

        var squash = direction switch { -1 => deformLeft, 1 => deformRight, _ => null };

        if (_seqMove != null)
            _seqMove.Kill(true);



        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count-1);

        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, pivot.position.y , pivot.position.z );

        _seqMove = DOTween.Sequence().OnComplete(()=> {squash.Factor = 0; isMoving = false; });
        _seqMove.Append(pivot.DOMove(targetpos, moveDuration));
        _seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration/2f, (v)=> squash.Factor = v ));
        _seqMove.Append(DOVirtual.Float(1f, 0f, moveDuration/2f, (v)=> squash.Factor = v ));
    }

    void HandleJump()
    {
        if ( isMoving == true || isJumping == true ) return;

        isJumping = true;

        pivot.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
                .OnComplete( ()=> isMoving = false )
                .SetEase(jumpEase);

        deformJumpUp.Factor = 0f;
        deformJumpDown.Factor = 0f;  

        Sequence seq = DOTween.Sequence().OnComplete( ()=> isJumping = false );
        seq.Append(DOVirtual.Float( 0f, 1f, jumpDuration * jumpIntervals[0], v => deformJumpUp.Factor = v ));
        seq.Append(DOVirtual.Float( 1f, 0f, jumpDuration * jumpIntervals[1], v => deformJumpUp.Factor = v ));        
        seq.Join(DOVirtual.Float( 0f, -0.25f, jumpDuration * jumpIntervals[2], v => deformJumpDown.Factor = v ));
        seq.Append(DOVirtual.Float( -0.25f, 0f, jumpDuration * jumpIntervals[3], v => deformJumpDown.Factor = v ));        
    }

    void HandleSlide()
    {
        if ( isMoving == true || isJumping == true ) return;

        isJumping = true;

        Sequence seq = DOTween.Sequence().OnComplete( ()=> isJumping = false );
        seq.Append(DOVirtual.Float( 0f, -0.15f, slideDuration, v => deformSlide.Factor = v ));
        seq.Append(DOVirtual.Float( -0.15f, 0f, slideDuration, v => deformSlide.Factor = v ));
    }

}