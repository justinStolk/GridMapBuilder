using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixData 
{
    public List<MatrixInfo> data = new();
}

[System.Serializable]
public class MatrixInfo
{
    public string UnitType, TerrainType;
    public int MatrixValue;

    public MatrixInfo(string unitType, string terrainType, int matrixValue)
    {
        UnitType = unitType;
        TerrainType = terrainType;
        MatrixValue = matrixValue;
    }

}