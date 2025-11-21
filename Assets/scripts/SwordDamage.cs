using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damage = 20;
    public float knockbackForce = 5f;
    public GameObject owner; // Kýlýcýn sahibi

    private void Start()
    {
        // Sahibini otomatik bul (Eðer atanmadýysa)
        if (owner == null)
            owner = GetComponentInParent<Rigidbody>()?.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Kýlýç sahibine (kendine) çarpýyorsa HÝÇBÝR ÞEY YAPMA
        if (owner != null && other.gameObject == owner) return;

        // 2. Çarptýðýmýz þeyin caný var mý? (Health scripti var mý?)
        Health targetHealth = other.GetComponent<Health>();

        if (targetHealth != null)
        {
            // VUR! (Tag kontrolünü kaldýrdýk, artýk herkes herkese vurabilir)
            Vector3 attackerPos = owner != null ? owner.transform.position : transform.position;
            targetHealth.TakeDamage(damage, attackerPos, knockbackForce);
        }
    }
}