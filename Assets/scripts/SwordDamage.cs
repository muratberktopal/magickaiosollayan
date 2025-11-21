using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damage = 20;          // Kaç vuracak?
    public float knockbackForce = 5f; // Ne kadar geriye uçuracak?
    public string targetTag = "Enemy"; // Player kýlýcýysa "Enemy", Bot kýlýcýysa "Player"
    public GameObject owner;         // Kýlýcýn sahibi (Inspector'dan ata veya kod otomatik bulur)

    private void Start()
    {
        // Eðer owner atanmadýysa, kýlýcýn en üstteki sahibini bulmaya çalýþ
        if (owner == null)
            owner = GetComponentInParent<Rigidbody>()?.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kendi sahibine vurma
        if (owner != null && other.gameObject == owner) return;

        // Hedeflediðimiz tag'e mi çarptý?
        if (other.CompareTag(targetTag) || (targetTag == "Actors" && (other.CompareTag("Player") || other.CompareTag("Enemy"))))
        {
            // Çarptýðýmýz objede "Health" scripti var mý?
            Health enemyHealth = other.GetComponent<Health>();

            if (enemyHealth != null)
            {
                // Hasar ver ve geriye it (Sahibinin pozisyonunu yolluyoruz ki zýt yöne itilsin)
                Vector3 attackerPos = owner != null ? owner.transform.position : transform.position;
                enemyHealth.TakeDamage(damage, attackerPos, knockbackForce);
            }
        }
    }
}