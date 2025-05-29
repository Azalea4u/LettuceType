using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Order Display")]
    public GameObject orderPrefab;
    public Transform orderContainer;
    public float orderSpacing = 120f;

    [Header("Input Display")]
    public TMP_InputField ingredientInput;
    public TextMeshProUGUI feedbackText;
    public Image progressBar;

    [Header("Score Display")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;

    private Dictionary<Order, GameObject> orderDisplays = new Dictionary<Order, GameObject>();

    private void Start()
    {
        if (ingredientInput != null)
        {
            ingredientInput.onValueChanged.AddListener(OnInputValueChanged);
            ingredientInput.Select();
            ingredientInput.ActivateInputField();
        }
    }

    public void DisplayOrder(Order order)
    {
        if (orderPrefab != null && orderContainer != null)
        {
            GameObject orderDisplay = Instantiate(orderPrefab, orderContainer);
            orderDisplays.Add(order, orderDisplay);
            UpdateOrderDisplay(order);
        }
    }

    public void UpdateOrderDisplay(Order order)
    {
        if (orderDisplays.TryGetValue(order, out GameObject display))
        {
            // Update the order display UI elements
            TextMeshProUGUI[] texts = display.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                // Update ingredients list
                string ingredients = "";
                foreach (Ingredient ingredient in order.requiredIngredients)
                {
                    ingredients += ingredient.displayName + "\n";
                }
                texts[0].text = ingredients;

                // Update timer
                texts[1].text = $"Time: {Mathf.CeilToInt(order.timeRemaining)}s";
            }

            // Update progress bar if it exists
            Image progressBar = display.GetComponentInChildren<Image>();
            if (progressBar != null)
            {
                progressBar.fillAmount = order.timeRemaining / order.timeLimit;
            }
        }
    }

    public void RemoveOrderDisplay(Order order)
    {
        if (orderDisplays.TryGetValue(order, out GameObject display))
        {
            Destroy(display);
            orderDisplays.Remove(order);
        }
    }

    private void OnInputValueChanged(string value)
    {
        // You can add real-time feedback here as the player types
        if (feedbackText != null)
        {
            // Add visual feedback logic here
        }
    }

    public void ShowFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
} 