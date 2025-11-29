using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PanicMobAI : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float runSpeed = 6.0f;   // Çok hýzlý koþsun
    public float changeDirTime = 1.0f; // Saniyede bir yön deðiþtirsin
    public float mapSize = 15f;     // Haritanýn ne kadar uzaðýna gidebilsin?

    private Rigidbody rb;
    private Animator animator;
    private Vector3 targetPoint;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        PickRandomPoint();
    }

    void FixedUpdate()
    {
        // Sürekli hareket halinde
        timer += Time.fixedDeltaTime;

        if (timer >= changeDirTime)
        {
            PickRandomPoint();
            timer = 0f;
        }

        MoveToTarget();

        // Animasyon (Hep koþuyor)
        if (animator != null) animator.SetBool("IsMoving", true);
    }

    void MoveToTarget()
    {
        // Hedefe dön
        Vector3 lookPos = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
        transform.LookAt(lookPos);

        // Koþ
        Vector3 dir = (targetPoint - transform.position).normalized;
        rb.linearVelocity = dir * runSpeed;

        // Y eksenini (düþmeyi) koru
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    void PickRandomPoint()
    {
        // Rastgele bir nokta seç (Player'dan kaçma mantýðý da eklenebilir ama rastgele daha komik durur)
        float x = Random.Range(-mapSize, mapSize);
        float z = Random.Range(-mapSize, mapSize);
        targetPoint = new Vector3(x, transform.position.y, z);
    }
}