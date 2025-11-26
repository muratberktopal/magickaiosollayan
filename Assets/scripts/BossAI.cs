using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    public enum BossType { Crusher, Dasher, Shooter }

    [Header("Boss Tipi")]
    public BossType bossType;

    [Header("Temel Özellikler")]
    public float moveSpeed = 2.5f;
    public float detectionRange = 20f;
    public float attackRange = 3f; // Normal vuruþ mesafesi

    [Header("Özel Yetenek Ayarlarý")]
    public float skillCooldown = 5f; // Yetenek ne sýklýkla kullanýlsýn?
    public float skillRange = 6f;    // Yetenek kullanma mesafesi
    public GameObject skillEffectPrefab; // Alan efekti veya Mermi prefabý
    public Transform firePoint;      // Merminin çýkacaðý yer

    [Header("Hasar Ayarlarý")]
    public int normalDamage = 20;
    public int skillDamage = 40;
    public float skillKnockback = 15f;

    // Durum Kontrolü
    private Transform player;
    private Rigidbody rb;
    private Animator animator;
    private float nextSkillTime = 0f;
    private bool isUsingSkill = false; // Þu an yetenek kullanýyor mu?

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Animator varsa al (SurvivalEnemyAI mantýðý)
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // Player'ý bul
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;
        if (isUsingSkill) return; // Yetenek kullanýyorsa hareket etmesin

        float dist = Vector3.Distance(transform.position, player.position);

        // Hareket Animasyonu
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        if (animator != null) animator.SetBool("IsMoving", isMoving);

        // 1. YETENEK KONTROLÜ (Cooldown doldu mu ve menzilde mi?)
        if (Time.time >= nextSkillTime && dist <= skillRange)
        {
            StartCoroutine(UseSpecialSkill());
        }
        // 2. NORMAL SALDIRI / KOVALAMA
        else
        {
            if (dist <= attackRange)
            {
                // Normal saldýrý (SurvivalEnemyAI'deki gibi basit temas hasarý veya animasyon)
                rb.linearVelocity = Vector3.zero;
                LookAt(player.position);
            }
            else
            {
                // Kovala
                MoveTo(player.position);
            }
        }
    }

    IEnumerator UseSpecialSkill()
    {
        isUsingSkill = true;
        rb.linearVelocity = Vector3.zero; // Dur
        LookAt(player.position);

        // --- HAZIRLIK (Telegraphing) ---
        // Oyuncuya "Kaç!" demek için rengini kýrmýzý yap veya animasyon oynat
        if (animator != null) animator.SetTrigger("SkillAttack");
        Debug.Log(bossType + " Yetenek Hazýrlanýyor!");

        yield return new WaitForSeconds(1.0f); // 1 saniye bekle (Oyuncuya kaçma fýrsatý)

        // --- YETENEK UYGULAMA ---
        switch (bossType)
        {
            case BossType.Crusher: // YERE VURMA (AOE)
                if (skillEffectPrefab != null)
                {
                    // Ayaklarýnýn dibinde patlama yarat
                    GameObject smash = Instantiate(skillEffectPrefab, transform.position, Quaternion.identity);
                    SetupDamage(smash, skillDamage, skillKnockback);
                    Destroy(smash, 1f);
                }
                break;

            case BossType.Dasher: // ATILMA (DASH)
                // Oyuncuya doðru fýrlat
                Vector3 dashDir = (player.position - transform.position).normalized;
                rb.AddForce(dashDir * 40f, ForceMode.Impulse); // Çok hýzlý itiþ
                // Dash sýrasýnda çarptýðýna hasar vermesi için collider triggerlanabilir
                // Þimdilik basit tutuyoruz, çarpýnca hasar verecek
                break;

            case BossType.Shooter: // MERMÝ ATMA
                if (skillEffectPrefab != null && firePoint != null)
                {
                    GameObject projectile = Instantiate(skillEffectPrefab, firePoint.position, transform.rotation);
                    // Mermi scriptini ayarla (Senin FireballProjectile veya SimpleWeapon)
                    SetupDamage(projectile, skillDamage, 5f);

                    // Eðer merminin kendi hareket kodu yoksa itelim:
                    Rigidbody projRb = projectile.GetComponent<Rigidbody>();
                    if (projRb != null) projRb.linearVelocity = transform.forward * 15f;
                }
                break;
        }

        // --- BÝTÝÞ ---
        yield return new WaitForSeconds(0.5f); // Biraz daha bekle (Recover)
        nextSkillTime = Time.time + skillCooldown;
        isUsingSkill = false;
    }

    // Yardýmcý: Oluþturulan saldýrýya hasar verisini iþle
    void SetupDamage(GameObject obj, int dmg, float kb)
    {
        SimpleWeapon sw = obj.GetComponent<SimpleWeapon>();
        if (sw == null) sw = obj.GetComponentInChildren<SimpleWeapon>();

        if (sw != null)
        {
            sw.owner = gameObject; // Vuran Boss
            sw.damage = dmg;
            sw.knockback = kb;
        }
    }

    void MoveTo(Vector3 dest)
    {
        LookAt(dest);
        Vector3 dir = (dest - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
        // Y eksenini koru (zýplamasýn)
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    void LookAt(Vector3 dest)
    {
        Vector3 lookPos = new Vector3(dest.x, transform.position.y, dest.z);
        transform.LookAt(lookPos);
    }
}