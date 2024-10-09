using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextDamage : MonoBehaviour
{

    public GameObject damageTextPrefab; // Reference to your TextMeshPro prefab
    public Canvas canvas; // Reference to the Canvas

    public IEnumerator ShowDamage(float damageAmount, Transform targetTransform)
    {
        // Instantiate the damage text prefab as a child of the canvas
        GameObject damageText = Instantiate(damageTextPrefab, canvas.transform);

        // Get the RectTransform component to position it correctly
        RectTransform rectTransform = damageText.GetComponent<RectTransform>();
        
        // Set the position to be above the target (enemy) character
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position + new Vector3(0, 2, 0));
        rectTransform.position = screenPosition;

        // Get the TextMeshPro component and set the damage number
        TMPro.TextMeshProUGUI textComponent = damageText.GetComponent<TMPro.TextMeshProUGUI>();
        textComponent.text = damageAmount.ToString();

        // Optionally, you can add a fade-out effect here
        yield return new WaitForSeconds(0.3f);
        // Fade out and destroy the text (implement if desired)
        Destroy(damageText);
    }
}
