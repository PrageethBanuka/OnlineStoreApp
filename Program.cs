using System;

class Program
{
    static void Main(string[] args)
    {
        // Initialize services
        var userService = new UserService();
        var productService = new ProductService();
        var cartService = new CartService();

        Console.WriteLine("Welcome to Online Store!");

        while (true)
        {
            Console.WriteLine("\n1. Register\n2. Login\n3. Exit");
            Console.Write("Select an option: ");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                // User Registration
                HandleUserRegistration(userService);
            }
            else if (choice == "2")
            {
                // User Login
                if (HandleUserLogin(userService))
                {
                    HandleUserDashboard(productService, cartService);
                }
            }
            else if (choice == "3")
            {
                // Exit the app
                Console.WriteLine("Goodbye!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice, try again.");
            }
        }
    }

    static void HandleUserRegistration(UserService userService)
    {
        Console.Write("Enter username: ");
        var username = Console.ReadLine();
        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        if (userService.RegisterUser(username, password))
        {
            Console.WriteLine("Registration successful!");
        }
        else
        {
            Console.WriteLine("Username already exists.");
        }
    }

    static bool HandleUserLogin(UserService userService)
    {
        Console.Write("Enter username: ");
        var username = Console.ReadLine();
        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        if (userService.LoginUser(username, password))
        {
            Console.WriteLine("Login successful!");
            return true;
        }
        else
        {
            Console.WriteLine("Invalid credentials. Try again.");
            return false;
        }
    }

    static void HandleUserDashboard(ProductService productService, CartService cartService)
    {
        while (true)
        {
            Console.WriteLine("\nDashboard:");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Add to Cart");
            Console.WriteLine("3. View Cart");
            Console.WriteLine("4. Logout");

            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                // Display Products
                DisplayProducts(productService);
            }
            else if (choice == "2")
            {
                // Add to Cart
                AddProductToCart(productService, cartService);
            }
            else if (choice == "3")
            {
                // View Cart
                cartService.ViewCart();
            }
            else if (choice == "4")
            {
                Console.WriteLine("Logged out.");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Try again.");
            }
        }
    }

    static void DisplayProducts(ProductService productService)
    {
        var products = productService.LoadProducts();
        if (products.Count == 0)
        {
            Console.WriteLine("No products available.");
        }
        else
        {
            Console.WriteLine("\nAvailable Products:");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.ProductId}. {product.Name} - {product.Category} - {product.Price:C} (Stock: {product.Stock})");
            }
        }
    }

    static void AddProductToCart(ProductService productService, CartService cartService)
    {
        Console.Write("Enter the product ID to add to the cart: ");
        if (!int.TryParse(Console.ReadLine(), out var productId))
        {
            Console.WriteLine("Invalid product ID.");
            return;
        }

        var product = productService.LoadProducts().Find(p => p.ProductId == productId);
        if (product != null)
        {
            Console.Write("Enter the quantity: ");
            if (int.TryParse(Console.ReadLine(), out var quantity))
            {
                cartService.AddToCart(product, quantity);
            }
            else
            {
                Console.WriteLine("Invalid quantity.");
            }
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }
}
