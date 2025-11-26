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
        // Sahibi elle atanmadýysa, en üstteki ebeveyni (Rigidbody olaný) bulmaya çalýþ
        if (owner == null)
        {
            Rigidbody parentRb = GetComponentInParent<Rigidbody>();
            if (parentRb != null) owner = parentRb.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. SAHÝP KONTROLÜ (Owner yoksa iþlem yapma)
        if (owner == null) return;

        // --- KENDÝNE VURMA KORUMASI (YENÝLENMÝÞ) ---

        // A. Çarptýðým þey sahibimin kendisi mi?
        if (other.gameObject == owner) return;

        // B. Çarptýðým þey sahibimin bir parçasý (çocuðu) mý? (Örn: Kolu, bacaðý, modeli)
        if (other.transform.IsChildOf(owner.transform)) return;

        // -------------------------------------------

        // 3. DOST ATEÞÝ KORUMASI (Ýsteðe Baðlý)
        // Player Player'a vurmasýn (Kendine vurmayý zaten engelledik ama 2. bir player varsa diye)
        if (owner.CompareTag("Player") && other.CompareTag("Player")) return;

        // 4. CAN SÝSTEMÝNÝ ARA
        HealthSystem target = other.GetComponent<HealthSystem>();

        // Direkt bulamazsan babasýna (Parent) bak
        if (target == null) target = other.GetComponentInParent<HealthSystem>();

        // 5. HASAR VER
        if (target != null)
        {
            Vector3 sourcePos = owner.transform.position;
            target.TakeDamage(damage, sourcePos, knockback);
        }
    }
}