using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatrixField : MonoBehaviour
{
    public string UnitType { get; private set; }
    public string TerrainType { get; private set; }

    [SerializeField] private InputField field;

    public void Initialize(string unitType, string terrainType)
    {
        UnitType = unitType;
        TerrainType = terrainType;
    }

    public void FillField(int fieldValue)
    {
        field.text = fieldValue.ToString();
    }

    public void UpdateValue(string value)
    {
        try
        {
            UnitTerrainMatrix.AddMatrixItem(UnitType, TerrainType, int.Parse(value));
        }
        catch (System.FormatException)
        {
            Debug.Log("No value found, defaulting to 0");
            UnitTerrainMatrix.AddMatrixItem(UnitType, TerrainType, 0);
        }
    }

}
