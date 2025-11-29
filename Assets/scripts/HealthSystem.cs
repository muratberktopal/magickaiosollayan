using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxHealth = 100;
    public int currentHealth;
    public bool destroyOnDeath = true;

    [Header("UI Baðlantýlarý")]
    public GameObject healthCanvasPrefab;
    public float heightOffset = 3.0f;     // YÜKSELTÝLDÝ: Daha yukarýda dursun

    // Gizli Deðiþkenler
    private Image healthBarFill;
    private GameObject currentCanvas;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthCanvasPrefab != null)
        {
            // ÖNEMLÝ DEÐÝÞÝKLÝK:
            // Son parametre 'null'. Yani bar karakterin Çocuðu OLMUYOR.
            // Böylece karakterin titremesinden etkilenmez.
            currentCanvas = Instantiate(healthCanvasPrefab, transform.position, Quaternion.identity, null);

            // Billboard scriptine "Beni takip et" diyoruz
            Billboard billboard = currentCanvas.GetComponent<Billboard>();
            if (billboard != null)
            {
                billboard.target = this.transform;        // Hedef benim
                billboard.offset = new Vector3(0, heightOffset, 0); // Yükseklik
            }

            // Fill objesini bul
            Transform fillObj = currentCanvas.transform.Find("Background/Fill");
            if (fillObj == null) fillObj = currentCanvas.transform.Find("Fill");

            if (fillObj != null)
                healthBarFill = fillObj.GetComponent<Image>();
        }

        UpdateUI();
    }

    // LateUpdate sildik çünkü artýk Billboard.cs her þeyi hallediyor.

    public void TakeDamage(int damage, Vector3 attackerPos, float knockbackForce)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateUI();

        if (FloatingTextManager.instance != null)
            FloatingTextManager.instance.ShowDamage(damage, transform.position);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (transform.position - attackerPos).normalized;
            dir.y = 0;
            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }

        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthBarFill != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = fillAmount;
        }
    }

    void Die()
    {
        LootDropper looter = GetComponent<LootDropper>();
        if (looter != null) looter.DropLoot();

        if (destroyOnDeath)
        {
            if (BattleRoyaleManager.instance != null) BattleRoyaleManager.instance.EnemyDied();
            Destroy(gameObject);
        }
        else
        {
            if (GameOverManager.instance != null) GameOverManager.instance.TriggerGameOver();
            gameObject.SetActive(false);
        }
    }
}