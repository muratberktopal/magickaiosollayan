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
    public float attackRange = 3f;

    [Header("Normal Saldýrý Ayarlarý")] // <-- YENÝ
    public float normalAttackRate = 2f; // Kaç saniyede bir normal vursun?
    public int normalDamage = 15;       // Normal vuruþ hasarý

    [Header("Özel Yetenek Ayarlarý")]
    public float skillCooldown = 5f;
    public float skillRange = 6f;
    public GameObject skillEffectPrefab;
    public Transform firePoint;

    [Header("Yetenek Hasar Ayarlarý")]
    public int skillDamage = 40;
    public float skillKnockback = 15f;

    // Durum Kontrolü
    private Transform player;
    private Rigidbody rb;
    private Animator animator;

    private float nextSkillTime = 0f;
    private float nextNormalTime = 0f; // <-- YENÝ

    private bool isUsingSkill = false;
    private bool isAttackingNormal = false; // <-- YENÝ

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
        if (isUsingSkill || isAttackingNormal) return; // Saldýrýyorsa kýmýldamasýn

        float dist = Vector3.Distance(transform.position, player.position);

        // Hareket Animasyonu
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        if (animator != null) animator.SetBool("IsMoving", isMoving);

        // 1. ÖZEL YETENEK (Öncelikli)
        if (Time.time >= nextSkillTime && dist <= skillRange)
        {
            StartCoroutine(UseSpecialSkill());
        }
        // 2. NORMAL SALDIRI / KOVALAMA
        else
        {
            if (dist <= attackRange)
            {
                rb.linearVelocity = Vector3.zero; // Dur
                LookAt(player.position);

                // --- YENÝ EKLENEN KISIM: NORMAL VURUÞ ---
                if (Time.time >= nextNormalTime)
                {
                    StartCoroutine(DoNormalAttack());
                }
                // ----------------------------------------
            }
            else
            {
                MoveTo(player.position);
            }
        }
    }

    // --- YENÝ EKLENEN FONKSÝYON: NORMAL VURUÞ ---
    IEnumerator DoNormalAttack()
    {
        isAttackingNormal = true;
        rb.linearVelocity = Vector3.zero; // Saldýrýrken kaymasýn

        // Animasyonu tetikle (Unity'de "NormalAttack" diye bir Trigger açman gerekecek)
        if (animator != null) animator.SetTrigger("NormalAttack");

        // Vuruþun inmesi için bekle (Animasyonun hýzýna göre ayarla, örn: 0.5sn)
        yield return new WaitForSeconds(0.5f);

        // Hâlâ menzilde mi? (Oyuncu kaçtý mý?)
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= attackRange + 1f) // Biraz tolerans tanýyalým
            {
                // Direkt hasar ver
                HealthSystem hp = player.GetComponent<HealthSystem>();
                if (hp != null)
                {
                    hp.TakeDamage(normalDamage, transform.position, 5f);
                }

                // Ses
                if (AudioManager.instance != null) AudioManager.instance.PlayHit();
            }
        }

        // Animasyonun bitmesini bekle
        yield return new WaitForSeconds(0.5f);

        nextNormalTime = Time.time + normalAttackRate;
        isAttackingNormal = false;
    }
    // ---------------------------------------------

    IEnumerator UseSpecialSkill()
    {
        isUsingSkill = true;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        LookAt(player.position);

        if (animator != null) animator.SetTrigger("SkillAttack");

        yield return new WaitForSeconds(1.0f);

        switch (bossType)
        {
            case BossType.Crusher:
                if (skillEffectPrefab != null)
                {
                    Vector3 smashPos = transform.position;
                    smashPos += transform.forward * 2.5f;
                    smashPos.y = transform.position.y + 1f;

                    GameObject smash = Instantiate(skillEffectPrefab, smashPos, transform.rotation);
                    SetupDamage(smash, skillDamage, skillKnockback);
                    Destroy(smash, 1f);
                }
                break;
                // Diðer Boss tipleri buraya...
        }

        yield return new WaitForSeconds(0.5f);

        rb.isKinematic = false;
        nextSkillTime = Time.time + skillCooldown;
        isUsingSkill = false;
    }

    void SetupDamage(GameObject obj, int dmg, float kb)
    {
        SimpleWeapon sw = obj.GetComponent<SimpleWeapon>();
        if (sw == null) sw = obj.GetComponentInChildren<SimpleWeapon>();

        if (sw != null)
        {
            sw.owner = gameObject;
            sw.damage = dmg;
            sw.knockback = kb;
        }
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