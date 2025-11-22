using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;
    public FixedJoystick joystick; // Joystick'i Inspector'da tekrar atamayı unutma!

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Animator bileşeni modelin içinde olabilir veya ana objede olabilir.
        // Bu kod ikisine de bakar.
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        // Joystick verisini al
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        // Hareketi uygula
        Vector3 newVelocity = direction * moveSpeed;
        newVelocity.y = rb.linearVelocity.y; // Yerçekimini koru
        rb.linearVelocity = newVelocity;

        // Yönü döndür
        if (direction != Vector3.zero)
        {
            // Yumuşak dönme
            transform.forward = Vector3.Lerp(transform.forward, direction, 0.2f);
        }

        // --- ANİMASYON KISMI ---
        if (animator != null)
        {
            // Eğer joystick azıcık bile oynatıldıysa (0.1'den büyükse) koşuyordur
            bool isRunning = direction.magnitude > 0.1f;

            // Animator'daki şalteri indir/kaldır
            animator.SetBool("IsMoving", isRunning);
        }
    }
}