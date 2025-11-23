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
        if (allUpgrades == null || allUpgrades.Count == 0) return;

        Time.timeScale = 0;
        levelUpPanel.SetActive(true);

        Debug.Log("Panel Açýldý. Butonlarý baðlamaya baþlýyorum..."); // KONTROL 1

        for (int i = 0; i < cardButtons.Length; i++)
        {
            // Butonun kendisi var mý?
            if (cardButtons[i] == null)
            {
                Debug.LogError("HATA: " + i + ". Buton kutusu boþ! Inspector'dan ata.");
                continue;
            }

            int randomIndex = Random.Range(0, allUpgrades.Count);
            UpgradeData randomCard = allUpgrades[randomIndex];

            // UI Doldur (Textler vs.)
            if (nameTexts[i] != null) nameTexts[i].text = randomCard.upgradeName;
            // ... diðer doldurmalar ...

            // --- TIKLAMA BAÐLANTISI ---
            cardButtons[i].onClick.RemoveAllListeners();

            // Lambda (Týklama Emri)
            cardButtons[i].onClick.AddListener(() => {
                Debug.Log("Týklama Algýlandý! Kart: " + randomCard.upgradeName); // KONTROL 2
                ApplyUpgrade(randomCard);
            });

            Debug.Log("Buton " + i + " baþarýyla baðlandý."); // KONTROL 3
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