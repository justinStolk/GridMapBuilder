using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitTerrainMatrix
{
    public static Dictionary<Tuple<string, string>, int> MatrixDictionary = new();

    public static void AddMatrixItem(string unitType, string terrainType, int costValue)
    {
        Tuple<string, string> matrixKey = new Tuple<string, string>(unitType, terrainType);
        if (!MatrixDictionary.ContainsKey(matrixKey))
        {
            MatrixDictionary.Add(matrixKey, -1);
        }
        MatrixDictionary[matrixKey] = costValue;
    }
    public static void RemoveMatrixItem(string unitType, string terrainType)
    {
        Tuple<string, string> matrixKey = new Tuple<string, string>(unitType, terrainType);
        if (!MatrixDictionary.ContainsKey(matrixKey))
        {
            return;
        }
        MatrixDictionary.Remove(matrixKey);
    }
    public static int GetMatrixItem(string unitType, string terrainType)
    {
        Tuple<string, string> matrixKey = new Tuple<string, string>(unitType, terrainType);
        return MatrixDictionary[matrixKey];
    }


}
