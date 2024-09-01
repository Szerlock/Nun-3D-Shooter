using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;          // The player transform
    public Vector3 offset;            // The initial offset from the player
    public float rotationSpeed = 5f;  // Speed at which the camera rotates
    public Vector2 yawMinMax = new Vector2(-45f, 45f); // Clamp values for yaw

    private float yaw = 0f;           // Current yaw (left and right rotation)
    private CinemachineVirtualCamera cinemachineCamera; // Reference to Cinemachine Virtual Camera

    void Start()
    {
        // Find the Cinemachine Virtual Camera in the scene
        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (cinemachineCamera == null)
        {
            Debug.LogError("Cinemachine Virtual Camera not found.");
            return;
        }

        // Set the initial position of the camera
        transform.position = player.position + offset;

        // Lock the camera's initial position offset relative to the player
        cinemachineCamera.transform.position = transform.position;
    }

    void Update()
    {
        // Get mouse input for horizontal movement (yaw)
        float mouseX = Input.GetAxis("Mouse X");

        // Update yaw based on mouse input
        yaw += mouseX * rotationSpeed;

        // Clamp yaw to prevent the camera from rotating too far around the player
        yaw = Mathf.Clamp(yaw, yawMinMax.x, yawMinMax.y);
    }

    void LateUpdate()
    {
        // Calculate the desired yaw rotation
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);

        // Update the position of the camera relative to the player
        transform.position = player.position + rotation * offset;

        // Update the Cinemachine camera's position and rotation
        cinemachineCamera.transform.position = transform.position;
        cinemachineCamera.transform.rotation = rotation;
    }
}