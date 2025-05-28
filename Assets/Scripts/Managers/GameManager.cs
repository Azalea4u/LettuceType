using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float timeBetweenOrders = 5f;
    public int maxActiveOrders = 1;
    
    [Header("References")]
    public BurgerFactory burgerFactory;
    public TypingManager typingManager;
    public UIManager uiManager;
    
    [Header("Events")]
    public UnityEvent onGameStart;
    public UnityEvent onGameOver;
    
    private float currentOrderTimer;
    private bool isGameActive;
    private int completedOrderCount = 0;
    public int CompletedOrderCount => completedOrderCount;
    
    // Optional: Event for UI updates
    public UnityEvent<int> onOrderCountChanged;
    
    private void Start()
    {
        if (burgerFactory == null || typingManager == null || uiManager == null)
        {
            Debug.LogError("Missing required references in GameManager!");
            return;
        }
        
        // Subscribe to order events
        typingManager.onOrderComplete.AddListener(OnOrderComplete);
        typingManager.onOrderFailed.AddListener(OnOrderFailed);

        StartGame();
    }
    
    private void Update()
    {
        if (!isGameActive) return;
        
        // Update current order timer
        if (typingManager.currentOrder != null)
        {
            currentOrderTimer -= Time.deltaTime;
            uiManager.UpdateTimer(currentOrderTimer, typingManager.currentOrder.timeLimit);
            
            if (currentOrderTimer <= 0)
            {
                // Order timed out
                typingManager.onOrderFailed?.Invoke();
                GenerateNewOrder();
            }
        }
    }
    
    public void StartGame()
    {
        isGameActive = true;
        onGameStart?.Invoke();
        GenerateNewOrder();
    }
    
    public void EndGame()
    {
        isGameActive = false;
        onGameOver?.Invoke();
    }
    
    private void GenerateNewOrder()
    {
        if (!burgerFactory.CanGenerateOrder())
        {
            Debug.LogError("Burger factory cannot generate orders!");
            return;
        }
        
        OrderData newOrder = burgerFactory.GenerateRandomOrder();
        typingManager.SetNewOrder(newOrder);
        uiManager.UpdateOrderDisplay(newOrder);
        currentOrderTimer = newOrder.timeLimit;
    }
    
    // Called by TypingManager events
    public void OnOrderComplete()
    {
        completedOrderCount++;
        onOrderCountChanged?.Invoke(completedOrderCount);
        // Add score or other rewards here
        GenerateNewOrder();
    }
    
    public void OnOrderFailed()
    {
        // Handle failed order (penalties, etc.)
        GenerateNewOrder();
    }
} 