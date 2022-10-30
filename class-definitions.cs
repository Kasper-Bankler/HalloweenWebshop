namespace classes
{
    public class Product
    {
        public string name { get; set; }
        public int price { get; set; }

        public int stockStatus { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public int id { get; set; }
        
        public int quantity {get;set;}

        //Konstruktør
        public Product(string name, int price, int stockStatus, string description, string category,int id)
        {
            this.name = name;
            this.price = price;
            this.stockStatus = stockStatus;
            this.description = description;
            this.category = category;
            this.id = id;
            this.quantity=0;
        }

       //Metode der viser et produkt og dets information
        public void showProductInfo()
        {
            Console.WriteLine($"[{this.id}] {this.name}");
            System.Console.WriteLine(this.description);
            System.Console.WriteLine($"price: {this.price} kr.");
            System.Console.WriteLine($"{this.stockStatus} available in warehouse.");
        }

        public void ShoppingCartStatus(){
            Console.WriteLine($"{this.quantity} x {this.name} : {this.price} kr. each");
        }
    }

    public class Products
    {
        public List<Product> list { get; set; }
        public int totalPrice { get; set; }

        public int itemCount {get; set;}
        public Products()
        {
            this.list = new List<Product>();
            this.totalPrice = 0;
            this.itemCount=0;
        }

        //Denne metode er "Tilføj produkt til indkøbskurv" use casen
        
        //Denne metode tilføjer et produkt til indkøbskurven
        public void addProductToShoppingCart(Product product, int amount)
        {
            bool foundProduct=false;
            foreach (var item in this.list)
            {
                if (item==product){
                    item.quantity+=amount;
                    foundProduct=true;

                }

                
            }
            if(!foundProduct){
                product.quantity=amount;
                this.list.Add(product);
            }
            this.totalPrice = this.totalPrice + product.price*amount;

            this.itemCount++;
        }
        public int totalPriceCalc(){
            this.totalPrice=0;
            foreach (var item in this.list)
            {
                this.totalPrice=this.totalPrice+item.quantity*item.price;
            }
            return(this.totalPrice);
            
        }

        //tjekker om ønsket antal er ledig på lageret
        public bool available(List<Product> products){
            foreach (var product in products)
            {
                 if(product.stockStatus<product.quantity){
                    return(false);
                }
            }
            return(true);

        }
        //Denne metode opdaterer lageret
        public void reduceStockStatus(List<Product> products){
            foreach (var product in products)
            {
               
             product.stockStatus=product.stockStatus-product.quantity;
                
           
            }

        }
        //Denne metode finder et produkt ud fra dets ID
        public Product findProductById(int id){
            foreach (var item in this.list)
            {
                if(item.id==id){
                    return(item);
                }
            }
            return null;
        }

        
    }
}
