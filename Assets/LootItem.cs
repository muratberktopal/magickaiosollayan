using UnityEngine;

public class LootItem : MonoBehaviour
{
    public int xpAmount = 10;   // Ne kadar XP verecek?
    public float moveSpeed = 10f; // Mýknatýs hýzý

    private Transform target;   // Kime doðru uçuyor?
    private bool isMagnetized = false;

    void Update()
    {
        if (isMagnetized && target != null)
        {
            // ... hareket kodlarý ...

            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                target.GetComponent<LevelSystem>().GainXP(xpAmount);

                // --- SESÝ ÇAL ---
                if (AudioManager.instance != null)
                    AudioManager.instance.PlayXP();
                // ---------------

                Destroy(gameObject);
            }
        }
        // Eðer birisi beni çekiyorsa ona doðru uç
        if (isMagnetized && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Hedefe deðdi mi?
            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                // Hedefin büyüme kodunu çalýþtýr (Birazdan yazacaðýz)
                target.GetComponent<LevelSystem>().GainXP(xpAmount);
                Destroy(gameObject); // Kendini yok et
            }
        }
    }

    // Birisi yanýma gelirse (Trigger alanýma girerse)
    void OnTriggerEnter(Collider other)
    {
        if (isMagnetized) return; // Zaten birine gidiyorsam baþkasýna gitmem

        // Yaklaþan kiþi Player veya Enemy mi?
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Caný/Level sistemi varsa ona yapýþ
            if (other.GetComponent<LevelSystem>() != null)
            {
                target = other.transform;
                isMagnetized = true;

                // Yerçekimini kapat ki havada uçabilsin
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}