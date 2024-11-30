using Spectre.Console;
using System;
using System.Linq;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        var userService = new UserService();
        var productService = new ProductService();
        var cartService = new CartService();

        AnsiConsole.Write(
            new FigletText("Online Store")
                .Centered()
                .Color(Color.Yellow));

        AnsiConsole.WriteLine();

        while (true)
        {
            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold cyan]Select an [underline]option[/][/]:")
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: Color.Yellow))
                    .AddChoices("Register", "Login", "Exit")
            );

            switch (menuChoice)
            {
                case "Register":
                    HandleUserRegistration(userService);
                    break;
                case "Login":
                    if (HandleUserLogin(userService))
                    {
                        HandleUserDashboard(productService, cartService);
                    }
                    break;
                case "Exit":
                    AnsiConsole.MarkupLine("[bold red]Thank you for visiting our store. Goodbye![/]");
                    return;
            }
        }
    }

    static void HandleUserRegistration(UserService userService)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[yellow]User Registration[/]").RuleStyle("grey").LeftJustified());

        var username = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [bold green]username[/]:")
                .PromptStyle("cyan")
                .Validate(input => !string.IsNullOrEmpty(input), "[red]Username cannot be empty![/]")
        );

        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [bold green]password[/]:")
                .PromptStyle("cyan")
                .Secret()
        );

        AnsiConsole.Status()
            .Start("Registering user...", ctx => 
            {
                Thread.Sleep(1500); // Simulating some processing time
                if (userService.RegisterUser(username, password))
                {
                    ctx.Status("Registration successful!");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                    Thread.Sleep(1000);
                }
                else
                {
                    ctx.Status("Registration failed!");
                    ctx.Spinner(Spinner.Known.BoxBounce);
                    ctx.SpinnerStyle(Style.Parse("red"));
                    Thread.Sleep(1000);
                }
            });

        if (userService.RegisterUser(username, password))
        {
            AnsiConsole.MarkupLine("[bold green]Registration successful![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Username already exists.[/]");
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    static bool HandleUserLogin(UserService userService)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[yellow]User Login[/]").RuleStyle("grey").LeftJustified());

        var username = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [bold green]username[/]:")
                .PromptStyle("cyan")
                .Validate(input => !string.IsNullOrEmpty(input), "[red]Username cannot be empty![/]")
        );

        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [bold green]password[/]:")
                .PromptStyle("cyan")
                .Secret()
        );

        bool loginSuccess = false;

        AnsiConsole.Status()
            .Start("Logging in...", ctx => 
            {
                Thread.Sleep(1500); // Simulating some processing time
                loginSuccess = userService.LoginUser(username, password);
                if (loginSuccess)
                {
                    ctx.Status("Login successful!");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                }
                else
                {
                    ctx.Status("Login failed!");
                    ctx.Spinner(Spinner.Known.BoxBounce);
                    ctx.SpinnerStyle(Style.Parse("red"));
                }
                Thread.Sleep(1000);
            });

        if (loginSuccess)
        {
            AnsiConsole.MarkupLine("[bold green]Login successful![/]");
            return true;
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Invalid credentials. Try again.[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
            Console.ReadKey(true);
            return false;
        }
    }

    static void HandleUserDashboard(ProductService productService, CartService cartService)
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]User Dashboard[/]").RuleStyle("grey").LeftJustified());

            var dashboardChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold cyan]Choose an action:[/]")
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: Color.Yellow))
                    .AddChoices("View Products", "Add to Cart", "View Cart", "Logout")
            );

            switch (dashboardChoice)
            {
                case "View Products":
                    DisplayProducts(productService);
                    break;
                case "Add to Cart":
                    AddProductToCart(productService, cartService);
                    break;
                case "View Cart":
                    ViewCart(cartService);
                    break;
                case "Logout":
                    AnsiConsole.MarkupLine("[bold red]Logged out.[/]");
                    return;
            }
        }
    }

    static void DisplayProducts(ProductService productService)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[yellow]Product Catalog[/]").RuleStyle("grey").LeftJustified());

        var products = productService.LoadProducts();
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("ID").Centered())
            .AddColumn(new TableColumn("Name").Centered())
            .AddColumn(new TableColumn("Category").Centered())
            .AddColumn(new TableColumn("Price").Centered())
            .AddColumn(new TableColumn("Stock").Centered());

        foreach (var product in products)
        {
            table.AddRow(
                product.ProductId.ToString(),
                product.Name,
                product.Category,
                $"[green]{product.Price:C}[/]",
                product.Stock > 0 ? $"[blue]{product.Stock}[/]" : "[red]Out of stock[/]"
            );
        }

        AnsiConsole.Write(table);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    static void AddProductToCart(ProductService productService, CartService cartService)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[yellow]Add to Cart[/]").RuleStyle("grey").LeftJustified());

        var productId = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [bold green]Product ID[/] to add to the cart:")
                .PromptStyle("cyan")
                .Validate(input => input > 0, "[red]Product ID must be greater than 0.[/]")
        );

        var quantity = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [bold green]Quantity[/]:")
                .PromptStyle("cyan")
                .Validate(input => input > 0, "[red]Quantity must be greater than 0.[/]")
        );

        var product = productService.LoadProducts().FirstOrDefault(p => p.ProductId == productId);
        if (product != null)
        {
            AnsiConsole.Status()
                .Start("Adding to cart...", ctx => 
                {
                    Thread.Sleep(1000); // Simulating some processing time
                    cartService.AddToCart(product, quantity);
                    ctx.Status("Product added to cart!");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                    Thread.Sleep(1000);
                });

            AnsiConsole.MarkupLine("[bold green]Product added to cart![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Invalid Product ID.[/]");
        }

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    static void ViewCart(CartService cartService)
    {
        var cartItems = cartService.GetCartItems();

        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Shopping Cart[/]").RuleStyle("grey").LeftJustified());

            if (cartItems.Any())
            {
                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .AddColumn(new TableColumn("Product").Centered())
                    .AddColumn(new TableColumn("Quantity").Centered())
                    .AddColumn(new TableColumn("Price").Centered())
                    .AddColumn(new TableColumn("Total").Centered());

                foreach (var item in cartItems)
                {
                    table.AddRow(
                        item.ProductName,
                        item.Quantity.ToString(),
                        $"[green]{item.Price:C}[/]",
                        $"[yellow]{item.TotalPrice:C}[/]"
                    );
                }

                AnsiConsole.Write(table);

                var total = cartItems.Sum(item => item.TotalPrice);
                AnsiConsole.MarkupLine($"[bold]Total: [green]{total:C}[/][/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Your cart is empty![/]");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Press [green]Backspace[/] to go back to the dashboard or any other key to refresh.[/]");

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Backspace)
            {
                AnsiConsole.MarkupLine("[yellow]Returning to Dashboard...[/]");
                break;
            }
        }
    }
}

