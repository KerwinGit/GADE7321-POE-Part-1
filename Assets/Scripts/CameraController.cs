using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        Vector3 overShoulderDir = overShoulderFocus.position - new Vector3(transform.position.x, overShoulderFocus.position.y, transform.position.z);
        facing.forward = overShoulderDir.normalized;

        playerObject.forward = overShoulderDir.normalized;
    }

}

