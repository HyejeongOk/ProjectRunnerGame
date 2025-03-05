using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Data/Collectable")]
public class CollectableSO : ScriptableObject
{
     public List<CollectablePool> collectablePools;
     public List<LanepatternPool> lanepatternPools;

    [AsRange(0, 100)] public Vector2 interval; // 개별 스폰 간격
    
    [AsRange(0, 30)] public Vector2 quota; // 한 패턴에서 최대 찍을 개수
}
