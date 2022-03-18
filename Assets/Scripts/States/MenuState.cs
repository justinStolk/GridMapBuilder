using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : BaseState
{
    [SerializeField] private GameObject menuInterface;
    [SerializeField] private CameraPanner panner;
    public override void OnStateEnter()
    {
        panner.panCamera = false;
        menuInterface.SetActive(true);
    }

    public override void OnStateExit()
    {
        panner.panCamera = true;
        menuInterface.SetActive(false);
    }

    public override void OnStateUpdate()
    {
        
    }

}
