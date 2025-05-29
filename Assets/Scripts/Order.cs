using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order
{
    public OrderData orderData;
    public Dictionary<Ingredient, int> remainingIngredients;
    public List<Ingredient> typedIngredients;
    public float timeLimit;
    public float timeRemaining;
    public bool isComplete;
    public int customerSatisfaction;
    public int mistakesMade;
    public const int SATISFACTION_PENALTY_PER_MISTAKE = 15;

    public Order(OrderData orderData)
    {
        this.orderData = orderData;
        this.timeLimit = orderData.baseTimeLimit;
        this.timeRemaining = timeLimit;
        this.isComplete = false;
        this.customerSatisfaction = 100;
        this.mistakesMade = 0;
        
        // Initialize remaining ingredients
        remainingIngredients = new Dictionary<Ingredient, int>();
        typedIngredients = new List<Ingredient>();
        
        foreach (var requirement in orderData.requiredIngredients)
        {
            remainingIngredients[requirement.ingredient] = requirement.quantity;
        }
    }

    public void UpdateTime(float deltaTime)
    {
        if (!isComplete)
        {
            timeRemaining -= deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                customerSatisfaction = Mathf.Max(0, customerSatisfaction - 50);
            }
        }
    }

    public (bool isCorrect, bool isComplete) ProcessIngredient(string typedIngredient)
    {
        if (isComplete)
            return (false, true);

        // Find the ingredient that matches the typed name
        foreach (var kvp in remainingIngredients)
        {
            Ingredient ingredient = kvp.Key;
            if (ingredient.name.ToLower() == typedIngredient.ToLower())
            {
                typedIngredients.Add(ingredient);
                remainingIngredients[ingredient]--;
                
                if (remainingIngredients[ingredient] <= 0)
                {
                    remainingIngredients.Remove(ingredient);
                }

                // Check if all ingredients are complete
                if (remainingIngredients.Count == 0)
                {
                    isComplete = true;
                    return (true, true);
                }

                return (true, false);
            }
        }

        // Wrong ingredient typed
        mistakesMade++;
        customerSatisfaction = Mathf.Max(0, customerSatisfaction - SATISFACTION_PENALTY_PER_MISTAKE);
        return (false, false);
    }

    public List<(Ingredient ingredient, int remaining)> GetRemainingIngredients()
    {
        var result = new List<(Ingredient, int)>();
        foreach (var kvp in remainingIngredients)
        {
            result.Add((kvp.Key, kvp.Value));
        }
        return result;
    }
} 