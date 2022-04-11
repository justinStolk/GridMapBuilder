using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileInfo
{
    public string tileName;
    public string TerrainType;
    public int DefenseBonus;
    public int AvoidBonus;

    [HideInInspector] public Vector2Int Position;
    [HideInInspector] public int Index;

}
