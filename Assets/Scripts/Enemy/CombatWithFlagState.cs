using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatWithFlagState : BaseState
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        switch (enemy.enemyRefs.progressRef)
        {
            case EnemyRefs.progress.spawnArea:
                enemy.enemyRefs.agent.destination = enemy.enemyRefs.gameManager.enemyGoal.transform.position;
                break;
            case EnemyRefs.progress.centreArea:
                if(enemy.enemyRefs.gameManager.player.carryingFlag)
                {

                }
                else
                {

                }
                break;
            case EnemyRefs.progress.flagArea:
                if (enemy.enemyRefs.gameManager.player.carryingFlag)
                {

                }
                else
                {

                }
                break;
        }
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {


        if (!enemy.enemyRefs.playerVisible)
        {
            if (enemy.enemyRefs.gameManager.enemyFlagDropped || enemy.enemyRefs.gameManager.playerFlagDropped)
            {
                enemy.Transition(enemy.PickUpFlagState);
            }
            else
            {
                enemy.Transition(enemy.ReturnFlagState);
            }
        }
    }

    public override void OnTriggerEnter(EnemyStateMachine enemy, Collider other)
    {
        return;
    }
}
