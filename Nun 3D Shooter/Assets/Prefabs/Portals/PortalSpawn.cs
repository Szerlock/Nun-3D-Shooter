using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawn : MonoBehaviour
{
    private Animator animator;
    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        this.transform.LookAt(playerTransform);
        animator = GetComponent<Animator>();
        StartCoroutine(Delete());

    }

    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(4f);
        animator.SetTrigger("Play");
        Destroy(gameObject, 5f);
    }
}
