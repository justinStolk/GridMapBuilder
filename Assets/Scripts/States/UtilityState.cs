using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityState : BaseState
{
    public UtilityTile[] tileTypes;

    [SerializeField] GameObject utilityInterface;

    private MapBuilder mapBuilder;
    private UtilityTile activeUtilityTile;
    private int tileIndex = -1;
    private void Awake()
    {
        mapBuilder = GetComponent<MapBuilder>();
        for (int i = 0; i < tileTypes.Length; i++)
        {
            tileTypes[i].GetComponent<UtilityTile>().utilityInfo.Index = i;
        }
    }
    public override void OnStateEnter()
    {
        utilityInterface.SetActive(true);
    }

    public override void OnStateExit()
    {
        utilityInterface.SetActive(false);
    }

    public override void OnStateUpdate()
    {
        Vector3 cursorPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int roundedCursorPoint = new Vector3Int(Mathf.RoundToInt(cursorPoint.x), Mathf.RoundToInt(cursorPoint.y), -1);
        if (activeUtilityTile != null)
        {
            activeUtilityTile.transform.position = roundedCursorPoint;
            if (Input.GetButton("Fire1"))
            {
                Vector2Int activeTilePosition = new(Mathf.RoundToInt(activeUtilityTile.transform.position.x), Mathf.RoundToInt(activeUtilityTile.transform.position.y));
                if (!mapBuilder.utilityTiles.ContainsKey(activeTilePosition))
                {
                    return;
                }
                PlaceTile(activeTilePosition);
            }
        }
        if (Input.GetButton("Fire2"))
        {
            if (activeUtilityTile != null)
            {
                Destroy(activeUtilityTile.gameObject);
                return;
            }
            Vector2Int tilePosition = new Vector2Int(roundedCursorPoint.x, roundedCursorPoint.y);
            if (mapBuilder.utilityTiles.ContainsKey(tilePosition))
            {
                if (mapBuilder.utilityTiles[tilePosition] != null)
                {
                    Destroy(mapBuilder.utilityTiles[tilePosition].gameObject);
                    mapBuilder.utilityTiles[tilePosition] = null;
                }
            }
        }
    }
    private void PlaceTile(Vector2Int tilePosition)
    {
        if (mapBuilder.utilityTiles[tilePosition] != null)
        {
            if (mapBuilder.utilityTiles[tilePosition].utilityInfo.utilityType != activeUtilityTile.utilityInfo.utilityType)
            {
                Destroy(mapBuilder.utilityTiles[tilePosition].gameObject);
                UtilityTile newTile = Instantiate(tileTypes[tileIndex], new(tilePosition.x, tilePosition.y, -0.5f), Quaternion.identity);
                newTile.utilityInfo.Position = tilePosition;
                mapBuilder.utilityTiles[tilePosition] = newTile;
            }
        }
        else
        {
            UtilityTile newTile = Instantiate(tileTypes[tileIndex], new(tilePosition.x, tilePosition.y, - 0.5f), Quaternion.identity);
            newTile.utilityInfo.Position = tilePosition;
            mapBuilder.utilityTiles[tilePosition] = newTile;
        }
    }
    public void SetIndex(int newIndex)
    {
        tileIndex = newIndex;
        if (activeUtilityTile != null)
        {
            Destroy(activeUtilityTile.gameObject);
        }
        activeUtilityTile = Instantiate(tileTypes[tileIndex]);

    }
}
