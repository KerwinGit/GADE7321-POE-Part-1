using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public PlayerStats player;
    public PlayerController playerController;
    public EnemyRefs enemy;

    private int playerScore = 0;
    private int enemyScore = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private TMP_Text enemyHPText;

    [Header("GameObject References")]
    [SerializeField] private GameObject playerFlag;
    [SerializeField] private GameObject enemyFlag;
    [SerializeField] private GameObject playerGoal;
    [SerializeField] private GameObject enemyGoal;
    [SerializeField] private GameObject[] redBarriers;
    [SerializeField] private GameObject[] blueBarriers;

    [Header("Events")]
    public UnityEvent startRoundEvent;
    public UnityEvent resetRoundEvent;
    public UnityEvent playerHurt;
    public UnityEvent enemyHurt;
    public UnityEvent playerDeath;
    public UnityEvent enemyDeath;
    public UnityEvent winEvent;
    public UnityEvent loseEvent;

    void Start()
    {
        RoundSetup();
    }

    void Update()
    {
        
    }

    private void RoundSetup()
    {
        //handles activating barriers (enemy)
        int trappedBar = Random.Range(0, 3);
        int currentBar = 0;
        foreach (GameObject obj in redBarriers)
        {
            obj.SetActive(true);

            if (currentBar == trappedBar)
            {
                redBarriers[trappedBar].GetComponent<Barrier>().trapped = false;
            }
            else
            {
                redBarriers[trappedBar].GetComponent<Barrier>().trapped = true;
            }

            currentBar++;
        }

        //handles activating barriers (player)
        foreach (GameObject obj in blueBarriers)
        {
            obj.SetActive(true);
        }
    }

    public void ActivatePlayerGoal()
    {
        playerGoal.SetActive(true);
    }

    public void ActivateEnemyGoal()
    {
        enemyGoal.SetActive(true);
    }

    public void PlayerGoal()
    {
        Debug.Log("Goal");
    }

    public void EnemyGoal()
    {

    }

    public void ResetPlayerFlag()
    {
        playerFlag.SetActive(true);
    }

    public void ResetEnemyFlag() 
    {
        enemyFlag.SetActive(true);
    }
}
