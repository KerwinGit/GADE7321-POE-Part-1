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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void EquipFlag()
    {
        carryingFlag = true;
        equippedFlag.SetActive(true);     
        gameManager.playerGoal.SetActive(true);
    }

    public void UnequipFlag()
    {
        carryingFlag = false;
        equippedFlag.SetActive(false);
        gameManager.playerGoal.SetActive(false);
        gameManager.SpawnDroppedFlag(this.gameObject, gameManager.blueFlagPF);
    }

    public void TakeDamage()
    {
        if (carryingFlag)
        {
            UnequipFlag();
        }

        healthPoints--;

        if (healthPoints <= 0)
        {
            gameManager.playerDeath.Invoke();
        }
    }
}
