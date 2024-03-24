using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFlag : BaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        return;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        return;
    }

    public override void OnTriggerEnter(EnemyStateManager enemy, Collider other)
    {
        return;
    }
}
