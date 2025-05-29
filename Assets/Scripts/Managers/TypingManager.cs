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
    
    public int Score { get; private set; } = 0;
    public List<bool> ingredientCorrectness = new List<bool>();
    
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
        ingredientCorrectness = new List<bool>();
        if (currentOrder != null)
            for (int i = 0; i < currentOrder.ingredients.Count; i++)
                ingredientCorrectness.Add(false);
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
            ingredientCorrectness[currentIngredientIndex] = true;
            Score += 1;
            currentIngredientIndex++;
            onCorrectIngredient?.Invoke(expectedIngredient);
            currentInput = "";
            
            // Check if order is complete
            if (currentIngredientIndex >= currentOrder.ingredients.Count)
            {
                onOrderComplete?.Invoke();
            }
        }
        else if (currentName.Length == expectedName.Length)
        {
            // Input is incorrect and complete
            ingredientCorrectness[currentIngredientIndex] = false;
            Score -= 2;
            currentIngredientIndex++;
            onWrongIngredient?.Invoke(expectedIngredient);
            currentInput = "";
            // Check if order is complete
            if (currentIngredientIndex >= currentOrder.ingredients.Count)
            {
                onOrderComplete?.Invoke();
            }
        }
    }
    
    public void SetNewOrder(OrderData newOrder)
    {
        currentOrder = newOrder;
        ResetTyping();
    }
    
    public void ProcessFullInput(string input)
    {
        if (currentOrder == null || currentIngredientIndex >= currentOrder.ingredients.Count)
            return;

        IngredientData expectedIngredient = currentOrder.ingredients[currentIngredientIndex];
        string expectedName = isCaseSensitive ? expectedIngredient.ingredientName : expectedIngredient.ingredientName.ToLower();
        string currentName = isCaseSensitive ? input : input.ToLower();

        if (currentName == expectedName)
        {
            ingredientCorrectness[currentIngredientIndex] = true;
            Score += 1;
            currentIngredientIndex++;
            onCorrectIngredient?.Invoke(expectedIngredient);
            currentInput = "";
            if (currentIngredientIndex >= currentOrder.ingredients.Count)
            {
                onOrderComplete?.Invoke();
            }
        }
        else
        {
            ingredientCorrectness[currentIngredientIndex] = false;
            Score -= 2;
            currentIngredientIndex++;
            onWrongIngredient?.Invoke(expectedIngredient);
            currentInput = "";
            if (currentIngredientIndex >= currentOrder.ingredients.Count)
            {
                onOrderComplete?.Invoke();
            }
        }
    }
} 