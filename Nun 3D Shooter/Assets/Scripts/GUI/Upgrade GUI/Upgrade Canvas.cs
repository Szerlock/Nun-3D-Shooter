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
    public Image upgradeImage;

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
    private List<int> dashDamageUpgrades = new List<int> { 1, 1, 1, 1, 1 };

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

    public TextMeshProUGUI BladeSpinName;
    public TextMeshProUGUI BladeStormName;
    public TextMeshProUGUI DamageName;
    public TextMeshProUGUI GunName;
    public TextMeshProUGUI DashName;
    public TextMeshProUGUI HealthName;

    public bool Unlocked;
    public GameObject crown;
    public GameObject aura;
    public GameObject sword;
    public Material newMaterial;


    // Start is called before the first frame update
    void Start()
    {
        Unlocked = false;
        DashName.text = $"Phantom Step LV. {dashUpIndex + 1}";
        HealthName.text = $"Blessing of Vitality LV. {healthIndex + 1}";
        DamageName.text = $"Holy Annihilation LV. {dashUpIndex + 1}";
        GunName.text = $"Divine Caliber LV. {gunUpIndex + 1}";
        BladeStormName.text = $"Heaven's Judgement LV. {stormUpIndex + 1}";
        BladeSpinName.text = $"Scarlet Vortex LV. {bladeUpIndex + 1}";


        bladeSpinText.text = $"Extends the duration of rotating blades by {bladeSpinDurationUpgrades[bladeUpIndex]} second.\r\n";
        bladeStormText.text = $"Increase Divine Armament duration by {bladeStormDamageUpgrades[stormUpIndex]} second.\r\n";
        damageText.text = $"Increases base attack by an additional {damageUpgrades[damageUpIndex] * 100}%, compounding the total bonus.\r\n";
        gunText.text = $"Reduces the cooldown for gaining a bullet by {gunReloadUpgrades[gunUpIndex]} seconds.\r\n";
        dashText.text = $"Reduces dash cooldown by {dashDamageUpgrades[dashUpIndex]} second.\r\n";
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

        if (!Unlocked && damageUpIndex == 5 && bladeUpIndex == 5 && gunUpIndex == 5 && stormUpIndex == 5 && dashUpIndex == 5 && healthIndex == 5)
        {
            upgradeImage.color = Color.red;
            aura.SetActive(true);
            crown.SetActive(true);
            Renderer renderer = sword.GetComponent<Renderer>();
            renderer.material = newMaterial;
            bladeSpinDuration.Unlocked = true;
            bladeStorm.Unlocked = true;
            Unlocked = true;
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
            if (gunUpIndex == 5)
            {
                Button temp = gunButton.GetComponent<Button>();

                ColorBlock colorBlock = temp.colors;

                colorBlock.disabledColor = Color.red;

                temp.colors = colorBlock;
                gunButton.GetComponent<Button>().interactable = false;
                return;
            }
            gunText.text = $"Reduces the cooldown for gaining a bullet by {gunReloadUpgrades[gunUpIndex]} second.\r\n";
            GunName.text = $"Divine Caliber LV. {gunUpIndex + 1}";
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
                bladeStormText.text += $"Increases the number of swords thrown by {bladeStormDamageUpgrades[stormUpIndex]} swords.\r\n";
                BladeStormName.text = $"Heaven's Judgement LV. {stormUpIndex + 1}";
            }
            else
            {
                bladeStormUpgradeCosts.RemoveAt(0);
                bladeStorm.IncreaseDuration(bladeStormDamageUpgrades[0]);
                stormUpIndex++;
                if (stormUpIndex == 5)
                {
                    Button temp = stormButton.GetComponent<Button>();

                    ColorBlock colorBlock = temp.colors;

                    colorBlock.disabledColor = Color.red;

                    temp.colors = colorBlock;
                    stormButton.GetComponent<Button>().interactable = false;
                    return;
                }
                bladeStormText.text = $"Increase Divine Armament duration by {bladeStormDamageUpgrades[stormUpIndex]} second.\r\n";
                BladeStormName.text = $"Heaven's Judgement LV. {stormUpIndex + 1}";
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
            if (damageUpIndex == 5)
            {
                Button temp = damageButton.GetComponent<Button>();

                ColorBlock colorBlock = temp.colors;

                colorBlock.disabledColor = Color.red;

                temp.colors = colorBlock;
                damageButton.GetComponent<Button>().interactable = false;
                return;
            }
            damageText.text = $"Increases base attack by an additional {damageUpgrades[damageUpIndex] * 100}%, compounding the total bonus.\r\n";
            DamageName.text = $"Holy Annihilation LV. {damageUpIndex + 1}";
        }
        else
        {
            Debug.Log("Not enough currency");
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
            if (healthIndex == 5)
            {
                Button temp = healthButton.GetComponent<Button>();

                ColorBlock colorBlock = temp.colors;

                colorBlock.disabledColor = Color.red;

                temp.colors = colorBlock;
                healthButton.GetComponent<Button>().interactable = false;
                return;
            }
            healthText.text = $"Increases base hp by {healthAmountUpgrades[healthIndex]}.\r\n";
            HealingOrb.instance.IncreaseMaxValue(healthAmountUpgrades[0]);
            HealthName.text = $"Blessing of Vitality LV. {healthIndex + 1}";
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
            if (bladeUpIndex == 5)
            {
                Button temp = bladeSpinButton.GetComponent<Button>();

                ColorBlock colorBlock = temp.colors;

                colorBlock.disabledColor = Color.red;

                temp.colors = colorBlock;
                bladeSpinButton.GetComponent<Button>().interactable = false;
                return;
            }
            bladeSpinText.text = $"Extends the duration of rotating blades by {bladeSpinDurationUpgrades[bladeUpIndex]} seconds.\r\n";
            BladeSpinName.text = $"Scarlet Vortex LV. {bladeUpIndex + 1}";
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
            dashUpgradeCosts.RemoveAt(0);
            dash.DecreaseCooldown(dashDamageUpgrades[0]);
            dashUpIndex++;
            if (dashUpIndex == 5)
            {
                Button temp = dashButton.GetComponent<Button>();

                ColorBlock colorBlock = temp.colors;

                colorBlock.disabledColor = Color.red;

                temp.colors = colorBlock;
                dashButton.GetComponent<Button>().interactable = false;
                return;
            }
            dashText.text = $"Reduces dash cooldown by {dashDamageUpgrades[dashUpIndex]} second.\r\n";
            DashName.text = $"Phantom Step LV. {dashUpIndex + 1}";
        }
        else
        {
            Debug.Log("Not enough currency");
            return;
        }
    }
}
