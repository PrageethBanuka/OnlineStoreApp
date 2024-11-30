using System;

class Program
{
    static void Main(string[] args)
    {
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
                Console.Write("Enter username: ");
                var username = Console.ReadLine();
                Console.Write("Enter password: ");
                var password = Console.ReadLine();

                if (userService.RegisterUser(username, password))
                {
                    Console.WriteLine("Registration successful!");
                }
            }
            else if (choice == "2")
            {
                Console.Write("Enter username: ");
                var username = Console.ReadLine();
                Console.Write("Enter password: ");
                var password = Console.ReadLine();

                if (userService.LoginUser(username, password))
                {
                    Console.WriteLine("Login successful!");
                    // Add dashboard functionality here
                }
            }
            else if (choice == "3")
            {
                Console.WriteLine("Goodbye!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice, try again.");
            }
        }
    }
}
