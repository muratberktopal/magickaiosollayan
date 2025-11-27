using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ScytheEnemyAI : MonoBehaviour
{
    [Header("Hýz ve Hareket")]
    public float moveSpeed = 3.5f;
    public float wanderSpeed = 2.0f;

    [Header("Mesafe Ayarlarý")]
    public float detectionRange = 10f;
    public float attackRange = 3.0f;    // Týrpan geniþ olduðu için menzili kýlýçtan az uzun
    public float patrolRange = 6f;

    [Header("Saldýrý Ayarlarý")]
    public GameObject scythePrefab;     // ScythePivot prefabý
    public Transform firePoint;
    public float cooldownTime = 2.0f;

    [Header("Güç Ayarlarý")]
    public int damageAmount = 20;
    public float knockbackForce = 6f;

    [Header("Engel Algýlama")]
    public float obstacleCheckDist = 1.5f;
    public LayerMask obstacleLayer;

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
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        if (animator != null) animator.SetBool("IsMoving", isMoving);

        FindClosestTarget();

        if (isCoolingDown)
        {
            CooldownLogic();
        }
        else if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.position);

            if (dist <= attackRange)
            {
                PerformScytheAttack();
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

    void PerformScytheAttack()
    {
        rb.linearVelocity = Vector3.zero;
        if (currentTarget != null) LookAt(currentTarget.position);

        if (scythePrefab != null && firePoint != null)
        {
            // Týrpaný bel hizasýnda oluþtur (Y + 1)
            Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

            // Düþmanýn baktýðý yöne doðru oluþtur
            GameObject scythe = Instantiate(scythePrefab, spawnPos, transform.rotation);

            // Düþmana yapýþtýr (Dönerken takip etsin)
            scythe.transform.SetParent(this.transform);

            // Gücü aktar
            SimpleWeapon weapon = scythe.GetComponentInChildren<SimpleWeapon>();
            if (weapon != null)
            {
                weapon.owner = this.gameObject;
                weapon.damage = damageAmount;
                weapon.knockback = knockbackForce;
            }

            // Ses
            if (AudioManager.instance != null) AudioManager.instance.PlayScythe();
        }

        isCoolingDown = true;
        cooldownTimer = cooldownTime;
        PickRandomPoint();
    }

    // --- STANDART AI KISIMLARI ---
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
    void WanderBehavior() { MoveTo(wanderPoint, wanderSpeed); if (Vector3.Distance(transform.position, wanderPoint) < 1f) PickRandomPoint(); DetectObstacle(); }
    void DetectObstacle() { if (Physics.Raycast(transform.position + Vector3.up, transform.forward, obstacleCheckDist, obstacleLayer)) PickRandomPoint(); }
    void MoveTo(Vector3 d, float s) { LookAt(d); rb.linearVelocity = (d - transform.position).normalized * s; rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z); }
    void LookAt(Vector3 d) { transform.LookAt(new Vector3(d.x, transform.position.y, d.z)); }
    void PickRandomPoint() { wanderPoint = transform.position + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange)); }
}