using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public GameManager gameManager;

    [HideInInspector] public int healthPoints = 5;

    [HideInInspector] public bool carryingFlag;
    public GameObject equippedFlag;

    [HideInInspector] public bool canMove;

    public void EquipFlag()                                                         //equips flag if picked up from base or dropped flag
    {
        carryingFlag = true;
        equippedFlag.SetActive(true);     
        gameManager.playerGoal.SetActive(true);
        gameManager.playerFlagRetrievable = false;
        gameManager.playerFlagDropped = false;
    }

    public void UnequipFlag()                                                       //drops flag
    {
        carryingFlag = false;
        equippedFlag.SetActive(false);
        gameManager.playerGoal.SetActive(false);
        gameManager.SpawnDroppedFlag(this.gameObject, gameManager.blueFlagPF);
        gameManager.playerFlagDropped = true;
    }

    public void TakeDamage()
    {
        if (carryingFlag)
        {
            UnequipFlag();
        }

        healthPoints--;

        gameManager.playerHPText.text = "HP: " + healthPoints;

        if (healthPoints <= 0)
        {
            gameManager.playerDeath.Invoke();
        }
    }
}
