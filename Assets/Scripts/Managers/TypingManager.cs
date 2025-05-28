using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TypingManager : MonoBehaviour
{
    [Header("Order Settings")]
    public OrderData currentOrder;
    public int currentIngredientIndex = 0;
    
    [Header("Typing Settings")]
    public string currentInput = "";
    public bool isCaseSensitive = false;
    
    [Header("Events")]
    public UnityEvent<IngredientData> onCorrectIngredient;
    public UnityEvent<IngredientData> onWrongIngredient;
    public UnityEvent onOrderComplete;
    public UnityEvent onOrderFailed;
    
    private void Start()
    {
        if (currentOrder == null)
        {
            Debug.LogWarning("No order assigned to TypingManager!");
            return;
        }
        
        ResetTyping();
    }
    
    public void ResetTyping()
    {
        currentInput = "";
        currentIngredientIndex = 0;
    }
    
    public void AddCharacter(char c)
    {
        currentInput += c;
        CheckInput();
    }
    
    public void RemoveLastCharacter()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
        }
    }
    
    private void CheckInput()
    {
        if (currentOrder == null || currentIngredientIndex >= currentOrder.ingredients.Count)
            return;
            
        IngredientData expectedIngredient = currentOrder.ingredients[currentIngredientIndex];
        string expectedName = isCaseSensitive ? expectedIngredient.ingredientName : expectedIngredient.ingredientName.ToLower();
        string currentName = isCaseSensitive ? currentInput : currentInput.ToLower();
        
        // Check if input matches expected ingredient
        if (currentName == expectedName)
        {
            onCorrectIngredient?.Invoke(expectedIngredient);
            currentIngredientIndex++;
            currentInput = "";
            
            // Check if order is complete
            if (currentIngredientIndex >= currentOrder.ingredients.Count)
            {
                onOrderComplete?.Invoke();
            }
        }
        else if (currentName.Length >= expectedName.Length)
        {
            // Input is too long or doesn't match
            onWrongIngredient?.Invoke(expectedIngredient);
            onOrderFailed?.Invoke();
        }
    }
    
    public void SetNewOrder(OrderData newOrder)
    {
        currentOrder = newOrder;
        ResetTyping();
    }
} 