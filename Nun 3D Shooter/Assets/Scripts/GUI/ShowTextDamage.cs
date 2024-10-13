using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextDamage : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 offset = new Vector3(0f, 2f, 0);
    public Vector3 randomizeIntensity = new Vector3(0.5f, 0f, 0);

    private void Start()
    {
        Destroy(gameObject, DestroyTime);

        transform.localPosition += offset;
        Random.Range(-randomizeIntensity.y, randomizeIntensity.y);
        Random.Range(-randomizeIntensity.z, randomizeIntensity.z);
    }
}
