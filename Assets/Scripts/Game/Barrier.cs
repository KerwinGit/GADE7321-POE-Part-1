using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerStats playerStats;
    public EnemyRefs enemyRefs;

    public bool trapped = true;

    private void OnTriggerEnter(Collider other)
    {
        if(this.CompareTag("Red Barrier") && other.CompareTag("Player"))
        {
            if (trapped == true)
            {
                gameManager.playerDeathText.text = "Died To: Trapped Barrier";
                if (gameManager.player.carryingFlag)
                {
                    gameManager.player.UnequipFlag();
                }
                gameManager.playerDeath.Invoke();                
            }                
            this.gameObject.SetActive(false);
        }

        if(this.CompareTag("Blue Barrier") && other.CompareTag("Enemy"))
        {
            if(trapped == true)
            {
                if (gameManager.enemy.carryingFlag)
                {
                    gameManager.enemy.UnequipFlag();
                }
                gameManager.enemyDeath.Invoke();                
            }
            this.gameObject.SetActive(false);
        }

    }
}
