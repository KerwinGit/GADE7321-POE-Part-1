using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRefs : MonoBehaviour
{
    #region fields
    public GameManager gameManager;

    public NavMeshAgent agent;

    [HideInInspector] public int healthPoints = 5;

    [HideInInspector] public bool carryingFlag;
    public GameObject equippedFlag;
    public Transform shootOrigin;
    public LineRenderer laser;


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

    [Header("Vision")]
    public float visionRadius;
    public float angle;

    public LayerMask playerLayer;
    public LayerMask environmentLayer;

    public bool playerVisible;
    #endregion

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        progressRef = progress.spawnArea;        
    }

    private void OnEnable()
    {
        StartCoroutine(EnemyVision());
    }

    private void OnTriggerEnter(Collider other)     //depending on whether the enemy is carrying a flag or not, their progress is tracked using an enum which updates when passing certain checkpoints (barriers)
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

    public void EquipFlag()                                                         //equips flag
    {
        carryingFlag = true;
        equippedFlag.SetActive(true);
        gameManager.enemyGoal.SetActive(true);
        gameManager.enemyFlagRetrievable = false;
        gameManager.enemyFlagDropped = false;
    }

    public void UnequipFlag()                                                       //drops flag when hurt or dies
    {
        carryingFlag = false;
        equippedFlag.SetActive(false);
        gameManager.enemyGoal.SetActive(false);
        gameManager.SpawnDroppedFlag(this.gameObject, gameManager.redFlagPF);
        gameManager.enemyFlagDropped = true;
    }

    //the following code snippet was adapted from https://github.com/Comp3interactive/FieldOfView/blob/main/FieldOfView.cs (full reference in -> Assets/Scripts/References.txt)
    #region vision
    private IEnumerator EnemyVision()                                                                           //delay between updating player vision (allows for delay in transitioning out of combat states)
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (true)
        {
            yield return wait;
            CheckPlayerVisibile();
        }
    }

    private void CheckPlayerVisibile()
    {
        Collider[] checkRadius = Physics.OverlapSphere(transform.position, visionRadius, playerLayer);          //creates an array of colliders (player only) that are in vision range

        if (checkRadius.Length != 0)                                                                            //if a player was found the array contains an element
        {
            Transform playerTarget = checkRadius[0].transform;
            Vector3 playerDirection = (playerTarget.position - transform.position).normalized;                  //determines direction to player from enemy

            if (Vector3.Angle(transform.forward, playerDirection) < angle / 2)
            {
                float distance = Vector3.Distance(transform.position, playerTarget.position);                   //determines distance to player from enemy

                if (!Physics.Raycast(transform.position, playerDirection, distance, environmentLayer))
                    playerVisible = true;                                                                       //if there is no environment in front of the player from the enemy, the player is visible
                else
                    playerVisible = false;                                                                      //if there is environment in front of the player from the enemy, the player is not visible
            }
            else
                playerVisible = false;                                                                          //if the player is not blocked by environment and in range, but not within enemy's FOV angle, they are not visible
        }
        else if (playerVisible)
            playerVisible = false;                                                                              //if the player is not within the vision range, they are not visible
    }
    #endregion
}
