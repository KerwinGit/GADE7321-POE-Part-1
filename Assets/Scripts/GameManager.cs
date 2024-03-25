using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private GameObject playerFlag;
    [SerializeField] private GameObject enemyFlag;
    [SerializeField] private GameObject playerRespawn;
    [SerializeField] private GameObject enemyRespawn;
    public GameObject playerGoal;
    public GameObject enemyGoal;
    [SerializeField] private GameObject[] redBarriers;
    [SerializeField] private GameObject[] blueBarriers;
    [SerializeField] private GameObject blueFlagPF;
    [SerializeField] private GameObject redFlagPF;
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
        if(player.carryingFlag) 
        {
            player.UnequipFlag();
            SpawnDroppedFlag(playerGO, blueFlagPF);
        }

        playerGO.SetActive(false);
        thirdPersonCam.SetActive(false);
        HUDCanvas.SetActive(false);
        deathCam.SetActive(true);
        deathCanvas.SetActive(true);

        StartCoroutine(PlayerRespawnHelper());        
    }

    IEnumerator PlayerRespawnHelper()
    { 
        float remainingRespawn = respawnTimer; // Initialize remaining respawn time

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

    private void SpawnDroppedFlag(GameObject entity, GameObject flagPF)
    {
        Vector3 spawnPos = new Vector3(entity.transform.position.x, 1, entity.transform.position.z);
        Instantiate(explosionPF, spawnPos, entity.transform.rotation);
        Instantiate(flagPF, spawnPos, entity.transform.rotation);
    }
}
