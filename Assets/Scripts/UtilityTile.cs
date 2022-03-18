using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityTile : MonoBehaviour
{
    public UtilityInfo utilityInfo;
}

[System.Serializable]
public class UtilityInfo
{
    public enum UtilityType { StartTile, SeizeTile, ReachTile }

    public UtilityType utilityType;
    [HideInInspector] public Vector2Int Position;
    [HideInInspector] public int Index;
}
