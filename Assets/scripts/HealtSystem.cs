using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public int maxHealth = 100;
    public bool destroyOnDeath = true;

    [Header("Referanslar (BURAYI DOLDUR!)")]
    public Image healthBarFill; // <--- Burasý artýk elle doldurulacak!
    public GameObject healthCanvas; // <--- Canvas'ý da elle atayalým

    private int currentHealth;
    private Camera mainCam;

    void Start()
    {
        currentHealth = maxHealth;
        mainCam = Camera.main;
        UpdateUI();
    }

    void LateUpdate()
    {
        // Canvas kameraya baksýn
        if (healthCanvas != null && mainCam != null)
        {
            healthCanvas.transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
        }
    }

    public void TakeDamage(int damage, Vector3 attackerPos, float knockbackForce)
    {
        currentHealth -= damage;

        // Debug satýrý ekledim: Hasar alýyor mu görelim
        // Debug.Log("Can Azaldý! Kalan: " + currentHealth);

        UpdateUI();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (transform.position - attackerPos).normalized;
            dir.y = 0;
            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }

        if (currentHealth <= 0)
        {
            if (destroyOnDeath) Destroy(gameObject);
            else gameObject.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if (healthBarFill != null)
        {
            // Matematik hatasýný önlemek için float'a çeviriyoruz
            float oran = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = oran;
        }
        else
        {
            Debug.LogError("HATA: Health Bar Fill (Yeþil Resim) kutusu boþ! Sürüklemedin!");
        }
    }
}