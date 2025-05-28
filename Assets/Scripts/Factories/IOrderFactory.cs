using UnityEngine;

public interface IOrderFactory
{
    OrderData GenerateRandomOrder();
    bool CanGenerateOrder();
} 