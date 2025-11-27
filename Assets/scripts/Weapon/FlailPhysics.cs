using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class FlailPhysics : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform target;       // Kimi takip edecek? (Player)
    public float followForce = 500f; // Çekme gücü (Motor gücü)
    public float stopDistance = 2.5f; // Ne kadar yaklaþýnca dursa? (Zincir boyu)

    private Rigidbody rb;
    private LineRenderer chain;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        chain = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject); // Sahibi yoksa yok ol
            return;
        }

        // 1. HEDEF YÖNÜNÜ BUL
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        // 2. FÝZÝKSEL ÇEKÝM (Zincir gerilirse çek)
        if (distance > stopDistance)
        {
            // Hedefe doðru kuvvet uygula (AddForce)
            // Bu sayede anýnda dönmez, savrulur.
            rb.AddForce(direction.normalized * followForce * Time.fixedDeltaTime);
        }

        // 3. ZÝNCÝRÝ ÇÝZ
        DrawChain();
    }

    void DrawChain()
    {
        // Zincirin bir ucu bende, diðer ucu sahibimde
        chain.SetPosition(0, transform.position);

        // Sahibimin bel hizasýndan (Y+1) çýksýn
        Vector3 handPos = target.position + Vector3.up;
        chain.SetPosition(1, handPos);
    }

    // Player ýþýnlanýrsa (Map deðiþimi) top da ýþýnlansýn
    public void SnapToTarget()
    {
        if (target != null) transform.position = target.position + Vector3.back;
    }
}