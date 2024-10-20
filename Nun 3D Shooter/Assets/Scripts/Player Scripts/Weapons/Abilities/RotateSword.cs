using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSword : MonoBehaviour
{
    private Blades blades;
    private Transform target;
    private float speed = Blades.speed;


    private void Start()
    {
        blades = GetComponentInChildren<Blades>();
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        //Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.LookAt(target.position);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
