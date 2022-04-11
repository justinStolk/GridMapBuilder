using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fennec.Utility;

public class Navigator : MonoBehaviour
{
    public AStar AStar;
    public string unitType;
    public List<Vector2Int> NodesInRange { get; private set; }
    public List<Vector2Int> NodesInAttackRange { get; private set; }

     private int moveSpeed = 5;
    [SerializeField] private int moveRange;

    private List<Vector2Int> targetPath = new();
    private List<Vector2Int> rangeTiles = new();
    private List<Vector2Int> attackRangeTiles = new();
    private Vector2Int storedPosition;
    private TestingState testingState;
    public void InitializeNavigator(int gridWidth, int gridHeight, bool canMoveDiagonally, TestingState testState)
    {
        AStar = new AStar(gridWidth, gridHeight, canMoveDiagonally, this);
        testingState = testState;
    }
    // Update is called once per frame
    void Update()
    {
        if (targetPath != null && targetPath.Count > 0)
        {
            if (transform.position != Vector2IntToVector3(targetPath[0]))
            {
                float targetAngle = Mathf.Atan2(transform.position.x - targetPath[0].x, transform.position.z - targetPath[0].y);
                targetAngle *= Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, targetAngle - 180, 0);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector2IntToVector3(targetPath[0]) - transform.position), 360f * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, Vector2IntToVector3(targetPath[0]), moveSpeed * Time.deltaTime);
            }
            else
            {
                targetPath.RemoveAt(0);
            }
        }
    }

    public void OnSelected()
    {
        storedPosition = Vector3ToVector2Int(transform.position);

        NodesInRange = AStar.GetNodesInRange(Vector3ToVector2Int(transform.position), moveRange);
        foreach (Vector2Int v in NodesInRange)
        {
            testingState.moveTiles[v].SetActive(true);
        }


    }

    public void OnMovementUndo()
    {
        transform.position = Vector2IntToVector3(storedPosition);
        OnDeselect();
    }

    public void OnDeselect()
    {
        foreach (Vector2Int moveTilePosition in rangeTiles)
        {
            testingState.moveTiles[moveTilePosition].SetActive(false);
        }
        rangeTiles.Clear();
    }

    public void MoveToTile(Vector2Int target)
    {
        targetPath = AStar.FindPathToTarget(Vector3ToVector2Int(transform.position), target);
        List<Vector2Int> myNode = new();
        myNode.Add(target);
    }

    public Vector2Int Vector3ToVector2Int(Vector3 vector3)
    {
        return new Vector2Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.z));
    }
    public Vector3 Vector2IntToVector3(Vector2Int vector2Int)
    {
        return new Vector3(vector2Int.x, 0, vector2Int.y);
    }



}
