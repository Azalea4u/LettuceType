using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Order Display")]
    public TextMeshProUGUI orderTicketText;
    public TextMeshProUGUI OrderCompletedText;
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
    public GameManager gameManager;
    
    private void Start()
    {
        if (typingManager == null)
        {
            typingManager = FindObjectOfType<TypingManager>();
        }
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        
        // Subscribe to typing manager events
        typingManager.onCorrectIngredient.AddListener(OnCorrectIngredient);
        typingManager.onWrongIngredient.AddListener(OnWrongIngredient);
        typingManager.onOrderComplete.AddListener(OnOrderComplete);
        typingManager.onOrderFailed.AddListener(OnOrderFailed);

        // Setup typing input
        typingInput.onSubmit.AddListener(OnTypingSubmitted);

        if (gameManager != null)
        {
            gameManager.onOrderCountChanged.AddListener(UpdateOrderCountDisplay);
        }
    }

    public void UpdateOrderDisplay(OrderData order)
    {
        if (order == null) return;
        
        // Update order ticket (with color tags, bottom-to-top)
        string orderText = "Order: " + order.orderName + "\n";
        for (int i = order.ingredients.Count - 1; i >= 0; i--)
        {
            string colorTag;
            if (i < typingManager.currentIngredientIndex)
                colorTag = "<color=#56da56>"; // green
            else if (i == typingManager.currentIngredientIndex)
                colorTag = "<color=#FFFF00>"; // yellow
            else
                colorTag = "<color=#FFFFFF>"; // white
            orderText += $"\n{colorTag}{i + 1}. {order.ingredients[i].ingredientName}</color>";
        }
        orderTicketText.text = orderText;
        
        // Update completed order tally text
        if (OrderCompletedText != null && gameManager != null)
            OrderCompletedText.text = $"Orders Completed: {gameManager.CompletedOrderCount}";
        
        // Clear and reset burger stack
        ClearBurgerStack();
        
        // Add all ingredients to stack and color them (bottom-to-top)
        float yOffset = 0f;
        for (int i = order.ingredients.Count - 1; i >= 0; i--)
        {
            GameObject ingredientObj = Instantiate(ingredientPrefab, burgerStackContainer);
            IngredientDisplay display = ingredientObj.GetComponent<IngredientDisplay>();
            display.SetIngredient(order.ingredients[i]);
            display.SetDefaultColor(); // Always white in stack now
            ingredientObj.transform.SetSiblingIndex(0);
            // Set vertical position for stacking
            RectTransform rect = ingredientObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, yOffset);
            yOffset -= order.ingredients[i].height * 30f; // 100f is base height, adjust as needed
            // Hide future ingredients
            Image img = ingredientObj.GetComponent<Image>();
            if (i > typingManager.currentIngredientIndex)
                img.enabled = false;
            else
                img.enabled = true;
            currentStack.Add(ingredientObj);
        }
        
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

    private void OnTypingSubmitted(string value)
    {
        foreach (char c in value)
        {
            typingManager.AddCharacter(c);
        }
        typingInput.text = "";
        typingInput.ActivateInputField(); // Keeps the input focused
    }


    private void OnCorrectIngredient(IngredientData ingredient)
    {
        UpdateOrderDisplay(typingManager.currentOrder);
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
        // Deprecated: now handled in UpdateOrderDisplay for color logic
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

    private void UpdateOrderCountDisplay(int count)
    {
        // Refresh the tally display only
        if (OrderCompletedText != null && gameManager != null)
            OrderCompletedText.text = $"Orders Completed: {gameManager.CompletedOrderCount}";
    }
} 