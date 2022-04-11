using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMode : BaseState
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
                Vector2Int activeTilePosition = new(Mathf.RoundToInt(placementState.activeTile.transform.position.x), Mathf.RoundToInt(placementState.activeTile.transform.position.y));
                if (!mapBuilder.tiles.ContainsKey(activeTilePosition))
                {
                    return;
                }
                placementState.PlaceTile(activeTilePosition);
            }
        }
    }

}
