using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DroppedFlag : MonoBehaviour
{
    private GameManager gameManager;

    private bool canPickup = false;
    private float cooldown = 5f;

    [SerializeField] private GameObject flagCanvas;
    private Camera camera;
    public TMP_Text cooldownText;

    private void OnEnable()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        camera = Camera.main;

        StartCoroutine(PickUpCooldown());
    }

    private void Update()
    {
        flagCanvas.transform.rotation = Quaternion.LookRotation(flagCanvas.transform.position - camera.transform.position);
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
                    Destroy(gameObject, 0.2f);
                }
                else if (other.CompareTag("Enemy"))
                {
                    gameManager.ResetPlayerFlag();
                    Destroy(gameObject, 0.2f);
                }
            }

            if (this.CompareTag("Enemy Flag"))
            {
                if(other.CompareTag("Player"))
                {
                    gameManager.ResetEnemyFlag();
                    Destroy(gameObject, 0.2f);
                }
                else if(other.CompareTag("Enemy"))
                {
                    gameManager.enemy.EquipFlag();
                    Destroy(gameObject, 0.2f);
                }
            }

        }
    }

    IEnumerator PickUpCooldown()
    {
        float remainingCooldown = cooldown;                                                             //cooldown applied and displayed
        while (remainingCooldown > 0)
        {
            cooldownText.text = remainingCooldown.ToString("F1") + "s";

            remainingCooldown -= Time.deltaTime;

            yield return null;
        }

        canPickup = true;

        if (this.CompareTag("Player Flag"))
        {
            gameManager.playerFlagRetrievable = true;
        }
        else if (this.CompareTag("Enemy Flag"))
        {
            gameManager.enemyFlagRetrievable = true;
        }
    }
}
