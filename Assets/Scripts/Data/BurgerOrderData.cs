using UnityEngine;

[CreateAssetMenu(fileName = "New Burger Order", menuName = "Typing Chef/Burger Order Data")]
public class BurgerOrderData : OrderData
{
    public override bool IsValidOrder()
    {
        if (ingredients.Count < 2) return false; // Need at least top and bottom bun
        
        // Check if first ingredient is bottom bun and last is top bun
        bool hasBottomBun = ingredients[0].isBottomBun;
        bool hasTopBun = ingredients[ingredients.Count - 1].isTopBun;
        
        return hasBottomBun && hasTopBun;
    }
} 