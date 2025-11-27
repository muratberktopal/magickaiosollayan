using System.Collections;
using UnityEngine;

public class ElectricNetLogic : MonoBehaviour
{
    [Header("Uçuþ Ayarlarý")]
    public float speed = 20f;      // Hýz
    public float lifeTime = 4f;    // Boþa giderse yok olma süresi

    [Header("Þok Ayarlarý")]
    public float shockDuration = 4f; // Kaç saniye çarpsýn?
    public int damagePerTick = 5;    // Her cýzzlamada kaç vursun?
    public float tickRate = 0.5f;    // Saniyede kaç kere cýzzlasýn?

    private bool hasHit = false;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Havada sonsuza kadar gitmesin
    }

    void Update()
    {
        // Çarpmadýysa uçmaya devam et
        if (!hasHit)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        if (other.CompareTag("Player")) return; // Player'a çarpma
        if (other.gameObject.layer == gameObject.layer) return; // Baþka aðlara çarpma

        // Düþmaný Yakala
        if (other.CompareTag("Enemy"))
        {
            hasHit = true;
            StartCoroutine(ShockRoutine(other.gameObject));
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject); // Duvara çarparsa yok ol
        }
    }

    IEnumerator ShockRoutine(GameObject enemy)
    {
        // 1. YAPIÞ
        transform.SetParent(enemy.transform);
        transform.localPosition = new Vector3(0, 1f, 0); // Kafasýna/Göðsüne yapýþ

        // 2. DÜÞMANI DONDUR
        // (Düþmanýn üzerindeki tüm hareket scriptlerini bulup kapatýyoruz)
        MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            // AI scriptlerini devre dýþý býrak (Ýsimlerinde 'AI' geçenleri kapat)
            if (script.GetType().Name.Contains("AI"))
            {
                script.enabled = false;
            }
        }

        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
        if (enemyRb) enemyRb.isKinematic = true; // Fiziði kilitle (Kaymasýn)

        Animator anim = enemy.GetComponent<Animator>();
        if (anim) anim.speed = 0; // Animasyonu dondur

        // 3. SÜREKLÝ HASAR VER (Þokla)
        HealthSystem hp = enemy.GetComponent<HealthSystem>();
        float timer = 0f;

        while (timer < shockDuration && enemy != null)
        {
            if (hp != null)
            {
                // Hasar ver (Geri tepme 0 olsun, yerinde kalsýn)
                hp.TakeDamage(damagePerTick, enemy.transform.position, 0f);
            }
            yield return new WaitForSeconds(tickRate);
            timer += tickRate;
        }

        // 4. ÇÖZÜLME (Eðer ölmediyse)
        if (enemy != null)
        {
            foreach (var script in scripts)
            {
                if (script.GetType().Name.Contains("AI")) script.enabled = true;
            }
            if (enemyRb) enemyRb.isKinematic = false;
            if (anim) anim.speed = 1;
        }

        // 5. AÐI YOK ET
        Destroy(gameObject);
    }
}