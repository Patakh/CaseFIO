using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CaseFIO
{
    public class RussianNameProcessor
    {
        public const string sexM = "m";
        public const string sexF = "f";
        public const string Imenit = "nominative";
        public const string Rodit = "genitive";
        public const string Datel = "dative";
        public const string Vinit = "accusative";
        public const string Tvor = "nstrumentative";
        public const string Predl = "prepositional";
        private static bool initialized = false;

        /// <summary>
        /// Фаилия
        /// </summary>
        public Word LastName { get; set; } = new Word()
        {
            Exceptions = new List<string>
            {
                    "	дюма,тома,дега,люка,ферма,гамарра,петипа,шандра . . . . .",
                    "	гусь,ремень,камень,онук,богода,нечипас,долгопалец,маненок,рева,кива . . . . .",
                    "	вий,сой,цой,хой -я -ю -я -ем -е"
            },
            Suffixes = new List<string> {
                "f	б,в,г,д,ж,з,й,к,л,м,н,п,р,с,т,ф,х,ц,ч,ш,щ,ъ,ь . . . . .",
                "f	ска,цка  -ой -ой -ую -ой -ой",
                "f	ая       --ой --ой --ую --ой --ой",
                "	ская     --ой --ой --ую --ой --ой",
                "f	на       -ой -ой -у -ой -ой",

                "       орн,слон а у а ом е",
                "	иной -я -ю -я -ем -е",
                "	уй   -я -ю -я -ем -е",
                "	ца   -ы -е -у -ей -е",

                "	рих  а у а ом е",

                "	ия                      . . . . .",
                "	иа,аа,оа,уа,ыа,еа,юа,эа . . . . .",
                "	их,ых                   . . . . .",
                "	о,е,э,и,ы,у,ю           . . . . .",

                "	ова,ева            -ой -ой -у -ой -ой",
                "	га,ка,ха,ча,ща,жа  -и -е -у -ой -е",
                "	ца  -и -е -у -ей -е",
                "	а   -ы -е -у -ой -е",

                "	ь   -я -ю -я -ем -е",

                "	ия  -и -и -ю -ей -и",
                "	я   -и -е -ю -ей -е",
                "	ей  -я -ю -я -ем -е",

                "	ян,ан,йн   а у а ом е",

                "	ынец,обец  --ца --цу --ца --цем --це",
                "	онец,овец  --ца --цу --ца --цом --це",

                "	ц,ч,ш,щ   а у а ем е",

                "	ай  -я -ю -я -ем -е",
                "	гой,кой  -го -му -го --им -м",
                "	ой  -го -му -го --ым -м",
                "	ах,ив   а у а ом е",

                "	ший,щий,жий,ний  --его --ему --его -м --ем",
                "	кий,ый   --ого --ому --ого -м --ом",
                "	ий       -я -ю -я -ем -и",

                "	ок  --ка --ку --ка --ком --ке",
                "	ец  --ца --цу --ца --цом --це",

                "	в,н   а у а ым е",
                "	б,г,д,ж,з,к,л,м,п,р,с,т,ф,х   а у а ом е"
            }
        };

        /// <summary>
        /// Имя
        /// </summary>
        public Word FirstName { get; set; } = new Word()
        {
            Exceptions = new List<string> {
                 "	лев    --ьва --ьву --ьва --ьвом --ьве",
                "	павел  --ла  --лу  --ла  --лом  --ле",
                "m	шота   . . . . .",
                "m	пётр   ---етра ---етру ---етра ---етром ---етре",
                "f	рашель,нинель,николь,габриэль,даниэль   . . . . ."
            },
            Suffixes = new List<string> {
                 "	е,ё,и,о,у,ы,э,ю   . . . . .",
                "f	б,в,г,д,ж,з,й,к,л,м,н,п,р,с,т,ф,х,ц,ч,ш,щ,ъ   . . . . .",

                "f	ь   -и -и . ю -и",
                "m	ь   -я -ю -я -ем -е",

                "	га,ка,ха,ча,ща,жа  -и -е -у -ой -е",
                "	ша  -и -е -у -ей -е",
                "	а   -ы -е -у -ой -е",
                "	ия  -и -и -ю -ей -и",
                "	я   -и -е -ю -ей -е",
                "	ей  -я -ю -я -ем -е",
                "	ий  -я -ю -я -ем -и",
                "	й   -я -ю -я -ем -е",
                "	б,в,г,д,ж,з,к,л,м,н,п,р,с,т,ф,х,ц,ч	 а у а ом е"
            }
        };

        /// <summary>
        /// Отчество
        /// </summary>
        public Word MiddleName { get; set; } = new Word()
        {
            Suffixes = new List<string> {
                 "	ич   а  у  а  ем  е",
                "	на  -ы -е -у -ой -е"
            }
        };

        public RussianNameProcessor()
        {
            PrepareRules();
        }

        void PrepareRules()
        {
            LastName.Rule();
            FirstName.Rule();
            MiddleName.Rule();
        }

        /// <summary>
        /// склоняем слово по указанному набору правил и исключений
        /// </summary>
        /// <param name="word">слово</param>
        /// <param name="sex">пол</param>
        /// <param name="wordType">тип слова (фио)</param>
        /// <param name="gcase">падеж</param>
        /// <returns></returns>
        public static string Word(string word, string sex, Word rules, string gcase)
        {
            // исходное слово находится в именительном падеже
            if (gcase == Imenit) return word;

            // составные слова
            if (Regex.IsMatch(word, "/[-]/"))
            {
                string[]? list = word.Split(" ");
                for (int i = 0; i < list?.Length; i++)
                {
                    list[i] = Word(list[i], sex, rules, gcase);
                }
                return string.Join("-", list);
            }

            // Иванов И. И.
            if (Regex.IsMatch(word, @"/^[А-ЯЁ]\.?$/i")) return word;

            if (rules.ExceptionsRule != null)
            {
                var pick = Pick(word, sex, gcase, rules.ExceptionsRule, true);
                if (pick != null) 
                    return pick;
            }

            var pickSuffix = Pick(word, sex, gcase, rules.SuffixesRule, false);
            return pickSuffix ?? word;
        }

        /// <summary>
        /// выбираем из списка правил первое подходящее и применяем
        /// </summary>
        /// <param name="word">Слово</param>
        /// <param name="sex">Пол</param>
        /// <param name="gcase">Падеж</param>
        /// <param name="rules">Правила</param>
        /// <param name="matchWholeWord">Хз</param>
        /// <returns></returns>
        public static string? Pick(string word, string sex, string gcase, Rule[] rules, bool matchWholeWord)
        {
            var wordLower = word.ToLower();
            for (int i = 0; i < rules?.Length; i++)
            {
                if (RuleMatch(rules[i], wordLower, sex, matchWholeWord))
                {
                    return ApplyMod(rules[i], word, gcase);
                }
            }
            return null;
        }

        public static bool RuleMatch(Rule rule, string word, string sex, bool matchWholeWord)
        {
            if (rule.Sex == sexM && sex == sexF) return false; // male by default
            if (rule.Sex == sexF && sex != sexF) return false;

            for (int i = 0; i < rule.Test.Count(); i++)
            {
                var test = matchWholeWord ? word : word.Substring(Math.Max(word.Length - rule.Test[i].Length, 0));
                if (test == rule.Test[i]) return true;
            }
            return false;
        }


        public static string ApplyMod(Rule rule, string word, string gcase)
        {
            string mod = "";
            switch (gcase)
            {
                case Imenit: mod = "."; break;
                case Rodit: mod = rule.Mods[0]; break;
                case Datel: mod = rule.Mods[1]; break;
                case Vinit: mod = rule.Mods[2]; break;
                case Tvor: mod = rule.Mods[3]; break;
                case Predl: mod = rule.Mods[4]; break;
            }

            for (int i = 0; i < mod.Length; i++)
            {
                string c = mod.Substring(i, 1);
                switch (c)
                {
                    case ".": break;
                    case "-": word = word.Substring(0, word.Length - 1); break;
                    default: word += c; break;
                }
            }
            return word;
        }
    }
}