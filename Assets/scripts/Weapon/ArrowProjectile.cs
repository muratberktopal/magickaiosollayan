using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [Header("Ok Ayarlarý")]
    public float speed = 40f;      // Ok çok hýzlý olmalý
    public float lifeTime = 3f;    // Ömür

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // MANUEL HAREKET (Fizik motorunu bekleme, zorla git)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Player'a ve diðer oklara çarpma
        if (other.CompareTag("Player")) return;
        if (other.gameObject.layer == gameObject.layer) return;

        // Duvara veya Düþmana çarpýnca yok ol
        // (Hasarý üzerindeki SimpleWeapon verecek)
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}