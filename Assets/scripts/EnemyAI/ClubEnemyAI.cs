using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ClubEnemyAI : MonoBehaviour
{
    [Header("Hız ve Hareket")]
    public float moveSpeed = 3.0f;
    public float wanderSpeed = 1.5f;
    public float detectionRange = 10f;

    [Header("Saldırı Ayarları")]
    public GameObject clubPrefab;
    public Transform firePoint;
    public float attackRange = 2.0f;
    public float cooldownTime = 2.5f;

    [Header("Güç Ayarları")]
    public int damageAmount = 15;
    public float knockbackForce = 8f;

    // --- YENİ EKLENEN: ENGEL ALGILAMA ---
    [Header("Engel Algılama")]
    public float obstacleCheckDist = 1.5f; // Ne kadar önünü kontrol etsin?
    public LayerMask obstacleLayer;        // Neyi engel saysın? (Duvar, Default vb.)
    // ------------------------------------

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
                PerformClubAttack();
            }
            else
            {
                // Kovalarken de duvara toslamasın
                MoveTo(currentTarget.position, moveSpeed);
            }
        }
        else
        {
            WanderBehavior();
        }
    }

    // ... (PerformClubAttack aynı kalıyor) ...
    void PerformClubAttack()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClub();

        rb.linearVelocity = Vector3.zero;
        if (currentTarget != null) LookAt(currentTarget.position);

        if (clubPrefab != null && firePoint != null)
        {
            GameObject club = Instantiate(clubPrefab, firePoint.position, transform.rotation);
            SimpleWeapon weapon = club.GetComponentInChildren<SimpleWeapon>();
            if (weapon != null)
            {
                weapon.owner = this.gameObject;
                weapon.damage = damageAmount;
                weapon.knockback = knockbackForce;
            }
            Destroy(club, 0.4f);

            if (AudioManager.instance != null) AudioManager.instance.PlayClub();
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

            if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < closestDist) { closestDist = d; bestTarget = hit.transform; }
            }
        }
        currentTarget = bestTarget;
    }

    void CooldownLogic()
    {
        cooldownTimer -= Time.fixedDeltaTime;
        WanderBehavior();
        if (cooldownTimer <= 0) isCoolingDown = false;
    }

    void WanderBehavior()
    {
        MoveTo(wanderPoint, wanderSpeed);

        // Hedefe vardık mı?
        if (Vector3.Distance(transform.position, wanderPoint) < 1f)
        {
            PickRandomPoint();
        }

        // --- YENİ: Önümde duvar var mı? ---
        DetectObstacle();
        // ----------------------------------
    }

    // --- YENİ FONKSİYON: Engel Algılama ---
    void DetectObstacle()
    {
        // Işını yerden biraz yukarıdan atıyoruz
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        // Scene ekranında kırmızı çizgi çiz (Debug için)
        Debug.DrawRay(rayOrigin, transform.forward * obstacleCheckDist, Color.red);

        // Önümüzde "Obstacle Layer"a ait bir şey var mı?
        if (Physics.Raycast(rayOrigin, transform.forward, obstacleCheckDist, obstacleLayer))
        {
            // Duvar var! Hemen yeni bir yere dön.
            PickRandomPoint();
        }
    }

    void MoveTo(Vector3 d, float s)
    {
        LookAt(d);
        rb.linearVelocity = (d - transform.position).normalized * s;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    void LookAt(Vector3 d) { transform.LookAt(new Vector3(d.x, transform.position.y, d.z)); }
    void PickRandomPoint() { wanderPoint = transform.position + new Vector3(Random.Range(-6f, 6f), 0, Random.Range(-6f, 6f)); }
}