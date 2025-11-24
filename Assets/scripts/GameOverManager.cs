using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi (Restart) için þart

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance; // Her yerden ulaþmak için

    [Header("UI")]
    public GameObject gameOverPanel; // Az önce yaptýðýn panel

    void Awake()
    {
        instance = this;
    }

    // Bu fonksiyonu Player ölünce çaðýracaðýz
    public void TriggerGameOver()
    {
        // 1. Paneli Görünür Yap
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 2. Oyunu Durdur (Düþmanlar dursun)
        Time.timeScale = 0;
    }

    // Bu fonksiyonu Butona baðlayacaðýz
    public void RestartGame()
    {
        // 1. Zamaný Baþlat (Yoksa oyun donuk baþlar!)
        Time.timeScale = 1;

        // 2. Sahneyi Yeniden Yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}