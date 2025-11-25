using UnityEngine;
using System.Collections.Generic;

public class EvolutionManager : MonoBehaviour
{
    public static EvolutionManager instance;

    [Header("Ayarlar")]
    // Hangi levellarda eþya seçimi gelecek? (Örn: 4, 9, 14...)
    public List<int> milestoneLevels = new List<int> { 4, 9 };

    [Header("Veritabaný")]
    public List<MergeRecipe> allRecipes; // Tüm tarifleri buraya sürükle

    [Header("Mevcut Durum")]
    public List<ItemType> currentInventory = new List<ItemType>();
    public bool isMagicPath = false; // Asa seçildiyse true olacak

    // Aktif silah scriptlerini açýp kapatmak için referanslar (WeaponSelector'dan alabiliriz)
    public WeaponSelector weaponSelector;

    private void Awake()
    {
        instance = this;
    }

    // Oyun baþlarken ilk seçilen silahý kaydet
    public void SetStarterWeapon(ItemType starter)
    {
        currentInventory.Clear();
        currentInventory.Add(starter);

        if (starter == ItemType.Staff)
        {
            isMagicPath = true;
            Debug.Log("Büyücü Yolu Seçildi!");
        }
        else
        {
            isMagicPath = false;
            Debug.Log("Savaþçý Yolu Seçildi!");
        }
    }

    public bool IsEvolutionLevel(int currentLevel)
    {
        return milestoneLevels.Contains(currentLevel);
    }

    public void AddMaterial(ItemType newItem)
    {
        Debug.Log("Yeni Malzeme Eklendi: " + newItem);
        currentInventory.Add(newItem);
        CheckForMerge();
    }

    void CheckForMerge()
    {
        MergeRecipe bestMatch = null;
        int maxMatchCount = 0;

        foreach (var recipe in allRecipes)
        {
            // 1. Bu tarifin malzemeleri bizde tam olarak var mý?
            bool hasAll = true;

            // Eðer tarifin malzeme listesi boþsa (hatalý tarif) atla
            if (recipe.requiredIngredients == null || recipe.requiredIngredients.Count == 0) continue;

            foreach (var item in recipe.requiredIngredients)
            {
                // Envanterimizde bu malzemeden YETERÝNCE var mý kontrolü
                // Basit 'Contains' yerine Count kontrolü daha güvenlidir ama þimdilik Contains yeterli
                if (!currentInventory.Contains(item))
                {
                    hasAll = false;
                    break;
                }
            }

            // 2. En karmaþýk tarifi seçme mantýðý
            if (hasAll && recipe.requiredIngredients.Count > maxMatchCount)
            {
                bestMatch = recipe;
                maxMatchCount = recipe.requiredIngredients.Count;
            }
        }

        if (bestMatch != null)
        {
            PerformMerge(bestMatch);
        }
    }

    // --- HATANIN ÇÖZÜLDÜÐÜ YER BURASI ---
    void PerformMerge(MergeRecipe recipe)
    {
        Debug.Log("BÝRLEÞÝM BAÞARILI! Oluþan: " + recipe.resultItem);

        // Eski Hata Veren Kod: currentInventory.Remove(recipe.input1); 
        // YENÝ KOD: Listedeki malzemeleri tek tek bulup siliyoruz.
        foreach (var item in recipe.requiredIngredients)
        {
            currentInventory.Remove(item);
        }

        // 2. Sonucu ekle
        currentInventory.Add(recipe.resultItem);

        // 3. Silahý Aktif Et
        UnlockWeaponLogic(recipe.resultItem);
    }

    void UnlockWeaponLogic(ItemType type)
    {
        if (weaponSelector == null) return;

        switch (type)
        {
            case ItemType.Spear:
                weaponSelector.SelectSpear();
                break;
            case ItemType.Slash: // Eðer tekrar Slash seçilirse (geliþmiþi)
                weaponSelector.SelectSlash();
                break;
            case ItemType.Club:
                weaponSelector.SelectClub();
                break;
                // Buraya yeni birleþmiþ silahlarýný (case ItemType.Whip: vb.) eklemelisin
        }
    }
}