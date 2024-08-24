using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;         // The player transform
    public Vector3 offset;           // The initial offset from the player
    public float rotationSpeed = 5f; // Speed at which the camera rotates
    public Vector2 pitchMinMax = new Vector2(-30f, 60f); // Clamp values for pitch

    private float pitch = 0f;        // Current pitch (up and down rotation)
    private float yaw = 0f;          // Current yaw (left and right rotation)

    void Start()
    {
        // Set the initial position of the camera
        transform.position = player.position + offset;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Update yaw and pitch based on mouse input
        yaw += mouseX * rotationSpeed;
        pitch -= mouseY * rotationSpeed;

        // Clamp pitch to prevent flipping
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
    }

    void LateUpdate()
    {
        // Calculate the desired position and rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = player.position + rotation * offset;

        // Update the position and rotation of the camera
        transform.position = desiredPosition;
        transform.LookAt(player);
    }
}

