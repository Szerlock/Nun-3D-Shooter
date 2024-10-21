using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingIcon : MonoBehaviour
{
    public static HealingIcon instance { get; set; }
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
        if (amount < 0)
        { 
            StartCoroutine(SmoothSlideToValue(0, 0.2f));
            return;
        }

        // Apply the healing or damage
        healthSlider.value += amount;

        // Ensure the health stays within the valid range (from 0 to 1)
        healthSlider.value = Mathf.Clamp(healthSlider.value, 0, healthSlider.maxValue);

        // Calculate target health as the current value plus a specific amount, ensuring it doesn't exceed 1
        float targetHealth = healthSlider.value + amount;

        // If health isn't full and the amount is positive (i.e., healing), refill
        if (amount > 0)
        {
            refilling = true;
            StartCoroutine(RefillHealthOverTime(targetHealth));
        }
        else
        {
            refilling = false;
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
            healthSlider.value = Mathf.Clamp(healthSlider.value, 0, healthSlider.maxValue);

            // Wait for the next frame before continuing
            yield return null;
        }
    }

    private IEnumerator SmoothSlideToValue(float targetValue, float duration)
    {
        float startValue = healthSlider.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            healthSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        healthSlider.value = targetValue;
    }

    public void IncreaseMaxValue(float value)
    { 
        healthSlider.maxValue += value;
        healthSlider.value = healthSlider.maxValue;
    }
}
