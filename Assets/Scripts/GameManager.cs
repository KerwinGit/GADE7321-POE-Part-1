using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public PlayerStats player;
    public Enemy enemy;

    private int playerScore = 0;
    private int enemyScore = 0;    

    public UnityEvent startRoundEvent;

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private TMP_Text enemyHPText;

    [Header("GameObject References")]
    [SerializeField] private GameObject playerFlag;
    [SerializeField] private GameObject enemyFlag;
    [SerializeField] private GameObject playerGoal;
    [SerializeField] private GameObject enemyGoal;

    void Start()
    {
        
    }

    void Update()
    {
        
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
}
