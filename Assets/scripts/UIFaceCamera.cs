using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        // Ana kamerayý bul
        mainCam = Camera.main;
    }

    // LateUpdate kullanýyoruz ki düþman hareketini bitirdikten sonra biz düzeltelim
    void LateUpdate()
    {
        if (mainCam != null)
        {
            // KENDÝ ROTASYONUNU, KAMERANIN ROTASYONUYLA AYNI YAP
            // Bu sayede kamera nereye bakarsa, yazý da oraya bakar (dümdüz sana)
            transform.rotation = mainCam.transform.rotation;
        }
    }
}