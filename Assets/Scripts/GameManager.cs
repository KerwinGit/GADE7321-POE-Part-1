using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public PlayerStats player;
    public PlayerController playerController;
    public EnemyRefs enemy;

    private int playerScore = 0;
    private int enemyScore = 0;

    private float respawnTimer = 5f;

    [Header("UI")]
    [SerializeField] private GameObject generalCanvas;
    [SerializeField] private GameObject HUDCanvas;
    [SerializeField] private GameObject deathCanvas;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private TMP_Text playerRespawnText;
    public TMP_Text playerDeathText;
    [SerializeField] private TMP_Text enemyRespawnText;

    [Header("GameObject References")]
    [SerializeField] private GameObject playerGO;
    [SerializeField] private GameObject enemyGO;
    [SerializeField] private GameObject playerFlag;
    [SerializeField] private GameObject enemyFlag;
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

    void Start()
    {
        enemyRespawnText.enabled = false;
        RoundSetup();
    }

    void Update()
    {
        
    }

    private void RoundSetup()
    {
        //handles activating barriers (enemy)
        int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        System.Random random = new System.Random(seed);
        int trappedBar = random.Next(0, 3);

        Debug.Log(trappedBar);

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
        foreach (GameObject obj in blueBarriers)
        {
            obj.SetActive(true);
        }
    }

    public void PlayerScore()
    {
        Debug.Log("Goal");
    }

    public void EnemyScore()
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

    public void RespawnPlayer()
    {
        Vector3 spawnPos = new Vector3(playerGO.transform.position.x, 1, playerGO.transform.position.z);
        Instantiate(explosionPF, spawnPos, playerGO.transform.rotation);

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

        playerGO.SetActive(true);
        thirdPersonCam.SetActive(true);
        HUDCanvas.SetActive(true);
        deathCam.SetActive(false);
        deathCanvas.SetActive(false);
    }

    public void RespawnEnemy()
    {
        Vector3 spawnPos = new Vector3(enemyGO.transform.position.x, 1, enemyGO.transform.position.z);
        Instantiate(explosionPF, spawnPos, enemyGO.transform.rotation);

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

    public void SpawnDroppedFlag(GameObject entity, GameObject flagPF)
    {
        Vector3 spawnPos = new Vector3(entity.transform.position.x, 1, entity.transform.position.z);
        Instantiate(flagPF, spawnPos, entity.transform.rotation);
    }
}
