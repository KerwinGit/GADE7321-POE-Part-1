using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PickUpFlagState : BaseState
{
    private GameObject playerFlag;
    private GameObject enemyFlag;
    float searchCooldown = 0;
    private float strafeRadius = 10f;


    public override void EnterState(EnemyStateMachine enemy)
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
        if(!enemy.enemyRefs.gameManager.playerFlagRetrievable && !enemy.enemyRefs.gameManager.enemyFlagRetrievable)
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

        if (!enemy.enemyRefs.gameManager.playerFlagDropped && !enemy.enemyRefs.gameManager.enemyFlagDropped)
        {
            if (enemy.enemyRefs.carryingFlag)
            {
                enemy.Transition(enemy.ReturnFlagState);
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
    }

    //private void WaitToSearch(EnemyStateMachine enemy)
    //{
    //    searchCooldown += Time.deltaTime;

    //    if (searchCooldown >= 1f)
    //    {
    //        FindDroppedFlags(enemy);
    //    }
    //}

    private Vector3 SetRandomDestination(EnemyStateMachine enemy)
    {
        Vector3 randomPosition = Random.insideUnitSphere * strafeRadius; ;
        randomPosition += enemy.enemyRefs.transform.position;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPosition, out hit, strafeRadius, NavMesh.AllAreas);

        randomPosition = hit.position;

        return randomPosition;
    }
}
