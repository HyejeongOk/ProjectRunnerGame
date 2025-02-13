using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class PlayerControl : MonoBehaviour
{
    // 속성 : 인스펙터 노출
    [SerializeField] private float moveDuration = 0.5f;  // 이동에 걸리는 시간
    [SerializeField] Ease moveEase;
    // [SerializeField] AnimationCurve jumpCurve;  // 점프 모양
    [SerializeField] float jumpDuration = 0.5f;  // 점프 지속 시간
    [SerializeField] float jumpHeight = 3f;  // 점프 높이
    [SerializeField] Ease jumpEase;

    // 다른 클래스에 공개는 하지만 인스펙터 노출 안함
    [HideInInspector] public TrackManager trackMgr;

    // 내부 사용 : 인스펙터 노출 안함
    private int currentLane = 1;

    private Vector3 targetpos;

    float jumpStarttime;
    private bool isJumpinging = false;  //  true : 점프 중, false : 바닥에 붙어있는 중

    private bool isMoving = false;

    void Update()
    {
        if (Input.GetButtonDown("Left") && isMoving == false)
            HandleDirection(-1);

        else if (Input.GetButtonDown("Right") && isMoving == false)
            HandleDirection(1);

        else if (Input.GetButtonDown("Jump") && isMoving == false)
        {
            jumpStarttime = Time.time;
            HandleJump();   
        }

        // -1, 0, 1

        // if (Input.GetButtonDown("Left"))  // 왼쪽 이동
        // {
        //     HandlePlayer(-1);
        // }

        // else if (Input.GetButtonDown("Right") && isJumpinging == false) // 오른쪽 이동
        // {
        //     HandlePlayer(1);
        // }

        // else if (Input.GetButtonDown("Jump") && isJumpinging == false)  // 점프
        // {
        //     jumpStarttime = Time.time;
        //     isJumpinging = true;
        // }

        // if (isJumpinging == true)
        // {
        //     // 점프 시작 후 경과시간 체크
        //     float elapsedTime = Time.time - jumpStarttime;
        //     // 분자 / 분모 => 퍼센트

        //     // 0.1 / 0.5 => 20%
        //     // 0.2 / 0.5 => 40%
        //     // 0.5 / 0.5 => 100%
        //     // ---- 점프종료

        //     float p = Mathf.Clamp(elapsedTime / jumpDuration, 0f, 1f);
        //     float height =  jumpCurve.Evaluate(p) * jumpHeight;

        //     targetpos = new Vector3(transform.position.x, height, transform.position.z);

        //     // 점프 시간 종료 => isJumping을 false로 바꾼다    
        //     if(p >= 1f)
        //     {
        //         isJumpinging = false;
        //         transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        //     }
        // }



        // transform.position += Vector3.right * horz * horzspeed * Time.deltaTime;

    }

    // direction -1 이면 왼쪽, +1이면 오른쪽
    void HandleDirection(int direction)
    {
        isMoving = true;

        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count-1);
        
        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, transform.position.y, transform.position.z);

        transform.DOMove(targetpos, moveDuration)
                .OnComplete( () => isMoving = false)
                .SetEase(moveEase);
    }

    void HandleJump()
    {
        isMoving = true;
        transform.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
                .OnComplete( () => isMoving = false)
                .SetEase(jumpEase);
    }

    // void UpdatePosition()
    // {
    //     // Lerp ? Linear Interpolation : 선형보간
    //     transform.position = Vector3.Lerp(transform.position, targetpos, speed * Time.deltaTime);
    // }

}
