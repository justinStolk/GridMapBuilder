using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementState : BaseState
{

    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject tileSelector;

    private int tileIndex;
    private MapBuilder mapBuilder;
    private GameObject activeTile;
    private Button[] tileButtons;

    private void Awake()
    {
        mapBuilder = FindObjectOfType<MapBuilder>();
        tileButtons = tileSelector.GetComponentsInChildren<Button>();
        for(int i = 0; i < tilePrefabs.Length; i++)
        {
            tilePrefabs[i].GetComponent<TileInfo>().Index = i;
        }
    }
    public override void OnStateEnter()
    {
        foreach(Button b in tileButtons)
        {
            b.interactable = true;
        }
    }

    public override void OnStateExit()
    {
        foreach (Button b in tileButtons)
        {
            b.interactable = false;
        }
    }

    public override void OnStateUpdate()
    {
        Vector3 cursorPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int roundedCursorPoint = new Vector3Int(Mathf.RoundToInt(cursorPoint.x), Mathf.RoundToInt(cursorPoint.y), -1);
        if (activeTile != null)
        {
            activeTile.transform.position = roundedCursorPoint;
        }
        if (Input.GetButton("Fire1") && activeTile != null)
        {
            Vector2Int activeTilePosition = new(Mathf.RoundToInt(activeTile.transform.position.x), Mathf.RoundToInt(activeTile.transform.position.y));
            if (!mapBuilder.tiles.ContainsKey(activeTilePosition))
            {
                return;
            }
            GameObject newTile = Instantiate(tilePrefabs[tileIndex], new(activeTilePosition.x, activeTilePosition.y, 0), Quaternion.identity);
            if (mapBuilder.tiles[activeTilePosition] != null)
            {
                Destroy(mapBuilder.tiles[activeTilePosition]);
            }
            mapBuilder.tiles[activeTilePosition] = newTile;

            //mapBuilder.tiles[activeTilePosition] = activeTile;
            //activeTile = null;
        }
        if(Input.GetButton("Fire2"))
        {
            if(activeTile != null)
            {
                Destroy(activeTile);
                return;
            }
            Vector2Int tilePosition = new Vector2Int(roundedCursorPoint.x, roundedCursorPoint.y);
            if (mapBuilder.tiles[tilePosition] != null)
            {
                Destroy(mapBuilder.tiles[tilePosition]);
                mapBuilder.tiles[tilePosition] = null;
            }
        }

    }
    public void SetIndex(int newIndex)
    {
        
        tileIndex = newIndex;
        if (activeTile != null)
        {
            Destroy(activeTile);
        }
        activeTile = Instantiate(tilePrefabs[tileIndex]);

    }
}
