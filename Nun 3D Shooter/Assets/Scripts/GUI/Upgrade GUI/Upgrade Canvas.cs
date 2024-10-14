using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject upgradeCanvas;
    private SphereCollider sphereCollider;
    public GameObject interactText;
    [SerializeField]
    private GameObject upgradeBook;
    private bool isPlayerNearby = false;
    public float showTextRange = 5f;
    public bool isUpgradeCanvasActive = false;

    //  Reference to the player stats script
    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private Slash swordController;

    private List<int> gunUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> gunDamageUpgrades = new List<int> { 5, 10, 15, 20, 25 };

    private List<int> swordUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> swordDamageUpgrades = new List<int> { 5, 10, 15, 20, 25 };

    private List<int> healthUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> healthAmountUpgrades = new List<int> { 20, 30, 40, 60, 80 };

    private List<int> dashUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> dashDamageUpgrades = new List<int> { 10, 15, 20, 25, 40 };

    private List<int> gunCapacityUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> gunCapacityUpgrades = new List<int> { 1, 2, 3 };



    // Start is called before the first frame update
    void Start()
    {
        interactText.SetActive(false);
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
    }

    private void Update()
    {
        if (!WaveManager.instance.waveInProgress)
        { 
            sphereCollider.enabled = true;

            if (isPlayerNearby)
            {
                Debug.Log("Player is nearby");
                Transform cameraTransform = Camera.main.transform;
                interactText.transform.position = cameraTransform.position + cameraTransform.forward * showTextRange;

                interactText.transform.LookAt(cameraTransform);
                
                if (Input.GetKeyDown(KeyCode.U))
                {
                    ShowUpgradeCanvas();
                    HideInteractButton();
                }
            }  
        }
    }

    private void HideInteractButton()
    {
        interactText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ShowInteractButton();
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HideInteractButton();
            isPlayerNearby = false;
        }
    }

    private void ShowInteractButton()
    {
        interactText.SetActive(true);
    }

    public void ShowUpgradeCanvas()
    {
        isUpgradeCanvasActive = true;
        upgradeCanvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideUpgradeCanvas()
    {
        isUpgradeCanvasActive = false;
        upgradeCanvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpgradeGunDamage()
    {
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(gunUpgradeCosts[0]);
        if (enoughCurrency)
        {
            gunUpgradeCosts.RemoveAt(0);
            gunController.ChangeCurrentDamage(gunDamageUpgrades[0]);
            gunDamageUpgrades.RemoveAt(0);
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }

    // Maybe make this also increase the reload time probably no
    public void UpgradeGunCapacity()
    {
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(gunCapacityUpgradeCosts[0]);
        if (enoughCurrency)
        {
            gunCapacityUpgradeCosts.RemoveAt(0);
            gunController.IncreaseGunCapacity(gunCapacityUpgrades[0]);
            gunCapacityUpgrades.RemoveAt(0);
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }

    public void UpgradeSwordDamage()
    {
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(swordUpgradeCosts[0]);
        if (enoughCurrency)
        {
            swordUpgradeCosts.RemoveAt(0);
            swordController.ChangeCurrentDamage(swordDamageUpgrades[0]);
            swordDamageUpgrades.RemoveAt(0);
            return;
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }

    public void UpgradeHealth()
    { 
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(healthUpgradeCosts[0]);
        if (enoughCurrency)
        {
            healthUpgradeCosts.RemoveAt(0);
            playerStats.IncreaseHealth(healthAmountUpgrades[0]);
            healthAmountUpgrades.RemoveAt(0);
            return;
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }
}
