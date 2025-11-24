using UnityEngine;

public class SimpleWeapon : MonoBehaviour
{
    [Header("Silah Gücü")]
    public int damage = 20;
    public float knockback = 5f;

    [Header("Sahibi")]
    public GameObject owner;

    void Start()
    {
        // Sahibi otomatik bul
        if (owner == null)
        {
            Rigidbody parentRb = GetComponentInParent<Rigidbody>();
            if (parentRb != null) owner = parentRb.gameObject;
        }

        // KONTROL 1: Silah doðduðunda gücü var mý?
        Debug.Log($"SÝLAH DOÐDU: {gameObject.name} | Hasar Gücü: {damage} | Sahibi: {(owner != null ? owner.name : "YOK")}");
    }

    private void OnTriggerEnter(Collider other)
    {
        // KONTROL 2: Bir þeye deðiyor mu?
        Debug.Log($"TEMAS VAR: '{gameObject.name}' -> '{other.name}' objesine deðdi.");

        if (owner != null)
        {
            // Kendi sahibine mi deðdi?
            if (other.gameObject == owner || other.transform.IsChildOf(owner.transform))
            {
                // Debug.Log("ÝPTAL: Kendi sahibine deðdi.");
                return;
            }
        }

        // HealthSystem arýyoruz
        HealthSystem target = other.GetComponent<HealthSystem>();
        if (target == null) target = other.GetComponentInParent<HealthSystem>();

        if (target != null)
        {
            // KONTROL 3: Hedef bulundu, vuruyoruz!
            Debug.Log($"VURUÞ YAPILIYOR! Hedef: {target.name}, Vurulan Hasar: {damage}");

            Vector3 sourcePos = transform.position;
            if (owner != null) sourcePos = owner.transform.position;

            target.TakeDamage(damage, sourcePos, knockback);
        }
        else
        {
            // KONTROL 4: Deðdik ama caný yok
            Debug.LogWarning($"HATA: '{other.name}' objesinde 'HealthSystem' scripti bulunamadý!");
        }
    }
}