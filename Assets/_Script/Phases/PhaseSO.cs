using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

// MonoBehaviour -> Runtiome(실행중) 작동 클래스
// ScriptableObject -> 에디터 어디든 존재 할 수 있는 클래스 (DATA)

[CreateAssetMenu(menuName = "Data/Phase")]
public class PhaseSO : ScriptableObject
{
    public string displayName;
    [Preview(Size.small)] public Sprite Icon;
    public uint Mileage;

    public float scrollSpeed;

    // 장애물(Obstacle) 설정
    [Foldout] public ObstacleSO obstacleData;

    // 아이템(Collectables) 설정 - 코인 폼...
    [Foldout] public CollectableSO collectableData;
}