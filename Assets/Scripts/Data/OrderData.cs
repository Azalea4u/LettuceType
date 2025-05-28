using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Order", menuName = "Typing Chef/Order Data")]
public class OrderData : ScriptableObject
{
    [Header("Order Properties")]
    public string orderName;
    public List<IngredientData> ingredients = new List<IngredientData>();
    public float timeLimit = 20f;
    public float baseReward = 10f;
    public float penaltyMultiplier = 0.5f; // Multiplier for failed orders
    
    public virtual bool IsValidOrder()
    {
        return ingredients.Count > 0;
    }
} 