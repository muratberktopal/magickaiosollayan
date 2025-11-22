using UnityEngine;

public class SimpleWeapon : MonoBehaviour
{
    [Header("Silah Gücü")]
    public int damage = 20;
    public float knockback = 5f;

    [Header("Sahibi (Otomatik Bulur)")]
    public GameObject owner;

    void Start()
    {
        // Kýlýç kimin elindeyse (En üstteki objeyi bul) onu sahibi yap
        if (owner == null)
        {
            owner = GetComponentInParent<Rigidbody>()?.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Kendi sahibine vurma
        if (owner != null && other.gameObject == owner) return;

        // 2. Çarptýðýmýz þeyde "HealthSystem" var mý?
        HealthSystem target = other.GetComponent<HealthSystem>();

        if (target != null)
        {
            // Sahibinin pozisyonunu yolla ki o yönden itilsin
            Vector3 sourcePos = owner != null ? owner.transform.position : transform.position;

            target.TakeDamage(damage, sourcePos, knockback);

            // Vurduðumuzun ismini yaz (Test için)
            // Debug.Log(owner.name + ", " + other.name + " isimli objeye vurdu!");
        }
    }
}