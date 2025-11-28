using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    public int currentHealth;
    public bool destroyOnDeath = true;

    [Header("UI Referanslarý")]
    public Image healthBarFill;
    public GameObject healthCanvas;

    private Camera mainCam;

    void Start()
    {
        currentHealth = maxHealth;
        mainCam = Camera.main;
        UpdateUI();
    }

    void LateUpdate()
    {
        if (healthCanvas != null && mainCam != null)
        {
            healthCanvas.transform.LookAt(transform.position + mainCam.transform.forward);
        }
    }

    public void TakeDamage(int damage, Vector3 attackerPos, float knockbackForce)
    {

        // 1. CANI AZALT
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        // 2. HASAR YAZISI (Hata Çýkarsa Yoksay ve Devam Et)
        try
        {
            if (FloatingTextManager.instance != null)
            {
                FloatingTextManager.instance.ShowDamage(damage, transform.position);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Hasar Yazýsý Hatasý (Önemli Deðil): " + e.Message);
        }
        if (FloatingTextManager.instance != null)
        {
            // Düþmanýn olduðu yerde hasar yazýsýný çýkar
            FloatingTextManager.instance.ShowDamage(damage, transform.position);
        }
        // 3. UI GÜNCELLE
        try
        {
            if (healthBarFill != null) UpdateUI();
        }
        catch (System.Exception) { }

        // 4. GERÝ TEPME
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (transform.position - attackerPos).normalized;
            dir.y = 0;
            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }

        // 5. ÖLÜM KONTROLÜ
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateUI();
    }

    void Die()
    {
        Debug.Log(gameObject.name + " ÖLÜYOR...");

        // --- LOOT KISMI (Hata Çýkarsa Yoksay) ---
        try
        {
            LootDropper looter = GetComponent<LootDropper>();
            if (looter != null) looter.DropLoot();

            HealthDropper hpDropper = GetComponent<HealthDropper>();
            if (hpDropper != null) hpDropper.CheckDrop();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Loot Düþürme Hatasý: " + e.Message);
        }
       
        if (destroyOnDeath)
        {

            if (HUDManager.instance != null)
                HUDManager.instance.AddKill();

            if (BattleRoyaleManager.instance != null)
                BattleRoyaleManager.instance.EnemyDied();

            Destroy(gameObject); // Düþmansa YOK ET
        }
        else
        {
            
            if (GameOverManager.instance != null) GameOverManager.instance.TriggerGameOver();
            gameObject.SetActive(false);
        }
    }

    void UpdateUI()
    {
        float fillAmount = (float)currentHealth / maxHealth;
        healthBarFill.fillAmount = fillAmount;
    }
}