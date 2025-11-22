using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class SlashEnemyAI : MonoBehaviour
{
    [Header("Hız Ayarları")]
    public float moveSpeed = 3.5f;
    public float wanderSpeed = 2f;

    [Header("Mesafe Ayarları")]
    public float detectionRange = 10f;  // Kimi görecek?
    public float attackRange = 2.5f;
    public float patrolRange = 6f;

    [Header("Saldırı")]
    public GameObject slashPrefab;
    public Transform firePoint;
    public float cooldownTime = 2f;

    // Gizli Değişkenler
    private Rigidbody rb;
    private Animator animator;
    private Transform currentTarget; // O anki hedef (Player veya Enemy olabilir)

    private bool isCoolingDown = false;
    private float cooldownTimer = 0f;
    private Vector3 wanderPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Animator yoksa çocuklarda ara
        if (animator == null) animator = GetComponentInChildren<Animator>();

        PickRandomPoint();
    }

    void FixedUpdate()
    {
        // 1. ANİMASYON KONTROLÜ
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        if (animator != null) animator.SetBool("IsMoving", isMoving);

        // 2. HEDEF TARAMA (En yakındaki kurbanı bul)
        FindClosestTarget();

        // 3. KARAR MEKANİZMASI
        if (isCoolingDown)
        {
            // Saldırı sonrası dinlenme
            CooldownLogic();
        }
        else if (currentTarget != null)
        {
            // Hedef varsa -> SAVAŞ MODU
            float dist = Vector3.Distance(transform.position, currentTarget.position);
            CombatLogic(dist);
        }
        else
        {
            // Hedef yoksa -> SERSERİ MODU
            WanderBehavior();
        }
    }

    // --- YENİ HEDEF BULMA SİSTEMİ ---
    void FindClosestTarget()
    {
        // Etraftaki herkesi tara (Detection Range içindekileri)
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);

        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            // 1. Kendimi hedef alamam
            if (hit.gameObject == gameObject) continue;

            // 2. Ölüleri hedef alamam (HealthSystem'i yoksa geç)
            if (hit.GetComponent<HealthSystem>() == null) continue;

            // 3. Player MI yoksa Başka Düşman MI?
            if (hit.CompareTag("Player") || hit.CompareTag("Enemy"))
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);

                // En yakındakini seç
                if (d < closestDist)
                {
                    closestDist = d;
                    bestTarget = hit.transform;
                }
            }
        }
        // Hedefi güncelle
        currentTarget = bestTarget;
    }

    void CombatLogic(float distance)
    {
        if (distance <= attackRange)
        {
            PerformSlashAttack();
        }
        else
        {
            MoveTo(currentTarget.position, moveSpeed);
        }
    }

    void WanderBehavior()
    {
        MoveTo(wanderPoint, wanderSpeed);

        if (Vector3.Distance(transform.position, wanderPoint) < 1f)
        {
            PickRandomPoint();
        }
    }

    void CooldownLogic()
    {
        cooldownTimer -= Time.fixedDeltaTime;
        WanderBehavior(); // Dinlenirken de gezsin

        if (cooldownTimer <= 0)
        {
            isCoolingDown = false;
        }
    }

    void PerformSlashAttack()
    {
        rb.linearVelocity = Vector3.zero;

        if (currentTarget != null)
            LookAt(currentTarget.position);

        if (slashPrefab != null && firePoint != null)
        {
            Quaternion rotasyon = Quaternion.Euler(90, transform.eulerAngles.y, 0);
            GameObject slash = Instantiate(slashPrefab, firePoint.position, rotasyon);

            SimpleWeapon weapon = slash.GetComponent<SimpleWeapon>();
            if (weapon != null)
            {
                weapon.owner = this.gameObject; // Sahibi benim, bana vurma
                weapon.damage = 10;
            }
            Destroy(slash, 0.3f);
        }

        isCoolingDown = true;
        cooldownTimer = cooldownTime;
        PickRandomPoint();
    }

    void MoveTo(Vector3 dest, float speed)
    {
        LookAt(dest);
        Vector3 dir = (dest - transform.position).normalized;
        Vector3 vel = dir * speed;
        vel.y = rb.linearVelocity.y;
        rb.linearVelocity = vel;
    }

    void LookAt(Vector3 dest)
    {
        Vector3 lookPos = new Vector3(dest.x, transform.position.y, dest.z);
        transform.LookAt(lookPos);
    }

    void PickRandomPoint()
    {
        float x = Random.Range(-patrolRange, patrolRange);
        float z = Random.Range(-patrolRange, patrolRange);
        wanderPoint = transform.position + new Vector3(x, 0, z);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}