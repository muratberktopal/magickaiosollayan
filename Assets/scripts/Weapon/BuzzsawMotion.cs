using UnityEngine;

public class BuzzsawMotion : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float throwSpeed = 15f;  // Gidiþ hýzý
    public float returnSpeed = 20f; // Geri dönüþ hýzý
    public float maxDistance = 6f;  // Ne kadar uzaða gitsin?

    [Header("Testere Ayarlarý")]
    public float spinDuration = 2.5f; // Havada kaç saniye dönsün?
    public float spinSpeed = 1000f;   // Dönme hýzý

    [Header("Parçalar")]
    public Transform bladeVisual;   // Dönen parça (Child)
    public LineRenderer chain;      // Zincir

    private Vector3 startPos;
    private Transform owner;        // Player
    private int state = 0;          // 0:Gidiþ, 1:Bekleme(Dönme), 2:Dönüþ
    private float timer = 0f;

    void Start()
    {
        startPos = transform.position;

        // Görseli ve Zinciri bul
        if (bladeVisual == null) bladeVisual = transform.GetChild(0);
        if (chain == null) chain = GetComponent<LineRenderer>();

        // Sahibini bul
        SimpleWeapon weapon = GetComponentInChildren<SimpleWeapon>();
        if (weapon != null && weapon.owner != null) owner = weapon.owner.transform;
        else
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) owner = p.transform;
        }
    }

    void Update()
    {
        // 1. ZÝNCÝRÝ GÜNCELLE
        if (chain != null && owner != null)
        {
            chain.SetPosition(0, owner.position + Vector3.up); // Player'ýn eli/beli
            chain.SetPosition(1, transform.position);          // Testere
        }

        if (owner == null) { Destroy(gameObject); return; }

        // 2. TESTEREYÝ DÖNDÜR (Her zaman dönsün)
        if (bladeVisual != null)
        {
            bladeVisual.Rotate(0, spinSpeed * Time.deltaTime, 0);
        }

        // 3. DURUM MAKÝNESÝ
        switch (state)
        {
            case 0: // GÝDÝÞ
                transform.Translate(Vector3.forward * throwSpeed * Time.deltaTime);

                if (Vector3.Distance(startPos, transform.position) >= maxDistance)
                {
                    state = 1; // Dönme moduna geç
                    timer = spinDuration;
                }
                break;

            case 1: // BEKLEME & DÖNME (Buzzsaw Modu)
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    state = 2; // Geri gel
                }
                break;

            case 2: // DÖNÜÞ
                transform.position = Vector3.MoveTowards(transform.position, owner.position, returnSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, owner.position) < 1f)
                {
                    Destroy(gameObject); // Yakalandý
                }
                break;
        }
    }
}