using classes;

//Vi fortæller systemet hvor filen med produkterne ligger
string workingDirectory = Environment.CurrentDirectory;
string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
string path = Path.Combine(projectDirectory, "products.txt");

Products warehouse = new Products();


Products shoppingCart = new Products();
//function at udskrive 10 tomme linjer så consolen er mindre rodet
static void spaceInConsole(){
    for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("");
            }
}
//input function for at gøre inputs simplere
static dynamic input<T>(string x)
{
    try
    {
        Console.WriteLine(x);
        if (typeof(T) == typeof(int))
        {
            return Int32.Parse(Console.ReadLine());
        }
        else if (typeof(T) == typeof(string))
        {
            return Console.ReadLine();
        }
        else if (typeof(T) == typeof(double))
        {
            return Double.Parse(Console.ReadLine());
        }
        else
        {
            return (null);
        }
    }
    catch
    {
        Console.WriteLine("Wrong format, please try again");
        return (null);
    }
}

//vi aflæser produkterne fra text filen
try
{
    FileInfo fi = new FileInfo(path);
    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
    StreamReader sr = new StreamReader(fs);

    string line = "";
    int productId = 1;
    //Vi indlæser dokumentet linje med linje
    while ((line = sr.ReadLine()) != null)
    {
        //Hver linje bliver delt op i de argumenter, som vores Produkt konstruktør skal bruge. De nye produkter bliver tilføjet til warehouse Products objectet
        string[] text = line.Split(',');
        warehouse.list.Add(
            new Product(
                text[0],
                int.Parse(text[1]),
                int.Parse(text[2]),
                text[3],
                text[4],
                productId
            )
        );
        productId++;
    }
    fs.Close();
    sr.Close();
}
//Giver en fejlmeddelelse til brugeren
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

bool isProgramExiting = false;
//Velkomst til brugeren
Console.WriteLine("Welcome to the Halloween Shop");
while (!isProgramExiting)
{
    //Udskriver mulighederne brugeren har
    Console.WriteLine("You have the folllowing options:");
    Console.WriteLine("[1] Shopping");
    Console.WriteLine("[2] View your shopping cart");

    int numberInput = input<int>("[0] Exit program");

    //Vi gør brug af en switch case til at navigere programmet
    switch (numberInput)
    {

        case 0:
            //Lukker programmet
            spaceInConsole();
            Console.WriteLine("Program is now exiting, bye now");
            isProgramExiting = true;
            Console.ReadKey();
            break;

        case 1:
            //Svarer til use case 1: Tilføj produkt til indkøbskurv
            spaceInConsole();
            Console.WriteLine("List of products:");
            foreach (var item in warehouse.list)
            {
                Console.WriteLine("___________________________");

                item.showProductInfo();
            }
            //Tager et input fra brugeren
            int productIdChosen = input<int>(
                "Enter the number of the product that you want to add to your shopping cart or press [0] to go back to the main menu:"
            );
            //Checker om brugeren vil tilbage til forsiden
            if (productIdChosen == 0)
            {
                Console.WriteLine("Redirecting back to the menu");
            }
            //Tilføjer produktet til indkøbskurven
            else
            {
                try
                {
                    int amountInput = input<int>("How many do you want?");
                    Product chosenProduct = warehouse.findProductById(productIdChosen);
                    
                    shoppingCart.addProductToShoppingCart(chosenProduct,amountInput);
                }
                catch (System.Exception)
                {
                    System.Console.WriteLine("invalid id input, please try again");
                }
            }

            break;
        //Svarer til use case 2: Bestil ting fra indkøbskurv
        case 2:
            spaceInConsole();
            if (shoppingCart.list.Count < 1)
            {
                Console.WriteLine("Your shopping cart is empty");

                break;
            }
            else
            {
                //Udskriver antallet af varer samt navne på varerne
                Console.WriteLine($"You have {shoppingCart.itemCount} items in your shopping cart:");
                Console.WriteLine("");
                foreach (var item in shoppingCart.list)
                {
                    item.ShoppingCartStatus();
                }
            }
            
            Console.WriteLine("\nThe total price is: " + shoppingCart.totalPriceCalc() + "kr.");
            Console.WriteLine("Do you want to order the products from your shopping cart? (y/n)");
            string userInput = Console.ReadLine();
            //Svarer til use case 3: Opdater lager efter bestilling
            if (userInput == "y")
            {
                //tjekker om der er nok varer på lageret
                if(warehouse.available(shoppingCart.list))
                {
                
                //Her laver vi et metodekald til metoden reduceStockStatus

                warehouse.reduceStockStatus(shoppingCart.list);
                //Her fjerner vi alle varerne fra indkøbskurven
                shoppingCart.list.Clear();

                Console.WriteLine("Order succesfull");
                }
                //hvis der ikke er nok ledige
                else
                {
                    Product itemToRemove=null;
                    //finder det produkt der er for mange af
                    foreach (var item in shoppingCart.list)
                    {
                        if(item.quantity>item.stockStatus){
                            Console.WriteLine($"You have bought too many {item.name}s. This item will be removed from shopping cart and you have to try again.");
                            itemToRemove=item;
                        }
                    }

                    //produktet, der er formange af, bliver fjernet
                    shoppingCart.list.Remove(itemToRemove);

                }   
            }
            else
            {
                Console.WriteLine("Redirecting back to menu");
            }
            break;
        default:
            System.Console.WriteLine("Please enter a valid option.");
            break;
    }
}
