using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [Header("Büyü Ayarları")]
    public float speed = 20f;      // Hız
    public float lifeTime = 5f;    // Ömür
    public int damage = 25;
    public GameObject explosionEffect;

    void Start()
    {
        if (Time.timeScale == 0)
        {
            Debug.LogError("OYUNUN ZAMANI DURMUŞ! O yüzden gitmiyormuş. Şimdi başlattım.");
            Time.timeScale = 1;
        }
        // Süre dolunca yok et
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // --- İŞTE KESİN ÇÖZÜM ---
        // Fizik motorunu bekleme, her saniye ileri doğru git.
        // Space.Self = Kendi baktığı yöne git demektir.
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        // Hasar verme kısmı (HealthSystem varsa)
        HealthSystem targetHealth = other.GetComponent<HealthSystem>();

        // Eğer direkt bulamazsa babasına bak (Garanti olsun)
        if (targetHealth == null) targetHealth = other.GetComponentInParent<HealthSystem>();

        if (targetHealth != null)
        {
            // Hasar ver
            targetHealth.TakeDamage(damage, transform.position, 0f);
        }

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}