using System;
using System.Linq;

namespace SkyblockClient
{
    public class SimilaritiesAndDifferences
    {
        public readonly int Similarities;
        public readonly int Differences;

        public readonly int Total;

        public SimilaritiesAndDifferences(int similarities, int differences)
        {
            Similarities = similarities;
            Differences = differences;

            Total = differences - similarities;
        }
    }

    public class LookalikeCalculator
    {
        public LookalikeCalculator(string s, string t)
        {
            this.s = s;
            this.t = t;
        }

        public string s;
        public string t;

        private bool IsCached = false;
        private SimilaritiesAndDifferences CachedDifference;

        private int getLength(string str, int jLength)
        {
            int result = 0;
            result = Math.Max(str.Length - jLength, 0);
            if (result == 0)
                result = str.Length;
            return result;
        }

        public SimilaritiesAndDifferences SimilaritiesAndDifferences
        {
            get
            {
                if (IsCached)
                    return CachedDifference;

                int[] p;
                int n;
                {

                    if (s is null || t is null)
                    {
                        throw new ArgumentNullException("Strings must not be null");
                    }

                    n = s.Length; // length of s
                    int m = t.Length; // length of t

                    if (n == 0)
                        return new SimilaritiesAndDifferences(0, m);
                    else if (m == 0)
                        return new SimilaritiesAndDifferences(0, n);

                    p = new int[n + 1]; //'previous' cost array, horizontally
                    // https://stackoverflow.com/questions/6944056/c-sharp-compare-string-similarity

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

                }

                // our last action in the above loop was to switch d and p, so p now 
                // actually has the most recent cost counts

                var diff = p[n];

                var jLength = ".jar".Length;
                s = s.Substring(0, getLength(s, jLength));
                t = t.Substring(0, getLength(t, jLength));

                var badChars = "_.( )-0123456789";

                var similarities = 0;

                for (int i_s = 0; i_s < s.Length; i_s++)
                {
                    var charAtI_S = s[i_s];
                    var commonString = string.Empty;
                    for (int i_t = 0; i_t < t.Length; i_t++)
                    {
                        var charAtI_T = t[i_t];
                        commonString += charAtI_T;

                        // takes the position of the substring and divides it with the current index, 
                        // since s.IndexOf(commonString, i_s) doesn't work for some reason
                        var s_indexOf_commonString_minus_i_s = s.IndexOf(commonString) - i_s;

                        // doing it again because the strings might be swapped
                        var t_indexOf_commonString_minus_i_t = t.IndexOf(commonString) - i_t;

                        // if this is true, it means that part of the string matches up with the compare string
                        if (s_indexOf_commonString_minus_i_s == 0)
                        {
                            if (!badChars.Contains(charAtI_T) && commonString.Length > 2)
                                similarities++;
                        }
                        // doing it again because the strings might be swapped
                        else if (t_indexOf_commonString_minus_i_t == 0)
                        {
                            if (!badChars.Contains(charAtI_S) && commonString.Length > 2)
                                similarities++;
                        }
                        else
                            commonString = string.Empty;
                    }
                }

                s += ".jar";
                t += ".jar";

                var similaritiesAndDifferences = new SimilaritiesAndDifferences(similarities, diff);

                IsCached = true;
                CachedDifference = similaritiesAndDifferences;
                return similaritiesAndDifferences;
            }
        }
    }
}
