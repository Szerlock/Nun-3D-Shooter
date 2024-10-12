using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordControls : MonoBehaviour
{
    public GameObject text;

    [SerializeField]
    private GameObject vfx;


    public void SwordAttack()
    {
        StartCoroutine(DeactivateCollider(0.6f));
    }

    public IEnumerator DeactivateCollider(float delay)
    {
        vfx.SetActive(true);
        yield return new WaitForSecondsRealtime(delay);
        vfx.SetActive(false);
    }
}
