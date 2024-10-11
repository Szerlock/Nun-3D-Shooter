using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostProjectile : MonoBehaviour
{
    private float damage;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerStats player = col.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage, transform.position, 0);            
            }
            else
            {
                Debug.LogError("playerstats component not found on enemy object.");
            }

            Destroy(this);
        }
    }
}
