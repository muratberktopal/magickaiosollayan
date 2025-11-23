using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Ses Kaynaklarý")]
    public AudioSource musicSource; // Müziði çalan hoparlör
    public AudioSource sfxSource;   // Efektleri çalan hoparlör

    [Header("Ses Dosyalarý (Klipler)")]
    public AudioClip backgroundMusic;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip xpPickupSound;
    public AudioClip levelUpSound;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Oyun baþlar baþlamaz müziði çal
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    // --- SES ÇALMA FONKSÝYONLARI ---

    public void PlayAttack()
    {
        // Ses üst üste binmesin, her vuruþta perde (pitch) hafif deðiþsin ki doðal gelsin
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        sfxSource.PlayOneShot(attackSound);
    }

    public void PlayHit()
    {
        sfxSource.pitch = Random.Range(0.8f, 1.2f);
        sfxSource.PlayOneShot(hitSound);
    }

    public void PlayXP()
    {
        sfxSource.pitch = 1f; // XP sesi hep ayný olsun (veya hafif ince)
        sfxSource.PlayOneShot(xpPickupSound);
    }

    public void PlayLevelUp()
    {
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(levelUpSound);
    }
}