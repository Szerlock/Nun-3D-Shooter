using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingIcon : MonoBehaviour
{
    public static HealingIcon instance { get; set; }
    public Slider healthSlider;

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
            StartCoroutine(SmoothSlideToValue(0, 0.3f));
            return;
        }
        float targetHealth = healthSlider.value + amount;

        if (amount > 0)
        {
            StartCoroutine(SmoothSlideToValue(targetHealth, 0f));
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
