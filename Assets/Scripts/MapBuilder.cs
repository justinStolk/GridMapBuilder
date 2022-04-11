using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{
    public FSM fsm;
    public static MapBuilder instance;

    public Dictionary<Vector2Int, Tile> tiles = new();
    public Dictionary<Vector2Int, UtilityTile> utilityTiles = new();


    private string fileName;


    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        fsm = new FSM(typeof(PlacementState), GetComponents<BaseState>());
        //UnitTerrainMatrix.AddMatrixItem("Knight", "Forest", 3);
        //UnitTerrainMatrix.AddMatrixItem("InfantryA", "Forest", 2);
        //UnitTerrainMatrix.AddMatrixItem("Knight", "Forest", 4);
        //Debug.Log(UnitTerrainMatrix.GetMatrixItem("Knight", "Forest"));
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
    public void GoToPreparationState()
    {
        fsm.ChangeState(typeof(PreparationState));
    }
    public void GoToTestState()
    {
        fsm.ChangeState(typeof(TestingState));
    }

    public void ExitApplication()
    {
        if (!Application.isEditor)
        {
            Application.Quit();
        }
    }

}
