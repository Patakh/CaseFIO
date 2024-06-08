using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CaseFIO
{
    public class Word
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public List<string> Exceptions;
        public List<string> Suffixes;

        public Rule[] ExceptionsRule;
        public Rule[] SuffixesRule;

        private const string _matchPattern = @"^\s*([fm]?)\s*(\S+)\s+(\S+)\s+(\S+)\s+(\S+)\s+(\S+)\s+(\S+)\s*$";

        public void Rule()
        {
            if (Exceptions != null)
                ExceptionsRule = new Rule[Exceptions.Count];

            if (Suffixes != null)
                SuffixesRule = new Rule[Suffixes.Count];

            if (Exceptions != null && ExceptionsRule != null)
                for (int i = 0; i < Exceptions.Count; i++)
                {
                    Match m = Regex.Match(Exceptions[i], _matchPattern);
                    if (m.Success && m.Groups != null && m.Groups.Count > 1)
                    {
                        ExceptionsRule[i] = new Rule();
                        ExceptionsRule[i].Sex = m.Groups[1].Value;
                        ExceptionsRule[i].Test = new List<string>(m.Groups[2].Value.Split(' ').ToList());
                        ExceptionsRule[i].Mods = new List<string> { m.Groups[3].Value, m.Groups[4].Value, m.Groups[5].Value, m.Groups[6].Value, m.Groups[7].Value };
                    }
                }

            if (SuffixesRule != null && SuffixesRule != null)
                for (int i = 0; i < Suffixes.Count; i++)
                {
                    Match m = Regex.Match(Suffixes[i], _matchPattern);
                    if (m.Success && m.Groups != null && m.Groups.Count > 1)
                    {
                        SuffixesRule[i] = new Rule();
                        SuffixesRule[i].Sex = m.Groups[1].Value;
                        SuffixesRule[i].Test = new List<string>(m.Groups[2].Value.Split(' ').ToList());
                        SuffixesRule[i].Mods = new List<string> { m.Groups[3].Value, m.Groups[4].Value, m.Groups[5].Value, m.Groups[6].Value, m.Groups[7].Value };
                    }
                }
        }
    }
}