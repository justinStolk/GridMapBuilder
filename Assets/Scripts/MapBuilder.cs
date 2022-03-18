using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{
    public FSM fsm;

    public Dictionary<Vector2Int, Tile> tiles = new();
    public Dictionary<Vector2Int, UtilityTile> utilityTiles = new();

    [SerializeField] private InputField fileNameField;
    private string fileName;


    // Start is called before the first frame update
    void Start()
    {
        fsm = new FSM(typeof(PlacementState), GetComponents<BaseState>());
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnUpdate();
    }
    public void GoToMenuState()
    {
        fsm.ChangeState(typeof(MenuState));
    }
    public void GoToUtilityState()
    {
        fsm.ChangeState(typeof(UtilityState));
    }
    public void GoToPlacementState()
    {
        fsm.ChangeState(typeof(PlacementState));
    }
    public void GoToTestState()
    {
        fsm.ChangeState(typeof(TestingState));
    }

    public void SetFileName()
    {
        fileName = fileNameField.text;
    }
    public void SaveMapData()
    {
        MapData mapData = new();
        PlacementState p = GetComponent<PlacementState>();

        mapData.GridHeight = p.GridHeight;
        mapData.GridWidth = p.GridWidth;
        foreach(KeyValuePair<Vector2Int, Tile> tilePair in tiles)
        {
            if (tilePair.Value != null)
            {
                mapData.tilesInfo.Add(tilePair.Value.tileInfo);
            }
        }
        foreach (KeyValuePair<Vector2Int, UtilityTile> utilityPair in utilityTiles)
        {
            if(utilityPair.Value != null)
            {
                 mapData.utilityTileInfo.Add(utilityPair.Value.utilityInfo);
            }
        }

        string path = Application.persistentDataPath + "/" + fileName + ".txt";
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
        utilityTiles.Clear();

        PlacementState p = GetComponent<PlacementState>();
        UtilityState u = GetComponent<UtilityState>();

        string path = Application.persistentDataPath + "/" + fileName + ".txt";

        StreamReader reader = new(path);
        string loadedData = reader.ReadToEnd();
        reader.Close();

        MapData data = JsonUtility.FromJson<MapData>(loadedData);

        p.GridWidth = data.GridWidth;
        p.GridHeight = data.GridHeight;
        p.BuildGrid();
        foreach (KeyValuePair<Vector2Int, Tile> pair in tiles)
        {
            if (pair.Value != null)
            {
                Destroy(pair.Value.gameObject);
            }
        }
        foreach(KeyValuePair<Vector2Int, UtilityTile> uPair in utilityTiles)
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
            if (!tiles.ContainsKey(info.Position))
            {
                tiles.Add(info.Position, null);
            }
            tiles[info.Position] = loadedTile;
        }
        foreach (UtilityInfo info in data.utilityTileInfo)
        {
            UtilityTile loadedTile = Instantiate(u.tileTypes[info.Index], new Vector3(info.Position.x, info.Position.y, 0), Quaternion.identity);
            loadedTile.utilityInfo = info;
            if (!utilityTiles.ContainsKey(info.Position))
            {
                utilityTiles.Add(info.Position, null);
            }
            utilityTiles[info.Position] = loadedTile;
        }
    }

}
