using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using Deform;
using System.Collections.Generic;

public enum PlayerState {Idle = 0, Move, Jump, Slide};

public class PlayerControl : MonoBehaviour
{
    // 속성 : 인스펙터 노출
    [SerializeField] Transform pivot;
    [SerializeField] Collider colNormal, colSlide;

    [Space(20)]
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

    public PlayerState state;


    // 내부 사용 : 인스펙터 노출 안함
    private int currentLane = 1;
    private Vector3 targetpos;

    // private bool isMoving , isJumping;

    void Start()
    {
        SwitchCollider(true);
    }

    // b : TRUE -> 기본모드, FALSE -> 슬라이드
    void SwitchCollider(bool b)
    {
        colNormal.gameObject.SetActive(b);
        colSlide.gameObject.SetActive(!b);
    }

    void Update()
    {
        // [CHEAT]
        // 1 키 토글, 처음 => 멈춤, 다시 => 재개
        if (Input.GetKeyDown(KeyCode.Alpha1))
            GameManager.IsPlaying = !GameManager.IsPlaying;

        if (pivot == null || GameManager.IsPlaying == false)
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

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Collectable")
        {
            Debug.Log($"Collectable 획득 : {other.name}");
            GameManager.coins++;
        }

        else if (other.tag == "Obstacle")
        {
            Debug.Log($"장애물 충돌 : {other.name}");
            GameManager.IsPlaying = false;   

        }
    }


    private Sequence _seqMove;
    // direction -1 이면 왼쪽 , +1 이면 오른쪽
    void HandleDirection(int direction)
    {
        if ( state == PlayerState.Jump || state == PlayerState.Slide ) return;

        state = PlayerState.Move;

        var squash = direction switch { -1 => deformLeft, 1 => deformRight, _ => null };

        if (_seqMove != null)
        {
            _seqMove.Kill(true);
            state = PlayerState.Move;
        }



        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count-1);

        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, pivot.position.y , pivot.position.z );

        _seqMove = DOTween.Sequence().OnComplete(()=> {squash.Factor = 0; state = PlayerState.Idle; });
        _seqMove.Append(pivot.DOMove(targetpos, moveDuration));
        _seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration/2f, (v)=> squash.Factor = v ));
        _seqMove.Append(DOVirtual.Float(1f, 0f, moveDuration/2f, (v)=> squash.Factor = v ));
    }

    void HandleJump()
    {
        if ( state != PlayerState.Idle ) return;

        state = PlayerState.Jump;

        pivot.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
                .SetEase(jumpEase);

        deformJumpUp.Factor = 0f;
        deformJumpDown.Factor = 0f;  

        Sequence seq = DOTween.Sequence().OnComplete( ()=> state = PlayerState.Idle );
        seq.Append(DOVirtual.Float( 0f, 1f, jumpDuration * jumpIntervals[0], v => deformJumpUp.Factor = v ));
        seq.Append(DOVirtual.Float( 1f, 0f, jumpDuration * jumpIntervals[1], v => deformJumpUp.Factor = v ));        
        seq.Join(DOVirtual.Float( 0f, -0.25f, jumpDuration * jumpIntervals[2], v => deformJumpDown.Factor = v ));
        seq.Append(DOVirtual.Float( -0.25f, 0f, jumpDuration * jumpIntervals[3], v => deformJumpDown.Factor = v ));        
    }

    void HandleSlide()
    {
        if ( state != PlayerState.Idle ) return;

        state = PlayerState.Slide;
        SwitchCollider(false);

        Sequence seq = DOTween.Sequence().OnComplete( ()=> 
        {   state = PlayerState.Idle;
            SwitchCollider(true);
        });
        seq.Append(DOVirtual.Float( 0f, -0.15f, slideDuration, v => deformSlide.Factor = v ));
        seq.Append(DOVirtual.Float( -0.15f, 0f, slideDuration, v => deformSlide.Factor = v ));
    }

}