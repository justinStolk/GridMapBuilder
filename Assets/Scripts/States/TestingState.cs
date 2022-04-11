using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fennec.Utility;

public class TestingState : BaseState
{
    public LayerMask tileLayer;
    public Dictionary<Vector2Int, GameObject> moveTiles;
    public Dictionary<Vector2Int, Node> nodes { get; private set; }
    [SerializeField] private GameObject testingInterface;
    [SerializeField] private GameObject moveTilePrefab;
    private Navigator activeNavigator = null;
    private Navigator[] navigators;
    private GameObject tileParent;

    private void Awake()
    {
        nodes = new();
    }
    public override void OnStateEnter()
    {
        testingInterface.SetActive(true);
        navigators = FindObjectsOfType<Navigator>();
        PlacementState p = GetComponent<PlacementState>();
        tileParent = Instantiate(new GameObject("TileParent"));
        for(int x = 0; x < p.GridWidth; x++)
        {
            for(int y = 0; y < p.GridHeight; y++)
            {
                nodes.Add(new(x, y), new Node(new Vector2Int(x, y), 0, 0));
                GameObject newMoveTile = Instantiate(moveTilePrefab, tileParent.transform);
                moveTiles.Add(new Vector2Int(x, y), newMoveTile);
                newMoveTile.SetActive(false);
            }
        }
    }
    public override void OnStateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if(activeNavigator != null)
            {
                MoveNavigator();
                return;
            }
            TrySelectingNavigator();

        }
    }

    public override void OnStateExit()
    {
        testingInterface.SetActive(false);
        Destroy(tileParent);
        nodes.Clear();
        moveTiles.Clear();
    }
    private void MoveNavigator()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, tileLayer))
        {
            Tile tile = hit.transform.GetComponent<Tile>();
            activeNavigator.MoveToTile(tile.tileInfo.Position);
            activeNavigator.OnDeselect();
            activeNavigator = null;
        }
    }
    private void TrySelectingNavigator()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, tileLayer))
        {
            Tile tile = hit.transform.GetComponent<Tile>();
            Navigator navOnTile = nodes[tile.tileInfo.Position].navigator;
            if(navOnTile != null)
            {
                navOnTile.OnSelected();
                activeNavigator = navOnTile;
            }
            //Debug.Log(hit.transform.name);
        }
    }

}
