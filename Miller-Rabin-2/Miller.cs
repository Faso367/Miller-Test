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
        // Содержит каноническое разложение, то есть пары  число : степень
        private static Dictionary<int, int> q_a;

        /// <summary>
        /// Принимает на вход каноническое разложение числа m
        /// </summary>
        public Miller(Dictionary<int, int> canonicalDecompositionM)
        {
            q_a = canonicalDecompositionM;
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

            // Добавляем 2, тк  n - 1 = 2 * m
            if (!q_a.Keys.Contains(2))
                q_a.Add(2, 1);
            else
                q_a[2] += 1;


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
