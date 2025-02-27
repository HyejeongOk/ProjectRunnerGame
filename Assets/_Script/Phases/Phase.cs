using CustomInspector;
using UnityEngine;

[System.Serializable]
public struct Phase
{
    public string Name;
    public uint Mileage;

    [Space(5)]
    public float scrollSpeed;

    [AsRange(10,40)] public Vector2 obstacleInterval;
}
