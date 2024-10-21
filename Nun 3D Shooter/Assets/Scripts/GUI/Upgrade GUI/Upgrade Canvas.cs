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
    [SerializeField]
    private BladeStorm bladeStorm;
    [SerializeField]
    private SpinBlade bladeSpin;
    [SerializeField]
    private Dashing dash;
    [SerializeField]
    private PlayerMovement bladeSpinDuration;

    private List<int> gunUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> gunReloadUpgrades = new List<int> { 1, 1, 2, 2, 2 };

    private List<int> damageUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<float> damageUpgrades = new List<float> { 0.05f, 0.10f, 0.15f, 0.20f, 0.25f };

    private List<int> healthUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> healthAmountUpgrades = new List<int> { 20, 30, 40, 60, 80 };

    private List<int> dashUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> dashDamageUpgrades = new List<int> { 10, 1, 1, 1, 1 };

    private List<int> bladeSpinUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> bladeSpinDurationUpgrades = new List<int> { 1, 1, 1, 1, 1 };
    
    private List<int> bladeStormUpgradeCosts = new List<int> { 100, 200, 300, 400, 500 };
    private List<int> bladeStormDamageUpgrades = new List<int> { 1, 1, 1, 1, 1 };

    [SerializeField]
    private int currentUpgradeIndex = 0;
    private int currentDashIndex = 0;

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

    public void UpgradeGunReloadDamage()
    {
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(gunUpgradeCosts[0]);
        if (enoughCurrency)
        {
            gunUpgradeCosts.RemoveAt(0);
            gunController.ChangeCurrentReloadSpeed(gunReloadUpgrades[0]);
            gunReloadUpgrades.RemoveAt(0);
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }

    // Maybe make this also increase the reload time probably no
    public void UpgradeBladeStorm()
    {
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(bladeStormUpgradeCosts[0]);
        if (enoughCurrency)
        {
            if (currentUpgradeIndex == 1 || currentUpgradeIndex == 3)
            {
                bladeStormUpgradeCosts.RemoveAt(0);
                bladeStorm.IncreaseSwordCount(bladeStormDamageUpgrades[0]);
                bladeStormDamageUpgrades.RemoveAt(0);
            }
            else
            {
                bladeStormUpgradeCosts.RemoveAt(0);
                bladeStorm.IncreaseDuration(bladeStormDamageUpgrades[0]);
                bladeStormDamageUpgrades.RemoveAt(0);
            }
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
        currentUpgradeIndex++;
    }

    public void UpgradeDamage()
    {
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(damageUpgradeCosts[0]);
        if (enoughCurrency)
        {
            damageUpgradeCosts.RemoveAt(0);
            swordController.ChangeCurrentDamage(damageUpgrades[0]);
            gunController.ChangeCurrentDamage(damageUpgrades[0]);
            bladeSpin.ChangeCurrentDamage(damageUpgrades[0]);
            bladeStorm.ChangeCurrentDamage(damageUpgrades[0]);
            dash.IncreaseDamage(damageUpgrades[0]);
            damageUpgrades.RemoveAt(0);
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
            HealingOrb.instance.IncreaseMaxValue(healthAmountUpgrades[0]);
            healthAmountUpgrades.RemoveAt(0);
            return;
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }

    public void UpgradeSpinAbility()
    { 
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(bladeSpinUpgradeCosts[0]);
        if (enoughCurrency)
        {
            bladeSpinUpgradeCosts.RemoveAt(0);
            bladeSpinDuration.IncreaseDuration(bladeSpinDurationUpgrades[0]);
            bladeSpinDurationUpgrades.RemoveAt(0);
            return;
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }

    public void UpgradeDashAbility()
    {
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(dashUpgradeCosts[0]);
        if (enoughCurrency)
        {
            if (currentDashIndex == 0)
            {
                dashUpgradeCosts.RemoveAt(0);
                dash.Damage(dashDamageUpgrades[0]);
                dashDamageUpgrades.RemoveAt(0);
            }

        }
        else if (currentDashIndex > 0)
        { 
            dashUpgradeCosts.RemoveAt(0);
            dash.DecreaseCooldown(dashDamageUpgrades[0]);
            dashDamageUpgrades.RemoveAt(0);
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
        currentDashIndex++;
    }
}
