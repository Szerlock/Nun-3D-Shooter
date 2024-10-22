using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("TextBox")]
    public TextMeshProUGUI bladeSpinText;
    public TextMeshProUGUI bladeStormText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI gunText;
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI healthText;

    private int bladeUpIndex = 0;
    private int stormUpIndex = 0;
    private int dashUpIndex = 0;
    private int damageUpIndex = 0;
    private int gunUpIndex = 0;
    private int healthIndex = 0;

    [Header("Buttons")]
    public GameObject stormButton;
    public GameObject bladeSpinButton;
    public GameObject damageButton;
    public GameObject gunButton;
    public GameObject dashButton;
    public GameObject healthButton;



    // Start is called before the first frame update
    void Start()
    {
        bladeSpinText.text = $"Extends the duration of rotating blades by {bladeSpinDurationUpgrades[bladeUpIndex]} second.\r\n";
        bladeStormText.text = $"Increase Divine Armament duration by {bladeStormDamageUpgrades[stormUpIndex]} second.\r\n";
        damageText.text = $"Increases base attack by an additional {damageUpgrades[damageUpIndex] * 100}%, compounding the total bonus.\r\n";
        gunText.text = $"Reduces the cooldown for gaining a bullet by {gunReloadUpgrades[gunUpIndex]} seconds.\r\n";
        dashText.text = $"Dash now does {dashDamageUpgrades[dashUpIndex]} damage.\r\n";
        healthText.text = $"Increases base hp by {healthAmountUpgrades[healthIndex]}.\r\n";

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
        if (gunUpIndex > 4)
        {
            return;
        }
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(gunUpgradeCosts[0]);
        if (enoughCurrency)
        {
            gunUpgradeCosts.RemoveAt(0);
            gunController.ChangeCurrentReloadSpeed(gunReloadUpgrades[0]);
            gunUpIndex++;
            gunText.text = $"Reduces the cooldown for gaining a bullet by {gunReloadUpgrades[gunUpIndex]} second.\r\n";
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }

    public void UpgradeBladeStorm()
    {
        if (stormUpIndex > 4)
        {
            return;
        }
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(bladeStormUpgradeCosts[0]);
        if (enoughCurrency)
        {
            if (stormUpIndex == 0 || stormUpIndex == 2)
            {
                bladeStormUpgradeCosts.RemoveAt(0);
                bladeStorm.IncreaseSwordCount(bladeStormDamageUpgrades[0]);
                // change string to increase sword count
                stormUpIndex++;
                bladeStormText.text = $"Increases the number of swords thrown by {bladeStormDamageUpgrades[stormUpIndex]} swords.\r\n";
            }
            else
            {
                bladeStormUpgradeCosts.RemoveAt(0);
                bladeStorm.IncreaseDuration(bladeStormDamageUpgrades[0]);
                stormUpIndex++;
                if (stormUpIndex == 5)
                {
                    stormButton.GetComponent<Button>().interactable = false;
                }
                bladeStormText.text = $"Increase Divine Armament duration by {bladeStormDamageUpgrades[stormUpIndex]} second.\r\n";
            }
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }

    public void UpgradeDamage()
    {
        if (damageUpIndex > 4)
        {
            return;
        }
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(damageUpgradeCosts[0]);
        if (enoughCurrency)
        {
            damageUpgradeCosts.RemoveAt(0);
            swordController.ChangeCurrentDamage(damageUpgrades[0]);
            gunController.ChangeCurrentDamage(damageUpgrades[0]);
            bladeSpin.ChangeCurrentDamage(damageUpgrades[0]);
            bladeStorm.ChangeCurrentDamage(damageUpgrades[0]);
            dash.IncreaseDamage(damageUpgrades[0]);
            damageUpIndex++;
            damageText.text = $"Increases base attack by an additional {damageUpgrades[damageUpIndex] * 100}%, compounding the total bonus.\r\n";
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
        if (healthIndex > 4)
        {
            return;
        }
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(healthUpgradeCosts[0]);
        if (enoughCurrency)
        {
            healthUpgradeCosts.RemoveAt(0);
            playerStats.IncreaseHealth(healthAmountUpgrades[0]);
            healthIndex++;
            healthText.text = $"Increases base hp by {healthAmountUpgrades[healthIndex]}.\r\n";
            HealingOrb.instance.IncreaseMaxValue(healthAmountUpgrades[0]);
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
        if (bladeUpIndex > 4)
        {
            return;
        }
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(bladeSpinUpgradeCosts[0]);
        if (enoughCurrency)
        {
            bladeSpinUpgradeCosts.RemoveAt(0);
            bladeSpinDuration.IncreaseDuration(bladeSpinDurationUpgrades[0]);
            bladeUpIndex++;
            bladeSpinText.text = $"Extends the duration of rotating blades by {bladeSpinDurationUpgrades[bladeUpIndex]} seconds.\r\n";
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
        if (dashUpIndex > 4)
        {
            return;
        }
        bool enoughCurrency = CurrencyManager.Instance.SpendCurrency(dashUpgradeCosts[0]);
        if (enoughCurrency)
        {
            if (currentDashIndex == 0)
            {
                dashUpgradeCosts.RemoveAt(0);
                dash.Damage(dashDamageUpgrades[0]);
                currentDashIndex++;
                dashText.text = $"Dash now does {dashDamageUpgrades[dashUpIndex]} damage.\r\n";
                dashText.text = $"Reduces dash cooldown by {dashDamageUpgrades[dashUpIndex]} second.\r\n";
            }

        }
        else if (currentDashIndex > 0)
        { 
            dashUpgradeCosts.RemoveAt(0);
            dash.DecreaseCooldown(dashDamageUpgrades[0]);
            currentDashIndex++;
            dashText.text = $"Reduces dash cooldown by {dashDamageUpgrades[dashUpIndex]} second.\r\n";
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }
}
