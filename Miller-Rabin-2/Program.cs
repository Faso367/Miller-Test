using System.Collections;
using System.Numerics;


namespace Miller_Rabin_2
{
    class Program
    {
        // Порог до которого ищем простые числа
        const int _i = 10000;
        // Таблица малых простых чисел
        public static List<int> _primeNumbers = new();
        //public static List<int> _primeFactors = new();
        public static Dictionary<int, int> q_a = new();

        /// <summary>
        /// Решето Эратосфена - заполняет таблицу простыми числами
        /// </summary>
        /// <param name="MAX">Число, до которого ищем простые числа</param>
        public static void SieveEratosthenes(int MAX) {
            // Массив, который показывает является ли число простым (для всех чисел есть свое bool значение)
            bool[] primes = new bool[MAX + 1];
            for (int i = 0; i < primes.Length; i++)
            {
                primes[i] = true;
            }

            // Если это простое число, значит для всех его кратных установлено значение false,
            // поскольку они явно не могут быть простыми.
            for (int i = 2; i < Math.Sqrt(MAX) + 1; i++)
            {
                if (primes[i - 1])
                {
                    for (int j = (int)Math.Pow(i, 2); j <= MAX; j += i)
                    {
                        primes[j - 1] = false;
                    }
                }
            }

            int count = 0;
            for (int i = 2; i < primes.Length; i++)
            {
                // Если встречаем true, значит это простое число, значит заносим его в таблицу
                if (primes[i - 1])
                {
                    _primeNumbers.Add(i);
                    count++;
                }
            }
        }

        /// <summary>
        /// Создаёт число m
        /// </summary>
        /// <param name="numberLength">длина большого числа</param>
        /// <returns>случайное большое число, состоит из разложения</returns>
        public static BigInteger Create_m_New(int numberLength)
        {
            

            BigInteger m = 1;
            var rnd = new Random();
            int maxLength = numberLength - 1;

            bool flag = false;
            while (flag == false)
            {
                // Берём простое число из таблицы под случайным индексом
                int q = _primeNumbers[rnd.Next(0, _primeNumbers.Count - 1)];

                var mBL = (m * q).GetBitLength();

                // Если оставшаяся разрешенная длина <= битовой длины 
                if (mBL <= maxLength)
                {
                    m *= q;

                    // Если такого простого числа еще нет,
                    // то добавляем в словарь со степенью 1 
                    if (!q_a.Keys.Contains(q))
                        q_a.Add(q, 1);
                    else
                        // Если оно уже есть, то увеличиваем его степень
                        q_a[q] += 1;

                    if (m.GetBitLength() == maxLength)
                    {
                        flag = true;
                    }
                }
            }
            return m;
        }

        /*Строится число m= (где qi — различные случайные простые числа из таблицы,
         * αi – случайные целые числа),
         * размер которого на 1 бит меньше требуемого размера для простого числа;*/
        static void Main()
        {
            Console.WriteLine("Введите длину битовой последовательности простого числа: ");
            int numberLength = Convert.ToInt32(Console.ReadLine());

            // 1)
            SieveEratosthenes(_i);

            while (true)
            {
                // 2)
                var m = Create_m_New(numberLength);
                // 3)
                BigInteger n = 2 * m + 1;
                Console.WriteLine($"n = {n}");

                var ekz = new Miller(q_a);
                string res = ekz.MillerTest(n, 3);
                if (res == "n - простое число")
                {
                    Console.WriteLine(res);
                    break;
                }
            }
        }
    }
}
