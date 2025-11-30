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
        // Debug için envanteri yazdýralým
        string envanterListesi = "";
        foreach (var item in currentInventory) envanterListesi += item.ToString() + ", ";
        Debug.Log(" ÞU ANKÝ ENVANTER: " + envanterListesi);

        MergeRecipe bestMatch = null;
        int maxMatchCount = 0;

        foreach (var recipe in allRecipes)
        {
            if (recipe.requiredIngredients == null || recipe.requiredIngredients.Count == 0) continue;

            // --- DÜZELTME BURADA ---
            // Gerçek envanteri bozmamak için geçici bir kopyasýný alýyoruz.
            List<ItemType> tempInventory = new List<ItemType>(currentInventory);

            bool hasAll = true;

            // Tarifteki her bir malzemeyi tek tek arýyoruz
            foreach (var requiredItem in recipe.requiredIngredients)
            {
                if (tempInventory.Contains(requiredItem))
                {
                    // Eðer varsa, bu kopyadan SÝLÝYORUZ ki ikinci kez saymasýn!
                    tempInventory.Remove(requiredItem);
                }
                else
                {
                    // Yoksa (veya bitmiþse) bu tarif geçersizdir.
                    hasAll = false;
                    break;
                }
            }
            // -----------------------

            if (hasAll)
            {
                Debug.Log($" GEÇERLÝ TARÝF: {recipe.name} -> {recipe.resultItem}");

                // En çok malzeme harcayan tarifi önceliklendir
                if (recipe.requiredIngredients.Count > maxMatchCount)
                {
                    bestMatch = recipe;
                    maxMatchCount = recipe.requiredIngredients.Count;
                }
            }
        }

        if (bestMatch != null)
        {
            Debug.Log(" KAZANAN TARÝF: " + bestMatch.resultItem);
            PerformMerge(bestMatch);
        }
        else
        {
            Debug.Log(" HÝÇBÝR BÝRLEÞÝM BULUNAMADI (Bu normal, malzeme biriktiriyorsun).");
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
            case ItemType.Slash: 
                weaponSelector.SelectSlash();
                break;
            case ItemType.Club:
                weaponSelector.SelectClub();
                break;
            case ItemType.Scythe:
                weaponSelector.SelectScythe();
                break;
            case ItemType.GreatSword:
                weaponSelector.SelectGreatSword();
                break;
            case ItemType.Bow:
                weaponSelector.SelectBow();
                break;
            case ItemType.Nunchaku:
                weaponSelector.SelectNunchaku();
                break;
            case ItemType.Chain:
                weaponSelector.SelectChain();
                break;
            case ItemType.Flail:
                weaponSelector.SelectFlail();
                break;
            case ItemType.Composite_bow:
                weaponSelector.SelectComposite_bow();
                break;
            case ItemType.Net:
                weaponSelector.SelectNet();
                break;
            case ItemType.Double_Sided_Sword:
                weaponSelector.SelectDouble_Sided_Sword();
                break;
            case ItemType.Chaos:
                weaponSelector.SelectChaos();
                break; 
            case ItemType.Boomerang:
                weaponSelector.SelectBoomerang();
                break;
            case ItemType.Tree:
                weaponSelector.SelectTree();
                break;
            case ItemType.Excalibur:
                weaponSelector.SelectExcalibur();
                break;
            case ItemType.Razor:
                weaponSelector.SelectRazor();
                break;
            case ItemType.Buzzsaw:
                weaponSelector.SelectBuzzsaw();
                break;
            case ItemType.Lightning:
                weaponSelector.SelectTesla();
                break;

            case ItemType.IceShard:
                weaponSelector.SelectIceShard();
                break;
            case ItemType.Fireball:
                weaponSelector.SelectFireball();
                break;
        }
    }
}