using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Ses Kaynaklarý")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Genel Sesler")]
    public AudioClip backgroundMusic;
    public AudioClip hitSound;      // Hasar alma sesi
    public AudioClip xpPickupSound; // XP toplama
    public AudioClip levelUpSound;  // Level atlama

    [Header("Silah Sesleri (Buralarý Doldur)")]
    public AudioClip slashSound; // Kýlýç Sesi
    public AudioClip spearSound; // Mýzrak Sesi
    public AudioClip magicSound; // Büyü Sesi
    public AudioClip clubSound;  // Sopa Sesi

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    // --- ÖZEL SES FONKSÝYONLARI ---

    public void PlaySlash()
    {
        PlaySound(slashSound, 0.9f, 1.1f);
    }

    public void PlaySpear()
    {
        PlaySound(spearSound, 0.9f, 1.1f);
    }

    public void PlayMagic()
    {
        PlaySound(magicSound, 1.0f, 1.2f); // Büyü biraz daha ince sesli olsun
    }

    public void PlayClub()
    {
        PlaySound(clubSound, 0.7f, 0.9f); // Sopa daha kalýn/tok sesli olsun
    }

    // --- DÝÐER SESLER ---

    public void PlayHit()
    {
        PlaySound(hitSound, 0.8f, 1.2f);
    }

    public void PlayXP()
    {
        PlaySound(xpPickupSound, 1f, 1f);
    }

    public void PlayLevelUp()
    {
        PlaySound(levelUpSound, 1f, 1f);
    }

    // Yardýmcý Fonksiyon (Kod tekrarýný önlemek için)
    void PlaySound(AudioClip clip, float minPitch, float maxPitch)
    {
        if (clip == null) return;
        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        sfxSource.PlayOneShot(clip);
    }
}