using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveAndLoader : MonoBehaviour
{

    [SerializeField] private InputField fileNameField;

    private void Start()
    {
        LoadMatrix();
    }
    public void SaveMapData()
    {
        MapData mapData = new();
        PlacementState p = MapBuilder.instance.GetComponent<PlacementState>();

        mapData.GridHeight = p.GridHeight;
        mapData.GridWidth = p.GridWidth;
        foreach (KeyValuePair<Vector2Int, Tile> tilePair in MapBuilder.instance.tiles)
        {
            if (tilePair.Value != null)
            {
                mapData.tilesInfo.Add(tilePair.Value.tileInfo);
            }
        }
        foreach (KeyValuePair<Vector2Int, UtilityTile> utilityPair in MapBuilder.instance.utilityTiles)
        {
            if (utilityPair.Value != null)
            {
                mapData.utilityTileInfo.Add(utilityPair.Value.utilityInfo);
            }
        }

        string path = Application.persistentDataPath + "/" + fileNameField.text + ".txt";
        string data = JsonUtility.ToJson(mapData);

        StreamWriter streamWriter = new(path);

        streamWriter.Write(data);
        //Debug.Log("Trying to write data to: " + path + "!");
        streamWriter.Flush();
        streamWriter.Close();
    }
    public void LoadMapData()
    {
        //tiles.Clear();
        MapBuilder.instance.utilityTiles.Clear();

        PlacementState p = MapBuilder.instance.GetComponent<PlacementState>();
        UtilityState u = MapBuilder.instance.GetComponent<UtilityState>();

        string path = Application.persistentDataPath + "/" + fileNameField.text + ".txt";

        StreamReader reader = new(path);
        string loadedData = reader.ReadToEnd();
        reader.Close();

        MapData data = JsonUtility.FromJson<MapData>(loadedData);

        p.GridWidth = data.GridWidth;
        p.GridHeight = data.GridHeight;
        p.BuildGrid();
        foreach (KeyValuePair<Vector2Int, Tile> pair in MapBuilder.instance.tiles)
        {
            if (pair.Value != null)
            {
                Destroy(pair.Value.gameObject);
            }
        }
        foreach (KeyValuePair<Vector2Int, UtilityTile> uPair in MapBuilder.instance.utilityTiles)
        {
            if (uPair.Value != null)
            {
                Destroy(uPair.Value.gameObject);
            }
        }

        foreach (TileInfo info in data.tilesInfo)
        {
            Tile loadedTile = Instantiate(p.tileTypes[info.Index], new Vector3(info.Position.x, info.Position.y, 0), Quaternion.identity);
            loadedTile.tileInfo = info;
            if (!MapBuilder.instance.tiles.ContainsKey(info.Position))
            {
                MapBuilder.instance.tiles.Add(info.Position, null);
            }
            MapBuilder.instance.tiles[info.Position] = loadedTile;
        }
        foreach (UtilityInfo info in data.utilityTileInfo)
        {
            UtilityTile loadedTile = Instantiate(u.tileTypes[info.Index], new Vector3(info.Position.x, info.Position.y, 0), Quaternion.identity);
            loadedTile.utilityInfo = info;
            if (!MapBuilder.instance.utilityTiles.ContainsKey(info.Position))
            {
                MapBuilder.instance.utilityTiles.Add(info.Position, null);
            }
            MapBuilder.instance.utilityTiles[info.Position] = loadedTile;
        }
    }

    public void SaveMatrix()
    {
        MatrixData matrixValues = new();
        foreach(KeyValuePair<Tuple<string,string>, int> matrixItem in UnitTerrainMatrix.MatrixDictionary)
        {
            matrixValues.data.Add(new MatrixInfo(matrixItem.Key.Item1, matrixItem.Key.Item2, matrixItem.Value));
            //Debug.Log(matrixItem.Key.Item1 + " " + matrixItem.Key.Item2 + " " + matrixItem.Value);
        }
        Debug.Log(matrixValues);

        string path = Application.persistentDataPath + "/MatrixData.txt";
        string data = JsonUtility.ToJson(matrixValues);

        StreamWriter streamWriter = new(path);

        streamWriter.Write(data);
        streamWriter.Flush();
        streamWriter.Close();
    }
    private void LoadMatrix()
    {
        try 
        {
            UnitTerrainMatrix.MatrixDictionary.Clear();

            string path = Application.persistentDataPath + "/MatrixData.txt";

            MatrixUtility utility = FindObjectOfType<MatrixUtility>();


            StreamReader reader = new(path);
            string loadedData = reader.ReadToEnd();
            reader.Close();

            MatrixData data = JsonUtility.FromJson<MatrixData>(loadedData);

            List<string> uniqueTerrainTypes = new List<string>();
            List<string> uniqueUnitTypes = new List<string>();

            foreach (MatrixInfo matrixInfo in data.data)
            {
                UnitTerrainMatrix.AddMatrixItem(matrixInfo.UnitType, matrixInfo.TerrainType, matrixInfo.MatrixValue);
                if (!uniqueTerrainTypes.Contains(matrixInfo.TerrainType))
                {
                    uniqueTerrainTypes.Add(matrixInfo.TerrainType);
                    utility.CreateNewTerrainType(matrixInfo.TerrainType);
                }
                if (!uniqueUnitTypes.Contains(matrixInfo.UnitType))
                {
                    uniqueUnitTypes.Add(matrixInfo.UnitType);
                    utility.CreateNewUnitType(matrixInfo.UnitType);
                }
            }
            utility.EvaluateMatrixFields();
        } 
        catch(FileNotFoundException)
        {
            SetDefaultMatrix();
            Debug.Log("No Matrix Data file found, it's either deleted or not initialized");
        }

    }

    private void SetDefaultMatrix()
    {
        MatrixUtility utility = FindObjectOfType<MatrixUtility>();
        utility.CreateNewUnitType("Infantry");
        utility.CreateNewUnitType("Cavalier");
        utility.CreateNewUnitType("Flier");
        utility.CreateNewTerrainType("Plains");
        utility.CreateNewTerrainType("Desert");
        utility.CreateNewTerrainType("Sea");
        UnitTerrainMatrix.AddMatrixItem("Infantry", "Plains", 1);
        UnitTerrainMatrix.AddMatrixItem("Infantry", "Desert", 3);
        UnitTerrainMatrix.AddMatrixItem("Infantry", "Sea", 0);
        UnitTerrainMatrix.AddMatrixItem("Cavalier", "Plains", 1);
        UnitTerrainMatrix.AddMatrixItem("Cavalier", "Desert", 4);
        UnitTerrainMatrix.AddMatrixItem("Cavalier", "Sea", 0); 
        UnitTerrainMatrix.AddMatrixItem("Flier", "Plains", 1);
        UnitTerrainMatrix.AddMatrixItem("Flier", "Desert", 1);
        UnitTerrainMatrix.AddMatrixItem("Flier", "Sea", 1);
        utility.EvaluateMatrixFields();
    }

}
