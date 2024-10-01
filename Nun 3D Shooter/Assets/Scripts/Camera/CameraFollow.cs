using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameManager gameManager;
    public Transform target;  // Player's transform
    public Vector3 cameraOffset = new Vector3();  // Offset behind the player
    public float smoothSpeed = 0.125f;  // Smoothing factor for camera movement

    public float sensitivity = 3f; // Mouse sensitivity
    public float minY = -40f;      // Minimum Y rotation
    public float maxY = 80f;       // Maximum Y rotation

    private float currentX = 0f;   // Current X rotation (horizontal)
    private float currentY = 0f;   // Current Y rotation (vertical)

    void Update()
    {
        if (gameManager.IsGameStarted())
        {
            // Get mouse input for rotating the camera
            currentX += Input.GetAxis("Mouse X") * sensitivity;
            currentY -= Input.GetAxis("Mouse Y") * sensitivity;

            // Clamp vertical rotation (Y-axis)
            currentY = Mathf.Clamp(currentY, minY, maxY);
        }
    }

    void LateUpdate()
    {
        if (gameManager.IsGameStarted())
        {
            // Calculate camera rotation based on mouse input
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

            // Calculate the new camera position based on rotation and offset
            Vector3 desiredPosition = target.position + rotation * cameraOffset;

            // Smoothly interpolate to the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Make the camera always look at the player
            transform.LookAt(target.position);
        }
    }
}
