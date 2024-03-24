using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    [Header("References")]
    public Transform facing;
    [SerializeField] private LineRenderer laser;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 movementDirection;

    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f +0.2f, groundLayer);

        PlayerInputs();
        SpeedLimiter();

        if(grounded)
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
        Movement();
    }

    private void PlayerInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKeybind) && canJump && grounded)
        {
            canJump = false;

            Jump();

            Invoke(nameof(SetCanJump), jumpCooldown);
        }

        if(Input.GetButtonDown("Fire1"))
        {
            laser.SetPosition(0, shootOrigin.position);
            Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1f));
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, range))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("IgnoreRaycast"))
                {
                    return;
                }

                laser.SetPosition(1, hit.point);
            }
            else
            {
                laser.SetPosition(1, rayOrigin + (playerCam.transform.forward * range));
            }
            StartCoroutine(Shoot());
        }
    }

    private void Movement()
    {
        // calculate movement direction
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

    private void SpeedLimiter()
    {
        Vector3 XZVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(XZVelocity.magnitude > moveSpeed)
        {
            Vector3 maxedVelocity = XZVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(maxedVelocity.x, rb.velocity.y, maxedVelocity.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        //rb.AddForce(Vector3.down * 200f, ForceMode.Force);
    }

    private void SetCanJump()
    {
        canJump = true;
    }

    IEnumerator Shoot()
    {
        laser.enabled = true;
        yield return new WaitForSeconds(laserTime);
        laser.enabled = false;
    }
}
