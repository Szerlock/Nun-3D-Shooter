using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BladeStormSlider : MonoBehaviour
{
    public static BladeStormSlider instance { get; private set; }
    public Slider abilitySlider;
    private float cooldownSpeed;
    private bool isCoolingDown;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        abilitySlider = GetComponent<Slider>();
        abilitySlider.value = abilitySlider.maxValue;
        isCoolingDown = false;
    }

    // Call this method to start the cooldown with a custom amount
    public void StartCooldown(float refillAmount, float cooldownDuration)
    {
        if (!isCoolingDown)
        {
            cooldownSpeed = refillAmount / cooldownDuration;
            isCoolingDown = true;
            StartCoroutine(CooldownCoroutine(refillAmount));
        }
    }

    private IEnumerator CooldownCoroutine(float targetAmount)
    {
        abilitySlider.value = 0;

        while (abilitySlider.value < targetAmount)
        {
            abilitySlider.value += cooldownSpeed * Time.deltaTime;
            yield return null;
        }

        isCoolingDown = false;
    }

    public void ResetCooldown()
    {
        StopAllCoroutines();
        abilitySlider.value = abilitySlider.maxValue;
        isCoolingDown = false;
    }

    public void DecreaseMaxValue(float value)
    {
        abilitySlider.maxValue -= value;
        abilitySlider.value = abilitySlider.maxValue;
    }
}
