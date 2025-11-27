using UnityEngine;
using TMPro; // TextMeshPro için þart

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    [Header("UI Elemanlarý")]
    public TextMeshProUGUI timerText; // Sol üstteki süre (Survival için)
    public TextMeshProUGUI killText;  // Sað üstteki kill sayacý (YENÝ)

    // Deðiþkenler
    private float timer = 0f;
    private int killCount = 0; // Kaç kiþi öldürdük?
    private bool isTimerRunning = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Survival Modundaysak (Mod 1) saati baþlat
        if (PlayerPrefs.GetInt("GameMode", 0) == 1)
        {
            isTimerRunning = true;
        }

        // Baþlangýç yazýsýný yaz
        UpdateKillUI();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    // --- DIÞARIDAN ÇAÐRILACAK FONKSÝYON ---
    public void AddKill()
    {
        killCount++; // Sayýyý artýr
        UpdateKillUI(); // Ekrana yaz
    }

    void UpdateKillUI()
    {
        if (killText != null)
        {
            killText.text = "KILLS: " + killCount.ToString();
        }
    }

    void UpdateTimerUI()
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}