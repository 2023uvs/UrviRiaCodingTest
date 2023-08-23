// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;

namespace ATM_Payouts
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] denominations = { 10, 50, 100 };
            int[] payoutAmounts = { 30, 50, 60, 80, 140, 230, 370, 610, 980 };

            foreach (int amount in payoutAmounts)
            {
                Console.WriteLine($"Possible combinations for {amount} EUR:");
                Console.WriteLine();
                List<List<int>> combinations = CalculateCombinations(amount, denominations, 0, new List<int>());
                foreach (var combination in combinations)
                {
                    Console.WriteLine(string.Join(" + ", combination));
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Calcualting combinations based on inputs. Calling recurring functions. 
        /// </summary>
        /// <param name="targetAmount"></param>
        /// <param name="denominations"></param>
        /// <param name="startIndex"></param>
        /// <param name="currentCombination"></param>
        /// <returns></returns>
        static List<List<int>> CalculateCombinations(int targetAmount, int[] denominations, int startIndex, List<int> currentCombination)
        {
            List<List<int>> result = new List<List<int>>();

            if (targetAmount == 0)
            {
                result.Add(new List<int>(currentCombination));
                return result;
            }

            for (int i = startIndex; i < denominations.Length; i++)
            {
                if (targetAmount >= denominations[i])
                {
                    currentCombination.Add(denominations[i]);
                    List<List<int>> subCombinations = CalculateCombinations(targetAmount - denominations[i], denominations, i, currentCombination);
                    result.AddRange(subCombinations);
                    currentCombination.RemoveAt(currentCombination.Count - 1);
                }
            }

            return result;
        }
    }
}

