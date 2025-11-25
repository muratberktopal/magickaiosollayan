using UnityEngine;
using UnityEngine.Audio; // Mixer için þart
using UnityEngine.UI;    // Slider için þart

public class SettingsMenu : MonoBehaviour
{
    [Header("Baðlantýlar")]
    public AudioMixer mainMixer; // Oluþturduðun Mixer dosyasý
    public GameObject settingsPanel; // Siyah panel

    public void SetMasterVolume(float volume)
    {
        // Slider 0 ile 1 arasý deðer verir.
        // Mixer -80 ile 0 arasý (Logaritmik) çalýþýr.
        // Bu formül kaydýrmayý düzgün hissettirir.
        mainMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    // --- PANEL AÇMA KAPAMA ---
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}