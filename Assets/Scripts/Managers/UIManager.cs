using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Order Display")]
    public TextMeshProUGUI orderTicketText;
    public Image timerBar;
    public TextMeshProUGUI timerText;
    
    [Header("Typing Interface")]
    public TMP_InputField typingInput;
    public TextMeshProUGUI currentIngredientText;
    
    [Header("Burger Stack")]
    public Transform burgerStackContainer;
    public GameObject ingredientPrefab;
    private List<GameObject> currentStack = new List<GameObject>();
    
    [Header("References")]
    public TypingManager typingManager;
    
    private void Start()
    {
        if (typingManager == null)
        {
            typingManager = FindObjectOfType<TypingManager>();
        }
        
        // Subscribe to typing manager events
        typingManager.onCorrectIngredient.AddListener(OnCorrectIngredient);
        typingManager.onWrongIngredient.AddListener(OnWrongIngredient);
        typingManager.onOrderComplete.AddListener(OnOrderComplete);
        typingManager.onOrderFailed.AddListener(OnOrderFailed);
        
        // Setup typing input
        typingInput.onValueChanged.AddListener(OnTypingInputChanged);
    }
    
    public void UpdateOrderDisplay(OrderData order)
    {
        if (order == null) return;
        
        // Update order ticket
        string orderText = "Order: " + order.orderName + "\n\nIngredients:";
        for (int i = 0; i < order.ingredients.Count; i++)
        {
            orderText += "\n" + (i + 1) + ". " + order.ingredients[i].ingredientName;
        }
        orderTicketText.text = orderText;
        
        // Clear and reset burger stack
        ClearBurgerStack();
        
        // Update current ingredient text
        UpdateCurrentIngredientText();
    }
    
    private void UpdateCurrentIngredientText()
    {
        if (typingManager.currentOrder == null || 
            typingManager.currentIngredientIndex >= typingManager.currentOrder.ingredients.Count)
        {
            currentIngredientText.text = "No order";
            return;
        }
        
        IngredientData currentIngredient = typingManager.currentOrder.ingredients[typingManager.currentIngredientIndex];
        currentIngredientText.text = "Type: " + currentIngredient.ingredientName;
    }
    
    private void OnTypingInputChanged(string value)
    {
        // Handle each character input
        foreach (char c in value)
        {
            typingManager.AddCharacter(c);
        }
        
        // Clear input field after processing
        typingInput.text = "";
    }
    
    private void OnCorrectIngredient(IngredientData ingredient)
    {
        // Add ingredient to visual stack
        AddIngredientToStack(ingredient);
        UpdateCurrentIngredientText();
    }
    
    private void OnWrongIngredient(IngredientData ingredient)
    {
        // Visual feedback for wrong ingredient
        // Could add shake effect or red flash
        Debug.Log("Wrong ingredient typed!");
    }
    
    private void OnOrderComplete()
    {
        Debug.Log("Order complete!");
        // Add celebration effects or animations
    }
    
    private void OnOrderFailed()
    {
        Debug.Log("Order failed!");
        // Add failure effects or animations
    }
    
    private void AddIngredientToStack(IngredientData ingredient)
    {
        GameObject newIngredient = Instantiate(ingredientPrefab, burgerStackContainer);
        Image ingredientImage = newIngredient.GetComponent<Image>();
        ingredientImage.sprite = ingredient.ingredientSprite;
        
        // Position the ingredient in the stack
        RectTransform rectTransform = newIngredient.GetComponent<RectTransform>();
        float yOffset = 0;
        foreach (GameObject existingIngredient in currentStack)
        {
            yOffset += existingIngredient.GetComponent<RectTransform>().sizeDelta.y;
        }
        rectTransform.anchoredPosition = new Vector2(0, yOffset);
        
        currentStack.Add(newIngredient);
    }
    
    private void ClearBurgerStack()
    {
        foreach (GameObject ingredient in currentStack)
        {
            Destroy(ingredient);
        }
        currentStack.Clear();
    }
    
    public void UpdateTimer(float currentTime, float maxTime)
    {
        float fillAmount = currentTime / maxTime;
        timerBar.fillAmount = fillAmount;
        timerText.text = Mathf.CeilToInt(currentTime).ToString();
    }
} 