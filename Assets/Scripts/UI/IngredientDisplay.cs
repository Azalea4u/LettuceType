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
        rectTransform.sizeDelta = new Vector2(200f, 200f); // Default size, adjust as needed
    }

    public void SetCurrentColor()
    {
        if (image != null)
            image.color = Color.yellow;
    }
    public void SetCompletedColor()
    {
        if (image != null)
            image.color = Color.green;
    }
    public void SetDefaultColor()
    {
        if (image != null)
            image.color = Color.white;
    }
} 