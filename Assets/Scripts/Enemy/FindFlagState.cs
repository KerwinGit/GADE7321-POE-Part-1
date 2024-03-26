using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFlagState : BaseState  //this state is used by the AI to pathfind towards the flag that is in the player base
{
    public override void EnterState(EnemyStateMachine enemy)
    {   
        ChoosePath(enemy);
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        if(enemy.enemyRefs.playerVisible)
        {
            enemy.Transition(enemy.CombatState);
        }

        if(enemy.enemyRefs.gameManager.playerFlagDropped)
        {
            enemy.Transition(enemy.PickUpFlagState);
        }
    }

    public override void OnTriggerEnter(EnemyStateMachine enemy, Collider other) //chooses next waypoint to set agent destination to
    {
        if (other.CompareTag("Red Barrier")|| other.CompareTag("Blue Barrier"))
        {
            ChoosePath(enemy);
            return;
        }

        if(other.CompareTag("Enemy Flag"))
        {
            enemy.Transition(enemy.ReturnFlagState);
        }
    }

    private void ChoosePath(EnemyStateMachine enemy) //uses enemy progress ref enum to choose next destination
    {
        switch (enemy.enemyRefs.progressRef)
        {
            case EnemyRefs.progress.spawnArea:
                enemy.enemyRefs.agent.destination = chooseRedBarrier(enemy.enemyRefs).transform.position;
                break;
            case EnemyRefs.progress.centreArea:
                enemy.enemyRefs.agent.destination = chooseBlueBarrier(enemy.enemyRefs).transform.position;
                break;
            case EnemyRefs.progress.flagArea:
                enemy.enemyRefs.agent.destination = enemy.enemyRefs.gameManager.enemyFlag.transform.position;
                break;
        }
    }

    private GameObject chooseRedBarrier(EnemyRefs enemy) //randomly chooses a red barrier as agent destination
    {
        int seed = (int)System.DateTime.Now.Ticks;
        System.Random random = new System.Random(seed);

        return enemy.redBarriers[random.Next(0, 3)];
    }

    private GameObject chooseBlueBarrier(EnemyRefs enemy) //randomly chooses a blue barrier as agent destination
    {
        int seed = (int)System.DateTime.Now.Ticks;
        System.Random random = new System.Random(seed);

        return enemy.blueBarriers[random.Next(0, 3)];
    }
}