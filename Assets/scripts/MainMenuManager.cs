using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Baðlantýlar")]
    public GameObject settingsPanel;
    public AudioMixer mainMixer;

    [Header("Slider Referanslarý")]
    public Slider masterSlider;
    public Slider musicSlider;

    void Start()
    {
        // --- YÜKLEME ÝÞLEMÝ ---

        // 1. Master Sesi Yükle
        // Eðer kayýt yoksa 0.75 (Yüzde 75) sesle baþla
        float savedMaster = PlayerPrefs.GetFloat("MasterVolume", 0.75f);

        if (masterSlider != null)
        {
            masterSlider.value = savedMaster; // Slider'ý güncelle
        }

        // Mixer'ý güncelle (Logaritma hatasýný önlemek için 0.0001 kontrolü)
        float masterDb = Mathf.Log10(Mathf.Max(savedMaster, 0.0001f)) * 20;
        mainMixer.SetFloat("MasterVol", masterDb);

        Debug.Log("Master Ses Yüklendi: " + savedMaster); // KONSOLA BAK


        // 2. Müzik Sesini Yükle
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.75f);

        if (musicSlider != null)
        {
            musicSlider.value = savedMusic;
        }

        float musicDb = Mathf.Log10(Mathf.Max(savedMusic, 0.0001f)) * 20;
        mainMixer.SetFloat("MusicVol", musicDb);

        Debug.Log("Müzik Ses Yüklendi: " + savedMusic); // KONSOLA BAK
    }

    // --- SES AYARLARI (KAYITLI) ---

    public void SetMasterVolume(float volume)
    {
        // 0 gelirse patlamasýn diye 0.0001 yapýyoruz
        float volumeDb = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;

        mainMixer.SetFloat("MasterVol", volumeDb);

        // Deðiþikliði anýnda kaydet
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save(); // Zorla kaydet
    }

    public void SetMusicVolume(float volume)
    {
        float volumeDb = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;

        mainMixer.SetFloat("MusicVol", volumeDb);

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    // --- DÝÐERLERÝ ---
    public void PlayGame() { SceneManager.LoadScene(1); }
    public void OpenSettings() { settingsPanel.SetActive(true); }
    public void CloseSettings() { settingsPanel.SetActive(false); }
}