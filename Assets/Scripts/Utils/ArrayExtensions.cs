using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
public static class ArrayExtensions
{
    static System.Random rng = new System.Random();

    static List<int> lastCartelaIndex = new List<int>();
    public static T SelectRandom<T>(this IList<T> array)
    {
        if (array.Count == 0)
            return default(T);
        return array[UnityEngine.Random.Range(0, array.Count)];
    }

    public static List<T> SelectRandomMany<T>(this IList<T> array, int count)
    {
        var result = new List<T>();
        var arrayCopy = array.ToList();
        for (int i = 0; i < count; i++)
        {
            var number = arrayCopy.SelectRandom();
            result.Add(number);
            arrayCopy.Remove(number);
        }
        return result;
    }


    public static T SelectRandomNoRepeat<T>(this IList<T> array)
    {
        return array[GetIndexNoRepeat(array)];
    }

    public static int GetIndexNoRepeat<T>(this IList<T> array)
    {
        var cartelaIndex = 0;
        var antiLoop = 0;
        do
        {
            cartelaIndex = UnityEngine.Random.Range(0, array.Count);
            if (antiLoop++ > 100)
            {
                Debug.Log("## AntiLoop GetIndexNoRepeat array=" + array.ToStringValues());
                break;
            }
        } while (lastCartelaIndex.Contains(cartelaIndex) && array.Count > 0);

        if (lastCartelaIndex.Count >= 2)
            lastCartelaIndex.RemoveAt(0);
        lastCartelaIndex.Add(cartelaIndex);
        //Debug.Log("CartelaIndex="+cartelaIndex);
        return cartelaIndex;
    }


    public static bool HasIntersection<T>(this IList<T> a, IList<T> b)
    {
        //a contains all items in  b?
        //Except: b elements that doenst exist in a
        return !b.Except(a).Any();
    }

    public static string[] ToArrayString(this string str, char separator = ',')
    {
        if (str.Length == 0)
        {
            return new string[0];
        }

        if (str[0] == '[')
            str = str.Remove(0, 1);

        if (str[str.Length - 1] == ']')
            str = str.Remove(str.Length - 1, 1);
        if (str[str.Length - 1] == separator)
            str = str.Remove(str.Length - 1, 1);
        if (str.Length == 0)
            return new string[0];
        var array = str.Split(separator);
        var a = new string[array.Length];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = array[i];
        }
        return a;
    }

    public static int[] ToArrayInt(this string str, char separator = ',')
    {
        if (str.Length == 0)
        {
            return new int[0];
        }

        if (str[0] == '[')
            str = str.Remove(0, 1);

        if (str[str.Length - 1] == ']')
            str = str.Remove(str.Length - 1, 1);

        if (str[str.Length - 1] == separator)
            str = str.Remove(str.Length - 1, 1);
        if (str.Length == 0)
            return new int[0];
        var array = str.Split(separator);
        var a = new int[array.Length];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = int.Parse(array[i]);
        }
        return a;
    }

    public static float[] ToArrayFloat(this string str, char separator = ',')
    {
        if (str.Length == 0)
        {
            return new float[0];
        }
        if (str[0] == '[')
            str = str.Remove(0);
        if (str[str.Length - 1] == ']')
            str = str.Remove(str.Length - 1);
        if (str[str.Length - 1] == separator)
            str = str.Remove(str.Length - 1, 1);
        if (str.Length == 0)
            return new float[0];
        var array = str.Split(separator);
        var a = new float[array.Length];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = float.Parse(array[i]);
        }
        return a;
    }

    public static List<int> Insert(int[] pattern, int[] bolas, int ballsCount, int initialBallsCount)
    {

        //copia a origem para a lista (numeros do premio) 
        List<int> newBalls = pattern.ToList();

        //copia os numeros do destino sem repetir e sem passar de maxs
        for (int by = 0; by < bolas.Length; by++)
        {
            if (!newBalls.Contains(bolas[by]) && newBalls.Count < ballsCount)
            {
                if (newBalls.Count < initialBallsCount)
                    newBalls.Insert(0, bolas[by]);
                else
                    newBalls.Add(bolas[by]);
            }
        }

        return newBalls;
    }

    public static void ShuffleRange(int[] array, int initial, int end)
    {
        for (int t = initial; t < end; t++)
        {
            int tmp = array[t];
            int r = UnityEngine.Random.Range(initial, end);
            array[t] = array[r];
            array[r] = tmp;
        }
    }

    public static void ShuffleLimit(int[] texts, int tamanho)
    {
        for (int t = 0; t < tamanho; t++)
        {
            var tmp = texts[t];
            int r = UnityEngine.Random.Range(t, tamanho);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }




    public static T[] Concat<T>(params T[][] list)
    {
        var result = new T[list.Sum(a => a.Length)];
        int offset = 0;
        for (int x = 0; x < list.Length; x++)
        {
            list[x].CopyTo(result, offset);
            offset += list[x].Length;
        }
        return result;
    }

    public static string ToStringValues<T>(this IList<T> array, char separator = '_')
    {
        var str = "";
        foreach (var i in array)
        {
            str += i + separator.ToString();
        }
        return str;
    }


    public static string ToStringValues<T>(this IList<T> array, Func<T, string> onString, string separator = " ")
    {
        if (array == null)
        {
            return "";
        }
        string result = "";
        foreach (var valuee in array)
        {
            var str = onString(valuee);
            result += str + separator;
        }
        return result;
    }


    public static int GetNextBiggerNumber(this int[] array, int target)
    {

        foreach (var element in array)
        {
            if (element > target)
            {
                return element;
            }
        }

        return array[0];
    }

    public static int GetRandomWeightedIndex(float[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        float w;
        float t = 0;
        int i;
        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];

            if (float.IsPositiveInfinity(w))
            {
                return i;
            }
            else if (w >= 0f && !float.IsNaN(w))
            {
                t += weights[i];
            }
        }

        float r = UnityEngine.Random.value;
        float s = 0f;

        for (i = 0; i < weights.Length; i++)
        {
            w = weights[i];
            if (float.IsNaN(w) || w <= 0f) continue;

            s += w / t;
            if (s >= r) return i;
        }

        return -1;
    }

    public static string ToStringMatrix<T>(this T[,] array, string separator = " ")
    {
        var str = "";
        for (int i = 0; i < array.GetLength(0); i++)
        {
            str += "\n";
            for (int j = 0; j < array.GetLength(1); j++)
            {
                str += array[i, j] + separator;
            }
        }
        return str;
    }

}