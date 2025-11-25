using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Game/Merge Recipe")]
public class MergeRecipe : ScriptableObject
{
    [Header("Sonuç")]
    public ItemType resultItem; // EvolutionManager bunu arýyor!

    [Header("Gereken Malzemeler")]
    // Artýk input1, input2 yok. Liste var.
    public List<ItemType> requiredIngredients;
}