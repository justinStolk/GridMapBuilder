using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillMode : BaseState
{
    private PlacementState placementState;
    private MapBuilder mapBuilder;
    private void Awake()
    {
        mapBuilder = GetComponent<MapBuilder>();
        placementState = GetComponent<PlacementState>();
    }
    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        Vector3 cursorPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int roundedCursorPoint = new Vector3Int(Mathf.RoundToInt(cursorPoint.x), Mathf.RoundToInt(cursorPoint.y), -1);
        if (placementState.activeTile != null)
        {
            placementState.activeTile.transform.position = roundedCursorPoint;
            if (Input.GetButton("Fire1"))
            {
                List<Vector2Int> open = new();
                List<Vector2Int> result = new();

                Vector2Int activeTilePosition = new(Mathf.RoundToInt(placementState.activeTile.transform.position.x), Mathf.RoundToInt(placementState.activeTile.transform.position.y));
                open.Add(activeTilePosition);
                while(open.Count > 0)
                {


                }
                if (mapBuilder.tiles[activeTilePosition] == null)
                {
                    List<Vector2Int> neighbours = GetNeighbours(open[0]);
                    foreach(Vector2Int neighbour in neighbours)
                    {
                        open.Add(neighbour);
                        if(mapBuilder.tiles[neighbour] == null)
                        {
                            result.Add(neighbour);
                        }
                    }
                }
                
                if(mapBuilder.tiles[activeTilePosition].tileInfo.tileName != placementState.activeTile.tileInfo.tileName)
                {

                }
                //if (!mapBuilder.tiles.ContainsKey(activeTilePosition))
                //{
                //    return;
                //}
                //placementState.PlaceTile(activeTilePosition);
            }
        }
    }
    private List<Vector2Int> GetNeighbours(Vector2Int startPosition)
    {
        List<Vector2Int> neighbours = new();

        return neighbours;
    }

}
