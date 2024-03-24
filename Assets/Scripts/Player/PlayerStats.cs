using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [HideInInspector] public int healthPoints = 5;

    [HideInInspector] public bool carryingFlag;
    public GameObject equippedFlag;

    [HideInInspector] public bool canMove;    

    private enum element
    {
        Grass,
        Fire,
        Water
    }

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
    }
}
