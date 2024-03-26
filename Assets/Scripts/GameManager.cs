using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    #region fields
    public PlayerStats player;
    public PlayerController playerController;
    public EnemyRefs enemy;

    private int playerScore = 0;
    private int enemyScore = 0;

    private int playerTrap;

    private float respawnTimer = 5f;

    public bool enemyFlagDropped;
    public bool enemyFlagRetrievable;
    public bool playerFlagDropped;
    public bool playerFlagRetrievable;


    [Header("UI")]
    [SerializeField] private GameObject generalCanvas;
    [SerializeField] private GameObject HUDCanvas;
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private GameObject preRoundCanvas;
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject endCanvas;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreText2;
    [SerializeField] private TMP_Text goalText;
    [SerializeField] private TMP_Text endText;
    public TMP_Text playerHPText;
    [SerializeField] private TMP_Text playerRespawnText;
    public TMP_Text playerDeathText;
    [SerializeField] private TMP_Text enemyRespawnText;

    [Header("GameObject References")]
    public GameObject playerGO;
    [SerializeField] private GameObject enemyGO;
    public GameObject playerFlag;
    public GameObject enemyFlag;
    [SerializeField] private GameObject playerRespawn;
    [SerializeField] private GameObject enemyRespawn;
    public GameObject playerGoal;
    public GameObject enemyGoal;
    [SerializeField] private GameObject[] redBarriers;
    [SerializeField] private GameObject[] blueBarriers;
    public GameObject blueFlagPF;
    public GameObject redFlagPF;
    [SerializeField] private GameObject explosionPF;

    [Header("Camera References")]
    [SerializeField] private GameObject thirdPersonCam;
    [SerializeField] private GameObject deathCam;

    [Header("Events")]
    public UnityEvent startRoundEvent;
    public UnityEvent resetRoundEvent;
    public UnityEvent playerHurt;
    public UnityEvent enemyHurt;
    public UnityEvent playerDeath;
    public UnityEvent enemyDeath;
    public UnityEvent winEvent;
    public UnityEvent loseEvent;
    #endregion

    void Start()
    {
        startCanvas.SetActive(true);
        enemyRespawnText.enabled = false;
        resetRoundEvent.Invoke();
    }

    public void HideStartCanvas()
    {
        startCanvas.SetActive(false);
    }

    //handles UI before round starts
    public void PreRound()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerScore < 5 && enemyScore < 5)
        {
            deathCam.SetActive(false);
            deathCanvas.SetActive(false);
            enemyGO.SetActive(false);
            playerGO.SetActive(false);
            HUDCanvas.SetActive(false);

            preRoundCanvas.SetActive(true);
        }
        else if(playerScore >= 5)
        {
            Win();
        }
        else if (enemyScore >= 5)
        {
            Lose();
        }
    }

    //sets up the scene for the round
    public void RoundSetup()
    {
        HUDCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;        

        if(playerFlagDropped)
        {
            playerFlag.SetActive(false);
            GameObject droppedPlayerFlag = GameObject.FindWithTag("Player Flag");
            Destroy(droppedPlayerFlag);
        }
        if (enemyFlagDropped)
        {
            enemyFlag.SetActive(false);
            GameObject droppedEnemyFlag = GameObject.FindWithTag("Enemy Flag");
            Destroy(droppedEnemyFlag);
        }

        //activate player at spawn point
        playerGO.transform.position = playerRespawn.transform.position;
        player.healthPoints = 5;
        playerHPText.text = "HP: 5";
        player.carryingFlag = false;
        player.equippedFlag.SetActive(false);
        playerFlag.SetActive(true);
        playerFlagRetrievable = false;
        playerFlagDropped = false;
        playerGoal.SetActive(false);
        playerController.remainingCooldown = 0;
        playerController.canShoot = true;
        playerController.cooldownText.text = "Laser: Ready";

        playerGO.SetActive(true);

        //activate player at spawn point
        enemyGO.transform.position = enemyRespawn.transform.position;
        enemy.healthPoints = 5;
        enemy.carryingFlag = false;
        enemy.equippedFlag.SetActive(false);
        enemyFlag.SetActive(true);
        enemyFlagDropped = false;
        enemyFlagRetrievable = false;
        enemyGoal.SetActive(false);

        enemyGO.SetActive(true);

        //handles activating barriers (enemy)
        int seed = (int)System.DateTime.Now.Ticks;
        System.Random random = new System.Random(seed);
        int trappedBar = random.Next(0, 3);

        int currentBar = 0;
        foreach (GameObject obj in redBarriers)
        {
            obj.SetActive(true);

            if (currentBar == trappedBar)
            {
                redBarriers[currentBar].GetComponent<Barrier>().trapped = true;
            }
            else
            {
                redBarriers[currentBar].GetComponent<Barrier>().trapped = false;
            }

            currentBar++;
        }

        //handles activating barriers (player)
        currentBar = 0;
        foreach (GameObject obj in blueBarriers)
        {
            obj.SetActive(true);

            if (currentBar == playerTrap)
            {
                blueBarriers[currentBar].GetComponent<Barrier>().trapped = true;
            }
            else
            {
                blueBarriers[currentBar].GetComponent<Barrier>().trapped = false;
            }

            currentBar++;
        }
    }

    public void PlayerScore()
    {
        playerScore++;
        scoreText.text = $"Blue {playerScore} - {enemyScore} Red";
        scoreText2.text = $"Blue {playerScore} - {enemyScore} Red";
        goalText.text = "Blue Scored";
        resetRoundEvent.Invoke();
    }

    public void EnemyScore()
    {
        enemyScore++;
        scoreText.text = $"Blue {playerScore} - {enemyScore} Red";
        scoreText2.text = $"Blue {playerScore} - {enemyScore} Red";
        goalText.text = "Red Scored";
        resetRoundEvent.Invoke();
    }

    public void ResetPlayerFlag()
    {
        playerFlag.SetActive(true);
        playerFlagDropped = false;
        playerFlagRetrievable = false;
    }

    public void ResetEnemyFlag() 
    {
        enemyFlag.SetActive(true);
        enemyFlagDropped = false;
        enemyFlagRetrievable = false;
    }

    //handles respawning player and enemy when they die
    #region Respawn
    public void RespawnPlayer()
    {
        Vector3 spawnPos = new Vector3(playerGO.transform.position.x, 1, playerGO.transform.position.z);
        Instantiate(explosionPF, spawnPos, playerGO.transform.rotation);

        player.equippedFlag.SetActive(false);
        player.carryingFlag = false;
        playerGO.SetActive(false);
        thirdPersonCam.SetActive(false);
        HUDCanvas.SetActive(false);
        deathCam.SetActive(true);
        deathCanvas.SetActive(true);

        StartCoroutine(PlayerRespawnHelper());
    }

    IEnumerator PlayerRespawnHelper()
    { 
        float remainingRespawn = respawnTimer;

        while (remainingRespawn > 0)
        {
            playerRespawnText.text = "Respawning in: " + remainingRespawn.ToString("F1") + "s";
            remainingRespawn -= Time.deltaTime;
            yield return null;
        }

        playerGO.transform.position = playerRespawn.transform.position;
        player.healthPoints = 5;
        playerHPText.text = "HP: 5";

        playerGO.SetActive(true);
        thirdPersonCam.SetActive(true);
        HUDCanvas.SetActive(true);
        deathCam.SetActive(false);
        deathCanvas.SetActive(false);
        playerController.remainingCooldown = 0;
        playerController.canShoot = true;
        playerController.cooldownText.text = "Laser: Ready";
    }

    public void RespawnEnemy()
    {
        Vector3 spawnPos = new Vector3(enemyGO.transform.position.x, 1, enemyGO.transform.position.z);
        Instantiate(explosionPF, spawnPos, enemyGO.transform.rotation);

        enemy.equippedFlag.SetActive(false);
        enemy.carryingFlag = false;
        enemyGO.SetActive(false);
        enemyRespawnText.enabled = true;

        StartCoroutine (EnemyRespawnHelper());
    }

    IEnumerator EnemyRespawnHelper()
    {
        float remainingRespawn = respawnTimer;

        while (remainingRespawn > 0)
        {
            enemyRespawnText.text = "Enemy respawns in: " + remainingRespawn.ToString("F1") + "s";
            remainingRespawn -= Time.deltaTime;
            yield return null;
        }

        enemyGO.transform.position = enemyRespawn.transform.position;
        enemy.healthPoints = 5;

        enemyGO.SetActive(true);
        enemyRespawnText.enabled = false;
    }
    #endregion

    public void SpawnDroppedFlag(GameObject entity, GameObject flagPF)
    {
        Vector3 spawnPos = new Vector3(entity.transform.position.x, entity.transform.position.y, entity.transform.position.z);
        Instantiate(flagPF, spawnPos, Quaternion.identity);
    }

    #region Trapping
    public void setTrapLeft()
    {
        playerTrap = 0;
        preRoundCanvas.SetActive(false);
        startRoundEvent.Invoke();
    }
    public void setTrapMiddle()
    {
        playerTrap = 1;
        preRoundCanvas.SetActive(false);
        startRoundEvent.Invoke();
    }
    public void setTrapRight()
    {
        playerTrap = 2;
        preRoundCanvas.SetActive(false);
        startRoundEvent.Invoke();
    }
    #endregion

    private void Win()
    {
        endText.text = "You Win!";
        endCanvas.SetActive(true);
    }

    private void Lose()
    {
        endText.text = "You Lose!";
        endCanvas.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
