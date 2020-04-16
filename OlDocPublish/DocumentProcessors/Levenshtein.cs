using System;

namespace Levenshtein
{
    public static class EditDistance
    {
        public static bool IsCloseTo(string strA, string strB, double threshold)
        {
            int largestLength = strA.Length > strB.Length? strA.Length: strB.Length;
            int editDistance = GetEditDistance(strA, strB);
            double percent = ((largestLength - editDistance) / largestLength);

            return percent >= threshold;
        }

        public static int GetEditDistance(string s1, string s2)
        {
            int l1 = s1.Length;
            int l2 = s2.Length;
            int[,] d = new int[l1+1,l2+1];

            //Initiate the matrix
            for(int i = 0; i <= l1; i++) {d[i, 0] = i;}

            for(int j = 0; j <= l2; j++) {d[0, j] = j;}

            //Adjust for strings
            for(int i = 1; i <= l1; i++)
            {
                for(int j = 1; j <= l2 ; j++)
                {
                    int min1 = 0; 
                    int min2 = 0;
                    if(s1[i-1] == s2[j-1])
                    {
                    d[i,j] = d[i-1,j-1];
                    }
                    else
                    {
                    min1 = d[i - 1, j] + 1;
                    }
                    min2 = d[i, j - 1] + 1;
                    min1 = min2 < min1? min2: min1;
                    min2 = d[i - 1, j - 1] + 1;
                    min1 = min2 < min1? min2: min1;
                    d[i,j] = min1;
                }
            }

            //Return the end of the matrix
            return d[l1,l2];
        }
    }
}