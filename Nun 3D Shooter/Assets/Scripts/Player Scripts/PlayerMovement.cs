using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public float lastForwardVector;
    [HideInInspector]
    public Vector3 moveDir;
    [HideInInspector]
    public Vector3 lastMovedVector;

    Rigidbody rb;
    [SerializeField]
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastMovedVector = new Vector3(1, 0f, 0f); // Initialize with a default direction
    }

    void Update()
    {
        InputManagement();
    }

    void FixedUpdate()
    {
        Move();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        float moveZ = Input.GetAxisRaw("Depth"); 

        moveDir = new Vector3(moveX, moveY, moveZ).normalized;

        if(moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector3(lastHorizontalVector, lastMovedVector.y, lastMovedVector.z); // Update x component
        }

        if(moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector3(lastMovedVector.x, lastVerticalVector, lastMovedVector.z); // Update y component
        }

        if(moveDir.z != 0)
         {
            lastForwardVector = moveDir.z;
            lastMovedVector = new Vector3(lastMovedVector.x, lastMovedVector.y, lastForwardVector); // Update z component
        }

        if(moveDir.x != 0 && moveDir.y != 0 && moveDir.z != 0)
        {
            lastMovedVector = moveDir;
        }
    }
   
    void Move()
    {
        rb.velocity = lastMovedVector;
    }
}
