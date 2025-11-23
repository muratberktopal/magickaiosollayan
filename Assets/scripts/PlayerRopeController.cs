using UnityEngine;

public class PlayerRopeController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject ropePrefab; // Ýp Prefab'i (Uzun ince çubuk)
    public Transform firePoint;   // Çýkýþ noktasý

    [Header("Ayarlar")]
    public float attackRate = 0.6f;  // Saldýrý hýzý
    public float ropeLifeTime = 0.2f; // Ýp ne kadar ekranda kalsýn? (Kýsa olmalý, kýrbaç gibi)
    public int damageAmount = 25;    // Hasar

    private float nextAttackTime = 0f;

    void Update()
    {
        // Test için Space
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformAttack();
        }
#endif
    }

    public void PerformAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            SpawnRope();

            if (AudioManager.instance != null)
                AudioManager.instance.PlayAttack(); // Ýstersen "Whip crack" sesi bulup deðiþtirebilirsin

            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnRope()
    {
        if (ropePrefab == null) return;

        // Çýkýþ noktasý yoksa karakterin kendisini al
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        // Karakterin önüne doðru biraz ofset verelim ki içinden çýkmasýn
        Vector3 forwardPos = spawnPos + (transform.forward * 1.5f); // 1.5 birim önde oluþsun

        // Ýpi oluþtur (Karakterin baktýðý yöne baksýn)
        GameObject currentRope = Instantiate(ropePrefab, forwardPos, transform.rotation);

        // --- HASAR AYARI ---
        SimpleWeapon weapon = currentRope.GetComponent<SimpleWeapon>();
        if (weapon != null)
        {
            weapon.owner = this.gameObject;
            weapon.damage = damageAmount;
            weapon.knockback = 5f; // Ýp düþmaný çok itmez, hafif sarsar
        }

        // Kýrbaç etkisi için kýsa sürede yok et
        Destroy(currentRope, ropeLifeTime);
    }
}