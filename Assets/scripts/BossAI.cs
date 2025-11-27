using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    public enum BossType { Crusher, Dasher, Shooter }

    [Header("Boss Tipi")]
    public BossType bossType;

    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 2.5f;
    public float detectionRange = 25f;
    public float attackRange = 3f;

    [Header("Dasher Özel Ayarlarý")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.5f;
    public float dashWarningTime = 0.8f;

    [Header("Normal Saldýrý Ayarlarý")]
    public float normalAttackRate = 2f;
    public int normalDamage = 15;
    public GameObject normalHitEffect;

    [Header("Özel Yetenek (Skill) Ayarlarý")]
    public float skillCooldown = 5f;
    public float skillRange = 15f;
    public GameObject skillEffectPrefab;
    public Transform firePoint;

    [Header("Shooter (Mage) Ayarlarý")]
    public float safeDistance = 6f; // Bu mesafeden yakýna girmesin
    public float projectileSpeed = 12f;

    [Header("Yetenek Hasar Ayarlarý")]
    public int skillDamage = 40;
    public float skillKnockback = 20f;

    // Durum Kontrolü
    private Transform player;
    private Rigidbody rb;
    private Animator animator;

    private float nextSkillTime = 0f;
    private float nextNormalTime = 0f;

    private bool isUsingSkill = false;
    private bool isDashing = false;
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

        // Eylem halindeyken hareket etme
        if (isUsingSkill || isAttackingNormal) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Animasyon: Hýz varsa yürü
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        if (animator != null) animator.SetBool("IsMoving", isMoving);

        // --- YAPAY ZEKA KARAR MEKANÝZMASI ---

        // 1. ÖZEL YETENEK KONTROLÜ
        // Shooter ise Güvenli Mesafeden uzaktaysak ateþ edelim
        bool canShoot = (bossType == BossType.Shooter) ? (dist > safeDistance) : true;

        if (Time.time >= nextSkillTime && dist <= skillRange && canShoot)
        {
            StartCoroutine(UseSpecialSkill());
        }
        // 2. NORMAL SALDIRI (Çok yakýndaysa)
        else if (dist <= attackRange)
        {
            rb.linearVelocity = Vector3.zero;
            LookAt(player.position);

            if (Time.time >= nextNormalTime)
            {
                StartCoroutine(DoNormalAttack());
            }
        }
        // 3. HAREKET MANTIÐI (Burayý Deðiþtirdik)
        else
        {
            // SHOOTER ÝÇÝN ÖZEL DAVRANIÞ:
            if (bossType == BossType.Shooter)
            {
                // Eðer Skill menzilindeyiz ama Skill Cooldown'da ise -> YÜRÜME! BEKLE.
                // (Oyuncunun üzerine koþmasýný engelliyoruz)
                if (dist <= skillRange)
                {
                    rb.linearVelocity = Vector3.zero; // Dur ve bekle
                    LookAt(player.position); // Ama oyuncuya bakmaya devam et
                }
                else
                {
                    // Menzil dýþýndaysa yaklaþ
                    MoveTo(player.position);
                }
            }
            // DÝÐER BOSSLAR (Her zaman kovala)
            else
            {
                MoveTo(player.position);
            }
        }
    }

    // --- ÖZEL YETENEK FONKSÝYONU ---
    IEnumerator UseSpecialSkill()
    {
        isUsingSkill = true;
        rb.linearVelocity = Vector3.zero;
        LookAt(player.position);

        if (animator != null) animator.SetTrigger("SkillAttack");

        switch (bossType)
        {
            case BossType.Crusher:
                rb.isKinematic = true;
                yield return new WaitForSeconds(1.0f);
                if (skillEffectPrefab != null)
                {
                    Vector3 smashPos = transform.position + (transform.forward * 2.5f);
                    smashPos.y = transform.position.y + 0.1f;
                    GameObject smash = Instantiate(skillEffectPrefab, smashPos, transform.rotation);
                    SetupDamage(smash, skillDamage, skillKnockback);
                    Destroy(smash, 1f);
                }
                yield return new WaitForSeconds(0.5f);
                rb.isKinematic = false;
                break;

            case BossType.Dasher:
                Debug.Log("Dasher Hazýrlanýyor...");
                yield return new WaitForSeconds(dashWarningTime);
                Vector3 dashDir = (player.position - transform.position).normalized;
                dashDir.y = 0;
                isDashing = true;
                float startTime = Time.time;
                while (Time.time < startTime + dashDuration)
                {
                    rb.linearVelocity = dashDir * dashSpeed;
                    if (dashDir != Vector3.zero) transform.rotation = Quaternion.LookRotation(dashDir);
                    yield return null;
                }
                rb.linearVelocity = Vector3.zero;
                isDashing = false;
                break;

            case BossType.Shooter:
                LookAt(player.position);

                // 3 Kez ateþ et
                for (int i = 0; i < 3; i++)
                {
                    if (skillEffectPrefab != null && firePoint != null)
                    {
                        // --- DÜZELTME: HAFÝF RASTGELE POZÝSYON ---
                        // Mermiler tam üst üste binmesin diye çok hafif saða/sola kaydýrýyoruz
                        Vector3 spawnPos = firePoint.position + (transform.right * Random.Range(-0.2f, 0.2f));

                        GameObject projectile = Instantiate(skillEffectPrefab, spawnPos, transform.rotation);
                        SetupDamage(projectile, skillDamage, 5f);

                        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
                        if (projRb != null)
                        {
                            Vector3 dir = (player.position - firePoint.position).normalized;
                            projRb.linearVelocity = dir * projectileSpeed;
                        }

                        if (AudioManager.instance != null) AudioManager.instance.PlayMagic();
                    }
                    // --- BEKLEME SÜRESÝ ---
                    // Bu süre mermilerin arka arkaya (tren gibi) gitmesini saðlar
                    yield return new WaitForSeconds(0.4f);
                }
                break;
        }

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

        if (normalHitEffect != null)
        {
            Vector3 hitPos = transform.position + (transform.forward * 1.5f) + (Vector3.up * 1.0f);
            GameObject vfx = Instantiate(normalHitEffect, hitPos, transform.rotation);
            Destroy(vfx, 1f);
        }

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

    // --- DÝÐER FONKSÝYONLAR AYNI ---
    private void OnCollisionEnter(Collision collision)
    {
        if (bossType == BossType.Dasher && isDashing)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                HealthSystem hp = collision.gameObject.GetComponent<HealthSystem>();
                if (hp != null)
                {
                    hp.TakeDamage(skillDamage, transform.position, skillKnockback);
                    rb.linearVelocity = Vector3.zero;
                    isDashing = false;
                }
            }
        }
    }
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