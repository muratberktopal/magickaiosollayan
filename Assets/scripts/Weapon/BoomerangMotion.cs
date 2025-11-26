using UnityEngine;

public class BoomerangMotion : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float speed = 15f;       // Gidiþ hýzý
    public float returnSpeed = 20f; // Dönüþ hýzý (Daha hýzlý olsun)
    public float maxRange = 8f;     // Ne kadar uzaða gitsin?
    public float rotationSpeed = 720f; // Dönme hýzý

    [Header("Referans")]
    public Transform visualModel;   // Dönecek olan görsel kýsým (Child)

    private Vector3 startPos;
    private Transform owner;        // Sahibi (Geri döneceði kiþi)
    private bool isReturning = false; // Þu an dönüyor mu?

    void Start()
    {
        startPos = transform.position;

        // Sahibini SimpleWeapon üzerinden öðren
        SimpleWeapon weapon = GetComponent<SimpleWeapon>();
        if (weapon != null) owner = weapon.owner.transform;

        // Eðer görseli atamadýysan otomatik bul
        if (visualModel == null) visualModel = transform.GetChild(0);
    }

    void Update()
    {
        // 1. GÖRSELÝ DÖNDÜR (Fýrýldak gibi)
        if (visualModel != null)
        {
            visualModel.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

        // 2. HAREKET MANTIÐI
        if (!isReturning)
        {
            // --- GÝDÝÞ MODU ---
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // Menzile ulaþtý mý?
            if (Vector3.Distance(startPos, transform.position) >= maxRange)
            {
                isReturning = true; // Geri dön emri
            }
        }
        else
        {
            // --- DÖNÜÞ MODU ---
            if (owner != null)
            {
                // Sahibine doðru uç
                transform.position = Vector3.MoveTowards(transform.position, owner.position, returnSpeed * Time.deltaTime);

                // Sahibine vardý mý? (Yakalama)
                if (Vector3.Distance(transform.position, owner.position) < 1f)
                {
                    Destroy(gameObject); // Yakaladým, yok ol
                }
            }
            else
            {
                Destroy(gameObject); // Sahip öldüyse ben de yok olayým
            }
        }
    }
}