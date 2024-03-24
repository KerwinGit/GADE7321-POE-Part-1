using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public UnityEvent playerGoalEvent;
    public UnityEvent enemyGoalEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(this.CompareTag("Player Goal") && other.CompareTag("Player"))
        {
            playerGoalEvent.Invoke();
        }

        if (this.CompareTag("Enemy Goal") && other.CompareTag("Enemy"))
        {
            enemyGoalEvent.Invoke();
        }
    }
}
