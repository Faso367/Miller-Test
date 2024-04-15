using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Miller_Rabin_2
{
    public class Miller
    {
        //private static HashSet<int> _primeNumbers;
        private static List<int> _primeNumbers;

        //public Miller(HashSet<int> primeNumbers)
        public Miller(List<int> primeNumbers)
        {
            _primeNumbers = primeNumbers;
        }

        /// <summary>
        /// Генерирует случайное большое число в заданом диапазоне.
        /// Сама генерация происходит с помощью встроенного класса
        /// Затем соединяем куски через логическое И и сдвиги
        /// </summary> 
        /// <param name="random">Экземпляр класса Random</param>
        /// <param name="minValue">Первое значение диапазона (начало)</param>
        /// <param name="maxValue">Второе значение диапазона (конец)</param>
        /// <returns>Сгенерированное большое число</returns>
        public static BigInteger NextBigInteger(Random random, BigInteger minValue, BigInteger maxValue)
        {
            if (minValue > maxValue) throw new ArgumentException();
            if (minValue == maxValue) return minValue;
            BigInteger zeroBasedUpperBound = maxValue - 1 - minValue;
            Debug.Assert(zeroBasedUpperBound.Sign >= 0);
            byte[] bytes = zeroBasedUpperBound.ToByteArray();
            Debug.Assert(bytes.Length > 0);
            Debug.Assert((bytes[bytes.Length - 1] & 0b10000000) == 0);

            byte lastByteMask = 0b11111111;
            for (byte mask = 0b10000000; mask > 0; mask >>= 1, lastByteMask >>= 1)
            {
                if ((bytes[bytes.Length - 1] & mask) == mask) break;
            }

            while (true)
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= lastByteMask;
                var result = new BigInteger(bytes);
                Debug.Assert(result.Sign >= 0);
                if (result <= zeroBasedUpperBound) return result + minValue;
            }
        }


        //public void MillerTest(BigInteger n, int t)
        //{

        //    //List<int> qList = new ();

        //    HashSet<int> qList = new ();

        //    int tCopy = t;
        //    var nCopy = n - 1;

        //    //foreach(var pn in _primeNumbers)
        //    //{
        //    for(int i = 0; i <= Math.Sqrt(_primeNumbers.Count); i++) { 
        //        var pn = _primeNumbers[i];
        //        while(nCopy % pn == 0)
        //        {
        //        qList.Add(pn);
        //            nCopy /= pn;
        //        }
        //    }

        //    foreach(var i in qList)
        //    {
        //        Console.WriteLine(i);
        //    }
        //}

        public string MillerTest(BigInteger n, int t)
        {
            // Содержит каноническое разложение, то есть пары  число : степень
            Dictionary<BigInteger, int> q_a = new();
            int tCopy = t;
            var nCopy = n - 1;

            //Func<List<int>, string> Step123

            // Создаю каноническое разложение n - 1
            for (int i = 0; i <= Math.Sqrt(_primeNumbers.Count); i++)
            {
                int a = 0;
                var pn = _primeNumbers[i];
                if (nCopy % pn == 0)
                {
                    while (nCopy % pn == 0)
                    {
                        a++;
                        nCopy /= pn;
                    }
                    q_a.Add(pn, a);
                }
            }
            // ------------------------------------------

            BigInteger[] ajArr = new BigInteger[t];
            
            for(int i = 0; i < t; i++)
            {
                //aj[i] = NextBigInteger(new Random(), 1, n);
                // Генерим число от 1 до n (не включаем)

                // 1)
                //BigInteger randomNumber = NextBigInteger(new Random(), 2, n - 1);
                var aj = NextBigInteger(new Random(), 2, n - 1);
                ajArr[i] = aj;
                //aj[i] = NextBigInteger(new Random(), 2, n - 1);
                // 2)
                if (BigInteger.ModPow(aj, n - 1, n) != 1)
                {
                    return "n - составное число";
                }
            }
            // 3) не очень понятно что за qi
            //for(int i = 0; i < q_a.Count; i++)
            //int index = 0;
            foreach(var kvp in q_a)
            {
                int resCount = 0;

                for(int i = 0; i < ajArr.Length; i++)
                {
                    if (BigInteger.ModPow(ajArr[i], (n - 1) / kvp.Key, n) != 1) {
                        break;
                    }
                    else
                    {
                        resCount++;
                    }
                }
                if (resCount == ajArr.Length)
                    return "вероятно, n - составное число";
            }
            // 4)
            return "n - простое число";





            //foreach (var i in q_a)
            //{
            //    Console.WriteLine(i);
            //}
        }


        //const double e = 0.25;
        //public static List<BigInteger> primeNumbers = new();
        //public static List<int> iterationCounts = new();

        ///// <summary>
        ///// Тест Миллера-Рабина
        ///// </summary>
        ///// <param name="n">нечётное число, проверяемое на простоту</param>
        ///// <param name="t">параметр надёжности (количество итераций для одного числа)</param>
        ///// <returns>n вероятно простое ИЛИ n точно составное</returns>
        //public static string MillerRabinTest(BigInteger n, int t)
        //{
        //    int tCopy = t;
        //    var nCopy = n - 1;
        //    BigInteger r;
        //    int s = 0;
        //    if (!nCopy.IsPowerOfTwo)
        //    {
        //        while (nCopy % 2 == 0)
        //        {
        //            nCopy /= 2;
        //            s += 1;
        //        }
        //        r = nCopy;
        //    }
        //    else
        //    {
        //        r = 1;
        //        s = Convert.ToInt32(Math.Log2((double)nCopy));
        //    }

        //    int iterCount = 1;
        //    int countUnits = 0;
        //    // Проверяет встретилась ли единица
        //    bool firstUnitAppeared = false;

        //    while (t > 0)
        //    {
        //        BigInteger[] bEnum = new BigInteger[s + 1];
        //        BigInteger end = n - 2;
        //        var a = NextBigInteger(new Random(), 2, end);

        //        bEnum[0] = BigInteger.ModPow(a, r, n);

        //        if (bEnum[0] == 1) countUnits++;

        //        for (int j = 1; j < s + 1; j++)
        //        {
        //            bEnum[j] = BigInteger.ModPow(bEnum[j - 1], 2, n);
        //            iterCount++;

        //            // Если в последовательности встретилась единица
        //            if (bEnum[j] == 1) countUnits++;

        //            // Если мы еще не встретили ни одной единицы
        //            if (firstUnitAppeared == false)
        //            {
        //                if (bEnum[j] == 1)
        //                {
        //                    firstUnitAppeared = true;
        //                    // Если перед первой единицей стоит -1
        //                    if (bEnum[j - 1] == -1) return "n - составное";
        //                }

        //            }

        //        }
        //        // Если в последовательности не встретилось единиц
        //        if (countUnits == 0) return "n - составное";

        //        t--;
        //    }

        //    if (primeNumbers.Count < 10)
        //        primeNumbers.Add(n);

        //    if (iterationCounts.Count < 10)
        //        iterationCounts.Add(iterCount);

        //    return $"n - простое с вероятностью {1 - Math.Pow(e, tCopy)}";

        //}
    }
}
