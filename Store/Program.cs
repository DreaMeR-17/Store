using System;
using System.Collections.Generic;

namespace Store
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Shopping shopping = new Shopping();
            shopping.StartShopping();
        }
    }

    class Human
    {
        protected List<Product> Products;

        public Human(List<Product> products, int money)
        {
            Products = products;
            Money = money;
        }       

        public int Money { get; protected set; }

        public void ShowInfo()
        {
            Console.Clear();

            Console.WriteLine("Представлен списов товаров: ");

            foreach (var product in Products)
            {
                Console.WriteLine($"Товар: {product.Name} - {product.Cost} золотых.");
            }

            Console.WriteLine($"\nВ кошельке: {Money} золотых.");

            Console.ReadKey();
        }
    }

    class Seller : Human
    {
        public Seller(List<Product> products, int money = 0) : base(products, money)
        {
            FillListOfProducts(products);
        }

        public bool TryGetProduct(out Product product)
        {
            bool isSuch = false;
            string userInput;
            product = null;

            Console.WriteLine("Какой товар вы хотите приобрести?");
            userInput = Console.ReadLine();

            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i].Name == userInput)
                {
                    product = Products[i];
                    isSuch = true;
                    break;
                }
            }

            return isSuch;
        }

        public void Sell(Product product)
        {
            Products.Remove(product);
            Money += product.Cost;
        }

        private void FillListOfProducts(List<Product> products)
        {
            products.Add(new Product("Ковер самолет", 2000));
            products.Add(new Product("Зелье невидимости", 225));
            products.Add(new Product("Рог с медом", 250));
            products.Add(new Product("Копье Драупнира", 1750));
            products.Add(new Product("Сыр", 50));
            products.Add(new Product("Колбаса", 70));
            products.Add(new Product("Хлеб", 40));
            products.Add(new Product("Бочка с элем", 300));
        }
    }

    class Buyer : Human
    {
        public Buyer(List<Product> products, int money) : base(products, money) { }

        public bool IsEnoughMoney(int costOfProducts)
        {
            return Money > 0 && Money >= costOfProducts;
        }

        public void Buy(Product product)
        {
            Products.Add(product);
            Money -= product.Cost;
        }
    }

    class Product
    {
        public Product(string name, int cost)
        {
            this.Name = name;
            Cost = cost;
        }

        public string Name { get; private set; }
        public int Cost { get; private set; }
    }

    class Shopping
    {
        public void StartShopping()
        {
            const string CommandStoreProducts = "1";
            const string CommandBuyerProducts = "2";
            const string CommandPurchaseProducts = "3";
            const string CommandExit = "4";

            Random random = new Random();

            string userInput;

            bool isWork = true;

            int minMalueOfMoney = 500;
            int maxValueOfMoney = 4500;

            Seller seller = new Seller(new List<Product>());
            Buyer buyer = new Buyer(new List<Product>(), random.Next(minMalueOfMoney, maxValueOfMoney));

            while (isWork)
            {
                Console.Clear();

                Console.WriteLine($"{CommandStoreProducts} - Показать товары магазина и количество денег магазина.");
                Console.WriteLine($"{CommandBuyerProducts} - Показать товары покупателя и его кошелек.");
                Console.WriteLine($"{CommandPurchaseProducts} - Торговля.");
                Console.WriteLine($"{CommandExit} - выход.");

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandStoreProducts:
                        seller.ShowInfo();
                        break;

                    case CommandBuyerProducts:
                        buyer.ShowInfo();
                        break;

                    case CommandPurchaseProducts:
                        Trade(seller, buyer);
                        break;

                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("неверный ввод");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void Trade(Seller seller, Buyer buyer)
        {
            if (seller.TryGetProduct(out Product product))
            {
                if (buyer.IsEnoughMoney(product.Cost))
                {
                    seller.Sell(product);

                    buyer.Buy(product);

                    Console.WriteLine("Товар успешно куплен! Приходите к нам еще.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("У вас недостаточно денег.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Такого товара в магазине нет.");
                Console.ReadKey();
            }
        }
    }
}
