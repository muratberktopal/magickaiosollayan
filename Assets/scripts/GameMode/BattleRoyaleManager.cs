using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleRoyaleManager : MonoBehaviour
{
    public static BattleRoyaleManager instance;

    [Header("Ayarlar")]
    public int totalEnemies = 10;
    [HideInInspector] public int enemiesAlive;

    [Header("UI Referanslarý")]
    public TextMeshProUGUI aliveText;
    public GameObject victoryPanel;

    void Awake()
    {
        instance = this;
        enemiesAlive = totalEnemies;
    }

    void Start()
    {
        // --- GÜVENLÝK KONTROLÜ ---
        // Eðer Survival Modundaysak (Mod 1), bu script kendini kapatsýn ve UI'ý gizlesin
        int gameMode = PlayerPrefs.GetInt("GameMode", 0);

        if (gameMode == 1) // Survival
        {
            if (aliveText != null) aliveText.gameObject.SetActive(false); // Yazýyý gizle
            this.enabled = false; // Scripti devre dýþý býrak
            return; // Çýk
        }
        // -------------------------

        UpdateUI();
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public void EnemyDied()
    {
        // --- ÝKÝNCÝ KÝLÝT ---
        // Eðer script devre dýþýysa veya Survival modundaysak HÝÇBÝR ÞEY YAPMA.
        if (!this.enabled || PlayerPrefs.GetInt("GameMode", 0) == 1) return;
        // --------------------

        enemiesAlive--;
        UpdateUI();

        if (enemiesAlive <= 0)
        {
            WinGame();
        }
    }

    void UpdateUI()
    {
        if (aliveText != null)
        {
            aliveText.text = "KALAN: " + (enemiesAlive + 1).ToString();
        }
    }

    void WinGame()
    {
        Debug.Log("OYUN BÝTTÝ - KAZANDIN!");

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Time.timeScale = 0;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void ResetRound()
    {
        enemiesAlive = totalEnemies; // Sayýyý fulle (10'a çek)
        UpdateUI(); // Yazýyý güncelle ("Kalan: 10")

        if (victoryPanel != null) victoryPanel.SetActive(false); // Zafer ekranýný kapat

        Time.timeScale = 1; // Zamaný tekrar baþlat
    }
}