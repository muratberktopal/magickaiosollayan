using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [Header("Hız Ayarları")]
    public float moveSpeed = 3f;
    public float chaseRange = 10f;     // Kovalama menzili
    public float separationForce = 5f; // Birbirlerini itme gücü (Çarpışınca)

    [Header("Hedefler")]
    public Transform player;           // Oyuncu
    public string enemyTag = "Enemy";  // Diğer düşmanların etiketi

    [Header("Engel Algılama")]
    public float obstacleCheckDist = 1.5f;
    public LayerMask obstacleLayer;

    private Rigidbody rb;
    private Transform currentTarget;   // O an kovaladığı şey (Player veya Düşman)
    private Vector3 randomPatrolPoint;
    private bool onPatrol = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Rastgele devriye noktası seçerek başla
        PickNewPatrolPoint();

        // Eğer player atanmadıysa sahnede bulmaya çalış
        if (player == null && GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // 1. HEDEF BELİRLEME
        FindBestTarget();

        // 2. HEDEFE GİTME
        if (currentTarget != null)
        {
            MoveTowards(currentTarget.position);
        }
        else
        {
            Patrol();
        }

        // 3. AYRILMA (SEPARATION) - Diğer düşmanlarla iç içe girmeyi önle
        AvoidCrowding();
    }

    void FindBestTarget()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        // Öncelik 1: Oyuncu menzildeyse ona odaklan
        if (distToPlayer < chaseRange)
        {
            currentTarget = player;
            onPatrol = false;
            return;
        }

        // Öncelik 2: Oyuncu yoksa, yakındaki diğer düşmanı kovalamaya çalış (Kaos modu)
        // Bu biraz işlemci yer ama istediğin "birbirini kovalama" olayını yapar.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float closestDist = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform == transform) continue; // Kendini kovalama

            float d = Vector3.Distance(transform.position, enemy.transform.position);
            if (d < chaseRange && d < closestDist)
            {
                closestDist = d;
                closestEnemy = enemy.transform;
            }
        }

        if (closestEnemy != null)
        {
            currentTarget = closestEnemy;
            onPatrol = false;
        }
        else
        {
            currentTarget = null; // Kimse yoksa devriye gez
            onPatrol = true;
        }
    }

    void MoveTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;

        // Y eksenini koru (Havadaki düşman yere çakılmasın)
        Vector3 velocity = direction * moveSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

        // Yüzünü dön
        Vector3 lookPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        transform.LookAt(lookPos);

        DetectObstacle(); // Duvar kontrolü
    }

    // Çarpışınca uzaklaşma mantığı
    void AvoidCrowding()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (GameObject enemy in enemies)
        {
            if (enemy == this.gameObject) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Eğer başka bir düşmana 1.5 birimden fazla yaklaşırsam
            if (distance < 1.5f)
            {
                // Ondan zıt yöne doğru bir itme kuvveti hesapla
                Vector3 pushDir = transform.position - enemy.transform.position;
                rb.AddForce(pushDir.normalized * separationForce, ForceMode.Acceleration);
            }
        }
    }

    void Patrol()
    {
        MoveTowards(randomPatrolPoint);
        if (Vector3.Distance(transform.position, randomPatrolPoint) < 1f)
        {
            PickNewPatrolPoint();
        }
    }

    void PickNewPatrolPoint()
    {
        float randomX = Random.Range(-10, 10);
        float randomZ = Random.Range(-10, 10);
        randomPatrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }

    void DetectObstacle()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(rayOrigin, transform.forward, obstacleCheckDist, obstacleLayer))
        {
            if (onPatrol) PickNewPatrolPoint();
        }
    }
}