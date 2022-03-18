using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementState : BaseState
{
    public int GridWidth, GridHeight;
    public InputField WidthField, HeightField;
    public Text gridSizeText;

    public Tile[] tileTypes;
    [SerializeField] private GameObject tileSelector;
    [SerializeField] GameObject visualizationSprite;
    [SerializeField] GameObject placementInterface;
    [SerializeField] GameObject brush;
    [SerializeField] private float brushStepSize = 0.5f;

    private int tileIndex;
    private Button[] tileButtons; 
    private MapBuilder mapBuilder;
    public Tile activeTile;

    private List<GameObject> backgroundVisualizers = new();
    private int formerWidth, formerHeight;
    private float brushSize = 1;
    private bool brushIsActive;
    private FSM fsm;

    private void Awake()
    {
        mapBuilder = GetComponent<MapBuilder>();
        tileButtons = tileSelector.GetComponentsInChildren<Button>();
        //fsm = new FSM(typeof(SimpleMode), GetComponents<BaseState>());
        for(int i = 0; i < tileTypes.Length; i++)
        {
            tileTypes[i].tileInfo.Index = i;
        }
        gridSizeText.text = "Size: [" + GridWidth + "," + GridHeight + "]";
    }
    public override void OnStateEnter()
    {
        placementInterface.SetActive(true);
    }

    public override void OnStateExit()
    {
        placementInterface.SetActive(false);
    }

    public override void OnStateUpdate()
    {
        //fsm.OnUpdate();

        Vector3 cursorPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int roundedCursorPoint = new Vector3Int(Mathf.RoundToInt(cursorPoint.x), Mathf.RoundToInt(cursorPoint.y), -1);

        EvaluateBrushSize();
        brush.transform.position = new(cursorPoint.x, cursorPoint.y, -1);

        if (activeTile != null)
        {
            activeTile.transform.position = roundedCursorPoint;
            if (Input.GetButton("Fire1"))
            {
                Vector2Int activeTilePosition = new(Mathf.RoundToInt(activeTile.transform.position.x), Mathf.RoundToInt(activeTile.transform.position.y));
                if (!mapBuilder.tiles.ContainsKey(activeTilePosition))
                {
                    return;
                }
                PlaceTile(activeTilePosition);
            }
        }
        if (Input.GetButton("Fire2"))
        {
            Vector2Int tilePosition = new Vector2Int(roundedCursorPoint.x, roundedCursorPoint.y);
            if (mapBuilder.tiles.ContainsKey(tilePosition))
            {
                if (mapBuilder.tiles[tilePosition] != null)
                {
                    Destroy(mapBuilder.tiles[tilePosition].gameObject);
                    mapBuilder.tiles[tilePosition] = null;
                }
            }
        }

    }
    private void EvaluateBrushSize()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            brushSize -= brushStepSize;
            brushSize = Mathf.Clamp(brushSize, 0.5f, 4);
            brush.transform.localScale = new Vector3(brushSize, brushSize, brushSize);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            brushSize += brushStepSize;
            brushSize = Mathf.Clamp(brushSize, 0.5f, 4);
            brush.transform.localScale = new Vector3(brushSize, brushSize, brushSize);
        }
    }
    public void ParseFields()
    {
        try
        {
            GridWidth = int.Parse(WidthField.text);
            GridHeight = int.Parse(HeightField.text);
        }
        catch (System.FormatException)
        {
            Debug.Log("Found empty inputfield! Cannot parse!");
        }
    }
    public void BuildGrid()
    {
        RemoveOldTiles();
        CreateNewTiles();

        formerWidth = GridWidth;
        formerHeight = GridHeight;
        gridSizeText.text = "Size: [" + GridWidth + "," + GridHeight + "]";
    }
    private void RemoveOldTiles()
    {
        for(int x = 0; x < formerWidth; x++)
        {
            for(int y = 0; y < formerHeight; y++)
            {
                if(x >= GridWidth || y >= GridHeight)
                {
                    Vector2Int p = new(x, y);
                    if (mapBuilder.tiles.ContainsKey(p))
                    {
                        if (mapBuilder.tiles[p] != null)
                        {
                            Destroy(mapBuilder.tiles[p].gameObject);
                        }
                        mapBuilder.tiles.Remove(p);
                    }
                    if (mapBuilder.utilityTiles.ContainsKey(p))
                    {
                        if(mapBuilder.utilityTiles[p] != null)
                        {
                            Destroy(mapBuilder.utilityTiles[p].gameObject);
                        }
                        mapBuilder.utilityTiles.Remove(p);
                    }
                }
            }
        }
        foreach (GameObject g in backgroundVisualizers)
        {
            Destroy(g);
        }
    }

    private void CreateNewTiles()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Vector2Int p = new(x, y);
                GameObject visualizer = Instantiate(visualizationSprite, new Vector3(x, y, 1), Quaternion.identity);
                backgroundVisualizers.Add(visualizer);
                if (!mapBuilder.tiles.ContainsKey(p))
                {
                    mapBuilder.tiles.Add(p, null);
                }
                if (!mapBuilder.utilityTiles.ContainsKey(p))
                {
                    mapBuilder.utilityTiles.Add(p, null);
                }
            }
        }
    }

    public void PlaceTile(Vector2Int tilePosition)
    {
        if (mapBuilder.tiles[tilePosition] != null)
        {
            if (mapBuilder.tiles[tilePosition].tileInfo.tileName != activeTile.tileInfo.tileName)
            {
                Destroy(mapBuilder.tiles[tilePosition].gameObject);
                Tile newTile = Instantiate(tileTypes[tileIndex], new(tilePosition.x, tilePosition.y, 0), Quaternion.identity);
                newTile.tileInfo.Position = tilePosition;
                mapBuilder.tiles[tilePosition] = newTile;
            }
        }
        else
        {
            Tile newTile = Instantiate(tileTypes[tileIndex], new(tilePosition.x, tilePosition.y, 0), Quaternion.identity);
            newTile.tileInfo.Position = tilePosition;
            mapBuilder.tiles[tilePosition] = newTile;
        }
    }

    public void SetIndex(int newIndex)
    {
        tileIndex = newIndex;
        if (activeTile != null)
        {
            Destroy(activeTile.gameObject);
        }
        activeTile = Instantiate(tileTypes[tileIndex]);

    }
}
