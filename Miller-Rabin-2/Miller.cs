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
        // Таблица малых простых чисел
        private static List<int> _primeNumbers;

        /// <summary>
        /// Принимает на вход заполненную таблицу малых простых чисел
        /// </summary>
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

        /// <summary>
        /// Определяет, с некоторой вероятностью, является ли число простым
        /// </summary>
        /// <param name="n">Число, проверяемое на простоту</param>
        /// <param name="t">Параметр надёжности (количество итераций)</param>
        /// <returns></returns>
        public string MillerTest(BigInteger n, int t)
        {
            // Содержит каноническое разложение, то есть пары  число : степень
            Dictionary<BigInteger, int> q_a = new();
            var nCopy = n - 1;

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

            BigInteger[] ajArr = new BigInteger[t];
            
            for(int i = 0; i < t; i++)
            {
                // Генерим число от 2 до n (не включаем)
                // 1)
                var aj = NextBigInteger(new Random(), 2, n - 1);
                ajArr[i] = aj;

                // 2)
                if (BigInteger.ModPow(aj, n - 1, n) != 1) return "n - составное число";
            }

            // 3)
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
        }
    }
}
