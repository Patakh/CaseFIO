using System.Collections.Generic;

namespace CaseFIO
{
    public class Rule
    {
        public string Sex { get; set; } = string.Empty;
        public List<string> Test { get; set; }
        public List<string> Mods { get; set; }
        public Rule()
        {
            Test = new List<string>();
            Mods = new List<string>();
        }
    }
}