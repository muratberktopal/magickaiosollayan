using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class WhipEnemyAI : MonoBehaviour
{
    [Header("Hız ve Hareket")]
    public float moveSpeed = 3.5f;
    public float wanderSpeed = 2.0f;

    [Header("Mesafe Ayarları")]
    public float detectionRange = 12f;  // Görme menzili
    public float attackRange = 4.5f;    // KIRBAÇ MENZİLİ (Kılıçtan uzun, Büyüden kısa)
    public float patrolRange = 6f;

    [Header("Saldırı Ayarları")]
    public GameObject whipPrefab;       // Kırbaç Prefabı (WhipContainer veya WhipAttack)
    public Transform firePoint;         // Çıkış noktası
    public float cooldownTime = 2.0f;   // Atıştan sonra bekleme

    [Header("Güç Ayarları")]
    public int damageAmount = 15;
    public float knockbackForce = 5f;

    [Header("Engel Algılama")]
    public float obstacleCheckDist = 1.5f;
    public LayerMask obstacleLayer;

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
                PerformWhipAttack();
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

    void PerformWhipAttack()
    {
        rb.linearVelocity = Vector3.zero; // Dur
        if (currentTarget != null) LookAt(currentTarget.position); // Hedefe dön

        if (whipPrefab != null && firePoint != null)
        {
            // Kırbacı oluştur
            GameObject whip = Instantiate(whipPrefab, firePoint.position, transform.rotation);

            // Kırbacı düşmana yapıştır (Child yap) ki düşman dönerse kırbaç da dönsün
            whip.transform.SetParent(this.transform);

            // --- SAHİPLİK VE GÜÇ AYARI ---
            // Hem ana objede hem çocuklarda ara (Prefab yapına göre değişebilir)
            SimpleWeapon weapon = whip.GetComponent<SimpleWeapon>();
            if (weapon == null) weapon = whip.GetComponentInChildren<SimpleWeapon>();

            if (weapon != null)
            {
                weapon.owner = this.gameObject; // "Sahibi benim" de
                weapon.damage = damageAmount;
                weapon.knockback = knockbackForce;
            }
            // -----------------------------

            // Ses Çal (Özel Kırbaç sesi)
            if (AudioManager.instance != null) AudioManager.instance.PlayWhip();
        }

        isCoolingDown = true;
        cooldownTimer = cooldownTime;
        PickRandomPoint();
    }

    // --- STANDART AI FONKSİYONLARI ---

    void FindClosestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            if (hit.GetComponent<HealthSystem>() == null) continue;

            // Basit Battle Royale mantığı: Herkese saldır
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
        Gizmos.color = Color.magenta; // Kırbaç menzili Mor olsun
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}