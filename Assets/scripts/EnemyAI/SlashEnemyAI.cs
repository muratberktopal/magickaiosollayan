using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class SlashEnemyAI : MonoBehaviour
{
    [Header("Hız Ayarları")]
    public float moveSpeed = 3.5f;
    public float wanderSpeed = 2f;

    [Header("Mesafe Ayarları")]
    public float detectionRange = 10f;
    public float attackRange = 2.5f;
    public float patrolRange = 6f;

    [Header("Saldırı Ayarları")]
    public GameObject slashPrefab;
    public Transform firePoint;
    public float cooldownTime = 2f;

    // --- YENİ EKLENEN KISIM (HASAR AYARLARI) ---
    [Header("Güç Ayarları (Buradan Değiştir)")]
    public int damageAmount = 10;     // Kaç vuracak?
    public float knockbackForce = 5f; // Ne kadar itecek?
    // -------------------------------------------

    // Gizli Değişkenler
    private Rigidbody rb;
    private Animator animator;
    private Transform currentTarget;

    private bool isCoolingDown = false;
    private float cooldownTimer = 0f;
    private Vector3 wanderPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        PickRandomPoint();
    }

    void FixedUpdate()
    {
        // Animasyon
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        if (animator != null) animator.SetBool("IsMoving", isMoving);

        // Hedef Bul
        FindClosestTarget();

        // Davranış
        if (isCoolingDown)
        {
            CooldownLogic();
        }
        else if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.position);

            if (dist <= attackRange)
            {
                PerformSlashAttack();
            }
            else
            {
                MoveTo(currentTarget.position, moveSpeed);
            }
        }
        else
        {
            WanderBehavior();
        }
    }

    void PerformSlashAttack()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySlash(); // Slash sesi çağırıyoruz
        }
        rb.linearVelocity = Vector3.zero;
        if (currentTarget != null) LookAt(currentTarget.position);

        
        

        if (slashPrefab != null && firePoint != null)
        {
            // Slash efektini yatay çıkar
            Quaternion rotasyon = Quaternion.Euler(90, transform.eulerAngles.y, 0);
            GameObject slash = Instantiate(slashPrefab, firePoint.position, rotasyon);

            // --- GÜCÜ AKTARMA KISMI ---
            SimpleWeapon weapon = slash.GetComponent<SimpleWeapon>();
            if (weapon != null)
            {
                weapon.owner = this.gameObject;

                // Inspector'dan girdiğin değerleri silaha aktar
                weapon.damage = damageAmount;
                weapon.knockback = knockbackForce;
            }
            // --------------------------

            Destroy(slash, 0.3f);
        }

        isCoolingDown = true;
        cooldownTimer = cooldownTime;
        PickRandomPoint();
    }

    // --- STANDART FONKSİYONLAR ---
    void FindClosestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            if (hit.GetComponent<HealthSystem>() == null) continue;

            if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < closestDist) { closestDist = d; bestTarget = hit.transform; }
            }
        }
        currentTarget = bestTarget;
    }

    void CooldownLogic() { cooldownTimer -= Time.fixedDeltaTime; WanderBehavior(); if (cooldownTimer <= 0) isCoolingDown = false; }
    void WanderBehavior() { MoveTo(wanderPoint, wanderSpeed); if (Vector3.Distance(transform.position, wanderPoint) < 1f) PickRandomPoint(); }
    void MoveTo(Vector3 d, float s) { LookAt(d); rb.linearVelocity = (d - transform.position).normalized * s; rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z); }
    void LookAt(Vector3 d) { transform.LookAt(new Vector3(d.x, transform.position.y, d.z)); }
    void PickRandomPoint() { wanderPoint = transform.position + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange)); }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}