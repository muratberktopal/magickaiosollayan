using System.Collections;
using UnityEngine;

public class TeslaProjectile : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float speed = 5f;        // Yavaþ gitsin ki çok kiþiyi çarpsýn
    public float lifeTime = 4f;     // Ne kadar sürsün?

    [Header("Elektrik Ayarlarý")]
    public float zapRange = 4f;     // Ne kadar yakýndakileri çarpsýn?
    public float zapRate = 0.2f;    // Saniyede kaç kere çarpsýn? (Hýzlý)
    public int damage = 5;          // Çarpma baþý hasar (Az ama seri)

    private LineRenderer line;
    private float nextZapTime = 0f;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 1. ÝLERLEME (Yavaþça süzül)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // 2. ÇARPMA ZAMANI GELDÝ MÝ?
        if (Time.time >= nextZapTime)
        {
            ZapNearestEnemy();
            nextZapTime = Time.time + zapRate;
        }
    }

    void ZapNearestEnemy()
    {
        // Etraftaki herkesi bul
        Collider[] hits = Physics.OverlapSphere(transform.position, zapRange);
        Transform bestTarget = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            // Sadece Düþmanlarý hedef al (Kendine veya Player'a çarpma)
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

        // Hedef varsa ÇARP
        if (bestTarget != null)
        {
            // Hasar ver
            HealthSystem hp = bestTarget.GetComponent<HealthSystem>();
            if (hp != null) hp.TakeDamage(damage, transform.position, 1f);

            // Görsel efekt (Yýldýrým çiz)
            StartCoroutine(ShowLightning(bestTarget.position));
        }
    }

    IEnumerator ShowLightning(Vector3 targetPos)
    {
        if (line != null)
        {
            line.enabled = true;
            line.SetPosition(0, transform.position); // Topun merkezi
            line.SetPosition(1, targetPos);          // Düþman

            // 0.1 saniye gösterip kapat (Þimþek etkisi)
            yield return new WaitForSeconds(0.1f);

            line.enabled = false;
        }
    }
}