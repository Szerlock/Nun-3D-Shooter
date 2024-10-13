using System;
using System.Collections;
using UnityEngine;

public class ImpMovement : MonoBehaviour
{
    ImpEnemy enemy;
    Transform player;

    [SerializeField]
    public GameObject showTextDamage;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInChildren<ImpEnemy>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.IsAttacking)
        {
            transform.LookAt(player.transform);

            // Calculate new position
            Vector3 newPosition = Vector3.MoveTowards(transform.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);

            // Move the Imp to the new position
            transform.position = newPosition;
        }
        else
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
        enemy.animatorController.Play("Attack");
        yield return new WaitForSeconds(random.NextFloat(2f, 8f));
        enemy.IsAttacking = false;
    }

    public void ShowFloatingText(float dmg)
    {
        Transform cameraTransform = Camera.main.transform;

        var go = Instantiate(showTextDamage, transform.position, Quaternion.LookRotation(transform.position - cameraTransform.position), transform);
        go.GetComponent<TextMesh>().text = dmg.ToString();
    }
}
