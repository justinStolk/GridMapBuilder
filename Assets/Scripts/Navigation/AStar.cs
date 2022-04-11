using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Fennec.Utility
{

    public class AStar
    {
        private int gridsizeX, gridsizeY;
        private bool diagonalMovement;
        private Dictionary<Vector2Int, Node> nodes = new();
        private Navigator navigator;
        public AStar(int sizeX, int sizeY, bool canMoveDiagonally, Navigator owner)
        {
            gridsizeX = sizeX;
            gridsizeY = sizeY;
            diagonalMovement = canMoveDiagonally;
            navigator = owner;
            BuildNodes();
        }

        private void BuildNodes()
        {
            for (int x = 0; x <= gridsizeX; x++)
            {
                for (int y = 0; y <= gridsizeY; y++)
                {
                    nodes.Add(new Vector2Int(x, y), new Node(new Vector2Int(x, y), 0, 0));
                }
            }
        }
        public List<Vector2Int> GetNodesInRange(Vector2Int startPosition, int range)
        {
            List<Node> closed = new();
            List<Node> open = new();
            Node start = nodes[startPosition];
            start.GCost = 0;
            open.Add(start);
            while (open.Count > 0)
            {
                Node current = open[0];
                open.Remove(current);
                closed.Add(current);
                List<Node> neighbours = GetNeighbours(current, gridsizeX, gridsizeY);
                foreach (Node neighbour in neighbours)
                {
                    if (closed.Contains(neighbour) || !IsTraversable(neighbour))
                    {
                        continue;
                    }
                    float tentativeGScore = current.GCost + GetDistance(current, neighbour) * CostModifier(navigator.unitType, MapBuilder.instance.tiles[neighbour.Position].tileInfo.TerrainType);
                    if (tentativeGScore < neighbour.GCost || !open.Contains(neighbour))
                    {
                        neighbour.GCost = tentativeGScore;
                        if (neighbour.GCost > range * 10 || !nodes.ContainsKey(neighbour.Position))
                        {
                            continue;
                        }
                        if (!open.Contains(neighbour) && IsTraversable(neighbour))
                        {
                            open.Add(neighbour);
                        }
                    }
                }
            }
            List<Vector2Int> result = new();
            foreach (Node n in closed)
            {
                result.Add(n.Position);
            }
            return result;
        }

        public List<Vector2Int> GetNodesInAttackRange(List<Vector2Int> nodesInMoveRange, int minDistance, int maxDistance)
        {
            List<Vector2Int> undefinedAttackNodes = new();
            for (int x = -minDistance; x <= maxDistance; x++)
            {
                for (int y = -minDistance; y <= maxDistance; y++)
                {
                    foreach (Vector2Int n in nodesInMoveRange)
                    {
                        Vector2Int nodeInAttackRange = new Vector2Int(n.x + x, n.y + y);
                        if (nodesInMoveRange.Contains(nodeInAttackRange) || undefinedAttackNodes.Contains(nodeInAttackRange) || nodeInAttackRange.x < 0 || nodeInAttackRange.x >= gridsizeX || nodeInAttackRange.y < 0 || nodeInAttackRange.y >= gridsizeY)
                        {
                            continue;
                        }
                        if ((Mathf.Abs(x) + Mathf.Abs(y)) >= minDistance && (Mathf.Abs(x) + Mathf.Abs(y)) <= maxDistance)
                        {
                            undefinedAttackNodes.Add(nodeInAttackRange);
                        }

                    }
                }
            }
            return undefinedAttackNodes;
        }
        public List<Vector2Int> FindPathToTarget(Vector2Int startPosition, Vector2Int targetPosition)
        {
            List<Node> openNodes = new();
            List<Node> closedNodes = new();
            Node startNode = nodes[startPosition];
            Node endNode = nodes[targetPosition];
            startNode.HCost = Mathf.Abs(startPosition.x - targetPosition.x) + Mathf.Abs(startPosition.y - targetPosition.y);
            startNode.GCost = 0;
            openNodes.Add(startNode);
            while (openNodes.Count > 0)
            {
                Node currentNode = openNodes.OrderBy((x) => x.FCost).First();
                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);
                if (currentNode.Position == targetPosition)
                {
                    List<Vector2Int> path = new();
                    while (currentNode.Position != startNode.Position)
                    {
                        path.Add(currentNode.Position);
                        currentNode = currentNode.Parent;
                    }
                    path.Reverse();
                    return path;
                }
                List<Node> neighbours = GetNeighbours(currentNode, gridsizeX, gridsizeY);
                foreach (Node neighbour in neighbours)
                {
                    if (closedNodes.Contains(neighbour) || !IsTraversable(neighbour))
                    {
                        continue;
                    }

                    float tentativeGScore = currentNode.GCost + GetDistance(currentNode, neighbour) * CostModifier(navigator.unitType, MapBuilder.instance.tiles[neighbour.Position].tileInfo.TerrainType);
                    if (tentativeGScore < neighbour.GCost || !openNodes.Contains(neighbour))
                    {
                        neighbour.GCost = tentativeGScore;
                        neighbour.HCost = GetDistance(neighbour, endNode);
                        neighbour.Parent = currentNode;
                        if (!openNodes.Contains(neighbour) && IsTraversable(neighbour))
                        {
                            openNodes.Add(neighbour);
                        }
                    }

                }
            }

            Debug.Log("Did not find path");
            return null;
        }
        private int CostModifier(string unitType, string terrainType)
        {
            int modifierValue = UnitTerrainMatrix.GetMatrixItem(unitType, terrainType);
            if(modifierValue == 0)
            {
                return int.MaxValue;
            }
            return modifierValue;
        }
        private bool IsTraversable(Node nodeToCheck)
        {
            if(!nodeToCheck.Traversable || nodeToCheck.Occupied)
            {
                return false;
            }
            return true;
        }
        private List<Node> GetNeighbours(Node node, int gridWidth, int gridHeight)
        {
            List<Node> result = new();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    int nodeX = node.Position.x + x;
                    int nodeY = node.Position.y + y;
                    if (!diagonalMovement && Mathf.Abs(x) == Mathf.Abs(y))
                    {
                        continue;
                    }
                    if (nodeX < 0 || nodeX >= gridWidth || nodeY < 0 || nodeY >= gridHeight || x == 0 && y == 0)
                    {
                        continue;
                    }
                    Node neighbour = nodes[new Vector2Int(nodeX, nodeY)];
                    result.Add(neighbour);
                }
            }
            return result;
        }
        private int GetDistance(Node nodeA, Node nodeB)
        {
            if (!diagonalMovement)
            {
                int distanceX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
                int distanceY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

                return 10 * distanceX + 10 * distanceY;
            }
            else
            {
                int distanceX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
                int distanceY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);
                if (distanceX > distanceY)
                {
                    return 14 * distanceY + 10 * (distanceX - distanceY);
                }
                else
                {
                    return 14 * distanceX + 10 * (distanceY - distanceX);
                }
            }
        }
    }

    public class Node
    {
            public Vector2Int Position;
            public Node Parent;
            public float HCost;
            public float GCost;
            public float FCost { get { return HCost + GCost; } }
            public bool Traversable = true;
            public bool Occupied = false;
            public Navigator navigator = null;

            public Node(Vector2Int _position, int hCost, int gCost)
            {
                Position = _position;
                HCost = hCost;
                GCost = gCost;
            }
    }


}

