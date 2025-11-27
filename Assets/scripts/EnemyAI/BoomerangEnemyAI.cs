using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class BoomerangEnemyAI : MonoBehaviour
{
    [Header("Hýz ve Hareket")]
    public float moveSpeed = 4.0f;      // Orta hýz
    public float wanderSpeed = 2.0f;

    [Header("Mesafe Ayarlarý")]
    public float detectionRange = 12f;  // Görme menzili
    public float attackRange = 7f;      // Atýþ menzili (Orta mesafe)
    public float patrolRange = 6f;

    [Header("Saldýrý Ayarlarý")]
    public GameObject boomerangPrefab;  // Bumerang Prefabý (Player için yaptýðýný kullan)
    public Transform firePoint;         // Çýkýþ noktasý
    public float cooldownTime = 3f;     // Atýþtan sonra bekleme

    [Header("Güç Ayarlarý")]
    public int damageAmount = 15;
    public float knockbackForce = 5f;

    [Header("Engel Algýlama")]
    public float obstacleCheckDist = 1.5f;
    public LayerMask obstacleLayer;

    // Gizli Deðiþkenler
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

        // Davranýþ
        if (isCoolingDown)
        {
            CooldownLogic();
        }
        else if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.position);

            if (dist <= attackRange)
            {
                PerformBoomerangAttack();
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

    void PerformBoomerangAttack()
    {
        rb.linearVelocity = Vector3.zero; // Dur
        if (currentTarget != null) LookAt(currentTarget.position); // Hedefe dön

        if (boomerangPrefab != null && firePoint != null)
        {
            // Bumerangý oluþtur (Düþmanýn baktýðý yöne doðru)
            GameObject boomerang = Instantiate(boomerangPrefab, firePoint.position, transform.rotation);

            // --- SAHÝPLÝK VE GÜÇ AYARI (Çok Önemli) ---
            SimpleWeapon weapon = boomerang.GetComponent<SimpleWeapon>();
            if (weapon != null)
            {
                weapon.owner = this.gameObject; // "Bu bumerang BENÝM" de (Geri ona dönsün)
                weapon.damage = damageAmount;
                weapon.knockback = knockbackForce;
            }
            // ------------------------------------------

            // Ses Çal (Slash sesi veya özel bumerang sesi)
            if (AudioManager.instance != null) AudioManager.instance.PlaySlash();
        }

        isCoolingDown = true;
        cooldownTimer = cooldownTime;
        PickRandomPoint();
    }

    // --- STANDART AI FONKSÝYONLARI ---

    void FindClosestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue; // Kendini geç
            if (hit.GetComponent<HealthSystem>() == null) continue; // Caný olmayaný geç

            // Player veya Enemy ise hedef al (Battle Royale mantýðý)
            if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < closestDist)
                {
                    closestDist = d;
                    bestTarget = hit.transform;
                }
            }
        }
        currentTarget = bestTarget;
    }

    void CooldownLogic() { cooldownTimer -= Time.fixedDeltaTime; WanderBehavior(); if (cooldownTimer <= 0) isCoolingDown = false; }

    void WanderBehavior()
    {
        MoveTo(wanderPoint, wanderSpeed);
        if (Vector3.Distance(transform.position, wanderPoint) < 1f) PickRandomPoint();
        DetectObstacle();
    }

    void DetectObstacle()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(rayOrigin, transform.forward, obstacleCheckDist, obstacleLayer))
        {
            PickRandomPoint();
        }
    }

    void MoveTo(Vector3 d, float s) { LookAt(d); rb.linearVelocity = (d - transform.position).normalized * s; rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z); }
    void LookAt(Vector3 d) { transform.LookAt(new Vector3(d.x, transform.position.y, d.z)); }
    void PickRandomPoint() { wanderPoint = transform.position + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange)); }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}