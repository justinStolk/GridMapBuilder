using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationState : BaseState
{
    public LayerMask tileMask;
    [SerializeField] private GameObject preparationInterface;
    [SerializeField] private GameObject[] availableUnits;
    [SerializeField] private GameObject messageBox;
    [SerializeField] private Text messageText;
    private GameObject unitToPlace = null;


    public override void OnStateEnter()
    {
        preparationInterface.SetActive(true);   
    }

    public override void OnStateExit()
    {
        preparationInterface.SetActive(false);
    }

    public override void OnStateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Pressed mouse");
            if (unitToPlace != null)
            {
                Debug.Log("Should shoot ray");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log(hit.transform.name);
                    Tile hitTile = hit.transform.GetComponent<Tile>();
                    if (MapBuilder.instance.utilityTiles[hitTile.tileInfo.Position] != null)
                    {
                        Instantiate(unitToPlace, new(hitTile.tileInfo.Position.x, hitTile.tileInfo.Position.y, 0), Quaternion.identity);
                        messageBox.SetActive(false);
                    }
                }
                return;
            }

        }

    }
    public void SelectNavigatorPlacement(int index)
    {
        unitToPlace = availableUnits[index];
        messageText.text = "Placing: " + availableUnits[index].name;
        messageBox.SetActive(true);
    }

}
