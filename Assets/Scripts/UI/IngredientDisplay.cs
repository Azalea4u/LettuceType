using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IngredientDisplay : MonoBehaviour
{
    private Image image;
    private RectTransform rectTransform;
    
    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    
    public void SetIngredient(IngredientData ingredient)
    {
        if (ingredient == null) return;
        
        image.sprite = ingredient.ingredientSprite;
        rectTransform.sizeDelta = new Vector2(100f, 100f); // Default size, adjust as needed
    }
} 