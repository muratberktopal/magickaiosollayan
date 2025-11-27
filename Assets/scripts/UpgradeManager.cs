using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [Header("UI Parçalarý")]
    public GameObject levelUpPanel;
    public Button[] cardButtons;
    public TextMeshProUGUI[] nameTexts;
    public TextMeshProUGUI[] descTexts;
    public Image[] iconImages;

    [Header("Kart Listeleri")]
    public List<UpgradeData> statUpgrades;      // Hýz, Can, Hasar kartlarý
    public List<UpgradeData> physicalMaterials; // Sopa, Ýp, Kýlýç kartlarý
    public List<UpgradeData> magicMaterials;    // Ateþ, Buz vb. kartlar

    private void Awake()
    {
        instance = this;
    }

    public void ProcessLevelUp(int currentLevel)
    {
        bool isEvoLevel = EvolutionManager.instance.IsEvolutionLevel(currentLevel);

        if (isEvoLevel)
        {
            ShowMaterialOptions();
        }
        else
        {
            ShowStatOptions();
        }
    }

    void ShowStatOptions()
    {
        SetupPanel(statUpgrades, false);
    }

    void ShowMaterialOptions()
    {
        bool isMagic = EvolutionManager.instance.isMagicPath;
        List<UpgradeData> targetList = isMagic ? magicMaterials : physicalMaterials;
        SetupPanel(targetList, true);
    }

    void SetupPanel(List<UpgradeData> pool, bool isMaterial)
    {
        if (pool == null || pool.Count == 0) return;

        Time.timeScale = 0;
        levelUpPanel.SetActive(true);

        for (int i = 0; i < cardButtons.Length; i++)
        {
            if (cardButtons[i] == null) continue;

            int randomIndex = Random.Range(0, pool.Count);
            UpgradeData randomCard = pool[randomIndex];

            if (nameTexts.Length > i) nameTexts[i].text = randomCard.upgradeName;
            if (descTexts.Length > i) descTexts[i].text = randomCard.description;
            if (iconImages.Length > i) iconImages[i].sprite = randomCard.icon;

            cardButtons[i].onClick.RemoveAllListeners();
            cardButtons[i].onClick.AddListener(() => {
                if (isMaterial)
                {
                    EvolutionManager.instance.AddMaterial(randomCard.itemReward);
                    ClosePanel();
                }
                else
                {
                    ApplyStatUpgrade(randomCard);
                }
            });
        }
    }

    void ApplyStatUpgrade(UpgradeData card)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // --- 1. HAREKET HIZI ---
        if (card.upgradeType == UpgradeType.MoveSpeed)
        {
            var moveScript = player.GetComponent<PlayerMovement>();
            if (moveScript) moveScript.moveSpeed += card.value;
        }

        // --- 2. CAN (HEALTH) ---
        else if (card.upgradeType == UpgradeType.Health)
        {
            var hpScript = player.GetComponent<HealthSystem>();
            if (hpScript)
            {
                hpScript.maxHealth += (int)card.value;
                hpScript.Heal((int)card.value);
            }
        }

        // --- 3. HASAR (DAMAGE) ---
        else if (card.upgradeType == UpgradeType.Damage)
        {
            // Slash (Kýlýç)
            var slash = player.GetComponent<EffectAttack>();
            if (slash) slash.damage += (int)card.value;

            // Spear (Mýzrak)
            var spear = player.GetComponent<PlayerSpearController>();
            if (spear) spear.damage += (int)card.value;

            // Bow (Yay)
            var bow = player.GetComponent<PlayerBowController>();
            if (bow) bow.damage += (int)card.value;

            // Magic (Büyü)
            var magic = player.GetComponent<PlayerMagicCaster>();
            if (magic) magic.damage += (int)card.value;

            // Club (Sopa)
            var club = player.GetComponent<PlayerClubController>();
            if (club) club.damage += (int)card.value;
        }

        // --- 4. SALDIRI HIZI (ATTACK SPEED) ---
        else if (card.upgradeType == UpgradeType.AttackSpeed)
        {
            // --- HATAYI BURADA DÜZELTTÝM ---
            // EffectAttack içindeki deðiþkenin adý 'attackRate' idi.
            var slash = player.GetComponent<EffectAttack>();
            if (slash) slash.attackRate -= card.value;

            var spear = player.GetComponent<PlayerSpearController>();
            if (spear) spear.attackRate -= card.value;

            var bow = player.GetComponent<PlayerBowController>();
            if (bow) bow.attackRate -= card.value;

            var club = player.GetComponent<PlayerClubController>();
            if (club) club.attackRate -= card.value;
        }

        ClosePanel();
    }

    void ClosePanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
}