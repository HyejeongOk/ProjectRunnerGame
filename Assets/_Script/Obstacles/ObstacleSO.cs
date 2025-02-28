using System.Collections.Generic;
using System.Data.Common;
using CustomInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Obstacle")]
public class ObstacleSO : ScriptableObject
{
    public List<ObstaclePool> pools;

    [AsRange(0,100)] public Vector2 interval;
}
