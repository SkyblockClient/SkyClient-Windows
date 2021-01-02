using System;

namespace SkyblockClient
{
    // https://stackoverflow.com/questions/6944056/c-sharp-compare-string-similarity
    public class LookalikeCalculator
    {
        public LookalikeCalculator(string s, string t)
        {
            this.s = s;
            this.t = t;
        }

        public readonly string s;
        public readonly string t;

        private bool IsCached = false;
        private int CachedDifference;

        public int Difference
        {
            get
            {
                //*
                if (IsCached)
                    return CachedDifference;

                if (s is null || t is null)
                {
                    throw new ArgumentNullException("Strings must not be null");
                }

                int n = s.Length; // length of s
                int m = t.Length; // length of t

                if (n == 0)
                    return m;
                else if (m == 0)
                    return n;

                int[] p = new int[n + 1]; //'previous' cost array, horizontally
                int[] d = new int[n + 1]; // cost array, horizontally
                int[] _d; //placeholder to assist in swapping p and d

                // indexes into strings s and t
                int i; // iterates through s
                int j; // iterates through t

                char t_j; // jth character of t

                int cost; // cost

                for (i = 0; i <= n; i++)
                {
                    p[i] = i;
                }

                for (j = 1; j <= m; j++)
                {
                    t_j = t[j - 1];
                    d[0] = j;

                    for (i = 1; i <= n; i++)
                    {
                        cost = s[i - 1] == t_j ? 0 : 1;
                        // minimum of cell to the left+1, to the top+1, diagonally left and up +cost				
                        d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);
                    }

                    // copy current distance counts to 'previous row' distance counts
                    _d = p;
                    p = d;
                    d = _d;
                }

                // our last action in the above loop was to switch d and p, so p now 
                // actually has the most recent cost counts
                IsCached = true;
                CachedDifference = p[n];
                return p[n];
            }
        }
    }
}
