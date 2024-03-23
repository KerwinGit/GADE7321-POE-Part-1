using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public Transform facing;
    public Transform player;
    public Transform playerObject;
    public Rigidbody rb;
    public Transform overShoulderFocus;

    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // rotate orientation
        Vector3 facingDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        facing.forward = facingDirection.normalized;

        Vector3 overShoulderDir = overShoulderFocus.position - new Vector3(transform.position.x, overShoulderFocus.position.y, transform.position.z);
        facing.forward = overShoulderDir.normalized;

        playerObject.forward = overShoulderDir.normalized;

        //rotate player object
        //    float horizontalInput = Input.GetAxis("Horizontal");
        //    float verticalInput = Input.GetAxis("Vertical");
        //    Vector3 inputDirection = facing.forward * verticalInput + facing.right * horizontalInput;

        //    if(inputDirection != Vector3.zero)
        //    {
        //        playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, rotationSpeed * Time.deltaTime);
        //    }
    }

}

