using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera camera;
    private Transform cameraTransform;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
        cameraTransform = camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position, cameraTransform.forward);
    }
}
