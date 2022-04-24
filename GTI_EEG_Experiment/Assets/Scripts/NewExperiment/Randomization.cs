using System;
using System.Collections.Generic;

namespace NewExperiment
{
    public static class Randomization
    {
        public static string GenerateID()
        { 
            return Guid.NewGuid().ToString();
        }
        
        
        
        /// <summary>
        /// Fisher-Yates randomization shuffle
        /// </summary>
        /// <param name="list"> any type of List</param>
        /// <param name="rnd"> seed for randomisation</param>
        /// <typeparam name="T">input and return type of list</typeparam>
        /// <returns></returns>
        public static List<T> Shuffle<T>(List<T> list, System.Random rnd)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                T value;
                int k = rnd.Next(n + 1);
                value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}