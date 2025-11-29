using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlimeEnemyAI : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 2.5f; // Biraz yavaþ olsun
    public float stopDistance = 0.1f; // Dibine kadar girsin

    [Header("Temas Hasarý")]
    public int damage = 10;          // Dokunma hasarý
    public float knockbackForce = 5f; // Çarpýnca ne kadar itsin?
    public float hitRate = 1.0f;     // Saniyede kaç kere vursun? (Sürekli can gitmesin diye)

    private Transform player;
    private Rigidbody rb;
    private float nextHitTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Player'ý bul
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Player'a doðru yürü
        Vector3 direction = (player.position - transform.position).normalized;

        // Sadece X ve Z'de hareket et (Y'yi koru)
        // Unity 6 kullandýðýn için 'linearVelocity' kullanýyoruz
        rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);

        // Yüzünü dön
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    // --- TEMAS ANI ---
    // Slime, Player'a fiziksel olarak deðerse bu çalýþýr
    void OnCollisionStay(Collision collision)
    {
        // Çarpan þey Player mý? Ve Vurma sýrasý geldi mi?
        if (collision.gameObject.CompareTag("Player") && Time.time >= nextHitTime)
        {
            HealthSystem playerHp = collision.gameObject.GetComponent<HealthSystem>();

            if (playerHp != null)
            {
                // Hasar Ver
                // AttackerPos olarak kendi merkezimizi veriyoruz ki player bizden uzaða savrulsun
                playerHp.TakeDamage(damage, transform.position, knockbackForce);

                // Ses (Varsa výcýk sesi ekleyebilirsin)
                // if(AudioManager.instance) AudioManager.instance.PlayHit();

                // Bir sonraki vuruþ zamanýný ayarla
                nextHitTime = Time.time + hitRate;
            }
        }
    }
}