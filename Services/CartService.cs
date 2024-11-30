using System;
using System.Collections.Generic;

public class CartService
{
    private readonly List<CartItem> _cart = new();

    public void AddToCart(Product product, int quantity)
    {
        var existingItem = _cart.Find(item => item.ProductId == product.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            _cart.Add(new CartItem
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity
            });
        }

        Console.WriteLine($"{quantity} x {product.Name} added to the cart.");
    }
    public List<CartItem> GetCartItems() => _cart;
    public void ViewCart()
    {
        Console.WriteLine("\nYour Cart:");
        foreach (var item in _cart)
        {
            Console.WriteLine($"{item.ProductName} - {item.Quantity} x {item.Price:C} = {item.TotalPrice:C}");
        }
        Console.WriteLine($"Total: {GetTotalPrice():C}");
    }

    public decimal GetTotalPrice()
    {
        decimal total = 0;
        foreach (var item in _cart)
        {
            total += item.TotalPrice;
        }
        return total;
    }
}
