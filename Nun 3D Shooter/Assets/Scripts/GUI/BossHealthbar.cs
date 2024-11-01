using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    public Slider healthSlider;
    public Slider easeHealthBar;
    [SerializeField]
    public float maxHealth;
    [SerializeField]
    public float currentHealth;
    private float lerpSpeed = 0.2f;


    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != currentHealth)
        {
            healthSlider.value = currentHealth;
        }

        if (healthSlider.value != easeHealthBar.value)
        { 
            easeHealthBar.value = Mathf.Lerp(easeHealthBar.value, currentHealth, lerpSpeed);
        }
    }

    public void ReduceHealth(float dmg)
    {
        currentHealth -= dmg;
    }
}
