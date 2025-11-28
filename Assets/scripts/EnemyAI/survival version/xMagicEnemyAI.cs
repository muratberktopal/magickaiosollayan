using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class xMagicEnemyAI : MonoBehaviour
{
    [Header("Hýz ve Hareket")]
    public float moveSpeed = 3.0f;
    public float wanderSpeed = 2.0f;
    public float detectionRange = 12f;
    public float attackRange = 7f;
    public float patrolRange = 6f;

    [Header("Saldýrý Ayarlarý")]
    public GameObject magicBallPrefab;
    public Transform firePoint;
    public float cooldownTime = 3f;

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

        FindClosestTarget(); // Sadece Player

        if (isCoolingDown)
        {
            CooldownLogic();
        }
        else if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.position);
            if (dist <= attackRange) PerformMagicAttack();
            else MoveTo(currentTarget.position, moveSpeed);
        }
        else
        {
            WanderBehavior();
        }
    }

    void PerformMagicAttack()
    {
        rb.linearVelocity = Vector3.zero;
        if (currentTarget != null) LookAt(currentTarget.position);

        if (magicBallPrefab != null && firePoint != null)
        {
            GameObject magic = Instantiate(magicBallPrefab, firePoint.position, transform.rotation);
            SimpleWeapon weapon = magic.GetComponent<SimpleWeapon>();
            if (weapon != null) { weapon.owner = this.gameObject; weapon.damage = 15; }

            FireballProjectile projScript = magic.GetComponent<FireballProjectile>();
            if (projScript != null) { projScript.speed = 17f; projScript.enabled = true; }
            else
            {
                Rigidbody projRb = magic.GetComponent<Rigidbody>();
                if (projRb != null) { projRb.isKinematic = false; projRb.linearVelocity = transform.forward * 15f; }
            }

            if (AudioManager.instance != null) AudioManager.instance.PlayMagic();
        }

        isCoolingDown = true;
        cooldownTimer = cooldownTime;
        PickRandomPoint();
    }

    void FindClosestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            if (hit.GetComponent<HealthSystem>() == null) continue;

            // --- SADECE PLAYER (Survival) ---
            if (hit.CompareTag("Player"))
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < closestDist) { closestDist = d; bestTarget = hit.transform; }
            }
        }
        currentTarget = bestTarget;
    }

    // (Yardýmcý fonksiyonlar ayný)
    void CooldownLogic() { cooldownTimer -= Time.fixedDeltaTime; WanderBehavior(); if (cooldownTimer <= 0) isCoolingDown = false; }
    void WanderBehavior() { MoveTo(wanderPoint, wanderSpeed); if (Vector3.Distance(transform.position, wanderPoint) < 1f) PickRandomPoint(); DetectObstacle(); }
    void DetectObstacle() { if (Physics.Raycast(transform.position + Vector3.up, transform.forward, obstacleCheckDist, obstacleLayer)) PickRandomPoint(); }
    void MoveTo(Vector3 d, float s) { LookAt(d); rb.linearVelocity = (d - transform.position).normalized * s; rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z); }
    void LookAt(Vector3 d) { transform.LookAt(new Vector3(d.x, transform.position.y, d.z)); }
    void PickRandomPoint() { float x = Random.Range(-patrolRange, patrolRange); float z = Random.Range(-patrolRange, patrolRange); wanderPoint = transform.position + new Vector3(x, 0, z); }
}