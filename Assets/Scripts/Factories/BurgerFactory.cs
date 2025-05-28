using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Burger Factory", menuName = "Typing Chef/Factories/Burger Factory")]
public class BurgerFactory : ScriptableObject, IOrderFactory
{
    [Header("Burger Components")]
    public IngredientData bottomBun;
    public IngredientData topBun;
    public List<IngredientData> possibleFillings = new List<IngredientData>();
    
    [Header("Generation Settings")]
    public int minFillings = 1;
    public int maxFillings = 4;
    
    public OrderData GenerateRandomOrder()
    {
        BurgerOrderData burgerOrder = ScriptableObject.CreateInstance<BurgerOrderData>();
        burgerOrder.orderName = "Burger";
        burgerOrder.ingredients = new List<IngredientData>();
        
        // Add bottom bun
        burgerOrder.ingredients.Add(bottomBun);
        
        // Add random fillings
        int numFillings = Random.Range(minFillings, maxFillings + 1);
        for (int i = 0; i < numFillings; i++)
        {
            if (possibleFillings.Count > 0)
            {
                int randomIndex = Random.Range(0, possibleFillings.Count);
                burgerOrder.ingredients.Add(possibleFillings[randomIndex]);
            }
        }
        
        // Add top bun
        burgerOrder.ingredients.Add(topBun);
        
        return burgerOrder;
    }
    
    public bool CanGenerateOrder()
    {
        return bottomBun != null && topBun != null && possibleFillings.Count > 0;
    }
} 