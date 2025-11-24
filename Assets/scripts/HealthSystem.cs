using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    public int currentHealth;
    public bool destroyOnDeath = true; // Düþmanlar için TRUE yap

    [Header("UI Referanslarý")]
    public Image healthBarFill; // Yeþil bar
    public GameObject healthCanvas; // Barýn olduðu Canvas

    private Camera mainCam;

    void Start()
    {
        currentHealth = maxHealth;
        mainCam = Camera.main;
        UpdateUI();
    }

    void LateUpdate()
    {
        // Can barý kameraya baksýn
        if (healthCanvas != null && mainCam != null)
        {
            healthCanvas.transform.LookAt(transform.position + mainCam.transform.forward);
        }
    }

    // --- HASAR ALMA FONKSÝYONU ---
    public void TakeDamage(int damage, Vector3 attackerPos, float knockbackForce)
    {
        // 1. Can Azalt
        currentHealth -= damage;

        // 2. Ekrana Yazý Çýkar (Floating Text)
        if (FloatingTextManager.instance != null)
        {
            FloatingTextManager.instance.ShowDamage(damage, transform.position);
        }

        // 3. UI Güncelle
        UpdateUI();

        // 4. Geri Tepme (Fizik)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (transform.position - attackerPos).normalized;
            dir.y = 0; // Havaya uçmasýn
            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }

        // 5. Ölüm Kontrolü
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- ÝYÝLEÞME (Kart seçince vs.) ---
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateUI();
    }

    // --- ÖLÜM ---
    void Die()
    {
        // XP Taþý Düþür
        LootDropper looter = GetComponent<LootDropper>();
        if (looter != null) looter.DropLoot();

        if (destroyOnDeath)
        {
            // Düþmansa yok et
            Destroy(gameObject);
        }
        else
        {
            // Playersa Game Over ekranýný aç
            if (GameOverManager.instance != null)
                GameOverManager.instance.TriggerGameOver();

            gameObject.SetActive(false); // Karakteri gizle
        }
    }

    void UpdateUI()
    {
        if (healthBarFill != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = fillAmount;
        }
    }
}