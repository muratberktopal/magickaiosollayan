using UnityEngine;
using UnityEngine.UI; // UI (Image) için gerekli

public class HealthSystem : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    public int currentHealth; // Inspector'da anlýk görmek için public yaptým
    public bool destroyOnDeath = true; // Düþmanlar için TRUE, Player için FALSE olsun

    [Header("UI Referanslarý (Elle Sürükle!)")]
    public Image healthBarFill;   // Yeþil dolan bar
    public GameObject healthCanvas; // Kafasýnýn üstündeki Canvas objesi

    private Camera mainCam;

    void Start()
    {
        currentHealth = maxHealth;
        mainCam = Camera.main;

        UpdateUI();
    }

    void LateUpdate()
    {
        // Can barýnýn sürekli kameraya bakmasý (Billboard Effect)
        if (healthCanvas != null && mainCam != null)
        {
            // Kameranýn baktýðý yöne doðru çeviriyoruz
            healthCanvas.transform.LookAt(transform.position + mainCam.transform.forward);
        }
    }

    // Hasar Alma Fonksiyonu
    public void TakeDamage(int damage, Vector3 attackerPos, float knockbackForce)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayHit();

        currentHealth -= damage;

        // Caný eksiye düþerse 0'da sabitle
        if (currentHealth < 0) currentHealth = 0;

        UpdateUI();

        // --- GERÝ TEPME (KNOCKBACK) ---
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (transform.position - attackerPos).normalized;
            dir.y = 0; // Havaya uçmasýn, sadece geriye gitsin
            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }

        // --- ÖLÜM KONTROLÜ ---
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Ýyileþme (Can Artýrma) - Level Atlayýnca kullanýrýz
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateUI();
    }

    // Eksik olan o meþhur fonksiyon :)
    void Die()
    {
        // 1. LOOT DÜÞÜRME (Eðer LootDropper scripti varsa çalýþtýr)
        LootDropper looter = GetComponent<LootDropper>();
        if (looter != null)
        {
            looter.DropLoot();
        }

        // 2. ÖLÜM ÝÞLEMÝ
        if (destroyOnDeath)
        {
            // Bu bir Düþmansa yok et
            Destroy(gameObject);
        }
        else
        {
            // Bu bir Playersa oyunu bitir veya karakteri kapat
            Debug.Log("OYUN BÝTTÝ! PLAYER ÖLDÜ.");

            // Buraya ilerde "Game Over Paneli Aç" kodu gelecek
            // Þimdilik sadece karakteri gizliyoruz:
            gameObject.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if (healthBarFill != null)
        {
            // Matematik iþlemi (0 ile 1 arasý oran)
            float fillAmount = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = fillAmount;
        }
    }
}