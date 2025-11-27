using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ChaosEnemyAI : MonoBehaviour
{
    [Header("Hýz ve Hareket")]
    public float moveSpeed = 3.5f;
    public float wanderSpeed = 2.0f;

    [Header("Mesafe Ayarlarý")]
    public float detectionRange = 12f;
    public float attackRange = 5.0f;    // Zincirler uzadýðý için menzil uzun
    public float patrolRange = 6f;

    [Header("Saldýrý Ayarlarý")]
    public GameObject chaosPrefab;      // ChaosBladesContainer prefabý
    public Transform firePoint;
    public float cooldownTime = 3.0f;   // Güçlü olduðu için biraz daha beklesin

    [Header("Güç Ayarlarý")]
    public int damageAmount = 15;       // Çok hýzlý vurduðu için hasarý biraz düþük tutabilirsin
    public float knockbackForce = 4f;

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
                PerformChaosAttack();
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

    void PerformChaosAttack()
    {
        rb.linearVelocity = Vector3.zero;
        if (currentTarget != null) LookAt(currentTarget.position);

        if (chaosPrefab != null)
        {
            // Bel hizasýnda oluþtur
            Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

            GameObject chaos = Instantiate(chaosPrefab, spawnPos, transform.rotation);

            // Düþmana yapýþtýr (Yürürse onunla gelsin)
            chaos.transform.SetParent(this.transform);

            // Gücü aktar (Çocuklardaki tüm silah scriptlerine)
            SimpleWeapon[] weapons = chaos.GetComponentsInChildren<SimpleWeapon>();
            foreach (var weapon in weapons)
            {
                weapon.owner = this.gameObject;
                weapon.damage = damageAmount;
                weapon.knockback = knockbackForce;
            }

            // Ses (Slash sesi veya özel zincir sesi)
            if (AudioManager.instance != null) AudioManager.instance.PlaySlash();
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