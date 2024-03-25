using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFlagState : BaseState
{
    private GameObject playerFlag;
    private GameObject enemyFlag;

    public override void EnterState(EnemyStateMachine enemy)
    {
        if(enemy.enemyRefs.gameManager.playerFlagDropped)
        {
            playerFlag = GameObject.FindWithTag("Player Flag");
        }

        if (enemy.enemyRefs.gameManager.enemyFlagDropped)
        {
            playerFlag = GameObject.FindWithTag("Enemy Flag");
        }

        FindDroppedFlags(enemy);
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (enemy.enemyRefs.playerVisible)
        {
            if (enemy.enemyRefs.carryingFlag)
            {
                enemy.Transition(enemy.CombatWithFlagState);
            }
            else
            {
                enemy.Transition(enemy.CombatState);
            }
        }
    }

    public override void OnTriggerEnter(EnemyStateMachine enemy, Collider other)
    {
        return;
    }

    private void FindDroppedFlags(EnemyStateMachine enemy)
    {
        if(enemy.enemyRefs.gameManager.playerFlagRetrievable)
        {
            enemy.enemyRefs.agent.destination = playerFlag.transform.position;
        }
        else if(enemy.enemyRefs.gameManager.enemyFlagRetrievable)
        {
            enemy.enemyRefs.agent.destination = enemyFlag.transform.position;
        }
        else
        {
            if (!enemy.enemyRefs.gameManager.playerFlagDropped && !enemy.enemyRefs.gameManager.enemyFlagDropped)
            {
                
            }
            else
            {
                FindDroppedFlags(enemy);
            }
        }
    }
}
