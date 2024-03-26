using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region fields
    [SerializeField] PlayerStats player;

    [Header("Keybinds")]
    public KeyCode jumpKeybind = KeyCode.Space;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private float groundDrag;
    
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool canJump = true;

    [Header("Grounded")]
    public float height;
    public LayerMask groundLayer;
    private bool grounded;

    [Header("Shooting")]
    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform shootOrigin;
    private float range = 100f;
    private float laserTime = 0.05f;
    public bool canShoot = true;
    private float cooldownTime = 1f;
    public float remainingCooldown;

    [Header("References")]
    public Transform facing;
    [SerializeField] private LineRenderer laser;
    public TMP_Text cooldownText;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 movementDirection;

    [SerializeField] private Rigidbody rb;

    #endregion

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f +0.2f, groundLayer);     //checks if player is grounded for jumping

        PlayerInputs();                                                                                     //handles controls
        SpeedLimiter();

        if(grounded)                                                                                        //applies friction to running
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        Movement();                                                                                         //movement physics
    }

    private void PlayerInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");                                                   //A/D left/right
        verticalInput = Input.GetAxisRaw("Vertical");                                                       //W/S forward/back

        if(Input.GetKey(jumpKeybind) && canJump && grounded)                                                //jumping
        {
            canJump = false;

            Jump();

            Invoke(nameof(SetCanJump), jumpCooldown);
        }

        if(Input.GetButtonDown("Fire1") && canShoot)                                                                    //shooting laser
        {            
            StartCoroutine(Shoot());
        }
    }

    private void Movement()                                                                                 //movement physics
    {
        movementDirection = facing.forward * verticalInput + facing.right * horizontalInput;

        if(grounded)
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedLimiter()                                                                             //limits XZ axis movement to not exceed specified amount
    {
        Vector3 XZVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(XZVelocity.magnitude > moveSpeed)
        {
            Vector3 maxedVelocity = XZVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(maxedVelocity.x, rb.velocity.y, maxedVelocity.z);
        }
    }

    private void Jump()                                                                                     //jumping physics
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void SetCanJump()                                                                               //helper method for jumping
    {
        canJump = true;
    }

    IEnumerator Shoot()                                                                                    //shooting logic handled
    {
        canShoot = false;
        laser.SetPosition(0, shootOrigin.position);
        Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1f));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                player.gameManager.enemyHurt.Invoke();
            }

                laser.SetPosition(1, hit.point);
        }
        else
        {
            laser.SetPosition(1, rayOrigin + (playerCam.transform.forward * range));
        }

        //display laser visual
        laser.enabled = true;
        yield return new WaitForSeconds(laserTime);
        laser.enabled = false;

        float remainingCooldown = cooldownTime;                                                             //cooldown applied and displayed
        while (remainingCooldown > 0)
        {
            cooldownText.text = "Laser: " + remainingCooldown.ToString("F1") + "s";

            remainingCooldown -= Time.deltaTime;

            yield return null;
        }

        cooldownText.text = "Laser: Ready";        
        canShoot = true;        
    }
}
