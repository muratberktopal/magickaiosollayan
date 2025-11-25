using UnityEngine;

public class PlayerMagicCaster : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject fireballPrefab; // Ateş Topu Prefabı
    public Transform firePoint;       // Nereden çıkacak?

    // --- İŞTE EKSİK OLAN PARÇA BUYDU ---
    public int damage = 25;           // Başlangıç Hasarı
    // -----------------------------------

    public float attackRate = 1f;     // Saldırı hızı
    private float nextAttackTime = 0f;

    void Update()
    {
        // Sol tık veya Space ile ateş (Test için)
        // Mobildeysen butonun OnClick eventi Attack() fonksiyonunu çağırır.
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            // Test amaçlı buradan da çağırabilirsin ama esas WeaponSelector butona bağlıyor
            // Attack(); 
        }
    }

    // WeaponSelector veya Buton bu fonksiyonu çağıracak
    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            Shoot();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void Shoot()
    {
        if (fireballPrefab == null || firePoint == null) return;

        // Ateş topunu oluştur
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        // --- HASARI MERMİYE AKTAR ---
        // Player'ın damage değeri arttıysa, mermi de güçlensin
        FireballProjectile fbScript = fireball.GetComponent<FireballProjectile>();
        if (fbScript != null)
        {
            fbScript.damage = this.damage;
        }
    }
}