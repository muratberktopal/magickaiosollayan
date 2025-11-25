using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;
    public FixedJoystick joystick;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Animator'ı bul (Karakterin içinde mi, modelde mi bakar)
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        // 1. Önce Joystick verisini al
        float moveX = joystick.Horizontal;
        float moveZ = joystick.Vertical;

        // 2. BİLGİSAYAR İÇİN EKLEME:
        // Eğer Joystick kullanılmıyorsa (Değerler 0 ise), Klavyeyi kontrol et
#if UNITY_EDITOR || UNITY_STANDALONE
        if (moveX == 0 && moveZ == 0)
        {
            moveX = Input.GetAxisRaw("Horizontal"); // A-D veya Sol-Sağ Ok
            moveZ = Input.GetAxisRaw("Vertical");   // W-S veya Yukarı-Aşağı Ok
        }
#endif

        // Vektörü oluştur
        Vector3 direction = new Vector3(moveX, 0, moveZ).normalized; // .normalized çapraz giderken hızı dengeler

        // Hareketi uygula
        Vector3 newVelocity = direction * moveSpeed;
        newVelocity.y = rb.linearVelocity.y; // Yerçekimini koru
        rb.linearVelocity = newVelocity;

        // Yönü döndür
        if (direction != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, 0.2f);
        }

        // Animasyon Kontrolü
        if (animator != null)
        {
            bool isRunning = direction.magnitude > 0.1f;
            animator.SetBool("IsMoving", isRunning);
        }
    }
}