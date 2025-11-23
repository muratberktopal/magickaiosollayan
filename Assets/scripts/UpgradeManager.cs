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

    [Header("Tüm Kartlar (BURASI DOLU MU?)")]
    public List<UpgradeData> allUpgrades; // <-- SUÇLU MUHTEMELEN BURASI

    private void Awake()
    {
        instance = this;
    }

    public void ShowUpgradeOptions()
    {
        // 1. KART DESTESÝ KONTROLÜ (En sýk yapýlan hata)
        if (allUpgrades == null || allUpgrades.Count == 0)
        {
            Debug.LogError("HATA: 'All Upgrades' listesi boþ! Project panelinden oluþturduðun kartlarý buraya sürüklemedin.");
            return;
        }

        Time.timeScale = 0;
        levelUpPanel.SetActive(true);

        // Döngüye giriyoruz
        for (int i = 0; i < cardButtons.Length; i++)
        {
            // --- GÜVENLÝK KONTROLÜ (Crash Önleyici) ---
            // Eðer listelerden biri eksikse, döngüyü kýrma, o kýsmý atla ve hatayý söyle.

            if (i >= nameTexts.Length)
            {
                Debug.LogError("HATA: 'Name Texts' listesi eksik! Buton sayýsý kadar eleman yok. Sýra: " + i);
                continue;
            }
            if (i >= descTexts.Length)
            {
                Debug.LogError("HATA: 'Desc Texts' listesi eksik! Sýra: " + i);
                continue;
            }
            if (i >= iconImages.Length)
            {
                Debug.LogError("HATA: 'Icon Images' listesi eksik! Sýra: " + i);
                continue;
            }
            // ------------------------------------------

            // Rastgele Kart Seç
            int randomIndex = Random.Range(0, allUpgrades.Count);
            UpgradeData randomCard = allUpgrades[randomIndex];

            // UI Doldur
            if (nameTexts[i] != null) nameTexts[i].text = randomCard.upgradeName;
            if (descTexts[i] != null) descTexts[i].text = randomCard.description;
            if (iconImages[i] != null && randomCard.icon != null) iconImages[i].sprite = randomCard.icon;

            // Týklama Olayý
            cardButtons[i].onClick.RemoveAllListeners();
            cardButtons[i].onClick.AddListener(() => ApplyUpgrade(randomCard));
        }
    }

    void ApplyUpgrade(UpgradeData card)
    {
        Debug.Log("Seçilen: " + card.upgradeName);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            switch (card.upgradeType)
            {
                case UpgradeType.MoveSpeed:
                    var move = player.GetComponent<PlayerMovement>();
                    if (move) move.moveSpeed += card.value;
                    break;
                case UpgradeType.Damage:
                    Debug.Log("Hasar Arttý");
                    // Efekt hasarýný artýrabilirsin
                    break;
                case UpgradeType.Health:
                    var hp = player.GetComponent<HealthSystem>();
                    if (hp) { hp.maxHealth += (int)card.value; hp.Heal((int)card.value); }
                    break;
                case UpgradeType.AttackSpeed:
                    var atk = player.GetComponent<EffectAttack>();
                    if (atk) atk.attackRate -= card.value;
                    break;
            }
        }

        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
}