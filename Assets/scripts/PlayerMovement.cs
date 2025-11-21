using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;

    [Header("Referanslar")]
    public FixedJoystick joystick; // Unity'de kutucuğa sürüklemeyi unutma!

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // ÖNEMLİ KISIM:
        // Joystick'in Y verisini (yukarı/aşağı), 3D dünyadaki Z eksenine (ileri/geri) atıyoruz.
        // Vector3(x, y, z) -> (Joystick Sağ/Sol, 0, Joystick Yukarı/Aşağı)

        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        // Hız vektörünü uygula (Y eksenindeki yer çekimini koruyarak)
        Vector3 newVelocity = direction * moveSpeed;

        // Karakterin düşebilmesi için Y hızını (yerçekimi) bozmuyoruz
        newVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = newVelocity;

        // Karakteri gittiği yöne döndürme (Opsiyonel ama güzel görünür)
        if (direction != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, 0.15f);
        }
    }
}