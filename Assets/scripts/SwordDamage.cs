using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damage = 20;
    public string targetTag = "Player"; // Kime vuracak? (Bot için 'Player' veya 'Enemy')
    public GameObject owner; // Kýlýcý tutan kiþi (Kendine vurmasýn diye)

    private void OnTriggerEnter(Collider other)
    {
        // Kýlýç sahibine veya yere deðiyorsa iþlem yapma
        if (other.gameObject == owner || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            return;

        // Hedeflediðimiz etikete mi çarptý? (Örn: Player veya Enemy)
        if (other.CompareTag(targetTag) || (targetTag == "Actors" && (other.CompareTag("Player") || other.CompareTag("Enemy"))))
        {
            Debug.Log(other.name + " isimli objeye Kýlýçla Vuruldu! Hasar: " + damage);

            // ÝLERÝDE CAN SÝSTEMÝ EKLEYÝNCE BURAYI AÇACAKSIN:
            // other.GetComponent<Health>().TakeDamage(damage);
        }
    }
}