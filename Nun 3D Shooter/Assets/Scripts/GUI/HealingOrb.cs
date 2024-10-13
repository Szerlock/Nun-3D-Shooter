using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingOrb : MonoBehaviour
{
    public static HealingOrb instance { get; set; }
    public Slider healthSlider;
    public float refillSpeed = 0.3f;
    public bool refilling;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        healthSlider = GetComponent<Slider>();
    }

    public void ModifyHealth(float amount)
    {
        // Normalize the amount to the slider's 0-1 range by dividing it by totalHealth
        float normalizedAmount = amount / 100; // totalHealth should be defined (e.g., 100 for full health)

        // Apply the healing or damage
        healthSlider.value += normalizedAmount;

        // Ensure the health stays within the valid range (from 0 to 1)
        healthSlider.value = Mathf.Clamp(healthSlider.value, 0, 1);

        // Calculate target health as the current value plus a specific amount, ensuring it doesn't exceed 1
        float targetHealth = normalizedAmount;

        // If health isn't full and the amount is positive (i.e., healing), refill
        if (normalizedAmount > 0)
        {
            refilling = true;
            StartCoroutine(RefillHealthOverTime(targetHealth));
        }
        else
        {
            refilling = false;
            StopCoroutine(RefillHealthOverTime(targetHealth));
        }
    }

    // Coroutine to gradually refill health over time
    private IEnumerator RefillHealthOverTime(float targetHealth)
    {
        while (refilling)
        {
            // Refill health at the defined refill speed
            healthSlider.value += refillSpeed * Time.deltaTime;

            // Ensure the health value doesn't exceed the target health
            if (healthSlider.value >= targetHealth)
            {
                healthSlider.value = targetHealth; // Set to target health
                refilling = false; // Stop refilling once target is reached
            }

            // Ensure the health value doesn't go below 0 or exceed 1
            healthSlider.value = Mathf.Clamp(healthSlider.value, 0, 1);

            // Wait for the next frame before continuing
            yield return null;
        }
    }
}
