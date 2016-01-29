using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        public static void Start(List<User> users, List<Product> products)
        {
            // Write welcome message
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");

            // Login
            Login:

            // Prompt for user input
            Console.WriteLine("\nEnter Username:");
            string name = Console.ReadLine();

            if (!string.IsNullOrEmpty(name))
            {
                User user = validUserName(users, name);
                if (user != null)
                {
                    // Prompt for user input
                    Console.WriteLine("Enter Password:");
                    string password = Console.ReadLine();

                    user = validPassword(users, name, password);
                    if (user != null)
                    {
                        // Welcome
                        printMessage(ConsoleColor.Green, "", "Login successful! Welcome " + name + "!");
                        
                        // Show remaining balance
                        double bal = user.Bal;
                        Console.WriteLine("\nYour balance is " + bal.ToString("C"));

                        // Show product list
                        while (true)
                        {
                            printProductsList(products);

                            // Prompt for user input
                            Console.WriteLine("Enter a number:");
                            string answer = Console.ReadLine();
                            int num = Convert.ToInt32(answer) - 1;

                            // Check if user entered number that equals product count
                            if (num == 7)
                            {
                                // Update balance
                                foreach (var usr in users)
                                {
                                    // Check that name and password match
                                    if (usr.Name == name && usr.Pwd == password)
                                    {
                                        usr.Bal = bal;
                                    }
                                }

                                // Write out new balance
                                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                                File.WriteAllText(@"Data/Users.json", json);

                                // Write out new quantities
                                string json2 = JsonConvert.SerializeObject(products, Formatting.Indented);
                                File.WriteAllText(@"Data/Products.json", json2);


                                // Prevent console from closing
                                Console.WriteLine("\nPress Enter key to exit");
                                Console.ReadLine();
                                return;
                            }
                            else
                            {
                                Console.WriteLine("\nYou want to buy: " + products[num].Name);
                                Console.WriteLine("Your balance is " + bal.ToString("C"));

                                // Prompt for user input
                                Console.WriteLine("Enter amount to purchase:");
                                answer = Console.ReadLine();
                                int qty = Convert.ToInt32(answer);

                                // Check if balance - quantity * price is less than 0
                                if (bal - products[num].Price * qty < 0)
                                {
                                    printMessage(ConsoleColor.Red, "", "You do not have enough money to buy that.");
                                    continue;
                                }

                                // Check if quantity is less than quantity
                                if (products[num].Qty <= qty)
                                {
                                    printMessage(ConsoleColor.Red, "", "Sorry, " + products[num].Name + " is out of stock");
                                    continue;
                                }

                                // Check if quantity is greater than zero
                                if (qty > 0)
                                {
                                    // Balance = Balance - Price * Quantity
                                    bal = bal - products[num].Price * qty;

                                    // Quanity = Quantity - Quantity
                                    products[num].Qty = products[num].Qty - qty;
                                    printMessage(ConsoleColor.Green, "You bought " + qty + " " + products[num].Name, "Your new balance is " + bal.ToString("C"));
                                }
                                else
                                {
                                    // Quantity is less than zero
                                    printMessage(ConsoleColor.Yellow, "", "Purchase cancelled");
                                }
                            }
                        }
                    }
                    else
                    {
                        // Invalid Password
                        printMessage(ConsoleColor.Red, "", "You entered an invalid password.");
                        goto Login;
                    }
                }
                else
                {
                    // Invalid User
                    printMessage(ConsoleColor.Red, "", "You entered an invalid user.");
                    goto Login;
                }
            }

            // Prevent console from closing
            Console.WriteLine("\nPress Enter key to exit");
            Console.ReadLine();
        }

        public static User validUserName(List<User> users, String input)
        {
            foreach (User user in users)
            {
                if (user.Name == input)
                {
                    return user;
                }
            }

            return null;
        }

        public static User validPassword(List<User> users, String name, String password)
        {
            foreach (User user in users)
            {
                if (user.Name == name && user.Pwd == password)
                {
                    return user;
                }
            }

            return null;
        }

        public static void printProductsList(List<Product> products)
        {
            Console.WriteLine("\nWhat would you like to buy?");
            foreach (Product product in products)
            {
                Console.WriteLine(products.IndexOf(product) + 1 + ": " + product.Name + " (" + product.Price.ToString("C") + ")");
            }
            Console.WriteLine(products.Count + 1 + ": Exit");
        }

        public static void printMessage(ConsoleColor color, String messageOne, String messageTwo)
        {
            Console.Clear();
            Console.ForegroundColor = color;
            Console.WriteLine(messageOne);
            Console.WriteLine(messageTwo);
            Console.ResetColor();
        }
    }
 
}
