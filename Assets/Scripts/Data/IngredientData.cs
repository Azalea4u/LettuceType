using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Typing Chef/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    [Header("Ingredient Properties")]
    public string ingredientName;
    public Sprite ingredientSprite;
    public float height = 1f; // Height of the ingredient in the stack
    public Vector2 spriteOffset = Vector2.zero; // Offset for visual stacking
    
    [Header("Type Properties")]
    public bool isBun = false;
    public bool isTopBun = false;
    public bool isBottomBun = false;
} 