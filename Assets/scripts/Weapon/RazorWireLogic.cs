using UnityEngine;

public class RazorWireLogic : MonoBehaviour
{
    [Header("Hareket")]
    public float throwSpeed = 20f;  // Fýrlatma hýzý
    public float maxDistance = 8f;  // Ne kadar uzaða gitsin?
    public float stayTime = 4f;     // Ne kadar kalsýn?

    [Header("Parçalar")]
    public Transform wireHitBox;    // Aradaki görünmez collider (WireHitBox)
    public LineRenderer line;       // Kýrmýzý ip

    private Vector3 startPos;
    private Transform owner;        // Player
    private bool isStopped = false; // Kýlýç durdu mu?

    void Start()
    {
        startPos = transform.position;

        // Sahibini bul (SimpleWeapon içindeki owner'dan çekiyoruz)
        SimpleWeapon weapon = GetComponentInChildren<SimpleWeapon>();
        if (weapon != null && weapon.owner != null)
        {
            owner = weapon.owner.transform;
        }
        else
        {
            // Yedek: Player'ý bul
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) owner = p.transform;
        }

        // Süre bitince yok et
        Destroy(gameObject, stayTime);
    }

    void Update()
    {
        // Sahip öldüyse yok ol
        if (owner == null) { Destroy(gameObject); return; }

        // 1. KILIÇ HAREKETÝ
        if (!isStopped)
        {
            transform.Translate(Vector3.forward * throwSpeed * Time.deltaTime);

            // Menzile ulaþtý mý?
            if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            {
                isStopped = true; // Dur ve bekle
            }
        }

        // 2. ÝPÝ VE COLLIDER'I GÜNCELLE
        UpdateWire();
    }

    void UpdateWire()
    {
        // A. Çizgiyi Çiz
        line.SetPosition(0, owner.position + Vector3.up); // Player'ýn beli
        line.SetPosition(1, transform.position);          // Kýlýç

        // B. Collider'ý Uzat ve Döndür (Matematik Þov)
        if (wireHitBox != null)
        {
            // Ýki nokta arasýndaki orta noktayý bul
            Vector3 midPoint = (owner.position + transform.position) / 2;
            midPoint.y = 1f; // Yüksekliði sabitle

            // Collider'ý ortaya taþý
            wireHitBox.position = midPoint;

            // Kýlýca baktýr
            wireHitBox.LookAt(transform.position);

            // Boyutunu ayarla (Z ekseninde uzat)
            float dist = Vector3.Distance(owner.position, transform.position);
            wireHitBox.localScale = new Vector3(0.2f, 0.2f, dist);
        }
    }
}