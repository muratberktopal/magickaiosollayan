using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager instance;

    [Header("Ayarlar")]
    public GameObject popupPrefab; // Sarý yazý prefabý

    void Awake()
    {
        instance = this;
    }

    public void ShowDamage(int amount, Vector3 position)
    {
        if (popupPrefab != null)
        {
            // Yazýyý karakterin biraz tepesinde oluþtur (Kafasýnýn üstü)
            Vector3 spawnPos = position + new Vector3(0, 2.5f, 0);

            // Rastgelelik ekle (Hepsi üst üste binmesin)
            spawnPos.x += Random.Range(-0.5f, 0.5f);

            GameObject popup = Instantiate(popupPrefab, spawnPos, Quaternion.identity);

            // Yazýyý kur
            var script = popup.GetComponent<DamagePopup>();
            if (script != null) script.Setup(amount);
        }
    }
}