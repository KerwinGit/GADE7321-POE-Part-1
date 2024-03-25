using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameManager Manager;
    public PlayerStats playerStats;
    public EnemyRefs enemyRefs;

    public bool trapped = true;

    private void OnTriggerEnter(Collider other)
    {
        if(this.CompareTag("Red Barrier") && other.CompareTag("Player") && trapped == true)
        {
            this.gameObject.SetActive(false);
        }

        if(this.CompareTag("Blue Barrier") && other.CompareTag("Enemy") && trapped == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
