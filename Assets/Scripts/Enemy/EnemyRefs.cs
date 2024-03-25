using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRefs : MonoBehaviour
{
    public NavMeshAgent agent;

    [HideInInspector] public int healthPoints = 5;

    [HideInInspector] public bool carryingFlag;
    public GameObject equippedFlag;

    [HideInInspector] public bool canMove;

    public GameObject[] redBarriers;
    public GameObject[] blueBarriers;    

    public enum progress
    {
        spawnArea,
        centreArea,
        flagArea
    }

    public progress progressRef;

    private enum element
    {
        Grass,
        Fire,
        Water
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        progressRef = progress.spawnArea;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Red Barrier"))
        {
            progressRef = progress.centreArea;
        }
        if (other.CompareTag("Blue Barrier"))
        {
            progressRef = progress.flagArea;
        }
    }

    public void EquipFlag()
    {
        carryingFlag = true;
        equippedFlag.SetActive(true);
    }
}
