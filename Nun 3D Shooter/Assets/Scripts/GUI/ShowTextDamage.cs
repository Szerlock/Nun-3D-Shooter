using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextDamage : MonoBehaviour
{
    public GameObject damageTextPrefab;

    public IEnumerator ShowDamage(float damageAmount, Transform transform)
    {
    // Instantiate the damage text prefab at the enemy's position
    GameObject damageText = Instantiate(damageTextPrefab, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
    
    // Get the Text component (assuming TextMeshPro) and set the damage number
    damageText.GetComponent<TMPro.TextMeshProUGUI>().text = damageAmount.ToString();
    
    yield return new WaitForSeconds(1f);
    Destroy(damageText);
    //StartCoroutine(FadeAndDestroy(damageText));
    }
}
