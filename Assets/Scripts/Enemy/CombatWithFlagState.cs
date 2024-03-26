using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatWithFlagState : BaseState //this state is used when the AI has vision of the player and is carrying a flag
{
    #region fields
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
    #endregion

    public override void EnterState(EnemyStateMachine enemy)
    {
        
    }//unused in this state

    public override void UpdateState(EnemyStateMachine enemy)   //handles movement, shooting and destination choice
    {
        if(enemy.enemyRefs.gameManager.enemyFlagDropped)        //transition to normal combat if drop flag
        {
            enemy.Transition(enemy.CombatState);
        }


        if (!enemy.enemyRefs.playerVisible)
        {
            if (enemy.enemyRefs.gameManager.enemyFlagDropped || enemy.enemyRefs.gameManager.playerFlagDropped)  //transition to pick up if player not visible and flags dropped
            {
                enemy.Transition(enemy.PickUpFlagState);
            }
            else                                                                                                //if flags not dropped, transition to return flag
            {
                enemy.Transition(enemy.ReturnFlagState);
            }
        }
        else
        {
            if (!canShoot)
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

        //switch decides where the agent's destination is depending on where it is and whether the player is carrying a flag or not
        switch (enemy.enemyRefs.progressRef)
        {
            case EnemyRefs.progress.spawnArea:
                enemy.enemyRefs.agent.destination = enemy.enemyRefs.gameManager.enemyGoal.transform.position;
                break;
            case EnemyRefs.progress.centreArea:
                if (enemy.enemyRefs.gameManager.player.carryingFlag)
                {
                    if (enemy.enemyRefs.agent.remainingDistance < 0.1f)
                    {
                        enemy.enemyRefs.agent.SetDestination(SetRandomDestination(enemy));
                    }
                }
                else
                {
                    if (enemy.enemyRefs.agent.remainingDistance < 0.1f)
                    {
                        enemy.enemyRefs.agent.destination = enemy.enemyRefs.gameManager.enemyGoal.transform.position;
                    }
                }
                break;
            case EnemyRefs.progress.flagArea:
                if (enemy.enemyRefs.gameManager.player.carryingFlag)
                {
                    enemy.enemyRefs.agent.SetDestination(enemy.enemyRefs.gameManager.playerGO.transform.position);
                }
                else
                {
                    enemy.enemyRefs.agent.destination = enemy.enemyRefs.gameManager.enemyGoal.transform.position;
                }
                break;
        }

        //rotate to face player constantly during combat
        Vector3 playerDirection = enemy.enemyRefs.gameManager.playerGO.transform.position - enemy.enemyRefs.transform.position;
        playerDirection.y = 0f;

        if (playerDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            enemy.enemyRefs.transform.rotation = Quaternion.Slerp(enemy.enemyRefs.transform.rotation, targetRotation, 200f);
        }        
    }

    public override void OnTriggerEnter(EnemyStateMachine enemy, Collider other)
    {

    }//unused in this state

    private Vector3 SetRandomDestination(EnemyStateMachine enemy)
    {
        Vector3 randomPosition = Random.insideUnitSphere * strafeRadius; ;
        randomPosition += enemy.enemyRefs.transform.position;
        NavMeshHit hit;

        NavMesh.SamplePosition(randomPosition, out hit, strafeRadius, NavMesh.AllAreas);

        randomPosition = hit.position;

        return randomPosition;
    }//helper to set random destination for agent to move to

    #region Shooting
    private void Shoot(EnemyStateMachine enemy) //shoots at player using raycast to hit them with a laser
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

    public void ShootDelay()    //delay between laser shots
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= cooldownTime)
        {
            canShoot = true;
        }
    }
    #endregion
}
