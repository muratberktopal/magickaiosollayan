using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MeleeBotAI : MonoBehaviour
{
    [Header("Hız Ayarları")]
    public float moveSpeed = 4f;       // Koşma hızı
    public float patrolSpeed = 2f;     // Gezinme hızı

    [Header("Algılama")]
    public float detectionRange = 10f; // Görme mesafesi
    public float patrolRange = 8f;     // Rastgele gezme alanı

    [Header("Saldırı")]
    public GameObject handObject;      // Kılıcı tutan "Hand" objesi
    public float attackRange = 2.5f;
    public float attackRate = 1f;

    // Gizli Değişkenler
    private Rigidbody rb;
    private Transform currentTarget;
    private Vector3 patrolPoint;
    private bool isPatrolling = false;
    private float nextAttackTime = 0f;
    private bool isSwinging = false;
    private Quaternion defaultHandRot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PickNewPatrolPoint(); // İlk devriye noktası

        if (handObject != null)
            defaultHandRot = handObject.transform.localRotation;
    }

    void FixedUpdate()
    {
        if (isSwinging)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        FindTarget(); // Hedef Ara

        if (currentTarget != null)
        {
            // --- SAVAŞ MODU ---
            float dist = Vector3.Distance(transform.position, currentTarget.position);

            if (dist <= attackRange)
            {
                AttackBehavior();
            }
            else
            {
                MoveTo(currentTarget.position, moveSpeed);
            }
        }
        else
        {
            // --- GEZİNME (PATROL) MODU ---
            PatrolBehavior();
        }
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            // 1. Kendini geç
            if (hit.gameObject == gameObject) continue;

            // 2. HATA DÜZELTİLDİ: Artık "Health" değil "HealthSystem" arıyor
            if (hit.GetComponent<HealthSystem>() == null) continue;

            // 3. Player veya Enemy etiketli mi?
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

    void PatrolBehavior()
    {
        MoveTo(patrolPoint, patrolSpeed);

        if (Vector3.Distance(transform.position, patrolPoint) < 1f)
        {
            PickNewPatrolPoint();
        }
    }

    void PickNewPatrolPoint()
    {
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);
        patrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }

    void AttackBehavior()
    {
        rb.linearVelocity = Vector3.zero;
        LookAt(currentTarget.position);

        if (Time.time >= nextAttackTime && handObject != null)
        {
            StartCoroutine(SwingSword());
            nextAttackTime = Time.time + attackRate;
        }
    }

    void MoveTo(Vector3 target, float speed)
    {
        LookAt(target);
        Vector3 dir = (target - transform.position).normalized;
        Vector3 vel = dir * speed;
        vel.y = rb.linearVelocity.y;
        rb.linearVelocity = vel;
    }

    void LookAt(Vector3 target)
    {
        Vector3 lookPos = new Vector3(target.x, transform.position.y, target.z);
        transform.LookAt(lookPos);
    }

    IEnumerator SwingSword()
    {
        isSwinging = true;
        float t = 0;
        Quaternion start = defaultHandRot;
        Quaternion back = start * Quaternion.Euler(0, -45, 0);

        // Geri çekil
        while (t < 1) { t += Time.deltaTime * 5; handObject.transform.localRotation = Quaternion.Lerp(start, back, t); yield return null; }

        // İleri vur
        t = 0;
        Quaternion hit = start * Quaternion.Euler(0, 90, 0);
        while (t < 1) { t += Time.deltaTime * 10; handObject.transform.localRotation = Quaternion.Lerp(back, hit, t); yield return null; }

        // Dön
        t = 0;
        while (t < 1) { t += Time.deltaTime * 5; handObject.transform.localRotation = Quaternion.Lerp(hit, start, t); yield return null; }

        isSwinging = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}