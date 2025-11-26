using UnityEngine;
using TMPro; // Yazýlar için þart
using UnityEngine.SceneManagement; // Menüye dönmek için þart

public class BattleRoyaleManager : MonoBehaviour
{
    public static BattleRoyaleManager instance; // Her yerden eriþim

    [Header("Ayarlar")]
    public int totalEnemies = 10; // Kaç düþmanla baþlýyoruz?
    private int enemiesAlive;

    [Header("UI Referanslarý")]
    public TextMeshProUGUI aliveText; // "Kalan: 10" yazýsý
    public GameObject victoryPanel;   // Yeþil Zafer paneli

    void Awake()
    {
        instance = this; // Yönetici benim!
        enemiesAlive = totalEnemies;
    }

    void Start()
    {
        UpdateUI(); // Baþlangýç yazýsýný yaz

        // Paneli garanti olsun diye kapat
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    // Düþman ölünce HealthSystem buraya haber verecek
    public void EnemyDied()
    {
        enemiesAlive--; // Sayýyý düþür
        UpdateUI();     // Yazýyý güncelle

        // Kimse kalmadýysa KAZANDIN
        if (enemiesAlive <= 0)
        {
            WinGame();
        }
    }

    void UpdateUI()
    {
        if (aliveText != null)
        {
            // +1 ekliyoruz çünkü SEN de hayattasýn (Toplam kiþi sayýsý)
            aliveText.text = "KALAN: " + (enemiesAlive + 1).ToString();
        }
    }

    void WinGame()
    {
        Debug.Log("OYUN BÝTTÝ - KAZANDIN!");

        // Paneli aç
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // Oyunu dondur (Kutlama aný)
        Time.timeScale = 0;
    }

    // Butona baðlanacak fonksiyon
    public void BackToMenu()
    {
        Time.timeScale = 1; // Zamaný düzelt
        SceneManager.LoadScene(0); // 0. Sahne (Genelde Menü sahnesidir)
    }
}