using UnityEngine;

public class Billboard : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform target;  // Kimi takip ediyoruz?
    public Vector3 offset;    // Ne kadar yukarýda?
    public float smoothSpeed = 10f; // Takip etme yumuþaklýðý (Düþükse geç gelir, yüksekse yapýþýr)

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        // 1. Hedef öldüyse barý yok et
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (cam == null) cam = Camera.main;

        // 2. POZÝSYON TAKÝBÝ (Lerp ile Yumuþatma)
        // Karakter titrese bile bar yumuþakça o konuma gider
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // 3. ROTASYON KÝLÝTLEME (Kameraya bak)
        if (cam != null)
        {
            transform.rotation = cam.transform.rotation;
        }
    }
}