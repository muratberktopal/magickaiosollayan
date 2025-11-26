using UnityEngine;
using TMPro; // Yazýlar için þart

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    [Header("UI Elemanlarý")]
    public TextMeshProUGUI timerText; // Az önce yaptýðýn saat yazýsý
    public TextMeshProUGUI killText;  // Sað üstteki kill yazýsý

    private float timer = 0f;
    private int killCount = 0;
    private bool isTimerRunning = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Sadece Survival Modundaysak (Mod 1) saati baþlat
        if (PlayerPrefs.GetInt("GameMode", 0) == 1)
        {
            isTimerRunning = true;
        }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        // Matematiði: Saniyeyi Dakika ve Saniyeye böl
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        if (timerText != null)
        {
            // 00:00 formatýnda yaz
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // --- KILL SAYACI (VARSAYILAN) ---
    public void AddKill()
    {
        killCount++;
        if (killText != null) killText.text = "KILLS: " + killCount;
    }
}