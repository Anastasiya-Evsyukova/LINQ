using System;

namespace linq2
{
    // Класс для задания 2
    class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    // Класс для заданий 5 и 6
    class Order
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public decimal Amount { get; set; } //сумма заказа
    }

    internal class Program
    {
        static void Main()
        {
            //12.1 Лямбды и Func
            Console.WriteLine("12.1");
            Func<string, string, bool> isSubstring = (first, second) => first.Contains(second);
            string str1 = "Hello world";
            string str2 = "world";
            bool result1 = isSubstring(str1, str2);
            Console.WriteLine($"'{str2}' является подстрокой '{str1}'? {result1}\n");

            //12.2 Комбинированный запрос
            Console.WriteLine("\n12.2");
            var products = new List<Product>
        {
            new Product { Name = "Мышь", Price = 50, Category = "Компьютеры" },
            new Product { Name = "Клавиатура", Price = 120, Category = "Компьютеры" },
            new Product { Name = "Монитор", Price = 300, Category = "Компьютеры" },
            new Product { Name = "Книга", Price = 90, Category = "Книги" },
            new Product { Name = "Ручка", Price = 5, Category = "Канцтовары" },
            new Product { Name = "Стол", Price = 250, Category = "Мебель" },
            new Product { Name = "Стул", Price = 110, Category = "Мебель" }
        };

            var groupedProducts = products
                .Where(p => p.Price > 100)
                .GroupBy(p => p.Category)

                //Select — для каждой группы создаём анонимный объект:
                //Category = ключ группы
                //ProductNames = из группы выбираем только названия (проекция)
                .Select(g => new
                {
                    Category = g.Key,
                    ProductNames = g.Select(p => p.Name)
                })
                .OrderBy(g => g.Category);

            foreach (var group in groupedProducts)
            {
                Console.WriteLine($"Категория: {group.Category}");
                Console.WriteLine($"  Продукты: {string.Join(", ", group.ProductNames)}");
            }

            //12.3 Отложенное выполнение
            Console.WriteLine("\n12.3");
            var numbers = new List<int> { 1, 2, 3, 4, 5 };

            //запрос с Where — отложенное выполнение (не вычисляется сразу)
            var query = numbers.Where(n => n > 2);
            numbers.Add(6);
            numbers.Add(7);

            //при вызове ToList(), запрос выполняется и результат материализуется
            var result3 = query.ToList();
            Console.WriteLine($"Результат после добавления 6 и 7: {string.Join(", ", result3)}");
            //запрос выполняется при ToList(), поэтому учитываются добавленные элементы

            //12.4 Оптимизация цепочки
            Console.WriteLine("\n12.4");
            // Создаём тестовые данные
            var data = new[]
            {
            new { Name = "A", Age = 20, Score = 40 },
            new { Name = "B", Age = 19, Score = 60 },
            new { Name = "C", Age = 25, Score = 55 },
            new { Name = "D", Age = 30, Score = 80 },
            new { Name = "E", Age = 17, Score = 90 }
        };

            //неоптимальный запрос
            //var result = data
            //    .Select(x => new { x.Name, x.Age, x.Score })
            //    .Where(x => x.Age > 18)
            //    .OrderBy(x => x.Score)
            //    .Where(x => x.Score > 50)
            //    .Take(10);

            //оптимальный запрос
            //Where фильтрует по двум условиям одновременно (Age>18 и Score>50), что уменьшает количество элементов перед сортировкой.
            //OrderBy — сортируем только отфильтрованные элементы.
            //Select — проекция (создаём анонимный объект с нужными полями).
            //Take — берём первые 10
            var optimal = data
                .Where(x => x.Age > 18 && x.Score > 50)   //фильтрация до сортировки
                .OrderBy(x => x.Score)                    //сортировка только отфильтрованных
                .Select(x => new { x.Name, x.Age, x.Score })
                .Take(10);

            Console.WriteLine("Оптимальный порядок операторов:");
            foreach (var item in optimal)
                Console.WriteLine($"  {item.Name}: Age={item.Age}, Score={item.Score}");
            //фильтрация вынесена до сортировки, проекция – после фильтрации

            //12.5 Aggregate
            Console.WriteLine("\n12.5");
            // Сумма чисел
            List<int> ints = new List<int> { 1, 2, 3, 4, 5 };

            //перегрузка без начального значения: первый элемент становится начальным аккумулятором
            int sum = ints.Aggregate((acc, next) => acc + next);
            Console.WriteLine($"Сумма чисел 1..5: {sum}");

            //конкатенация строк через запятую
            List<string> words = new List<string> { "a", "b", "c" };
            string concatenated = words.Aggregate((acc, next) => $"{acc},{next}");
            Console.WriteLine($"Конкатенация 'a','b','c': {concatenated}");

            //сумма заказов через Aggregate с явным начальным значением (seed = 0m)
            var ordersForAggregate = new List<Order>
        {
            new Order { Id = 1, Customer = "Alice", Amount = 500 },
            new Order { Id = 2, Customer = "Bob", Amount = 1200 },
            new Order { Id = 3, Customer = "Alice", Amount = 800 }
        };
            decimal totalSum = ordersForAggregate.Aggregate(0m, (sumSoFar, order) => sumSoFar + order.Amount);
            Console.WriteLine($"Общая сумма заказов: {totalSum}\n");

            //12.6 Query vs Method syntax
            Console.WriteLine("\n12.6");
            var orders = new List<Order>
        {
            new Order { Id = 1, Customer = "Alice", Amount = 500 },
            new Order { Id = 2, Customer = "Bob", Amount = 1200 },
            new Order { Id = 3, Customer = "Alice", Amount = 800 },
            new Order { Id = 4, Customer = "Bob", Amount = 300 },
            new Order { Id = 5, Customer = "Charlie", Amount = 2000 }
        };

            //Method syntax
            var methodResult = orders
                .GroupBy(o => o.Customer)
                .Select(g => new { Customer = g.Key, Total = g.Sum(o => o.Amount) })  //сумма по группе
                .Where(c => c.Total > 1000)
                .OrderByDescending(c => c.Total);

            //Query syntax
            var queryResult = from o in orders
                              group o by o.Customer into g
                              let total = g.Sum(o => o.Amount) //переменная total для суммы группы
                              where total > 1000
                              orderby total descending
                              select new { Customer = g.Key, Total = total };

            Console.WriteLine("Method syntax результат:");
            foreach (var item in methodResult)
                Console.WriteLine($"  {item.Customer}: {item.Total}");

            Console.WriteLine("Query syntax результат:");
            foreach (var item in queryResult)
                Console.WriteLine($"  {item.Customer}: {item.Total}");
        }
    }
}
