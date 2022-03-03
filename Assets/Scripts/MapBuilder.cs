using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{

    public int GridWidth, GridHeight;
    public InputField WidthField, HeightField;
    public FSM fsm;
    public Dictionary<Vector2Int, GameObject> tiles = new();

    [SerializeField] GameObject visualizationSprite;
    private List<GameObject> backgroundVisualizers = new();
    private int formerWidth, formerHeight;


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


    public void BuildGrid()
    {
        GridWidth = int.Parse(WidthField.text);
        GridHeight = int.Parse(HeightField.text);
        foreach (GameObject g in backgroundVisualizers)
        {
            Destroy(g);
        }
        if (formerHeight > GridHeight || formerWidth > GridWidth)
        {
            tiles.Clear();
        }
        for(int x = 0; x < GridWidth; x++)
        {
            for(int y = 0; y < GridHeight; y++)
            {
                Vector2Int position = new(x, y);
                GameObject visualizer = Instantiate(visualizationSprite, new Vector3(x, y, 1), Quaternion.identity);
                backgroundVisualizers.Add(visualizer);
                if (!tiles.ContainsKey(position))
                {
                    Debug.Log("Added new position at: " + position);
                    tiles.Add(position, null);
                }
            }
        }
        formerWidth = GridWidth;
        formerHeight = GridHeight;
    }

    //private void RemoveOutOfBoundTiles()
    //{
    //    for(int x = formerWidth - 1; x >= GridWidth; x--)
    //    {
    //       for(int y = formerHeight - 1; y >= GridHeight; y--)
    //        {
    //            Vector2Int position = new(x, y);
    //            if (tiles.ContainsKey(position))
    //            {
    //                Debug.Log("Removed position at: " + position);
    //                tiles.Remove(position);
    //            }
    //        }
    //    }
    //}

}
