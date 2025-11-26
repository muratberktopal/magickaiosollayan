using UnityEngine;

public class WhipStrike : MonoBehaviour
{
    [Header("Baðlantýlar")]
    public Transform tipObject;   // Uzaktaki kare kutu (WhipTip)
    public LineRenderer line;     // Ýp

    [Header("Ayarlar")]
    public float lifeTime = 0.3f; // Ekranda ne kadar kalsýn? (Þak sesi kadar)

    void Start()
    {
        // Otomatik bul (Eðer atamazsan)
        if (line == null) line = GetComponent<LineRenderer>();
        if (tipObject == null) tipObject = transform.GetChild(0); // Ýlk çocuðu al

        // Süre dolunca yok et
        Destroy(gameObject, lifeTime);
    }

    void LateUpdate()
    {
        if (line != null && tipObject != null)
        {
            // Nokta 0: Karakterin olduðu yer (Merkez)
            line.SetPosition(0, transform.position);

            // Nokta 1: Uzaktaki kutunun olduðu yer
            line.SetPosition(1, tipObject.position);
        }
    }
}