using Unity.Mathematics;
using UnityEngine;

// abstract (추상 클래스)
// Obstacle 타입들의 근본
// 베이스 클래스 
public abstract class Obstacle : MonoBehaviour
{
    public abstract void SetLanePosition(int lane, float zpos, TrackManager tm);
   
}
