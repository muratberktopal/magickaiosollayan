using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [Header("Ok Ayarlarý")]
    public float speed = 40f;      // Ok hýzý
    public float lifeTime = 3f;    // Ömür

    void Start()
    {
        // 3 saniye sonra kesin yok ol
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // MANUEL HAREKET (En garantisi)
        // Her karede ileri doðru itiyoruz
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Player'a çarpma
        if (other.CompareTag("Player")) return;

        // Diðer oklara çarpma
        if (other.gameObject.layer == gameObject.layer) return;

        // Duvara veya Düþmana çarpýnca görseli yok et
        // (Hasarý SimpleWeapon verdiði için biz sadece kendimizi siliyoruz)
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}