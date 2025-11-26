using UnityEngine;
using UnityEngine.SceneManagement; // Sahne deðiþtirmek için
using UnityEngine.Audio;           // Ses için
using UnityEngine.UI;              // Slider için

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject settingsPanel;      // Ayarlar Paneli
    public GameObject modeSelectionPanel; // YENÝ: Mod Seçim Paneli

    [Header("Ses Ayarlarý")]
    public AudioMixer mainMixer;
    public Slider masterSlider;
    public Slider musicSlider;

    void Start()
    {
        // Ses ayarlarýný hafýzadan yükle (Eski kodun aynýsý)
        float savedMaster = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        if (masterSlider != null) masterSlider.value = savedMaster;
        mainMixer.SetFloat("MasterVol", Mathf.Log10(Mathf.Max(savedMaster, 0.0001f)) * 20);

        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        if (musicSlider != null) musicSlider.value = savedMusic;
        mainMixer.SetFloat("MusicVol", Mathf.Log10(Mathf.Max(savedMusic, 0.0001f)) * 20);

        // Baþlangýçta panellerin kapalý olduðundan emin ol
        if (settingsPanel) settingsPanel.SetActive(false);
        if (modeSelectionPanel) modeSelectionPanel.SetActive(false);
    }

    // --- OYUN MODU SEÇÝM FONKSÝYONLARI (YENÝ) ---

    // 1. Adým: Ana Menüdeki OYNA butonuna baðlanacak
    public void OpenModeSelection()
    {
        if (modeSelectionPanel != null)
        {
            modeSelectionPanel.SetActive(true); // Paneli Aç
        }
        else
        {
            Debug.LogError("HATA: Mode Selection Panel kutusu boþ! Inspector'dan ata.");
        }
    }

    // 2. Adým: Paneldeki "BATTLE ROYALE" butonuna baðlanacak
    public void PlayBattleRoyale()
    {
        PlayerPrefs.SetInt("GameMode", 0); // 0 = Battle Royale
        SceneManager.LoadScene(1); // Oyun Sahnesini Aç
    }

    // 2. Adým: Paneldeki "SURVIVAL" butonuna baðlanacak
    public void PlaySurvival()
    {
        PlayerPrefs.SetInt("GameMode", 1); // 1 = Survival
        SceneManager.LoadScene(1); // Oyun Sahnesini Aç
    }

    // Paneldeki "X" veya "Geri" butonuna baðlanacak
    public void CloseModeSelection()
    {
        if (modeSelectionPanel != null) modeSelectionPanel.SetActive(false);
    }

    // --- AYARLAR FONKSÝYONLARI (ESKÝSÝ GÝBÝ) ---
    public void OpenSettings() { settingsPanel.SetActive(true); }
    public void CloseSettings() { settingsPanel.SetActive(false); }

    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("MasterVol", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVol", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}