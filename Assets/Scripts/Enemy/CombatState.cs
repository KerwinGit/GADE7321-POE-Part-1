using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : BaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        return;
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (!enemy.enemyRefs.playerVisible)
        {
            if(enemy.enemyRefs.gameManager.enemyFlagDropped || enemy.enemyRefs.gameManager.playerFlagDropped)
            {
                enemy.Transition(enemy.PickUpFlagState);
            }
            else
            {
                enemy.Transition(enemy.FindFlagState);
            }
        }
    }

    public override void OnTriggerEnter(EnemyStateMachine enemy, Collider other)
    {
        return;
    }
}
