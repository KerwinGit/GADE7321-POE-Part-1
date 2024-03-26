using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFlagState : BaseState    //this state is used by the AI to pathfind back to its goal, while it is carrying the flag
{
    public override void EnterState(EnemyStateMachine enemy)
    {
        ChoosePath(enemy);
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (enemy.enemyRefs.playerVisible)                          //transition to combat with flag if player visible
        {
            enemy.Transition(enemy.CombatWithFlagState);
        }

        if(enemy.enemyRefs.gameManager.enemyFlagDropped)            //transition to combat if player hits the enemy and they drop their flag
        {
            enemy.Transition(enemy.CombatState);
        }

        if (enemy.enemyRefs.gameManager.playerFlagDropped)          //transition to pick up if the player flag is dropped (like if they die to trap)
        {
            enemy.Transition(enemy.PickUpFlagState);
        }
    }

    public override void OnTriggerEnter(EnemyStateMachine enemy, Collider other)    //chooses next waypoint to set agent destination to
    {
        if (other.CompareTag("Red Barrier") || other.CompareTag("Blue Barrier"))
        {
            ChoosePath(enemy);
        }
    }

    private void ChoosePath(EnemyStateMachine enemy)                                //uses enemy progress ref enum to choose next destination
    {
        switch (enemy.enemyRefs.progressRef)
        {
            case EnemyRefs.progress.spawnArea:
                enemy.enemyRefs.agent.destination = enemy.enemyRefs.gameManager.enemyGoal.transform.position;       // chooses goal if enemy is in their third of the map
                break;
            case EnemyRefs.progress.centreArea:
                enemy.enemyRefs.agent.destination = chooseRedBarrier(enemy.enemyRefs).transform.position;           // chooses red barrier if they are in the middle third
                break;
            case EnemyRefs.progress.flagArea:
                enemy.enemyRefs.agent.destination = chooseBlueBarrier(enemy.enemyRefs).transform.position;          // chooses blue barrier if they are in the player's third
                break;
        }
    }

    private GameObject chooseRedBarrier(EnemyRefs enemy)                            //randomly chooses a red barrier as agent destination
    {
        int seed = (int)System.DateTime.Now.Ticks;
        System.Random random = new System.Random(seed);

        return enemy.redBarriers[random.Next(0, 3)];
    }

    private GameObject chooseBlueBarrier(EnemyRefs enemy)                           //randomly chooses a blue barrier as agent destination
    {
        int seed = (int)System.DateTime.Now.Ticks;
        System.Random random = new System.Random(seed);

        return enemy.blueBarriers[random.Next(0, 3)];
    }
}
