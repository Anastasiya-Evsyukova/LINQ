using System;

namespace linq
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            //2.1

            Func<double, double> sqr = r => (Math.PI * (r * r)); //делегат, принимающий double и возвращающий double + лямбда-выражение
            //r формальный параметр лямбда-выражения. Он существует только внутри лямбды и обозначает тот аргумент, который будет передан при вызове.

            double rad = 5;
            double area = sqr(rad); //вызываем делегат sqr с аргументом rad, результат сохраняем в area

            Console.WriteLine("2.1 Площадь круга");
            Console.WriteLine(area);

            //2.2

            Func<string, bool> valid = s => !string.IsNullOrEmpty(s) && s.Length > 5;

            string test = "HelloWorld";
            string test1 = "Hi";

            Console.WriteLine("\n2.2 Не пустая строка длиннее 5 символов");
            Console.WriteLine(valid(test));
            Console.WriteLine(valid(test1));

            //2.3

            Func<int, int, bool> div = (a, b) => b != 0 && a % b == 0;

            Console.WriteLine("\n2.3 Деление без остатка");
            Console.WriteLine(div(10,2));
            Console.WriteLine(div(10,3));

            //9.1

            List<int> numbers = new List<int> { -5, 10, 3, -2, 8, 0, 7 };

            //метод-синтаксис
            //Where — фильтрует элементы, оставляя только те, для которых лямбда вернёт true
            var positiveEvenMethod = numbers.Where(n => n > 0 && n % 2 == 0);

            //выражение-синтаксис query syntax (синтаксис запросов, похожий на SQL)
            //from n in numbers — источник данных
            //where — фильтр
            //select — проекция (возвращаем сам элемент n)
            var positiveEvenQuery = from n in numbers
                                    where n > 0 && n % 2 == 0
                                    select n;

            Console.WriteLine("\n9.1 Положительные чётные числа:");

            //string.Join преобразует коллекцию в строку с разделителем ", "
            Console.WriteLine("Method: " + string.Join(", ", positiveEvenMethod));
            Console.WriteLine("Query:  " + string.Join(", ", positiveEvenQuery));

            //9.2

            List<string> fruits = new List<string> { "apple", "banana", "cherry" };

            //Select преобразует каждый элемент: берёт его длину (свойство Length)
            var lengths = fruits.Select(s => s.Length);

            Console.WriteLine("\n9.2 Длины строк: " + string.Join(", ", lengths));

            //9.3

            List<string> words = new List<string> { "cat", "elephant", "dog", "bee" };
            var sortedWords = words.OrderBy(w => w.Length)      //сначала по длине
                                   .ThenBy(w => w);             //потом по алфавиту

            Console.WriteLine("\n9.3 Отсортированные слова:");
            Console.WriteLine("  " + string.Join(", ", sortedWords));

            //9.4
            //GroupBy — группирует элементы по ключу, полученному из элемента
            //ключ = x % 3 (остаток от деления на 3)
            List<int> groupNumbers = new List<int> { 1, 4, 7, 2, 5, 8, 3, 6, 9 };
            var groups = groupNumbers.GroupBy(x => x % 3);

            //groups — коллекция групп, каждая группа имеет свойство Key и является IEnumerable<T>
            //(этот объект можно перебрать (просмотреть) элемент за элементом)
            Console.WriteLine("\n9.4 Группировка по остатку от деления на 3:");
            foreach (var group in groups)
            {
                Console.WriteLine($"  Остаток {group.Key}: [{string.Join(", ", group)}]");
            }

            //9.5
            //All — проверяет, удовлетворяют ли ВСЕ элементы условию (n % 2 == 0)
            List<int> allEven = new List<int> { 2, 4, 6, 8 };
            bool allAreEven = allEven.All(n => n % 2 == 0);

            //Any — проверяет, есть ли ХОТЯ БЫ ОДИН элемент, удовлетворяющий условию (n < 0)
            List<int> hasNegative = new List<int> { 1, 2, 3 };
            bool anyNegative = hasNegative.Any(n => n < 0);

            Console.WriteLine("\n9.5 Any / All:");
            Console.WriteLine($"  Все числа в [2,4,6,8] чётные? {allAreEven}");
            Console.WriteLine($"  В [1,2,3] есть отрицательное число? {anyNegative}");

            //9.6

            //FirstOrDefault — ищет первый элемент, для которого x > 20
            //если такой элемент есть — возвращает его
            //если нет — возвращает значение по умолчанию. Здесь передаётся -1, иначе было бы 0.
            List<int> squares = new List<int> { 1, 4, 9, 16, 25, 36 };
            int firstSquareAbove20 = squares.FirstOrDefault(x => x > 20, -1);

            Console.WriteLine("\n9.6 Первый квадрат > 20: " + firstSquareAbove20);

            //9.7

            //вложенный список
            List<List<int>> nested = new List<List<int>>
        {
            new List<int> { 1, 2, 3 },
            new List<int> { 4, 5 },
            new List<int> { 6, 7, 8 }
        };
            //SelectMany — разворачивает вложенные списки в одну плоскую последовательность
            //list => list — функция, возвращающая каждый внутренний список как IEnumerable<int>
            //затем Where — оставляет только числа > 4
            var result = nested.SelectMany(list => list)   //разворачиваем в плоскую последовательность
                               .Where(num => num > 4);     //фильтруем > 4

            Console.WriteLine("\n9.7 Элементы > 4 из вложенных списков:");
            Console.WriteLine("  " + string.Join(", ", result));
        }
    }
}
