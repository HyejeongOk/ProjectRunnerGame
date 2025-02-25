using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public abstract void SetLanePosition(int lane, float ypos, float zpos, TrackManager tm);

    public abstract void Collect();
}
