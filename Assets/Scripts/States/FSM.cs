using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private BaseState currentState;
    private Dictionary<System.Type, BaseState> states = new();
    public FSM(System.Type startingState, params BaseState[] allStates)
    {
        foreach(BaseState state in allStates)
        {
            state.Initialize(this);
            states.Add(state.GetType(), state);
        }
        ChangeState(startingState);
    }

    public void ChangeState(System.Type newState)
    {
        currentState?.OnStateExit();
        currentState = states[newState];
        currentState?.OnStateEnter();
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        currentState?.OnStateUpdate();
    }
}
