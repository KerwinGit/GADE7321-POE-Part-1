using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRefs : MonoBehaviour
{
    public GameManager gameManager;

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

    [Header("FOV")]
    public float fovRadius;
    public float angle = 45f;

    public LayerMask playerLayer;
    public LayerMask environmentLayer;

    public bool playerVisible;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        progressRef = progress.spawnArea;
        StartCoroutine(EnemyVision());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Red Barrier"))
        {
            if(!carryingFlag)
            {
                progressRef = progress.centreArea;
            }
            else
            {
                progressRef = progress.spawnArea;
            }
        }
        if (other.CompareTag("Blue Barrier"))
        {
            if (!carryingFlag)
            {
                progressRef = progress.flagArea;
            }
            else
            {
                progressRef = progress.centreArea;
            }
        }
    }

    public void TakeDamage()
    {
        if(carryingFlag)
        {
            UnequipFlag();
        }

        healthPoints--;

        if(healthPoints <= 0)
        {
            gameManager.enemyDeath.Invoke();
        }
    }

    public void EquipFlag()
    {
        carryingFlag = true;
        equippedFlag.SetActive(true);
        gameManager.enemyGoal.SetActive(true);
        gameManager.enemyFlagRetrievable = false;
        gameManager.enemyFlagDropped = false;
    }

    public void UnequipFlag()
    {
        carryingFlag = false;
        equippedFlag.SetActive(false);
        gameManager.enemyGoal.SetActive(false);
        gameManager.SpawnDroppedFlag(this.gameObject, gameManager.redFlagPF);
    }
    private IEnumerator EnemyVision()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return wait;
            CheckPlayerVisibile();
        }
    }

    private void CheckPlayerVisibile()
    {
        Collider[] checkRadius = Physics.OverlapSphere(transform.position, fovRadius, playerLayer);

        if (checkRadius.Length != 0)
        {
            Transform playerTarget = checkRadius[0].transform;
            Vector3 playerDirection = (playerTarget.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, playerDirection) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, playerTarget.position);

                if (!Physics.Raycast(transform.position, playerDirection, distanceToTarget, environmentLayer))
                    playerVisible = true;
                else
                    playerVisible = false;
            }
            else
                playerVisible = false;
        }
        else if (playerVisible)
            playerVisible = false;
    }
}
