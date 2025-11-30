using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Rigidbody kesin olsun
public class TeslaProjectile : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float speed = 10f;       // Hýz (Inspector'da 0 olmasýn dikkat et!)
    public float lifeTime = 5f;     // Ömür

    [Header("Elektrik Ayarlarý")]
    public float zapRange = 4f;     // Çarpma mesafesi
    public float zapRate = 0.2f;    // Çarpma hýzý
    public int damage = 5;          // Hasar

    private LineRenderer line;
    private float nextZapTime = 0f;
    private Rigidbody rb;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();

        // 1. YERÇEKÝMÝNÝ KODLA KAPAT (Garanti)
        rb.useGravity = false;
        rb.isKinematic = false;

        // 2. YÖNÜ DÜZELT (Yere Paralel Yap)
        // Topun þu anki açýsýný al ama X'ini (Yukarý/Aþaðý eðimini) sýfýrla.
        Vector3 flatForward = transform.forward;
        flatForward.y = 0;
        flatForward.Normalize(); // Yönü tekrar tam boyuta getir

        // 3. MOTORU ÇALIÞTIR
        // Unity 6 ise 'linearVelocity', eski ise 'velocity'
        rb.linearVelocity = flatForward * speed;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Hareket artýk fizikte, biz sadece çarpmayý yönetelim
        if (Time.time >= nextZapTime)
        {
            ZapNearestEnemy();
            nextZapTime = Time.time + zapRate;
        }
    }

    void ZapNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, zapRange);
        Transform bestTarget = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            // Sadece Canlý Düþmanlarý bul
            if (hit.CompareTag("Enemy") && hit.GetComponent<HealthSystem>() != null)
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < closestDist)
                {
                    closestDist = d;
                    bestTarget = hit.transform;
                }
            }
        }

        if (bestTarget != null)
        {
            // Hasar Ver
            HealthSystem hp = bestTarget.GetComponent<HealthSystem>();
            if (hp != null) hp.TakeDamage(damage, transform.position, 1f);

            // Þimþek Efekti
            StartCoroutine(ShowLightning(bestTarget.position));
        }
    }

    IEnumerator ShowLightning(Vector3 targetPos)
    {
        if (line != null)
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, targetPos);
            yield return new WaitForSeconds(0.1f); // Þimþek çakýp sönsün
            line.enabled = false;
        }
    }
}