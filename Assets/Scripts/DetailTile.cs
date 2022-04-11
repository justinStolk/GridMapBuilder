using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailTile : MonoBehaviour
{
    public DetailInfo detailInfo;
}

[System.Serializable]
public class DetailInfo
{
    public string tileName;
    public int DefenseBonus;
    public int AvoidBonus;

    [HideInInspector] public Vector2Int Position;
    [HideInInspector] public int Index;
}
