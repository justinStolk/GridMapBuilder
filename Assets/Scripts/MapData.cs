using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public int GridWidth, GridHeight;

    public List<TileInfo> tilesInfo = new();
    public List<UtilityInfo> utilityTileInfo = new();
}
