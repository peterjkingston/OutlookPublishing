using System;

namespace Levenshtein
{
    public static class EditDistance
    {
        /// <summary>
        /// Returns true if strA is at least [threshold]% in common with strB.
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <param name="threshold">The comparison threshold percent (1.00 == 100%)</param>
        /// <returns></returns>
        public static bool IsCloseTo(string strA, string strB, double threshold)
        {
            double largestLength = strA.Length > strB.Length? strA.Length: strB.Length;
            double editDistance = GetEditDistance(strA, strB);
            double percent = (largestLength - editDistance) / largestLength;

            return percent >= threshold;
        }

        public static int GetEditDistance(string s1, string s2)
        {
            int l1 = s1.Length;
            int l2 = s2.Length;
            int[,] d = new int[l1+1,l2+1];

            //Exit immediately if either string is empty
            if(l1 == 0) { return l2; }
            if(l2 == 0) { return l1; }

            //Initiate the matrix
            for(int i = 0; i <= l1; i++) {d[i, 0] = i;}
            for(int j = 0; j <= l2; j++) {d[0, j] = j;}

            //Adjust for strings
            for(int i = 1; i <= l1; i++)
            {
                for(int j = 1; j <= l2 ; j++)
                {
                    int cost = (s2[j - 1] == s1[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            //Return the end of the matrix
            return d[l1,l2];
        }
    }
}