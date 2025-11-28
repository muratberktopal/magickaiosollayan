using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class SurvivalEnemyAI : MonoBehaviour
{
    [Header("Hareket")]
    public float moveSpeed = 3.5f;
    public float attackRange = 2.5f;

    [Header("Sald�r�")]
    public GameObject weaponPrefab; // K�l��, Sopa vb.
    public Transform firePoint;
    public float attackCooldown = 2f;

    [Header("G��")]
    public int damage = 10;
    public float knockback = 5f;

    // Hedef hep Player olacak
    private Transform player;
    private Rigidbody rb;
    private Animator animator;
    private float nextAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // Sadece Player'� bul
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return; // Player �ld�yse dur

        // Animasyon
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        if (animator != null) animator.SetBool("IsMoving", isMoving);

        // Mesafe �l�
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            // Menzilde -> Sald�r
            if (Time.time >= nextAttackTime)
            {
                PerformAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
            else
            {
                rb.linearVelocity = Vector3.zero; // Beklerken dur
                LookAt(player.position);
            }
        }
        else
        {
            // Uzakta -> Kovala
            MoveTo(player.position);
        }
    }

    void PerformAttack()
    {
        LookAt(player.position);

        // Silah� olu�tur
        if (weaponPrefab != null && firePoint != null)
        {
            // Slash ise yatay, di�erleri d�z
            Quaternion rot = transform.rotation;
            // Basit kontrol: E�er prefab isminde "Slash" ge�iyorsa yat�r (�ste�e ba�l�)
            // �imdilik d�z slash mant���yla gidelim:
            rot = Quaternion.Euler(90, transform.eulerAngles.y, 0);

            GameObject weapon = Instantiate(weaponPrefab, firePoint.position, rot);

            SimpleWeapon sw = weapon.GetComponent<SimpleWeapon>();
            if (sw != null)
            {
                sw.owner = gameObject;
                sw.damage = damage;
                sw.knockback = knockback;
            }
            Destroy(weapon, 0.3f); // K�sa s�rede yok et

            if (AudioManager.instance != null) AudioManager.instance.PlaySlash();
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