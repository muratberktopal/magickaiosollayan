using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform target; // Takip edilecek obje (Senin Player kapsülün)
    public float smoothSpeed = 0.125f; // Takip etme yumuþaklýðý (0-1 arasý)
    public Vector3 offset; // Kamera ile oyuncu arasýndaki mesafe farký

    void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyon: Oyuncunun konumu + aradaki baþlangýç mesafesi
        Vector3 desiredPosition = target.position + offset;

        // Lerp: Kameranýn þu anki konumundan hedef konuma yumuþakça kaymasýný saðlar
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Pozisyonu güncelle
        transform.position = smoothedPosition;

        // (Opsiyonel) Kameranýn sürekli oyuncuya bakmasýný garantiye alýr
        // transform.LookAt(target); 
    }
}