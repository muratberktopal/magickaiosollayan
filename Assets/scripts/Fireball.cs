using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f; // 2 saniye sonra yok olur
    public int damage = 10;

    void Start()
    {
        // Doðar doðmaz ileri fýrla
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Kendimize veya baþka büyülere çarpmasýn
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            // Burada ilerde "Can Azaltma" kodunu çaðýracaðýz
            // other.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject); // Çarpýnca yok ol
        }
        else if (!other.CompareTag("Projectile")) // Duvara çarparsa
        {
            Destroy(gameObject);
        }
    }
}