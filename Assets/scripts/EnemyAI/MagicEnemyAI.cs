using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class MagicEnemyAI : MonoBehaviour
{
    [Header("H�z ve Hareket")]
    public float moveSpeed = 3.0f;
    public float wanderSpeed = 2.0f;

    [Header("Mesafe Ayarlar�")]
    public float detectionRange = 12f; // Uzaktan g�rs�n
    public float attackRange = 7f;     // Uzaktan s�ks�n (Dibine girmesin)
    public float patrolRange = 6f;

    [Header("Sald�r� Ayarlar�")]
    public GameObject magicBallPrefab;  // Mavi B�y� Prefab� (Magicball)
    public Transform firePoint;         // ��k�� noktas�
    public float cooldownTime = 3f;     // Ate� ettikten sonra 3 sn gezsin

    // Gizli De�i�kenler
    private Rigidbody rb;
    private Animator animator;
    private Transform currentTarget;    // Hedef (Player veya Enemy)

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
        // 1. AN�MASYON KONTROL�
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        if (animator != null) animator.SetBool("IsMoving", isMoving);

        // 2. HEDEF BULMA (Battle Royale)
        FindClosestTarget();

        // 3. DAVRANI� A�ACI
        if (isCoolingDown)
        {
            // Sald�r� yapt�, so�uma s�resinde (Geziniyor)
            CooldownLogic();
        }
        else if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.position);

            // Menzile girdiyse SALDIR
            if (dist <= attackRange)
            {
                PerformMagicAttack();
            }
            else
            {
                // Uzaktaysa KOVALA
                MoveTo(currentTarget.position, moveSpeed);
            }
        }
        else
        {
            // Hedef yoksa SERSER� MODU
            WanderBehavior();
        }
    }

    void PerformMagicAttack()
    {
        // Dur ve hedefe d�n
        rb.linearVelocity = Vector3.zero;
        if (currentTarget != null) LookAt(currentTarget.position);

        // B�y�y� olu�tur
        if (magicBallPrefab != null && firePoint != null)
        {
            // B�y�y� karakterin bakt��� y�ne do�ru olu�tur
            GameObject magic = Instantiate(magicBallPrefab, firePoint.position, transform.rotation);

            // --- SAH�PL�K AYARI ---
            SimpleWeapon weapon = magic.GetComponent<SimpleWeapon>();
            if (weapon != null)
            {
                weapon.owner = this.gameObject; // "Bu b�y� benim, bana vurma" de
                weapon.damage = 15; // B�y�c� hasar�
            }

            // --- HAREKET AYARI (�nemli) ---
            // FireballProjectile scriptini bul ve uyand�r
            FireballProjectile projScript = magic.GetComponent<FireballProjectile>();
            if (projScript != null)
            {
                // Düşman için hızı DÜŞÜRÜYORUZ (Örn: 8 veya 10 yap)
                // Senin Player hızın 40 ise bu 8 olmalı ki oyuncu kaçabilsin.
                projScript.speed = 17f;

                projScript.enabled = true; // Sonra çalıştır
            }
            else
            {
                // Yedek plan (Rigidbody varsa)
                Rigidbody projRb = magic.GetComponent<Rigidbody>();
                if (projRb != null)
                {
                    projRb.isKinematic = false;
                    projRb.linearVelocity = transform.forward * 8f; // Burayı da 8 yap
                }
            }

            // --- SES �AL ---
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayMagic();
            }
        }

        // Dinlenme moduna ge�
        isCoolingDown = true;
        cooldownTimer = cooldownTime;
        PickRandomPoint(); // Ka�acak yer se�
    }

    // --- YARDIMCI FONKS�YONLAR ---

    void FindClosestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue; // Kendini ge�
            if (hit.GetComponent<HealthSystem>() == null) continue; // Can� olmayan� ge�

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

    void CooldownLogic()
    {
        cooldownTimer -= Time.fixedDeltaTime;
        WanderBehavior(); // Beklerken durmas�n, gezsin

        if (cooldownTimer <= 0) isCoolingDown = false;
    }

    void WanderBehavior()
    {
        MoveTo(wanderPoint, wanderSpeed);
        if (Vector3.Distance(transform.position, wanderPoint) < 1f) PickRandomPoint();
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

    // Edit�rde menzili g�rmek i�in
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue; // B�y�c� menzili Mavi olsun
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}