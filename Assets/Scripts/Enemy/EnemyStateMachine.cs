using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyRefs enemyRefs;

    BaseState activeState;

    [HideInInspector] public FindFlagState FindFlagState = new FindFlagState();
    [HideInInspector] public ReturnFlagState ReturnFlagState = new ReturnFlagState();
    [HideInInspector] public CombatState CombatState = new CombatState();
    [HideInInspector] public CombatWithFlagState CombatWithFlagState = new CombatWithFlagState();
    [HideInInspector] public PickUpFlagState PickUpFlagState = new PickUpFlagState();

    //defaults to pick up state if flag is dropped when spawned or find flag if the flag is in the player base
    void OnEnable()
    {
        if(enemyRefs.gameManager.enemyFlagDropped || enemyRefs.gameManager.playerFlagDropped)
        {
            activeState = PickUpFlagState;
        }
        else
        {
            activeState = FindFlagState;
        }
        activeState.EnterState(this);
        Debug.Log(activeState);
    }

    void Update()
    {
        activeState.UpdateState(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        activeState.OnTriggerEnter(this, other);
    }

    public void Transition(BaseState state)
    {
        activeState = state;
        state.EnterState(this);
        Debug.Log(activeState);
    }
}
