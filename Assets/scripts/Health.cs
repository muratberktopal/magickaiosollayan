using UnityEngine;
using UnityEngine.UI; // UI iþlemleri için gerekli

public class Health : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI Referansý")]
    public Image healthBarFill; // Can barýnýn doluluðunu gösteren yeþil resim
    public GameObject uiCanvas; // Kafasýnýn üstündeki Canvas (Ölünce yok etmek için)

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Bu fonksiyon dýþarýdan çaðýrýlacak (Kýlýç deðdiðinde)
    public void TakeDamage(int damage, Vector3 hitterPosition, float knockbackForce)
    {
        // 1. Caný azalt
        currentHealth -= damage;
        UpdateHealthUI();

        // 2. GERÝ TEPME (Knockback) Kodu
        if (rb != null)
        {
            // Vuran kiþiden bana doðru olan yönü bul
            Vector3 knockbackDir = (transform.position - hitterPosition).normalized;
            knockbackDir.y = 0; // Havaya uçmasýn, sadece geriye gitsin

            // Fiziksel darbe uygula (Impulse = Anlýk itiþ)
            rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
        }

        // 3. Öldü mü?
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            // Caný yüzdeye çevir (0 ile 1 arasý)
            float fillAmount = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = fillAmount;
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " ÖLDÜ!");

        // Efekt ekleyebilirsin (Instantiate(deathParticle, ...))

        Destroy(gameObject); // Karakteri sahneden sil
    }

    // Can barýnýn sürekli kameraya bakmasý için (Opsiyonel ama þýk durur)
    void LateUpdate()
    {
        if (uiCanvas != null)
        {
            uiCanvas.transform.LookAt(Camera.main.transform);
            // Ters dönmemesi için ufak bir düzeltme gerekebilir, kameraya göre deðiþir.
            // Genelde LookAt yeterlidir.
        }
    }
}