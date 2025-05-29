using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Lettuce Eat/Ingredient")]
public class Ingredient : ScriptableObject
{
    public string displayName;
    public float preparationTime;
    public Sprite icon;
    public Color ingredientColor = Color.white;
    [TextArea(2, 4)]
    public string description;
} 