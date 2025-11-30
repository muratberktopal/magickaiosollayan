using UnityEngine;

public class FloatingName : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 2.5f, 0); // Ne kadar yukarýda dursun?
    private Transform target; // Takip edilecek kiþi (Baban)
    private Camera cam;

    void Start()
    {
        // Ýçinde bulunduðum objenin Babasýný (Player/Enemy) hedef al
        if (transform.parent != null)
        {
            target = transform.parent;
        }

        cam = Camera.main;
    }

    // LateUpdate: Hareketler bittikten sonra çalýþýr, titremeyi önler
    void LateUpdate()
    {
        if (target != null)
        {
            // 1. POZÝSYON KÝLÝTLEME
            // Babanýn rotasyonunu umursama, sadece dünya pozisyonunu al + Yükseklik ekle
            transform.position = target.position + offset;

            // 2. KAMERAYA BAKMA (Billboard)
            // Yazý hep kameraya dönsün
            if (cam != null)
            {
                transform.rotation = cam.transform.rotation;
            }
        }
        else
        {
            // Eðer baba öldüyse (yok olduysa) ben de yok olayým
            Destroy(gameObject);
        }
    }
}