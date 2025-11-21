using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeleeBotAI : MonoBehaviour
{
    enum State { Roaming, ChasingLoot, Attacking }
    State currentState;

    [Header("Genel Ayarlar")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("K�l�� Ayarlar�")]
    public GameObject swordObject;     // Botun elindeki k�l�� objesi
    public float attackRange = 2.0f;   // Ne kadar yakla��nca vursun? (K�sa mesafe)
    public float attackRate = 1.0f;    // Saniyede ka� vuru�?
    public float swingSpeed = 0.2f;    // K�l�c� savurma h�z�

    [Header("Alg�lama")]
    public float detectionRadius = 10f;
    public LayerMask targetLayer;
    public LayerMask lootLayer;

    private Rigidbody rb;
    private Transform currentTarget;
    private float nextAttackTime = 0f;
    private bool isSwinging = false;   // �u an k�l�� sall�yor mu?
    private Quaternion defaultSwordRot; // K�l�c�n duru� pozisyonu

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentState = State.Roaming;

        if (swordObject != null)
        {
            defaultSwordRot = swordObject.transform.localRotation;
            // K�l�c�n sahibini (kendimizi) tan�tal�m ki bize vurmas�n
            var damageScript = swordObject.GetComponent<SwordDamage>();
            if (damageScript) damageScript.owner = this.gameObject;
        }
    }

    void FixedUpdate()
    {
        if (isSwinging) return; // K�l�� sallarken hareket etmesin (Opsiyonel)

        DecideState();

        switch (currentState)
        {
            case State.Roaming:
                Patrol();
                break;
            case State.ChasingLoot:
                MoveToTarget();
                break;
            case State.Attacking:
                AttackBehavior();
                break;
        }
    }

    void DecideState()
    {
        // Yak�ndaki d��manlar� bul
        Transform potentialEnemy = FindClosestTarget(targetLayer, "Enemy", "Player");

        // Sald�r� menziline girdiyse
        if (potentialEnemy != null && Vector3.Distance(transform.position, potentialEnemy.position) <= attackRange)
        {
            currentTarget = potentialEnemy;
            currentState = State.Attacking;
            return;
        }
        // Uzaktaysa ama g�r�yorsa ona do�ru ko�mal� (Chase eklendi)
        else if (potentialEnemy != null)
        {
            currentTarget = potentialEnemy;
            currentState = State.ChasingLoot; // Loot kovalar gibi d��mana ko�sun
            return;
        }

        // D��man yoksa Loot ara
        Transform potentialLoot = FindClosestTarget(lootLayer, "Loot");
        if (potentialLoot != null)
        {
            currentTarget = potentialLoot;
            currentState = State.ChasingLoot;
        }
        else
        {
            currentState = State.Roaming;
            currentTarget = null;
        }
    }

    void MoveToTarget()
    {
        if (currentTarget == null) return;
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        Move(direction);
        LookAt(currentTarget.position);
    }

    void AttackBehavior()
    {
        if (currentTarget == null) return;

        rb.linearVelocity = Vector3.zero; // Vururken dur
        LookAt(currentTarget.position);

        if (Time.time >= nextAttackTime)
        {
            StartCoroutine(PerformSwordSwing());
            nextAttackTime = Time.time + attackRate;
        }
    }

    // K�l�c� Kod ile Sallama Efekti (Animasyonun yoksa bunu kullan)
    IEnumerator PerformSwordSwing()
    {
        isSwinging = true;

        // 1. Geri �ekil (Haz�rl�k)
        Quaternion startRot = swordObject.transform.localRotation;
        Quaternion windupRot = startRot * Quaternion.Euler(0, -45, 0);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / (swingSpeed * 0.5f);
            swordObject.transform.localRotation = Quaternion.Lerp(startRot, windupRot, t);
            yield return null;
        }

        // 2. �leri Savur (Vuru�)
        Quaternion swingRot = startRot * Quaternion.Euler(0, 90, 0); // 90 derece savur
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / swingSpeed;
            swordObject.transform.localRotation = Quaternion.Lerp(windupRot, swingRot, t);
            yield return null;
        }

        // 3. Eski yerine d�n
        swordObject.transform.localRotation = defaultSwordRot;
        isSwinging = false;
    }

    void Patrol()
    {
        Move(transform.forward);
        if (Random.value < 0.02f) transform.Rotate(0, Random.Range(-90, 90), 0);
        if (Physics.Raycast(transform.position, transform.forward, 2f)) transform.Rotate(0, 150, 0);
    }

    void Move(Vector3 dir)
    {
        Vector3 newVel = dir * moveSpeed;
        newVel.y = rb.linearVelocity.y;
        rb.linearVelocity = newVel;
    }

    void LookAt(Vector3 targetPos)
    {
        Vector3 lookPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        transform.LookAt(lookPos);
    }

    Transform FindClosestTarget(LayerMask mask, params string[] tags)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, mask);
        Transform bestTarget = null;
        float closestDist = Mathf.Infinity;
        foreach (Collider hit in hits)
        {
            if (hit.transform == transform) continue;
            bool tagMatch = false;
            foreach (string t in tags) if (hit.CompareTag(t)) { tagMatch = true; break; }
            if (!tagMatch) continue;

            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closestDist) { closestDist = dist; bestTarget = hit.transform; }
        }
        return bestTarget;
    }
}