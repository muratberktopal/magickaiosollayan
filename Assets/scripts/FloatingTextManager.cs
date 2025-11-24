using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager instance;

    public GameObject popupPrefab; // Hazýrladýðýn Prefab

    void Awake()
    {
        instance = this;
    }

    public void ShowDamage(int amount, Vector3 position)
    {
        if (popupPrefab != null)
        {
            // Yazýyý hasar yiyen kiþinin biraz tepesinde oluþtur
            Vector3 spawnPos = position + new Vector3(0, 2f, 0); // Yükseklik ayarý

            GameObject popup = Instantiate(popupPrefab, spawnPos, Quaternion.identity);

            // Yazýnýn içindeki Setup fonksiyonunu çalýþtýr
            popup.GetComponent<DamagePopup>().Setup(amount);
        }
    }
}