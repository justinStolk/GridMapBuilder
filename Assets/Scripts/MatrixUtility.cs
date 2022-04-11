using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MatrixUtility : MonoBehaviour
{

    [SerializeField] private InputField typeField;
    [SerializeField] private Button matrixButton;
    [SerializeField] private MatrixField matrixField;
    [SerializeField] private GameObject matrixInterface;
    private List<HorizontalLayoutGroup> rowObjects = new();

    // Start is called before the first frame update
    void Awake()
    {
        HorizontalLayoutGroup[] children = matrixInterface.GetComponentsInChildren<HorizontalLayoutGroup>();
        foreach(HorizontalLayoutGroup child in children)
        {
            rowObjects.Add(child);
        }
    }

    public void CreateNewTerrainType()
    {
        if(typeField.text == null)
        {
            return;
        }
        string newType = typeField.text;
        Button newButton = Instantiate(matrixButton, rowObjects[0].transform);
        newButton.GetComponentInChildren<Text>().text = newType;
        //newButton.onClick.AddListener(Remove);
        for (int i = 1; i < rowObjects.Count; i++)
        {
            MatrixField newField = Instantiate(matrixField, rowObjects[i].transform);
            Button rowButton = rowObjects[i].GetComponentInChildren<Button>();
            newField.Initialize(rowObjects[i].GetComponentInChildren<Button>().GetComponentInChildren<Text>().text, newType);
            Debug.Log(newField);
        }
    }
    public void CreateNewTerrainType(string newTerrainType)
    {
        Button newButton = Instantiate(matrixButton, rowObjects[0].transform);
        newButton.GetComponentInChildren<Text>().text = newTerrainType;
        //newButton.onClick.AddListener(Remove);
        for (int i = 1; i < rowObjects.Count; i++)
        {
            MatrixField newField = Instantiate(matrixField, rowObjects[i].transform);
            Button rowButton = rowObjects[i].GetComponentInChildren<Button>();
            newField.Initialize(rowObjects[i].GetComponentInChildren<Button>().GetComponentInChildren<Text>().text, newTerrainType);
        }
    }
    public void CreateNewUnitType()
    {
        if (typeField.text == null)
        {
            return;
        }
        string newType = typeField.text;
        GameObject newRow = Instantiate(new GameObject("Standard Row", typeof(RectTransform), typeof(HorizontalLayoutGroup)), matrixInterface.transform);
        HorizontalLayoutGroup newGroup = newRow.GetComponent<HorizontalLayoutGroup>();
        newGroup.childControlWidth = false;
        newGroup.childControlHeight = false;
        Button newButton = Instantiate(matrixButton, newRow.transform);
        rowObjects.Add(newGroup);
        newButton.GetComponentInChildren<Text>().text = newType;
        //Debug.Log(rowObjects[0].GetComponentsInChildren<Button>().Length);
        for (int i = 0; i < rowObjects[0].GetComponentsInChildren<Button>().Length; i++)
        {
            MatrixField newField = Instantiate(matrixField, newRow.transform);
            newField.Initialize(newType, rowObjects[0].GetComponentsInChildren<Button>()[i].GetComponentInChildren<Text>().text);
        }
    }
    public void CreateNewUnitType(string newUnitType)
    {
        GameObject newRow = Instantiate(new GameObject("Standard Row", typeof(RectTransform), typeof(HorizontalLayoutGroup)), matrixInterface.transform);
        HorizontalLayoutGroup newGroup = newRow.GetComponent<HorizontalLayoutGroup>();
        newGroup.childControlWidth = false;
        newGroup.childControlHeight = false;
        Button newButton = Instantiate(matrixButton, newRow.transform);
        rowObjects.Add(newGroup);
        newButton.GetComponentInChildren<Text>().text = newUnitType;
        //Debug.Log(rowObjects[0].GetComponentsInChildren<Button>().Length);
        for (int i = 0; i < rowObjects[0].GetComponentsInChildren<Button>().Length; i++)
        {
            MatrixField newField = Instantiate(matrixField, newRow.transform);
            newField.Initialize(newUnitType, rowObjects[0].GetComponentsInChildren<Button>()[i].GetComponentInChildren<Text>().text);
        }
    }

    public void EvaluateMatrixFields()
    {
        for(int i = 1; i < rowObjects.Count; i++)
        {
            MatrixField[] fields = rowObjects[i].GetComponentsInChildren<MatrixField>();
            foreach(MatrixField field in fields)
            {
                field.FillField(UnitTerrainMatrix.GetMatrixItem(field.UnitType, field.TerrainType));
            }
        }
    }

    public void ToggleMatrixInterface()
    {
        matrixInterface.SetActive(!matrixInterface.activeSelf);
    }

    private void Remove()
    {
       
    }
}
