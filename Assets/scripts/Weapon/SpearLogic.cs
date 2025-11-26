using UnityEngine;

public class SpearLogic : MonoBehaviour
{
    [Header("Mýzrak Ayarlarý")]
    public int damage = 35;
    public float lifeTime = 0.3f; // Ne kadar süre ekranda kalsýn?
    public float knockback = 8f;

    // Mýzraðýn sahibi (Kendi kendine vurmasýn diye)
    [HideInInspector] public GameObject owner;

    void Start()
    {
        // Süre dolunca yok ol
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // 1. Sahibine vurma
        if (other.gameObject == owner) return;

        // 2. Can Sistemini bul
        HealthSystem targetHealth = other.GetComponent<HealthSystem>();

        if (targetHealth != null)
        {
            // Vuruþ yönünü hesapla (Mýzraðýn baktýðý yön)
            Vector3 pushDir = transform.position;
            if (owner != null) pushDir = owner.transform.position;

            // Hasar ver
            targetHealth.TakeDamage(damage, pushDir, knockback);
        }
    }
}