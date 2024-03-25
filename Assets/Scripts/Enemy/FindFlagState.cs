using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFlagState : BaseState
{


    public override void EnterState(EnemyStateMachine enemy)
    {
        switch(enemy.enemyRefs.progressRef)
        {
            case EnemyRefs.progress.spawnArea:

                break;
            case EnemyRefs.progress.centreArea:

                break;
            case EnemyRefs.progress.flagArea:

                break;
        }
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (enemy.enemyRefs.canMove == true)
        {
            
        }
    }

    public override void OnTriggerEnter(EnemyStateMachine enemy, Collider other)
    {
        return;
    }

    //private GameObject chooseRedBarrier()
    //{
    //    return ;
    //}

    //private GameObject chooseBlueBarrier()
    //{

    //}
}
