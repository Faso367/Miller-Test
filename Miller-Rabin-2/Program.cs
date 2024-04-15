using System.Collections;
using System.Numerics;


namespace Miller_Rabin_2
{
    class Program
    {
        //private static int[] _primeNumbers = new int[100000];
        const int _i = 10000;
        //public static HashSet<int> _primeNumbers = new();
        public static List<int> _primeNumbers = new();



        //static List<int> SieveEratosthenes(int n)
        //{
        //    //var numbers = new List<uint>();
        //    //заполнение списка числами от 2 до n-1
        //    for (var i = 2; i < n; i++)
        //    {
        //        _primeNumbers.Add(i);
        //    }

        //    for (var i = 0; i < _primeNumbers.Count; i++)
        //    {
        //        for (var j = 2; j < n; j++)
        //        {
        //            //удаляем кратные числа из списка
        //            _primeNumbers.Remove(_primeNumbers[i] * j);
        //        }
        //    }

        //    return _primeNumbers;
        //}

       // public class PrimesSieve
        //{
            //static void Main()
            //{
           public static void SieveEratosthenes(int MAX) {
                //const int MAX = 1000000;
                // Create an array of boolean values indicating whether a number is prime.
                // Start by assuming all numbers are prime by setting them to true.
                bool[] primes = new bool[MAX + 1];
                for (int i = 0; i < primes.Length; i++)
                {
                    primes[i] = true;
                }

                // Loop through a portion of the array (up to the square root of MAX). If
                // it's a prime, ensure all multiples of it are set to false, as they
                // clearly cannot be prime.
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

                // Output the results
                int count = 0;
                for (int i = 2; i < primes.Length; i++)
                {
                    if (primes[i - 1])
                    {
                    //Console.WriteLine(i);
                    _primeNumbers.Add(i);
                    count++;
                    }
                }

            //_primeNumbers = primes.ToList();

                //Console.WriteLine($"Получили {count} простых чисел до {MAX}");
            }


        /*Строится число m= (где qi — различные случайные простые числа из таблицы,
         * αi – случайные целые числа),
         * размер которого на 1 бит меньше требуемого размера для простого числа;*/
        static void Main()
        {
            Console.WriteLine("Введите длину битовой последовательности простого числа: ");
            int numberLength = Convert.ToInt32(Console.ReadLine());
            //int targetLength = 

            // 1)
            SieveEratosthenes(_i);

            //foreach(var z in _primeNumbers)
            //{
            //    Console.WriteLine(z);
            //}
            
            // 2)
            BigInteger m = 1;
            var rnd = new Random();
            int maxLength = numberLength - 1;
            Console.WriteLine($"maxLength = {maxLength}");
            while (maxLength > 0)
            {
                Console.WriteLine($"maxLength = {maxLength}");
                // Берём простое число из таблицы под случайным индексом
                int q = _primeNumbers[rnd.Next(0, _primeNumbers.Count - 1)];
                // Берём случайное целое положительное а
                int a = rnd.Next(0, int.MaxValue);

                //Console.WriteLine($"q: {q}");
                //Console.WriteLine($"a: {a}");

                //Console.WriteLine($"Math.Log2(q) = {Math.Log2(q)}");
                //Console.WriteLine($"(int)Math.Ceiling = {(int)Math.Ceiling(Math.Log2(q))}");
                // Максимальная прогнозируемая длина q^a (нужна чтобы не возводить число в степень)
                // Чтобы не возникло переполнения пишем тип BigInteger

                //int maxPredictedLengthBIG = a * (int)(Math.Ceiling(Math.Log2(q))) + 1);

                BigInteger maxPredictedLengthBIG = ((BigInteger)a * (BigInteger)(Math.Ceiling(Math.Log2(q))) + 1);

                //Console.WriteLine($"maxPredictedLengthBIG: {maxPredictedLengthBIG}");
                // Усекаем
                //int maxPredictedLength = (int)maxPredictedLengthBIG;
                //Console.WriteLine($"Усеченное: {maxPredictedLength}");

                // Если прогнозируемая длина <= максимальной
                //if (maxPredictedLength <= maxLength)
                if (maxPredictedLengthBIG <= maxLength)
                {
                    // Возводим q в степень а
                    int mPart = (int)Math.Pow(q, a);
                    // Увеличиваем число m
                    m *= mPart;
                    // Настоящая длина (может отличаться от прогнозиуремой на единицу)
                    int realLength = new BitArray(mPart).Length;
                    // Уменьшаем максимально возможную длину на длину двоичного числа q^a
                    maxLength -= realLength;

                    // Прибавляем разницу прогнозируемой и реальной длины
                    //maxLength += (maxPredictedLength - realLength);
                    //if (maxLength == 1) m *= (int) _primeNumbers[rnd.Next(0, _primeNumbers.Count - 1)];
                }

            }
            // ----------------------
            Console.WriteLine(m);

            // 3)
            BigInteger n = 2 * m + 1;

            var ekz = new Miller(_primeNumbers);
            //ekz.MillerTest(4481, 3);
            ekz.MillerTest(n, 3);
        }
    }
}

//int currentLength = 0;
//while (currentLength != targetLength)
//{
//    int randomIndex = rnd.Next(0, _primeNumbers.Count - 1);
//    int q = _primeNumbers[randomIndex];
//    int a = rnd.Next(0, int.MaxValue);
//    currentLength += new BitArray((int)Math.Pow(q, a)).Length;

//    if (currentLength > targetLength)
//    {
//        rnd.
//    }
//}

//int ostatok = targetLength % 8;
//int maxByteArrLength = (targetLength - ostatok) / 8;
