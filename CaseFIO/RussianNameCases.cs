using System;
using System.Text.RegularExpressions;

namespace CaseFIO
{ 
    public class RuNameCases
    {
        public string sexM = RussianNameProcessor.sexM;
        public string sexF = RussianNameProcessor.sexF;
        public string Imenit = RussianNameProcessor.Imenit;
        public string Rodit = RussianNameProcessor.Rodit;
        public string Datel = RussianNameProcessor.Datel;
        public string Vinit = RussianNameProcessor.Vinit;
        public string Tvor = RussianNameProcessor.Tvor;
        public string Predl = RussianNameProcessor.Predl;
        public bool fullNameSurnameLast;

        public string ln;
        public string fn;
        public string mn;
        public string sex;

        public static RussianNameProcessor russianNameProcessor = new RussianNameProcessor();

        public RuNameCases(string fullName)
        {
            if (!string.IsNullOrEmpty(fullName))
            {
#if NET6_0_OR_GREATER
                string[] fio = fullName.Split(" ");
#else 
                string[] fio = fullName.Split(' ');
#endif
                string lastName = fio.Length > 0 ? fio[0] : "";
                string firstName = fio.Length > 1 ? fio[1] : "";
                string middleName = fio.Length > 2 ? fio[2] : "";

                Init(lastName, firstName, middleName); 
            }
        }
          
        public RuNameCases(string lastName, string firstName, string middleName, string sex = null)
        {
            Init(lastName, firstName, middleName, sex);
        }

        void Init (string lastName, string firstName, string middleName, string sex = null)
        { 
            if (string.IsNullOrEmpty(firstName))
            {
                Match m = Regex.Match(lastName, @"^\s*(\S+)(\s+(\S+)(\s+(\S+))?)?\s*$");
                if (m.Groups[5].Success && Regex.IsMatch(m.Groups[3].Value, @"(ич|на)$") && !Regex.IsMatch(m.Groups[5].Value, @"(ич|на)$"))
                {
                    ln = m.Groups[5].Value;
                    fn = m.Groups[1].Value;
                    mn = m.Groups[3].Value;
                    fullNameSurnameLast = true;
                }
                else
                {
                    ln = m.Groups[1].Value;
                    fn = m.Groups[3].Value;
                    mn = m.Groups[5].Value;
                }
            }
            else
            {
                ln = lastName;
                fn = firstName;
                mn = middleName;
            }
            this.sex = sex ?? GetSex();
        }

        public string GetSex()
        {
            if (mn.Length > 2)
            {
                switch (mn.Substring(mn.Length - 2))
                {
                    case "ич":
                        return sexM;
                    case "на":
                        return sexF;
                }
            }
            return "";
        }

        public string FullName(string gcase)
        { 
            return ((fullNameSurnameLast ? "" : LastName(gcase) + " ") + FirstName(gcase) + " " + MiddleName(gcase) + (fullNameSurnameLast ? " " + LastName(gcase) : "")).Trim();
        }

        public string LastName(string gcase)
        {
            if (string.IsNullOrEmpty(ln)) return null;
            return RussianNameProcessor.Word(ln, sex, russianNameProcessor.LastName, gcase);
        }

        public string FirstName(string gcase)
        {
            if (string.IsNullOrEmpty(fn)) return null;
            return RussianNameProcessor.Word(fn, sex, russianNameProcessor.FirstName, gcase);
        }

        public string MiddleName(string gcase)
        {
            if (string.IsNullOrEmpty(mn)) return null;
            return RussianNameProcessor.Word(mn, sex, russianNameProcessor.MiddleName, gcase);
        }
    }
}