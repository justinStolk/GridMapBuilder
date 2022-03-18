using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarHandler : MonoBehaviour
{
    public GameObject[] toolbars;
    private int toolbarIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToolbar(int newToolbarIndex)
    {
        if(toolbarIndex != newToolbarIndex)
        {
            toolbars[toolbarIndex].SetActive(false);
            toolbarIndex = newToolbarIndex;
            toolbars[toolbarIndex].SetActive(true);
        }
    }
}
