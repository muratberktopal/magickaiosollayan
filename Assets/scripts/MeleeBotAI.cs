using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeleeBotAI : MonoBehaviour
{
    [Header("Hız ve Hareket")]
    public float moveSpeed = 4f;
    public float detectionRange = 10f; // Görme mesafesi
    public float attackRange = 2.5f;   // Saldırı mesafesi (Kılıç boyuna göre ayarla)

    [Header("Saldırı Ayarları")]
    public GameObject handObject;      // "Hand" objesini buraya sürükle!
    public float attackRate = 1.0f;    // Kaç saniyede bir vursun?
    public float swingSpeed = 0.2f;    // Sallama hızı (Düşükse hızlı sallar)

    // Gizli değişkenler
    private Rigidbody rb;
    private Transform currentTarget;
    private float nextAttackTime = 0f;
    private bool isSwinging = false;
    private Quaternion defaultHandRot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (handObject != null)
            defaultHandRot = handObject.transform.localRotation;
        else
            Debug.LogError("HATA: 'Hand Object' kısmına kılıcı tutan boş objeyi atamadın!");
    }

    void FixedUpdate()
    {
        // Kılıç sallarken hareket etmesin (İstersen burayı silebilirsin)
        if (isSwinging)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        FindTarget(); // Hedef Ara

        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);

            if (distance <= attackRange)
            {
                // Menzildeyse -> Saldır
                Attack();
            }
            else
            {
                // Uzaktaysa -> Kovala
                MoveTowards(currentTarget.position);
            }
        }
        else
        {
            // Hedef yoksa -> Olduğu yerde dursun veya devriye gezsin
            rb.linearVelocity = Vector3.zero;
        }
    }

    void FindTarget()
    {
        // Layer maskesi kullanmadan, etraftaki herkesi tara
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDist = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            // Kendimizi görmeyelim
            if (hit.transform == transform) continue;

            // Player veya Enemy tagi var mı?
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

    void MoveTowards(Vector3 target)
    {
        // Yüzünü dön
        Vector3 lookPos = new Vector3(target.x, transform.position.y, target.z);
        transform.LookAt(lookPos);

        // Yürü
        Vector3 dir = (target - transform.position).normalized;
        Vector3 velocity = dir * moveSpeed;
        velocity.y = rb.linearVelocity.y; // Yerçekimini koru
        rb.linearVelocity = velocity;
    }

    void Attack()
    {
        // Saldırı anında da hedefe dön (Yoksa boşa sallar)
        Vector3 lookPos = new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z);
        transform.LookAt(lookPos);

        rb.linearVelocity = Vector3.zero; // Dur

        if (Time.time >= nextAttackTime)
        {
            StartCoroutine(SwingSword());
            nextAttackTime = Time.time + attackRate;
        }
    }

    IEnumerator SwingSword()
    {
        isSwinging = true;

        if (handObject != null)
        {
            // 1. Hazırlık (Geri çekilme)
            Quaternion startRot = defaultHandRot;
            Quaternion backRot = startRot * Quaternion.Euler(0, -60, 0); // -60 derece geri

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / (swingSpeed * 0.5f);
                handObject.transform.localRotation = Quaternion.Lerp(startRot, backRot, t);
                yield return null;
            }

            // 2. Vuruş (İleri savurma)
            Quaternion hitRot = startRot * Quaternion.Euler(0, 80, 0); // 80 derece ileri
            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / swingSpeed;
                handObject.transform.localRotation = Quaternion.Lerp(backRot, hitRot, t);
                yield return null;
            }

            // 3. Düzelme (Eski yerine dön)
            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / (swingSpeed * 0.5f);
                handObject.transform.localRotation = Quaternion.Lerp(hitRot, startRot, t);
                yield return null;
            }
        }

        isSwinging = false;
    }

    // Editörde görmen için çizgiler
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Görme alanı

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Saldırı alanı
    }
}