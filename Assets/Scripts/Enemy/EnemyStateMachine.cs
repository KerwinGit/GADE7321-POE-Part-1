using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{

    BaseState activeState;

    public FindFlagState FindFlagState = new FindFlagState();
    public ReturnFlagState ReturnFlagState = new ReturnFlagState();
    public CombatState CombatState = new CombatState();
    public CombatWithFlagState CombatWithFlagState = new CombatWithFlagState();
    public PickUpFlagState PickUpFlagState = new PickUpFlagState();

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
