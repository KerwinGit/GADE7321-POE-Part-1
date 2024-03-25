using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DroppedFlag : MonoBehaviour
{
    private GameManager gameManager;

    private bool canPickup = false;
    private float cooldown = 5f;

    public TMP_Text cooldownText;

    private void OnEnable()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(PickUpCooldown());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(canPickup)
        {
            if (this.CompareTag("Player Flag"))
            {
                if (other.CompareTag("Player"))
                {
                    gameManager.player.EquipFlag();
                }
                else if (other.CompareTag("Enemy"))
                {
                    gameManager.ResetPlayerFlag();
                }
                Destroy(gameObject);
            }

            if (this.CompareTag("Enemy Flag"))
            {
                if(other.CompareTag("Player"))
                {
                    gameManager.ResetEnemyFlag();
                }
                else if(other.CompareTag("Enemy"))
                {
                    gameManager.enemy.EquipFlag();
                }
                Destroy(gameObject);
            }
        }
    }

    IEnumerator PickUpCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canPickup = true;
    }
}
