using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Order", menuName = "Lettuce Eat/Order Data")]
public class OrderData : ScriptableObject
{
    [System.Serializable]
    public class IngredientRequirement
    {
        public Ingredient ingredient;
        public int quantity = 1;
    }

    public string orderName;
    [TextArea(2, 4)]
    public string description;
    public List<IngredientRequirement> requiredIngredients;
    public float baseTimeLimit = 60f;
    public int basePoints = 100;
    public Sprite orderIcon;
} 