using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PickUpFlagState : BaseState    // this state is used by the AI when flags are dropped on the ground
{
    #region fields
    private GameObject playerFlag;
    private GameObject enemyFlag;
    private float strafeRadius = 10f;
    #endregion

    public override void EnterState(EnemyStateMachine enemy)    //gets references to the dropped flags in the scene
    {
        if(enemy.enemyRefs.gameManager.playerFlagDropped)
        {
            playerFlag = GameObject.FindWithTag("Player Flag");
        }

        if (enemy.enemyRefs.gameManager.enemyFlagDropped)
        {
            enemyFlag = GameObject.FindWithTag("Enemy Flag");
        }
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        if(!enemy.enemyRefs.gameManager.playerFlagRetrievable && !enemy.enemyRefs.gameManager.enemyFlagRetrievable)     //if the flags are not yet retriavable, roam around a bit
        {
            if (enemy.enemyRefs.agent.remainingDistance < 0.1f)
            {
                enemy.enemyRefs.agent.SetDestination(SetRandomDestination(enemy));
            }
        }
        else
        {
            FindDroppedFlags(enemy);
        }

        if (enemy.enemyRefs.playerVisible)                                                                              //transition to combat states if the player becomes visible
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

        if (!enemy.enemyRefs.gameManager.playerFlagDropped && !enemy.enemyRefs.gameManager.enemyFlagDropped)
        {
            if (enemy.enemyRefs.carryingFlag)
            {
                enemy.Transition(enemy.ReturnFlagState);                                                        //transition to return state if carrying and flags have been picked up
            }
            else
            {
                enemy.Transition(enemy.FindFlagState);                                                          //transition to find state if not carrying " " " "
            }
        }
    }

    public override void OnTriggerEnter(EnemyStateMachine enemy, Collider other) //unused in this state
    {
        
    }

    private void FindDroppedFlags(EnemyStateMachine enemy)  //sets destination of agent to the flags
    {
        if(enemy.enemyRefs.gameManager.playerFlagRetrievable)
        {
            enemy.enemyRefs.agent.destination = playerFlag.transform.position;
        }
        else if(enemy.enemyRefs.gameManager.enemyFlagRetrievable)
        {
            enemy.enemyRefs.agent.destination = enemyFlag.transform.position;
        }
    }

    private Vector3 SetRandomDestination(EnemyStateMachine enemy)   //helper to set random destination for agent to move to
    {
        Vector3 randomPosition = Random.insideUnitSphere * strafeRadius; ;
        randomPosition += enemy.enemyRefs.transform.position;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPosition, out hit, strafeRadius, NavMesh.AllAreas);

        randomPosition = hit.position;

        return randomPosition;
    }
}
