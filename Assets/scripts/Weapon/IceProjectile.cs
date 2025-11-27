using UnityEngine;

public class IceShard : MonoBehaviour
{
    [Header("Buz Parçasý Ayarlarý")]
    public float speed = 15f;    // Hýz
    public float damage = 10f;   // Hasar
    public float lifeTime = 3f;  // Ekranda kalma süresi

    void Start()
    {
        // Performans için: 3 saniye sonra kendi kendini yok et
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Sadece ileri (kendi saðýna) doðru git
        // Not: Sprite'ýnýn ucu saða bakmalý
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer çarptýðýmýz obje 'Enemy' etiketine sahipse
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Ice Shard düþmana çarptý!");

            // --- Hasar Verme Kodu Buraya Gelecek ---
            // Örnek: other.GetComponent<EnemyHealth>().TakeDamage(damage);

            // Mermiyi yok et (içinden geçip gitmesini istemiyorsan)
            Destroy(gameObject);
        }
    }
}