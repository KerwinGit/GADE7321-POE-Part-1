using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CombatState : BaseState
{
    //shooting
    private bool isLaserActive = false;
    private float range = 100f;
    private float laserTimer = 0f;
    private float laserTime = 0.05f;
    private float shootTimer = 0f;
    private float cooldownTime = 1f;
    private bool canShoot = false;

    //movement
    private float strafeRadius = 10f;

    public override void EnterState(EnemyStateMachine enemy)
    {
        enemy.enemyRefs.agent.SetDestination(SetRandomDestination(enemy));
    }

    public override void UpdateState(EnemyStateMachine enemy)
    {
        if (!enemy.enemyRefs.playerVisible)
        {
            if (enemy.enemyRefs.gameManager.enemyFlagDropped || enemy.enemyRefs.gameManager.playerFlagDropped)
            {
                enemy.Transition(enemy.PickUpFlagState);
            }
            else
            {
                enemy.Transition(enemy.FindFlagState);
            }
        }
        else
        {   
            if(!canShoot)
            {
                ShootDelay();
            }
            else
            {
                Shoot(enemy);
            }
        }
        UpdateLaser();

        //toggles laser when fired
        if (isLaserActive)
        {
            enemy.enemyRefs.laser.enabled = true;
        }
        else
        {
            enemy.enemyRefs.laser.enabled = false;
        }

        //face player constantly during combat
        Vector3 playerDirection = enemy.enemyRefs.gameManager.playerGO.transform.position - enemy.enemyRefs.transform.position;
        playerDirection.y = 0f;

        if (playerDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            enemy.enemyRefs.transform.rotation = Quaternion.Slerp(enemy.enemyRefs.transform.rotation, targetRotation, 200f);
        }

        if(enemy.enemyRefs.agent.remainingDistance < 0.1f)
        {
            enemy.enemyRefs.agent.SetDestination(SetRandomDestination(enemy));
        }
    }

    public override void OnTriggerEnter(EnemyStateMachine enemy, Collider other)
    {
        return;
    }

    private Vector3 SetRandomDestination(EnemyStateMachine enemy)
    {
        Vector3 randomPosition = Random.insideUnitSphere * strafeRadius; ;
        randomPosition += enemy.enemyRefs.transform.position;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPosition, out hit, strafeRadius, NavMesh.AllAreas);

        randomPosition = hit.position;

        return randomPosition;
    }

    #region Shooting
    private void Shoot(EnemyStateMachine enemy)
    {
        enemy.enemyRefs.laser.SetPosition(0, enemy.enemyRefs.shootOrigin.position);
        RaycastHit hit;
        Vector3 playerPos = enemy.enemyRefs.gameManager.playerGO.transform.position;

        if (Physics.Raycast(enemy.enemyRefs.shootOrigin.position, (playerPos - enemy.enemyRefs.shootOrigin.position), out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                enemy.enemyRefs.gameManager.playerHurt.Invoke();
            }

            enemy.enemyRefs.laser.SetPosition(1, hit.point);
        }
        else
        {
            enemy.enemyRefs.laser.SetPosition(1, hit.point);
        }

        isLaserActive = true;
        laserTimer = 0f;

        canShoot = false;
        shootTimer = 0f;
    }

    public void UpdateLaser()   //timer for laser toggle
    {
        if (isLaserActive)
        {
            laserTimer += Time.deltaTime;

            if (laserTimer >= laserTime)
            {
                isLaserActive = false;
            }
        }
    }

    public void ShootDelay()
    {
        shootTimer += Time.deltaTime;

        if(shootTimer >= cooldownTime)
        {
            canShoot = true;
        }
    }
    #endregion
}