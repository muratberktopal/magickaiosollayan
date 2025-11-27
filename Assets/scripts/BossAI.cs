using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    public enum BossType { Crusher, Dasher, Shooter }

    [Header("Boss Tipi")]
    public BossType bossType;

    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 3.5f;     // Normal yürüme hýzý
    public float detectionRange = 20f;
    public float attackRange = 3f;     // Normal vuruþ mesafesi

    [Header("Dasher Özel Ayarlarý")]
    public float dashSpeed = 15f;      // Atýlma hýzý (Çok aþýrý olmasýn)
    public float dashDuration = 0.5f;  // Ne kadar süre kayacak?
    public float dashWarningTime = 0.8f; // Atýlmadan önce ne kadar beklesin? (Dodge þansý)

    [Header("Normal Saldýrý Ayarlarý")]
    public float normalAttackRate = 2f;
    public int normalDamage = 15;
    public GameObject normalHitEffect;

    [Header("Özel Yetenek (Skill) Ayarlarý")]
    public float skillCooldown = 6f;
    public float skillRange = 8f;     // Dasher için biraz uzak olmalý (Uzaktan atýlsýn)
    public GameObject skillEffectPrefab; // Crusher için efekt (Dasher için boþ kalabilir)
    public Transform firePoint;

    [Header("Yetenek Hasar Ayarlarý")]
    public int skillDamage = 40;
    public float skillKnockback = 20f; // Dasher çarparsa çok fýrlatsýn

    // Durum Kontrolü
    private Transform player;
    private Rigidbody rb;
    private Animator animator;

    private float nextSkillTime = 0f;
    private float nextNormalTime = 0f;

    private bool isUsingSkill = false;       // Skill genel kontrolü
    private bool isDashing = false;          // Sadece Dash atarken true olur
    private bool isAttackingNormal = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Eðer Skill kullanýyorsa veya Normal saldýrý yapýyorsa hareket kodunu çalýþtýrma
        if (isUsingSkill || isAttackingNormal) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Animasyon: Hýz varsa yürü
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        if (animator != null) animator.SetBool("IsMoving", isMoving);

        // --- YAPAY ZEKA KARAR MEKANÝZMASI ---

        // 1. ÖZEL YETENEK (DASH) - Öncelikli
        // Cooldown dolduysa VE Menzile girdiysek
        if (Time.time >= nextSkillTime && dist <= skillRange)
        {
            StartCoroutine(UseSpecialSkill());
        }
        // 2. NORMAL SALDIRI / KOVALAMA - (Skill cooldown'daysa burasý çalýþýr)
        else
        {
            if (dist <= attackRange)
            {
                // Menzildeyiz, dur ve normal vur
                rb.linearVelocity = Vector3.zero;
                LookAt(player.position);

                if (Time.time >= nextNormalTime)
                {
                    StartCoroutine(DoNormalAttack());
                }
            }
            else
            {
                // Menzilde deðiliz, kovala
                MoveTo(player.position);
            }
        }
    }

    // --- ÖZEL YETENEK FONKSÝYONU ---
    IEnumerator UseSpecialSkill()
    {
        isUsingSkill = true;
        rb.linearVelocity = Vector3.zero; // Önce bir dur
        LookAt(player.position);

        if (animator != null) animator.SetTrigger("SkillAttack"); // Hazýrlýk animasyonu

        // --- BOSS TÝPÝNE GÖRE DAVRANIÞ ---
        switch (bossType)
        {
            // --- CRUSHER (Eski kod aynen kalýyor) ---
            case BossType.Crusher:
                // ... (Eski Crusher kodlarýn burada olacak, yer kaplamasýn diye kýsalttým) ...
                // Crusher için: Hazýrlýk bekle -> Efekt çýkar -> Bekle -> Bitir.
                rb.isKinematic = true;
                yield return new WaitForSeconds(1.0f);
                if (skillEffectPrefab != null) { /* Efekt Kodu */ }
                yield return new WaitForSeconds(0.5f);
                rb.isKinematic = false;
                break;

            // --- DASHER (YENÝ MANTIK) ---
            case BossType.Dasher:

                // 1. UYARI (Telegraphing)
                // Oyuncu "Geliyor!" deyip kaçabilsin diye bekleme
                Debug.Log("Dasher: Hedefe kilitlendi, atýlmaya hazýrlanýyor...");
                yield return new WaitForSeconds(dashWarningTime);

                // 2. HEDEF BELÝRLEME
                // Tam atýlmadan hemen önce yönü son kez güncelle (Homing missile olmasýn diye dash sýrasýnda dönmeyecek)
                Vector3 dashDir = (player.position - transform.position).normalized;
                dashDir.y = 0; // Havaya uçmasýn

                // 3. ATILMA (DASH)
                isDashing = true; // Çarpýþma hasarýný aç
                float startTime = Time.time;

                while (Time.time < startTime + dashDuration)
                {
                    // AddForce yerine Velocity kullanýyoruz ki duvara çarpýnca dursun, titremesin.
                    rb.linearVelocity = dashDir * dashSpeed;

                    // Dash atarken yüzü gittiði yere baksýn
                    if (dashDir != Vector3.zero)
                        transform.rotation = Quaternion.LookRotation(dashDir);

                    yield return null; // Bir sonraki kareye geç
                }

                // 4. DURMA
                rb.linearVelocity = Vector3.zero; // Kaymayý engellemek için fren yap
                isDashing = false; // Çarpýþma hasarýný kapat
                break;

            case BossType.Shooter:
                // ... Shooter kodlarý ...
                break;
        }

        // Toparlanma Süresi (Dash attýktan sonra hemen dönemesin)
        yield return new WaitForSeconds(0.5f);

        nextSkillTime = Time.time + skillCooldown;
        isUsingSkill = false;
    }

    // --- NORMAL VURUÞ ---
    IEnumerator DoNormalAttack()
    {
        isAttackingNormal = true;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;

        if (animator != null) animator.SetTrigger("NormalAttack");

        yield return new WaitForSeconds(0.5f);

        // Efekt (Boss'un önünde)
        if (normalHitEffect != null)
        {
            Vector3 hitPos = transform.position + (transform.forward * 1.5f) + (Vector3.up * 1.0f);
            GameObject vfx = Instantiate(normalHitEffect, hitPos, transform.rotation);
            Destroy(vfx, 1f);
        }

        // Hasar
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= attackRange + 1.5f)
            {
                HealthSystem hp = player.GetComponent<HealthSystem>();
                if (hp != null) hp.TakeDamage(normalDamage, transform.position, 5f);
                if (AudioManager.instance != null) AudioManager.instance.PlayHit();
            }
        }

        yield return new WaitForSeconds(0.5f);
        rb.isKinematic = false;
        nextNormalTime = Time.time + normalAttackRate;
        isAttackingNormal = false;
    }

    // --- DASHER ÇARPIÞMA HASARI (Gövde Darbesi) ---
    private void OnCollisionEnter(Collision collision)
    {
        // Sadece Dasher tipindeysek VE þu an Dash atýyorsak hasar ver
        if (bossType == BossType.Dasher && isDashing)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                HealthSystem hp = collision.gameObject.GetComponent<HealthSystem>();
                if (hp != null)
                {
                    // Skill hasarýný vur ve oyuncuyu geriye fýrlat
                    hp.TakeDamage(skillDamage, transform.position, skillKnockback);

                    // Çarpýnca Boss hafif dursun (Ýçinden geçmesin)
                    rb.linearVelocity = Vector3.zero;
                    isDashing = false;

                    Debug.Log("DASHER ÇARPTI!");
                }
            }
        }
    }

    // --- YARDIMCI FONKSÝYONLAR ---
    void SetupDamage(GameObject obj, int dmg, float kb)
    {
        SimpleWeapon sw = obj.GetComponent<SimpleWeapon>();
        if (sw == null) sw = obj.GetComponentInChildren<SimpleWeapon>();
        if (sw != null) { sw.owner = gameObject; sw.damage = dmg; sw.knockback = kb; }
    }

    void MoveTo(Vector3 dest)
    {
        LookAt(dest);
        Vector3 dir = (dest - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    void LookAt(Vector3 dest)
    {
        Vector3 lookPos = new Vector3(dest.x, transform.position.y, dest.z);
        transform.LookAt(lookPos);
    }
}