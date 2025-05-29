using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public float minTimeBetweenOrders = 5f;
    public float maxTimeBetweenOrders = 15f;
    public int maxSimultaneousOrders = 3;

    [Header("Order Configuration")]
    public List<OrderData> availableOrders;
    [Range(0, 1)]
    public float difficultyMultiplier = 1f;

    [Header("UI References")]
    public TMP_InputField inputField;
    public Transform orderContainer;
    public GameObject orderPrefab;
    
    [Header("References")]
    public UIManager uiManager;

    private List<Order> activeOrders = new List<Order>();
    private float nextOrderTime;
    private int totalScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        nextOrderTime = Time.time + Random.Range(minTimeBetweenOrders, maxTimeBetweenOrders);
        if (inputField != null)
        {
            inputField.onSubmit.AddListener(ProcessInput);
        }
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(totalScore);
        }
    }

    private void Update()
    {
        // Update active orders
        for (int i = activeOrders.Count - 1; i >= 0; i--)
        {
            Order order = activeOrders[i];
            order.UpdateTime(Time.deltaTime);
            
            if (uiManager != null)
            {
                uiManager.UpdateOrderDisplay(order);
            }

            // Remove completed orders that have been displayed long enough
            if (order.isComplete && order.timeRemaining <= -2f)
            {
                if (uiManager != null)
                {
                    uiManager.RemoveOrderDisplay(order);
                }
                activeOrders.RemoveAt(i);
            }
        }

        // Generate new orders
        if (Time.time >= nextOrderTime && activeOrders.Count < maxSimultaneousOrders)
        {
            GenerateNewOrder();
            nextOrderTime = Time.time + Random.Range(minTimeBetweenOrders, maxTimeBetweenOrders);
        }
    }

    private void GenerateNewOrder()
    {
        if (availableOrders == null || availableOrders.Count == 0)
        {
            Debug.LogWarning("No available orders configured!");
            return;
        }

        // Select a random order from available orders
        OrderData orderData = availableOrders[Random.Range(0, availableOrders.Count)];
        
        // Create new order with adjusted time based on difficulty
        Order newOrder = new Order(orderData);
        newOrder.timeLimit *= difficultyMultiplier;
        newOrder.timeRemaining = newOrder.timeLimit;
        
        activeOrders.Add(newOrder);
        
        if (uiManager != null)
        {
            uiManager.DisplayOrder(newOrder);
        }
    }

    private void ProcessInput(string input)
    {
        input = input.ToLower().Trim();
        bool ingredientProcessed = false;
        
        // Check each active order for the ingredient
        foreach (Order order in activeOrders)
        {
            if (!order.isComplete)
            {
                var (isCorrect, isComplete) = order.ProcessIngredient(input);
                
                if (isCorrect)
                {
                    ingredientProcessed = true;
                    if (uiManager != null)
                    {
                        uiManager.ShowFeedback("Correct!", Color.green);
                        uiManager.UpdateOrderDisplay(order);
                        
                        if (isComplete)
                        {
                            // Calculate score based on time remaining and mistakes
                            int orderScore = CalculateOrderScore(order);
                            totalScore += orderScore;
                            uiManager.UpdateScore(totalScore);
                            uiManager.ShowFeedback($"Order Complete! +{orderScore} points", Color.green);
                        }
                    }
                    break;
                }
            }
        }

        if (!ingredientProcessed && uiManager != null)
        {
            uiManager.ShowFeedback("Wrong ingredient! -15 satisfaction", Color.red);
        }

        if (inputField != null)
        {
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    private int CalculateOrderScore(Order order)
    {
        float timeBonus = Mathf.Max(0, order.timeRemaining / order.timeLimit);
        float satisfactionMultiplier = order.customerSatisfaction / 100f;
        return Mathf.RoundToInt(order.orderData.basePoints * timeBonus * satisfactionMultiplier);
    }
} 