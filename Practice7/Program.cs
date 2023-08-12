using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Practice7
{
    class Program
    {
        static void Main(string[] args)
        {
        }
        /*
         * Test
         */
        public static bool validate(string domain)
        {
            string valid = "0123456789.-abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (domain.Length > 253 ||
                domain.Where(x => valid.Contains(x)).Count() != domain.Length ||
                ".-".Contains(domain[0]) ||
                ".-".Contains(domain[domain.Length - 1]))
            {
                return false;
            }
            var levels = domain.Split('.');
            if (levels.Length < 2 ||
                levels.Length > 127 ||
                levels.Any(x => string.IsNullOrWhiteSpace(x)) ||
                levels.Where(x => x.Length < 63).Count() != levels.Length ||
                string.Join("", levels).All(x => char.IsNumber(x)) ||
                levels.Where(x => ".-".Contains(x[0]) || ".-".Contains(x[x.Length - 1])).Count() > 0 ||
                levels[levels.Length - 1].All(x => char.IsNumber(x)))
            {
                return false;
            }
            return true;
        }
        public static bool IsBalanced(string s, string caps)
        {
            var contain = caps.Where(x => s.Contains(x)).Distinct().Count();
            if (contain == caps.Length)
            {
                int index = 0;
                var split = string.Join("", caps.Select(x => index++ % 2 == 0 ? x.ToString() : $"{x} ")).Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                for (int i = 0; i < split.Count; i++)
                {
                    int index2 = 0;
                    var lastIndex = Convert.ToInt32(s.Select(x => index2++ < s.Length && x == split[i][1] ? $"{index2 - 1}" : "").Where(x => x != "").Last());
                    string check = s.Substring(s.IndexOf(split[i][0]), lastIndex - s.IndexOf(split[i][0]) + 1);

                    int even = 0;
                    for (int j = 0; j < check.Length; j++)
                    {
                        if (check[j] == split[i][0])
                        {
                            even++;
                        }
                        if (check[j] == split[i][1])
                        {
                            even--;
                        }
                        if (even < 0)
                        {
                            return false;
                        }
                    }
                    if (even != 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            if (contain == 0)
            {
                return true;
            }
            return false;
        }
        public static char[] Loneliest(string result)
        {
            if (result.Contains(' '))
            {
                Dictionary<char, List<int>> spaceCounts = new Dictionary<char, List<int>>();
                int gap = 0;
                result = result.Trim();
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i] == ' ')
                    {
                        gap++;
                    }
                    else
                    {
                        if (spaceCounts.Count > 0)
                        {
                            spaceCounts[spaceCounts.ElementAt(spaceCounts.Count - 1).Key].Add(gap);
                        }
                        spaceCounts.Add(result[i], new List<int> { gap });
                        gap = 0;
                    }
                    if (i == result.Length - 1)
                    {
                        spaceCounts[spaceCounts.ElementAt(spaceCounts.Count - 1).Key].Add(gap);
                    }
                }
                List<char> mostCushion = new List<char>(spaceCounts.Select(x => x.Key));
                List<char> tempCushion = new List<char>(spaceCounts.Select(x => x.Key));
                int count = 0;
                while (tempCushion.Count > 0)
                {
                    tempCushion = spaceCounts.Where(x => x.Value[0] > count && x.Value[1] > count).Select(x => x.Key).ToList();
                    count++;
                    if (tempCushion.Count > 0)
                    {
                        mostCushion.Clear();
                        mostCushion = tempCushion;
                    }
                }
                var totalSpace = spaceCounts.Where(x => mostCushion.Contains(x.Key)).Select(x => new { Character = x, SpaceSum = x.Value[0] + x.Value[1] }).OrderByDescending(x => x.SpaceSum);
                return totalSpace.Where(x => x.SpaceSum == totalSpace.First().SpaceSum).Select(x => x.Character.Key).ToArray();
            }
            return result.Select(x => x).ToArray();
            //var sort = spaceCounts.OrderByDescending(x => x.Value[0]).ThenByDescending(x => x.Value[1]);
            //return sort.Where(x => x.Value[0] == sort.First().Value[0] && x.Value[1] == sort.First().Value[1]).Select(x => x.Key).ToArray();
        }
        public static string HackMyTerminal(int passLength, string machineCode)
        {
            if (string.IsNullOrEmpty(machineCode) || passLength == 0)
            {
                return null;
            }
            var trim = string.Join("", machineCode.Select(x => char.IsLetter(x) ? x : ' ')).Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Where(x => x.Length == passLength).ToList();
            if (trim.Count() == 1)
            {
                return trim.First();
            }
            int index = 0;
            for (int i = 0; i < passLength; i++)
            {
                var placeCheck = trim.Select(x => x[i]).ToList();
                if (!placeCheck.All(x => x == placeCheck.First()) && placeCheck.Distinct().Count() != placeCheck.Count)
                {
                    index = placeCheck.IndexOf(placeCheck.Select(x => new { Value = x, Count = placeCheck.Where(y => y == x).Count() }).Where(x => x.Count == 1).Select(x => x.Value).First()) ;
                    break;
                }
            }
            return trim[index];
        }
        public static string EncodeCipher(string text, string key)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            string remaining = string.Join("", alpha.Where(x => !key.Contains(x)));
            string completeKey = string.Join("", key.GroupBy(x => x).Select(x => x.Key)) + remaining;

            string cipherEncode = "";
            int letterPlace = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsLetter(text[i]))
                {
                    letterPlace++;
                    int tempMove = completeKey.IndexOf(char.ToLower(text[i])) + letterPlace;
                    while (tempMove > 25)
                    {
                        tempMove -= 26;
                    }
                    char tempLetter = completeKey[tempMove];
                    if (char.IsUpper(text[i]))
                    {
                        tempLetter = char.ToUpper(tempLetter);
                    }
                    cipherEncode += tempLetter;
                }
                else
                {
                    cipherEncode += text[i];
                    letterPlace = 0;
                }
            }
            return cipherEncode;
        }
        public static string DecodeCipher(string text, string key)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            string remaining = string.Join("", alpha.Where(x => !key.Contains(x)));
            string completeKey = string.Join("", key.GroupBy(x => x).Select(x => x.Key)) + remaining;

            string cipherDecode = "";
            int letterPlace = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsLetter(text[i]))
                {
                    letterPlace++;
                    int tempMove = completeKey.IndexOf(char.ToLower(text[i])) - letterPlace;
                    while (tempMove < 0)
                    {
                        tempMove += 26;
                    }
                    char tempLetter = completeKey[tempMove];
                    if (char.IsUpper(text[i]))
                    {
                        tempLetter = char.ToUpper(tempLetter);
                    }
                    cipherDecode += tempLetter;
                }
                else
                {
                    cipherDecode += text[i];
                    letterPlace = 0;
                }
            }
            return cipherDecode;
        }
        public static string Biggest1(int[] nums)
        {
            var ordered = string.Join("", nums.Select(x => x.ToString()).OrderByDescending(x => x[0]).ThenByDescending(x => x.Length).ThenByDescending(x => Convert.ToInt32(x)));
            return ordered;
        }
        public static string UserModEntry { get; set; }
        public static string ResultMessage { get; set; }
        public static string ResultStatus { get; set; }

        public static List<string> WordPresence(string wordCheck)
        {
            List<string> splitWords = new List<string>();
            if (wordCheck.Contains(' ') || wordCheck.Contains('.') || wordCheck.Contains('!') || wordCheck.Contains('?') || wordCheck.Contains(',') || wordCheck.Contains(';') || wordCheck.Contains(':'))
            {
                string replace = wordCheck.Replace('.', ' ').Replace('!', ' ').Replace('?', ' ').Replace(',', ' ').Replace(';', ' ').Replace(':', ' ');
                if (replace.Any(x => x != ' '))
                {
                    splitWords = replace.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
                }
            }
            else
            {
                splitWords.Add(wordCheck);
            }
            return splitWords;
        }

        public static void OrderWords(string text)
        {
            var wordList = WordPresence(text);
            if (wordList.Count > 0)
            {
                if (wordList.Count > 1)
                {
                    string modifiedText = string.Join(" ", wordList.OrderBy(x => x));
                    if (modifiedText != UserModEntry)
                    {
                        UserModEntry = modifiedText;
                        ResultStatus = "Modification Successful:";
                        ResultMessage = "All words in text entry have been ordered.";
                    }
                    else
                    {
                        ResultStatus = "Modification Unsuccessful:";
                        ResultMessage = "All words in text entry are already ordered.";
                    }
                }
                else
                {
                    ResultStatus = "Modification Unsuccessful:";
                    ResultMessage = $"Insufficient amount of words for ordering. Text entry only contains one qualified word.";
                }
            }
            else
            {
                ResultStatus = "Modification Unsuccessful:";
                ResultMessage = "Text entry does not contain any qualified words for ordering.";
            }

        }

        public static int SearchArray(object[][] arrayToSearch, object[] query)
        {
            var searchCheck = arrayToSearch.Where(x => !x.GetType().IsArray || x.Length != 2);
            if (searchCheck.Count() > 0 || query.Length != 2)
            {
                throw new Exception();
            }
            int index = 0;
            string placement = "";
            if (Type.GetTypeCode(query[0].GetType()) == TypeCode.Int32)
            {
                placement = arrayToSearch.Select(x => index++ < arrayToSearch.GetLength(0) && Convert.ToInt32(x[0]) == Convert.ToInt32(query[0]) && Convert.ToInt32(x[1]) == Convert.ToInt32(query[1]) ? $"{index - 1}" : "").Where(x => x != "").FirstOrDefault();
            }
            else
            {
                placement = arrayToSearch.Select(x => index++ < arrayToSearch.GetLength(0) && Convert.ToString(x[0]) == Convert.ToString(query[0]) && Convert.ToString(x[1]) == Convert.ToString(query[1]) ? $"{index - 1}" : "").Where(x => x != "").FirstOrDefault();
            }
            return placement != null && placement != "" ? Convert.ToInt32(placement) : -1;
        }
        public static int[] ValidateBet(int N, int M, string text)
        {
            var charCheck = text.Where(x => x != ' ' && x != ',' && !char.IsDigit(x));
            if (charCheck.Count() > 0)
            {
                return null;
            }
            var divide = text.Split(new char[] { ',', ' '}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).OrderBy(x => x);
            if (divide.Count() == N && divide.Last() <= M && !divide.Contains(0) && divide.Count() == divide.Distinct().Count())
            {
                return divide.ToArray();
            }
            return null;
        }
        public static string Decode1(string message, string key, int initShift)
        {
            var keyDistinct = string.Join("", key.Distinct());
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            var alphaDistinct = string.Join("", alpha.Where(x => !keyDistinct.Contains(x)).DefaultIfEmpty(' '));
            var finalKey = alphaDistinct != " " ? $"{keyDistinct}{alphaDistinct}" : keyDistinct;

            string decoded = "";
            int last = initShift;
            for (int i = 0; i < message.Length; i++)
            {
                if (finalKey.Contains(message[i]))
                {
                    int numKey = finalKey.IndexOf(message[i]) + 1;
                    int index = numKey - 1 - last;
                    while (index < 0)
                    {
                        index += 26;
                    }
                    decoded += finalKey[index];
                    last = finalKey.IndexOf(decoded.Last()) + 1;
                }
                else
                {
                    decoded += message[i];
                }
            }
            return decoded;
        }
        public static string Encode1(string message, string key, int initShift)
        {
            var keyDistinct = string.Join("", key.Distinct());
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            var alphaDistinct = string.Join("", alpha.Where(x => !keyDistinct.Contains(x)).DefaultIfEmpty(' '));
            var finalKey = alphaDistinct != " " ? $"{keyDistinct}{alphaDistinct}" : keyDistinct;

            string encoded = "";
            int last = initShift;
            for (int i = 0; i < message.Length; i++)
            {
                if (finalKey.Contains(message[i]))
                {
                    int numKey = finalKey.IndexOf(message[i]) + 1;
                    int index = numKey - 1 + last;
                    while (index > 25)
                    {
                        index -= 26;
                    }
                    encoded += finalKey[index];
                    last = numKey;
                }
                else
                {
                    encoded += message[i];
                }
            }
            return encoded;
        }
        public static string[] SplitMessage(string message, int count)
        {
            if (count <= 0)
            {
                return null;
            }
            if (string.IsNullOrEmpty(message))
            {
                var empty = new string[count];
                return empty.Select(x => string.Empty).ToArray();
            }
            List<string> friends = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var temp = "";
                int index = i + 1;
                int hits = 1;
                for (int j = 0; j < message.Length; j++)
                {
                    if (j + 1 == index)
                    {
                        temp += message[j];
                        index = (hits * count) + (i + 1);
                        hits++;
                    }
                    else
                    {
                        temp += '-';
                    }
                }
                friends.Add(temp);
            }
            return friends.ToArray();
        }
        public static string GetChange(decimal price, decimal inputMoney)
        {
            if (price <= 0 || price > 1 || inputMoney > 1)
            {
                return "Invalid input.";
            }
            if (price == inputMoney)
            {
                return "No Change.";
            }

            int quarters = 0;
            int dimes = 0;
            int pennies = 0;
            decimal change = inputMoney - price;

            while (change > 0)
            {
                if (change >= .25m)
                {
                    quarters++;
                    change -= .25m;
                }
                else if (change >= .10m)
                {
                    dimes++;
                    change -= .10m;
                }
                else if (change >= .01m)
                {
                    pennies++;
                    change -= .01m;
                }
            }
            string qFinal = quarters > 0 ? quarters == 1 ? $"1 Quarter, " : $"{quarters} Quarters, ": "";
            string dFinal = dimes > 0 ? dimes == 1 ? $"1 Dime, " : $"{dimes} Dimes, " : "";
            string pFinal = pennies > 0 ? pennies == 1 ? $"1 Penny, " : $"{pennies} Pennies, " : "";
            string final = string.Concat(qFinal, dFinal, pFinal);

            return $"{final.Remove(final.Length - 2)}.";
        }
        public static string InitializeNames(string name)
        {
            var names = name.Split(' ');
            if (names.Length <= 2)
            {
                return name;
            }
            int count = 0;
            var initial = names.Select(x => count++ != 0 || (count - 1) != names.Length - 1 ? $"{x.Substring(0, 1)}." : x);
            return string.Join(" ", initial);
        }
        public class Solutions
        {
            public static string Check(List<string> wordBank, string search)
            {
                string found = "";
                var wordCheck = wordBank.Where(x => x.ToArray().Distinct().Select(y => x.Count(z => z == y)).All(a => a == 1) &&  x.Where((ch, i) => search[i] == ch).Count() == 3).DefaultIfEmpty().First();
                if (!string.IsNullOrEmpty(wordCheck))
                {
                    found = wordCheck;
                }
                return found;
            }
            public static int Mutations(string[] alice, string[] bob, string word, int turn)
            {
                int person = turn;
                string lastWord = new string(word);
                string aliceLast = "";
                string bobLast = "";
                List<string> aliceList = new List<string>(alice);
                List<string> bobList = new List<string>(bob);
                bool aTry = true;
                bool bTry = true;

                while (person == 0 || person == 1)
                {
                    if (person == 0)
                    {
                        if (aliceLast == lastWord)
                        {
                            break;
                        }
                        string checker = Check(aliceList, lastWord);
                        if (checker.Length > 0)
                        {
                            if (bTry)
                            {
                                person++;
                                aTry = true;
                                lastWord = checker;
                                aliceList.Remove(lastWord);
                                aliceLast = checker;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (bTry)
                            {
                                person++;
                                aTry = false;
                            }
                            else
                            {
                                person = -1;
                            }
                        }
                    }
                    else if (person == 1)
                    {
                        if (bobLast == lastWord)
                        {
                            break;
                        }
                        string checker = Check(bobList, lastWord);
                        if (checker.Length > 0)
                        {
                            if (aTry)
                            {
                                person--;
                                bTry = true;
                                lastWord = checker;
                                bobList.Remove(lastWord);
                                bobLast = checker;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (aTry)
                            {
                                person--;
                                bTry = false;
                            }
                            else
                            {
                                person = -1;
                            }
                        }

                    }
                }
                return person;
            }
        }
        public class Preloadeds
        {
            public static string[] WORDS = new []{ "ACT","ADD","ALL","APE","AND","ANN","ANY","ANT","ARE","ART","ASS","BAD","BAR","BAT","BAY","BEE","BIG","BIT","BOB","BOY","BUN","BUT","CAN","CAR","CAT","COT","COW","CUT","DAD","DAY","DEW","DID","DIN","DOG","DON","DOT","DUD","EAR","EAT","EEL","EGG","ERR","EYE","FAG","FAR","FLY","FOR","FUN","FUR","GAY","GET","GOT","GUM","GUN","GUY","GUT","GYM","HAS","HAT","HER","HEY","HIM","HIS","HIT","HOW","HUG","HUN","ICE","INK","ITS","IVE","JAN","JET","JOB","JOT","JOY","KEY","LAP","LAY","LIE","LET","LOG","MAN","MAP","MAY","MEN","MOM","MUD","MUM","NAP","NEW","NOD","NOT","NOW","OAR","ODD","OFF","OLD","ONE","OUR","OUT","PAN","PAL","PAT","PAW","PEN","PET","PIG","PIT","POT","PRO","PUT","QUO","RAG","RAM","RAN","RAP","RAT","RED","RIP","ROD","ROT","RUN","RUT","SAT","SAW","SAY","SEA","SEE","SEX","SHE","SOY","SUN","SUX","TAN","TAT","TEA","THE","TIN","TIP","TIT","TON","TOP","TOO","TWO","URN","USE","VAN","VET","VIP","WAR","WAS","WAY","WED","WHO","WHY","WIN","WON","XXX","YAK","YAM","YAP","YOU","YUM","ZAP","ZIP","ZIT","ZOO","ABLE","ACED","AGOG","AHEM","AHOY","ALLY","AMEN","ANTI","ANTS","ANUS","APES","ARMY","ARSE","ARTY","AVID","AWED","BABY","BARS","BATS","BAYS","BEAR","BEES","BILL","BITE","BITS","BLOW","BLUE","BOLD","BONE","BOOB","BOOM","BOSS","BOYS","BUFF","BUNG","BUNS","BUMS","BURP","BUST","BUSY","BUZZ","CANS","CANT","CARS","CART","CATS","CHAP","CHIC","CHUM","CIAO","CLAP","COCK","CODE","COOL","COWS","COZY","CRAB","CREW","CURE","CULT","DADS","DAFT","DAWN","DAYS","DECK","DEED","DICK","DING","DOGS","DOTS","DOLL","DOLT","DONG","DOPE","DOWN","DRAW","DUCK","DUDE","DUMB","DUTY","EARL","EARN","EARS","EASY","EATS","EDGE","EELS","EGGS","ENVY","EPIC","EYES","FACE","FAGS","FANG","FARM","FART","FANS","FAST","FEAT","FEET","FISH","FIVE","FIZZ","FLAG","FLEW","FLIP","FLOW","FOOD","FORT","FUCK","FUND","GAIN","GEEK","GEMS","GIFT","GIRL","GIST","GIVE","GLEE","GLOW","GOLD","GOOD","GOSH","GRAB","GRIN","GRIT","GROT","GROW","GRUB","GUNS","GUSH","GYMS","HAIL","HAIR","HALO","HANG","HATS","HEAD","HEAL","HEIR","HELL","HELP","HERE","HERO","HERS","HIGH","HIRE","HITS","HOLY","HOPE","HOST","HUNK","HUGE","HUNG","HUNS","HURT","ICON","IDEA","IDLE","IDOL","IOTA","JAZZ","JERK","JESS","JETS","JINX","JOBS","JOHN","JOKE","JUMP","JUNE","JULY","JUNK","JUST","KATA","KEYS","KICK","KIND","KING","KISS","KONG","KNOB","KNOW","LARK","LATE","LEAN","LICE","LICK","LIKE","LION","LIVE","LOGS","LOCK","LONG","LOOK","LORD","LOVE","LUCK","LUSH","MAKE","MANY","MART","MATE","MAXI","MEEK","MIKE","MILD","MINT","MMMM","MOMS","MOOD","MOON","MOOT","MUCH","MUFF","MUMS","MUTT","NAPS","NAZI","NEAT","NECK","NEED","NEWS","NEXT","NICE","NICK","NOON","NOSE","NOTE","OARS","OATS","ONCE","ONLY","OPEN","ORGY","OVAL","OVER","PANS","PALS","PART","PAST","PATS","PAWS","PEAR","PERT","PENS","PETS","PHEW","PIPE","PIPS","PLAN","PLUM","PLUS","POET","POOF","POOP","POSH","POTS","PROS","PSST","PUKE","PUNK","PURE","PUSH","PUSS","QUAD","QUAK","QUID","QUIT","RANT","RAPE","RAPS","RAPT","RATE","RAMS","RATS","REAP","RICK","RING","RIPE","ROOT","ROSE","ROSY","ROTS","RUNT","RUTS","SAFE","SAGE","SANE","SAVE","SAWS","SEEK","SEXY","SHAG","SHIT","SICK","SIGH","SIRE","SLAG","SLIT","SLUT","SNAP","SNOG","SNUG","SOFT","SOON","SOUL","SOUP","SPRY","STIR","STUN","SUCK","SWAG","SWAY","TACT","TANK","TANS","THAT","THIS","TIME","TINS","TINY","TITS","TOES","TONS","TONY","TOPS","TOYS","UBER","URNS","USED","USER","USES","VAIN","VAMP","VARY","VEIN","VENT","VERY","VEST","VIEW","VIVA","VOLT","VOTE","WAFT","WAGE","WAKE","WALK","WALL","WANG","WANK","WANT","WARD","WARM","WARP","WARS","WART","WASH","WAVE","WEAR","WEDS","WEED","WEEN","WELD","WHAT","WHEE","WHEW","WHIP","WHIZ","WHOA","WIFE","WILL","WIND","WING","WINK","WINS","WIRE","WISH","WITH","WORD","WORK","WRAP","XMAN","XMEN","XRAY","XTRA","XXXX","YANK","YAKS","YAMS","YAPS","YARD","YARN","YELP","YERN","YOKE","YOLK","YULE","ZANY","ZAPS","ZIPS","ZITS","ZERO","ZOOM","ZOOS" };
        }
        public static HashSet<string> Check1800(string str)
        {
            List<string> letters = new List<string> { "ABC", "DEF", "GHI", "JKL", "MNO", "PQRS", "TUV", "WXYZ" };
            var nums = Preloadeds.WORDS.Select(x => new { Original = x, Number = string.Join("", x.Select(y => letters.IndexOf(letters.Where(z => z.Contains(y)).First()) + 2)) }).OrderBy(X => X.Number);

            var first = char.IsLetter(str[9]) ? str.Substring(6, 4) : str.Substring(6, 3);
            var firNums = string.Join("", first.Select(y => letters.IndexOf(letters.Where(z => z.Contains(y)).First()) + 2));
            var second = first.Length == 4 ? str.Substring(11) : str.Substring(10);
            var secNums = string.Join("", second.Select(y => letters.IndexOf(letters.Where(z => z.Contains(y)).First()) + 2));

            if (!Preloadeds.WORDS.Contains(first) || !Preloadeds.WORDS.Contains(second))
            {
                return new HashSet<string> { };
            }

            var firstMatch = nums.Where(x => x.Number == firNums).ToArray();
            var secondMatch = nums.Where(x => x.Number == secNums).ToArray();

            string word = first.Length > second.Length ? $"{first}{second}" : $"{second}{first}";
            List<string> matches = new List<string> { word };
            for (int i = 0; i < firstMatch.Length; i++)
            {
                for (int j = 0; j < secondMatch.Length; j++)
                {
                    var temp = firstMatch[i].Original.Length > secondMatch[j].Original.Length ? $"{firstMatch[i].Original}{secondMatch[j].Original}" : $"{secondMatch[j].Original}{firstMatch[i].Original}";
                    if (!matches.Contains(temp))
                    {
                        matches.Add(temp);
                    }
                }
            }

            return matches.Select(x => first.Length > second.Length ? $"1-800-{x.Substring(0, 4)}-{x.Substring(4)}" : $"1-800-{x.Substring(4)}-{x.Substring(0, 4)}").ToHashSet();
        }
        public static int Recur(int num)
        {
            if (num == 0)
            {
                return 1;
            }
            return num * Recur(num - 1);
        }
        public static List<Tuple<char, int>> OrderedCount(string input)
        {
            return input.Distinct().Select(x => new { Letter = x, Count = input.Where(y => y == x).Count() }).Select(x => Tuple.Create(x.Letter, x.Count)).ToList(); 
        }
        public static int SumOfMinimums(int[,] numbers)
        {
            int min = 0;
            int arrays = numbers.GetLength(0);
            int lengths = numbers.GetLength(1);
            for (int i = 0; i < arrays; i++)
            {
                List<int> temp = new List<int>();
                for (int j = 0; j < lengths; j++)
                {
                    temp.Add(numbers[i, j]);
                }
                min += temp.Min();
            }

            return min;
        }
        public static object[] ArrayLowerCase(object[] arr)
        {
            return arr.Select(x => x is string ? x.ToString().ToLower() : x).ToArray();
        }
        public static class SimpleAssembler
        {
            public static Dictionary<string, int> Registers = new Dictionary<string, int>();

            public static string[] Instructions(string instruct)
            {
                string action = instruct.Substring(0, 3);
                string regName = instruct[4].ToString();
                string amount = instruct.Length > 5 ? instruct.Substring(6) : "1";
                string jnzAction = "";

                return new string[] { action, regName, amount, jnzAction};
            }
            public static void Mov(string[] info)
            {
                int num = 0;
                var isNum = int.TryParse(info[2], out num);
                if (isNum)
                {
                    if (Registers.ContainsKey(info[1]))
                    {
                        Registers[info[1]] += num;
                    }
                    else
                    {
                        Registers.Add(info[1], num);
                    }
                }
                else
                {
                    if (Registers.ContainsKey(info[1]))
                    {
                        Registers[info[1]] += Registers[info[2]];
                    }
                    else
                    {
                        Registers.Add(info[1], Registers[info[2]]);
                    }
                }
            }
            public static void Inc(string[] info)
            {
                if (Registers[info[1]] < 0)
                {
                    Registers[info[1]] = 0;
                }
                else
                {
                    Registers[info[1]] += 1;
                }
            }
            public static void Dec(string[] info)
            {
                if (Registers[info[1]] < 0)
                {
                    Registers[info[1]] += Registers[info[1]];
                }
                else
                {
                    if (info[3] == "X")
                    {
                        Registers[info[1]] = 0;
                    }
                    else
                    {
                        Registers[info[1]] -= 1;
                    }
                }
            }
            public static Dictionary<string, int> Interpret(string[] program)
            {
                for (int i = 0; i < program.Length; i++)
                {
                    var instructions = Instructions(program[i]);

                    if (instructions[0] == "jnz")
                    {
                        if (Registers.ContainsKey(instructions[1]) && Registers[instructions[1]] > 0)
                        {
                            int move = i + Convert.ToInt32(instructions[2]);
                            if (move < program.Length - 1 && move > 0)
                            {
                                instructions = Instructions(program[move]);
                                instructions[3] = "X"; 
                            }
                        }
                    }
                    if (instructions[0] == "mov")
                    {
                        Mov(instructions);
                    }
                    if (instructions[0] == "inc")
                    {
                        Inc(instructions);
                    }
                    if (instructions[0] == "dec")
                    {
                        Dec(instructions);
                    }
                }
                var answer = new Dictionary<string, int>(Registers);
                Registers = new Dictionary<string, int>();
                return answer;
            }
        }
        public class Preloaded
        {
            public static List<string> ANIMALS = new List<string> { "aardvark", "alligator", "armadillo", "antelope", "baboon", "bear", "bobcat", "butterfly    ","cat  ","camel    ","cow  ","chameleon    ","dog  ","dolphin  ","duck ","dragonfly    ","eagle    ","elephant ","emu  ","echidna  ","fish ","frog ","flamingo ","fox  ","goat ","giraffe  ","gibbon   ","gecko    ","hyena    ","hippopotamus ","horse    ","hamster  ","insect   ","impala   ","iguana   ","ibis ","jackal   ","jaguar   ","jellyfish    ","kangaroo ","kiwi ","koala    ","killerwhale  ","lemur    ","leopard  ","llama    ","lion ","monkey   ","mouse    ","moose    ","meercat  ","numbat   ","newt ","ostrich  ","otter    ","octopus  ","orangutan    ","penguin  ","panther  ","parrot   ","pig  ","quail    ","quokka   ","quoll    ","rat  ","rhinoceros   ","racoon   ","reindeer ","rabbit   ","snake    ","squirrel ","sheep    ","seal ","turtle   ","tiger    ","turkey   ","tapir    ","unicorn  ","vampirebat   ","vulture  ","wombat   ","walrus   ","wildebeast   ","wallaby  ","yak  ","zebra" };
        }
        public static string RoadKill(string photo)
        {
            if (string.IsNullOrEmpty(photo) || photo.Contains(" "))
            {
                return "??";
            }
            var distinct = string.Join("", photo.Where(x => char.IsLetter(x)).Distinct().OrderBy(x => x));
            var animals = Preloaded.ANIMALS.Select(x => new { Original = x, Ordered = string.Join("", x.Where(y => char.IsLetter(y)).Distinct().OrderBy(z => z)) }).Select(x => x.Ordered == distinct ? x.Original : "").OrderByDescending(x => x);

            if (animals.First().Length > 0)
            {
                int singleIndex = -1;
                var singleOrder = photo.Where(x => char.IsLetter(x)).ToArray();
                var singleCheck = singleOrder.Where(x => singleIndex++ == -1 || x != singleOrder[singleIndex - 1]).ToArray();

                int singIndexAnimal = -1;
                var singOrderAnimal = animals.First().Where(x => char.IsLetter(x)).ToArray();
                var singCheckAnimal = singOrderAnimal.Where(x => singIndexAnimal++ == -1 || x != singOrderAnimal[singIndexAnimal - 1]).ToArray();

                if (string.Join("", singleCheck) == string.Join("", singCheckAnimal) || string.Join("", singleCheck.Reverse()) == string.Join("", singCheckAnimal))
                {
                    var distCount = photo.Where(x => char.IsLetter(x)).Distinct().Select(x => new { Letter = x, Count = photo.Where(y => y == x).Count() }).OrderBy(x => x.Letter).ToArray();
                    var animCount = animals.First().Where(x => char.IsLetter(x)).Distinct().Select(x => new { Letter = x, Count = animals.First().Where(y => y == x).Count() }).OrderBy(x => x.Letter).ToArray();
                    int index = 0;
                    var check = animCount.Select(x => distCount[index++].Count >= x.Count);
                    if (!check.Contains(false))
                    {
                        return animals.First();
                    }
                }
            }

            return "??";
        }
        public static string HungryFoxes(string farm)
        {
            string outsideTemp = "";
            string outside = "";
            string insideTemp = "";
            List<string> inside = new List<string>();
            bool cage = false;
            bool foxInside = false;
            bool foxOutside = false;
            bool poisonInside = false;
            bool poisonOutside = false;
            for (int i = 0; i < farm.Length; i++)
            {
                if (farm[i] == '[')
                {
                    cage = true;
                }
                else if (farm[i] == ']')
                {
                    if (poisonInside)
                    {
                        insideTemp = string.Join("", insideTemp.Select(x => x == 'F' ? '.' : x));
                    }
                    outsideTemp += '+';
                    inside.Add(insideTemp);
                    insideTemp = "";
                    cage = false;
                    poisonInside = false;
                    foxInside = false;
                }
                else if (cage)
                {
                    if (farm[i] == '.')
                    {
                        insideTemp += farm[i];
                    }
                    if (farm[i] == 'X')
                    {
                        poisonInside = true;
                        insideTemp += farm[i];
                    }
                    if (farm[i] == 'F')
                    {
                        int index = 0;
                        var xIndexes = poisonInside ? insideTemp.Select(x => index++ < insideTemp.Length && x == 'X' ? index - 1 : -1).Where(x => x != -1).Last() : 0;
                        int index2 = 0;
                        insideTemp = string.Join("", insideTemp.Select(x => index2++ >= xIndexes && x == 'C' ? '.' : x));
                        foxInside = true;
                        insideTemp += farm[i];
                    }
                    if (farm[i] == 'C')
                    {
                        if (foxInside)
                        {
                            if (poisonInside)
                            {
                                int index = 0;
                                var xIndexes = insideTemp.Select(x => index++ < insideTemp.Length && x == 'X' ? index - 1 : -1).Where(x => x != -1).Last();
                                if (insideTemp.Substring(xIndexes).Contains('F'))
                                {
                                    insideTemp += '.';
                                }
                                else
                                {
                                    insideTemp += farm[i];
                                }
                            }
                            else
                            {
                                insideTemp += '.';
                            }
                        }
                        else
                        {
                            insideTemp += farm[i];
                        }
                    }
                }
                else 
                {
                    outsideTemp += farm[i];
                }
            }
            for (int i = 0; i < outsideTemp.Length; i++)
            {
                if (outsideTemp[i] == '.' || outsideTemp[i] == '+')
                {
                    outside += outsideTemp[i];
                }
                if (outsideTemp[i] == 'X')
                {
                    poisonOutside = true;
                    outside += outsideTemp[i];
                }
                if (outsideTemp[i] == 'F')
                {
                    int index = 0;
                    var xIndexes = poisonOutside ? outside.Select(x => index++ < outside.Length && x == 'X' ? index - 1 : -1).Where(x => x != -1).Last() : 0;
                    int index2 = 0;
                    outside = string.Join("", outside.Select(x => index2++ >= xIndexes && x == 'C' ? '.' : x));
                    foxOutside= true;
                    outside += outsideTemp[i];
                }
                if (outsideTemp[i] == 'C')
                {
                    if (foxOutside)
                    {
                        if (poisonOutside)
                        {
                            int index = 0;
                            var xIndexes = outside.Select(x => index++ < outside.Length && x == 'X' ? index - 1 : -1).Where(x => x != -1).Last();
                            if (outside.Substring(xIndexes).Contains('F'))
                            {
                                outside += '.';
                            }
                            else
                            {
                                outside += outsideTemp[i];
                            }
                        }
                        else
                        {
                            outside += '.';
                        }
                    }
                    else
                    {
                        outside += outsideTemp[i];
                    }
                }
            }
            if (poisonOutside)
            {
                outside = string.Join("", outside.Select(x => x == 'F' ? '.' : x));
            }
            int count = 0;
            return string.Join("",outside.Select(x => x == '+' ? $"[{inside[count++]}]" : x.ToString()));
        }
        public static string SearchForKey(string[] messages, string[] secrects)
        {
            Dictionary<string, List<int>> messInfo = new Dictionary<string, List<int>>();
            for (int i = 0; i < messages.Length; i++)
            {
                for (int j = 0; j < messages[i].Length; j++)
                {
                    if (!messInfo.ContainsKey(messages[i][j].ToString()))
                    {
                        messInfo.Add(messages[i][j].ToString(), new List<int>());
                    }
                    messInfo[messages[i][j].ToString()].Add(j); 
                }
            }
            var messOrder = messInfo.Select(x => new { Letter = x.Key, Indexes = string.Join("", x.Value.OrderBy(y => y)) });

            Dictionary<string, List<int>> secrInfo = new Dictionary<string, List<int>>();
            for (int i = 0; i < secrects.Length; i++)
            {
                for (int j = 0; j < secrects[i].Length; j++)
                {
                    if (!secrInfo.ContainsKey(secrects[i][j].ToString()))
                    {
                        secrInfo.Add(secrects[i][j].ToString(), new List<int>());
                    }
                    secrInfo[secrects[i][j].ToString()].Add(j);
                }
            }
            var secrOrder = secrInfo.Select(x => new { Letter = x.Key, Indexes = string.Join("", x.Value.OrderBy(y => y)) });

            var pairs = messOrder.Where(x => x.Indexes.Length > 1 && messOrder.Where(y => y.Indexes == x.Indexes).Count() == 1)
                                 .Select(x => string.Join("", $"{x.Letter}{secrOrder.Where(y => y.Indexes == x.Indexes).Select(z => z.Letter).First()}".ToArray().OrderBy(a => a)))
                                 .Distinct().Where(x => x[0] != x[1]).OrderBy(x => x).ToList();

            if (pairs.Count == 6)
            {
                return string.Join("", pairs);
            }

            int counter = 0;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < messages.Length; j++)
                {
                    if (messages[j].Contains(pairs[counter][0]) || messages[j].Contains(pairs[counter][1]))
                    {
                        int indexLetter = messages[j].Contains(pairs[counter][0]) ? messages[j].IndexOf(pairs[counter][0]) : messages[j].IndexOf(pairs[counter][1]);
                        var sec = messages[j].Contains(pairs[counter][0]) ? secrects.Where(x => x[indexLetter] == pairs[counter][1]).First() : secrects.Where(x => x[indexLetter] == pairs[counter][0]).First();
                        for (int k = 0; k < messages[j].Length; k++)
                        {
                            string addPair = string.Join("", $"{messages[j][k]}{sec[k]}".ToArray().OrderBy(x => x));
                            if (addPair[0] != addPair[1] && !pairs.Contains(addPair) && pairs.Where(x => x.Contains(addPair[0])).Count() == 0 && pairs.Where(x => x.Contains(addPair[1])).Count() == 0)
                            {
                                pairs.Add(addPair);
                                if (pairs.Count == 6)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                if (pairs.Count < 6)
                {
                    i = -1;
                    counter++;
                }
            }
            return string.Join("", pairs.OrderBy(x => x));
        }
        public static string StringFunc(string s, long x)
        {
            var sArray = s.ToArray();
            int wordLen = 0;
            int backIndex = sArray.Length - 1;
            int frontIndex = 0;
            for (long i = 0; i < x; i++)
            {
                sArray = sArray.Select(x => wordLen++ < sArray.Length && (wordLen - 1) % 2 == 0 ? sArray[backIndex--] : sArray[frontIndex++]).ToArray();
                wordLen = 0;
                backIndex = sArray.Length - 1;
                frontIndex = 0;
            }
            return string.Join("", sArray);
        }
        public class Opstrings
        {
            public static string Rot90Clock(string strng)
            {
                string diag = Diag1Sym(strng);
                return string.Join('\n', diag.Split('\n').Select(x => string.Join("", x.ToArray().Reverse())));
            }
            public static string Diag1Sym(string strng)
            {
                var split = strng.Split('\n');
                List<string> diag = new List<string>();
                for (int i = 0; i < split.Length; i++)
                {
                    diag.Add(string.Join("", split.Select(x => x[i])));
                }
                return string.Join('\n', diag);
            }
            public static string SelfieAndDiag1(string strng)
            {
                var diag = Diag1Sym(strng).Split('\n');
                var split = strng.Split('\n');
                List<string> sAndD = new List<string>();
                for (int i = 0; i < diag.Length; i++)
                {
                    sAndD.Add($"{split[i]}|{diag[i]}");
                }
                return string.Join('\n', sAndD);
            }
            public static string Oper(Func<string, string> fct, string s)
            {
                return fct(s);
            }
        }
        public static char[,] SeparateLiquids(char[,] glass)
        {
            var arrays = glass.GetLength(0);
            var lengths = glass.GetLength(1);
            string elements = "OAWH";
            int index = 0;
            var liquid = string.Join("", glass.Cast<char>().ToArray().OrderBy(x => elements.IndexOf(x)));
            for (int i = 0; i < glass.GetLength(0); i++)
            {
                for (int j = 0; j < glass.GetLength(1); j++)
                {
                    glass[i, j] = liquid[index];
                    index++;
                }
            }
            return glass;
        }
        public static string Encrypter(string text)
        {
            string region = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.,:;-?! '()$%&\"";
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            if (text.Any(x => !region.Contains(x)))
            {
                throw new Exception();
            }
            int index1 = 0;
            var step1 = text.Select(x => index1++ % 2 != 0 ? char.IsUpper(x) ? char.ToLower(x) : char.ToUpper(x) : x).ToArray();
            int index2 = -1;
            var step2 = step1.Select(x => index2++ != -1 ? region.IndexOf(step1[index2 - 1]) - region.IndexOf(x) < 0 ?
                                                           region[region.IndexOf(step1[index2 - 1]) - region.IndexOf(x) + 77] : region[region.IndexOf(step1[index2 - 1]) - region.IndexOf(x)] : x).ToArray();
            //step3
            return $"{region[76 - region.IndexOf(step2[0])]}{string.Join("", step2).Remove(0, 1)}";
        }

        public static string Decrypter(string encryptedText)
        {
            string region = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.,:;-?! '()$%&\"";
            if (string.IsNullOrEmpty(encryptedText))
            {
                return encryptedText;
            }
            if (encryptedText.Any(x => !region.Contains(x)))
            {
                throw new Exception();
            }
            string rev3 = $"{region[76 - region.IndexOf(encryptedText[0])]}{encryptedText.Remove(0, 1)}";
            string rev2 = "";
            for (int i = 0; i < rev3.Length; i++)
            {
                if (rev2.Length == 0)
                {
                    rev2 += rev3[i];
                }
                else
                {
                    int index2 = 77 + region.IndexOf(rev2.Last()) - region.IndexOf(rev3[i]) > 76 ? region.IndexOf(rev2.Last()) - region.IndexOf(rev3[i]) : 77 + region.IndexOf(rev2.Last()) - region.IndexOf(rev3[i]);
                    rev2 += region[index2];
                }
            }
            //rev1
            int index1 = 0;
            return string.Join("", rev2.Select(x => index1++ % 2 != 0 ? char.IsUpper(x) ? char.ToLower(x) : char.ToUpper(x) : x));
        }
        public static bool RotatePaper(string number)
        {
            string flip = "0x2xx59x86";
            string oneEighty = string.Join("", number.Select(x => flip[Convert.ToInt32(x) - 48]));
            string rev = string.Join("", number.ToArray().Reverse());
            return oneEighty == rev;
        }
        public static int ShorterestTime(int n, int m, (int, int, int, int) speeds)
        {
            if (n == 0)
            {
                return 0;
            }

            int stairs = n * speeds.Item4;
            int both = (n == m ? 0 : (Math.Abs(n - m) * speeds.Item4)) + (speeds.Item2 * 2) + (m * speeds.Item1) + speeds.Item3;
            int elevator = (n == m ? 0 : (Math.Abs(n - m) * speeds.Item1)) + (speeds.Item2 * 2) + (n * speeds.Item1) + speeds.Item3;
            
            return new int[] { stairs, both, elevator}.Min();
        }
        public static bool GenerationLoss(string orig, string copy)
        {
            if (orig.Length != copy.Length)
            {
                return false;
            }
            string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#+:. ";
            int index = 0;
            var check = orig.Where(x => x == copy[index++] || !possible.Contains(x) && x == copy[index - 1] || char.IsLetter(x) ?
                                                                                                                               char.IsUpper(x) ? $"{x}{char.ToLower(x)}#+:. ".Contains(copy[index - 1]) : $"{x}#+:. ".Contains(copy[index - 1]) :
                                                                                                                               x == '#' ? "#+:. ".Contains(copy[index - 1]) :
                                                                                                                               x == '+' ? "+:. ".Contains(copy[index - 1]) :
                                                                                                                               x == ':' ? ":. ".Contains(copy[index - 1]) :
                                                                                                                               x == '.' ? ". ".Contains(copy[index - 1]) :
                                                                                                                               x == ' ' ? " ".Contains(copy[index - 1]) : x == copy[index - 1]).Count();
            if (orig.Length == check)
            {
                return true;
            }
            return false;
        }
        public static string FindTheKey(string[] messages, string[] secrets)
        {
            string messJoin = string.Join(" ", messages);
            string secrJoin = string.Join(" ", secrets);
            int index = 0;
            return string.Join("", messJoin.Select(x => index++ < messJoin.Length - 1 && x != secrJoin[index - 1] ? string.Join("", $"{x}{secrJoin[index - 1]}".OrderBy(x => x)) : "")
                                           .Distinct()
                                           .OrderBy(x => x));
        }
        public static string Encrypted(string text, int key)
        {
            List<string> keyboard = new List<string>
            { "qwertyuiop", "asdfghjkl", "zxcvbnm,." };
            string words = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (keyboard.Where(x => x.Contains(char.ToLower(text[i]))).Any() || ",.<>".Contains(text[i]))
                {
                    string line = ",.<>".Contains(text[i]) ? keyboard[2] : keyboard.Where(x => x.Contains(char.ToLower(text[i]))).First();
                    int index = ",<".Contains(text[i]) ? 7 : ".>".Contains(text[i]) ? 8 : line.IndexOf(char.ToLower(text[i]));
                    string keyString = key.ToString().Length > 2 ? key.ToString() : key.ToString().Length == 2 ? $"0{key}" : $"00{key}";
                    int count = Convert.ToInt32(keyString[keyboard.IndexOf(line)]) - 48;
                    int moveIndex = line.Length == 9 ? index - count < 0 ? index - count + 9 : index - count : index - count < 0 ? index - count + 10 : index - count;
                    bool shift = char.IsUpper(text[i]) || "<>".Contains(text[i]);
                    char add = shift ? char.ToUpper(line[moveIndex]) : line[moveIndex];
                    words += shift && ",.".Contains(add) ? add == ',' ? '<' : '>' : add;
                }
                else
                {
                    words += text[i];
                }
            }
            return words;
        }

        public static string Decrypted(string encryptedText, int key)
        {
            List<string> keyboard = new List<string>
            { "qwertyuiop", "asdfghjkl", "zxcvbnm,." };
            string words = "";
            for (int i = 0; i < encryptedText.Length; i++)
            {
                if (keyboard.Where(x => x.Contains(char.ToLower(encryptedText[i]))).Any() || ",.<>".Contains(encryptedText[i]))
                {
                    string line = ",.<>".Contains(encryptedText[i]) ? keyboard[2] : keyboard.Where(x => x.Contains(char.ToLower(encryptedText[i]))).First();
                    int index = ",<".Contains(encryptedText[i]) ? 7 : ".>".Contains(encryptedText[i]) ? 8 : line.IndexOf(char.ToLower(encryptedText[i]));
                    string keyString = key.ToString().Length > 2 ? key.ToString() : key.ToString().Length == 2 ? $"0{key}" : $"00{key}";
                    int count = Convert.ToInt32(keyString[keyboard.IndexOf(line)]) - 48;
                    int moveIndex = line.Length == 9 ? index - count < 0 ? index - count + 9 : index - count : index - count < 0 ? index - count + 10 : index - count;
                    bool shift = char.IsUpper(encryptedText[i]) || "<>".Contains(encryptedText[i]);
                    char add = shift ? char.ToUpper(line[moveIndex]) : line[moveIndex];
                    words += shift && ",.".Contains(add) ? add == ',' ? '<' : '>' : add;
                }
                else
                {
                    words += encryptedText[i];
                }
            }
            return words;
        }
        public static int[][] Closest2(string strng)
        {
            if (string.IsNullOrEmpty(strng))
            {
                return new int[0][];
            }
            int index = 0;
            var split = strng.Split(" ");
            var stats = split.Select(x => new int[] { x.ToArray().Select(y => Convert.ToInt32(y) - 48).Sum(), index++, Convert.ToInt32(x)});
            var order = stats.OrderBy(x => x[0]).ThenBy(x => x[1]).ToList();
            int i = 0;
            var diff = order.Select(x => i++ < order.Count() - 1 ? new { Arr = x, Diff = (int?)order[i][0] - x[0] } : new { Arr = x, Diff = (int?)null });
            var smallest = diff.Where(x => x.Diff != null).OrderBy(x => x.Diff).ThenBy(x => x.Arr[0]).ThenBy(x => x.Arr[1]).ToList();
            var answer = new int[][] { smallest[0].Arr, order[order.IndexOf(smallest[0].Arr) + 1] };
            return new int[][] { smallest[0].Arr, order[order.IndexOf(smallest[0].Arr) + 1] };
        }
        public static int[][] Closest(string strng)
        {
            
            int index = 0;
            var stats = strng.Split(" ")
                .Select(x => new List<int?> { x.ToArray().Select(y => Convert.ToInt32(y) - 48).Sum(), index++, Convert.ToInt32(x), null });
            var order = stats.OrderBy(x => x[0]).ThenBy(x => x[1]).ToList();
            int i = 1;
            //try stating the list, then somehow appending it
            var diff = order.Select(x => i++ < order.Count() - 1 ? x[2] = order[i][0] - x[0] : x[2] = null);
            return new int[0][];
        }
        public static string AnySimilarity(List<int> firstList, List<int> secondList, int n)
        {
            string similar = "";
            int start = 0;
            string compare = $"{string.Join(",", secondList)},";
            for (int i = 0; i < 1; i++)
            {
                if (start + n - 1 <= firstList.Count - 1)
                {
                    string range = $"{string.Join(",", firstList.GetRange(start, n))},";
                    if (compare.Contains(range))
                    {
                        string add = $" {range.Remove(range.Length - 1)} |";
                        if (!similar.Contains(add))
                        {
                            similar += add;
                        }
                    }
                    start++;
                    i = -1;
                }
            }
            return similar.Length > 0 ? similar.Substring(1).Remove(similar.Length - 3) : similar;
        }
        public static string Soundex(string names)
        {
            var split = names.Split(' ');
            Dictionary<string, string> replace = new Dictionary<string, string>
            { {"bfpv", "1"}, {"cgjkqsxz", "2"}, {"dt", "3"}, {"l", "4"}, {"mn", "5"}, {"r", "6"}};
            for (int i = 0; i < split.Length; i++)
            {
                string firstLetter = char.ToUpper(split[i][0]).ToString();
                string valuefirst = replace.Where(x => x.Key.Contains(char.ToLower(firstLetter[0]))).Count() > 0 ? replace.Where(x => x.Key.Contains(char.ToLower(firstLetter[0]))).First().Value: "";
                string rest = split[i].Length > 1 ? split[i].Substring(1) : "";
                rest = string.Join("", rest.Where(x => !"hw".Contains(x))
                                           .Select(x => replace.Where(y => y.Key.Contains(x)).Count() > 0 ?
                                           replace.Where(y => y.Key.Contains(x)).First().Value : x.ToString()));
                int index = 0;
                rest = string.Join("", rest.Where(x => index++ == 0 || x != rest[index - 2]));
                if (rest.Length > 0 && valuefirst.ToString() == rest[0].ToString())
                {
                    rest = rest.Substring(1);
                }
                rest = string.Join("", rest.Where(x => !"aeiouy".Contains(x)));
                if (rest.Length < 3)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append('0', 3 - rest.Length);
                    rest += sb;
                }
                if (rest.Length > 3)
                {
                    rest = rest.Substring(0, 3);
                }
                split[i] = $"{firstLetter}{rest}";
            }
            return string.Join(" ", split);
        }
        class Rainfall
        {
            public static double Mean(string town, string strng)
            {
                if (!strng.Contains($"{town}:"))
                {
                    return -1;
                }
                var cities = strng.Split('\n');
                var selected = cities.Where(x => x.Contains(town)).First();
                var data = selected.Substring(selected.IndexOf(':') + 1).Split(',').Select(x => x.Substring(4)).Select(x => Convert.ToDouble(x));
                return data.Sum() / data.Count();
            }

            public static double Variance(string town, string strng)
            {
                if (!strng.Contains($"{town}:"))
                {
                    return -1;
                }
                var cities = strng.Split('\n');
                var selected = cities.Where(x => x.Contains(town)).First();
                var data = selected.Substring(selected.IndexOf(':') + 1).Split(',').Select(x => x.Substring(4)).Select(x => Convert.ToDouble(x));
                double mean = data.Sum() / data.Count();
                return data.Select(x => Math.Pow(x - mean, 2)).Sum() / data.Count();
            }
        }
        public static int SelNumber(int n, int d)
        {
            if (n < 12)
            {
                return 0;
            }
            int nums = 0;
            for (int i = 12; i < n + 1; i++)
            {
                var temp = i.ToString();
                int index = 0;
                var orderCheck = string.Join("", temp.Where(x => index++ < temp.Length - 1 && x < temp[index] && Math.Abs(x - temp[index]) <= d));
                if (orderCheck == temp.Remove(temp.Length - 1))
                {
                    nums++;
                }
            }
            return nums;
        }
        public static List<long> FindAll(int sumDigits, int numDigits)
        {
            string start = "1";
            string end = "9";
            if (numDigits > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('0', numDigits - 1);
                start += sb;

                sb.Clear();
                sb.Append('9', numDigits - 1);
                end += sb;
            }
            List<long> nums = new List<long>();
            for (long i = Convert.ToInt64(start); i < Convert.ToInt64(end) + 1; i++)
            {
                var temp = i.ToString();
                int index = 0;
                var orderCheck = string.Join("", temp.Where(x => index++ < temp.Length - 1 && x <= temp[index]));
                if (orderCheck == temp.Remove(temp.Length - 1) && temp.Select(x => Convert.ToInt32(x) - 48).Sum() == sumDigits)
                {
                    nums.Add(i);
                }
            }
            if (nums.Count == 0)
            {
                return new List<long>();
            }
            return new List<long> { nums.Count, nums[0], nums.Last() };
        }
        public static string Encode(string text, int shift)
        {
            if (string.IsNullOrEmpty(text) || text.All(x => x == ' '))
            {
                return "";
            }

            string lower = "abcdefghijklmnopqrstuvwxyz";
            string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string answer = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsLetter(text[i]))
                {
                    if (lower.Contains(text[i]))
                    {
                        int index = lower.IndexOf(text[i]) + shift;
                        bool changeCase = false;
                        while (index > 25)
                        {
                            index -= 26;
                            changeCase = true;
                        }
                        while (index < 0)
                        {
                            index += 26;
                            changeCase = true;
                        }
                        answer += changeCase ? char.ToUpper(lower[index]) : lower[index];
                    }
                    else
                    {
                        int index = upper.IndexOf(text[i]) + shift;
                        bool changeCase = false;
                        while (index > 25)
                        {
                            index -= 26;
                            changeCase = true;
                        }
                        while (index < 0)
                        {
                            index += 26;
                            changeCase = true;
                        }
                        answer += changeCase ? char.ToLower(upper[index]) : upper[index];
                    }
                }
                else
                {
                    answer += text[i];
                }
            }
            return answer;
        }
        public static int[] TakeWhile(int[] arr, Func<int, bool> pred)
        {
            List<int> take = new List<int>();
            for (int i = 0; i < arr.Length; i++)
            {
                if (pred.Equals(true))
                {
                    if (arr[i] % 2 == 0)
                    {
                        take.Add(arr[i]);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (arr[i] % 2 != 0)
                    {
                        take.Add(arr[i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return take.ToArray();
        }
        public static string Encode(string str, string key)
        {
            string word = "";
            for (int i  = 0; i < str.Length; i++)
            {
                if (key.Contains(char.ToLower(str[i])))
                {
                    word += char.IsLower(str[i]) ? key.IndexOf(str[i]) % 2 == 0 ? key[key.IndexOf(str[i]) + 1] : key[key.IndexOf(str[i]) - 1] :
                                                   key.IndexOf(char.ToLower(str[i])) % 2 == 0 ? char.ToUpper(key[key.IndexOf(char.ToLower(str[i])) + 1]) : char.ToUpper(key[key.IndexOf(char.ToLower(str[i])) - 1]);
                }
                else
                {
                    word += str[i];
                }
            }
            return word;
        }
        public static List<string> Sorts(List<string> unsortedTitles)
        {
            if (unsortedTitles == null || unsortedTitles.Count == 0)
            {
                return unsortedTitles;
            }
            Dictionary<string, string> altered = new Dictionary<string, string>();
            for (int i = 0; i < unsortedTitles.Count; i++)
            {
                if (unsortedTitles[i] != "A " || unsortedTitles[i] != "An " || unsortedTitles[i] != "The ")
                {
                    var temp = unsortedTitles[i].Split(" ").ToList();
                    if (temp.Count > 2)
                    {
                        if (temp[0] == "A" || temp[0] == "An" || temp[0] == "The")
                        {
                            temp[temp.Count - 1] = $"{temp[temp.Count - 1]},";
                            string first = temp[0];
                            temp.RemoveAt(0);
                            temp.Add(first);
                            altered.Add(string.Join(" ", temp), unsortedTitles[i]);
                            unsortedTitles[i] = string.Join(" ", temp);
                        }
                    }
                }
            }
            return unsortedTitles.OrderBy(x => x).Select(x => altered.ContainsKey(x) ? altered[x]: x).ToList();
        }
        public static string Revamp(string s)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            return string.Join(" ", s.Split(" ").Select(x => x.ToCharArray().OrderBy(y => y))
                          .Select(x => new { Word = string.Join("", x), Asc = x.Select(y => alpha.IndexOf(y) + 97).Sum() })
                          .OrderBy(x => x.Asc).ThenBy(x => x.Word.Length).ThenBy(x => x.Word)
                          .Select(x => x.Word));
        }
        public static string PeteTalk(string speech, params string[] ok)
        {
            bool newSentence = false;
            var passWords = ok.Length > 1 ? ok.Where(x => !x.Contains(' ')).Select(x => x.ToLower()).ToList() : new List<string>();
            string edit = "";
            var split = speech.Split(" ");

            for (int i = 0; i < split.Length; i++)
            {
                string word = split[i].ToLower();
                string punctuation = "";

                while (!char.IsLetter(word[word.Length - 1]))
                {
                    punctuation = punctuation.Insert(0, word[word.Length - 1].ToString());
                    word = word.Remove(word.Length - 1);
                }

                if (word.Length >= 3)
                {
                    if (passWords.Count > 0 && passWords.Contains(word))
                    {
                        //ok list
                    }
                    else
                    {
                        int index = 0;
                        word = string.Join("", word.Select(x => index++ == 0 || index == word.Length ? x : '*'));
                    }
                }

                if (i == 0 || newSentence)
                {
                    word = $"{char.ToUpper(word[0])}{word.Substring(1)}";
                }

                if (punctuation.Length > 0)
                {
                    word += punctuation;
                    
                    if (punctuation[punctuation.Length - 1] == '.' || punctuation[punctuation.Length - 1] == '!' || punctuation[punctuation.Length - 1] == '?')
                    {
                        newSentence = true;
                    }
                    else
                    {
                        newSentence = false;
                    }
                }
                else
                {
                    newSentence = false;
                }

                edit += $"{word} ";
            }
            return edit.Trim();
        }
        public static int Exec(List<int> data)
        {
            int great = 0;
            var distinct = data.Distinct().ToList();
            for (int i = 0; i < distinct.Count; i++)
            {
                if (data.Where(x => x == distinct[i]).Count() >= 2)
                {
                    int index = 0;
                    var iValues = data.Select(x => index++ < data.Count && x == distinct[i] ? $"{index - 1}" : "").Where(x => x != "").Select(x => Convert.ToInt32(x)).ToList();
                    int temp = iValues[iValues.Count - 1] - iValues[0];
                    if (temp > great)
                    {
                        great = temp;
                    }
                }
            }
            return great;
        }
        public static string toexuto(string text)
        {
            Dictionary<string, string> alpha = new Dictionary<string, string>
            {{"a", "bcd"}, {"e", "fgh" }, {"i", "jklmn" }, {"o", "pqrst"}, {"u", "vwxyz"}};

            return string.Join("", text.Select(x => !char.IsLetter(x) ? x.ToString() : !alpha.ContainsKey(char.ToLower(x).ToString()) ? $"{x}{alpha.Where(y => y.Value.Contains(char.ToLower(x))).First().Key}" : $"{x}"));

        }
        public static Tuple<int, int> MineLocation(int[,] field)
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i,j] == 1)
                    {
                        return new Tuple<int, int> (i, j);
                    }
                }
            }
            return new Tuple<int, int>(0, 0);
        }
        public static string Switcher(string[] x)
        {
            string key = "zyxwvutsrqponmlkjihgfedcba!? ";
            return string.Join("", x.Select(x => key[Convert.ToInt32(x) - 1]));
        }
        public string StolenLunch(string note)
        {
            var test = string.Join("", note.Select(x => char.IsNumber(x) ?
                                     x == '0' ? 'a' :
                                     x == '1' ? 'b' :
                                     x == '2' ? 'c' :
                                     x == '3' ? 'd' :
                                     x == '4' ? 'e' :
                                     x == '5' ? 'f' :
                                     x == '6' ? 'g' :
                                     x == '7' ? 'h' :
                                     x == '8' ? 'i' :
                                     'j' : x));
            return "";
        }
        public static int AdditionWithoutCarrying(int a, int b)
        {
            string one = string.Join("", a.ToString().ToCharArray().Reverse());
            string two = string.Join("", b.ToString().ToCharArray().Reverse());
            string sum = "";
            int length = one.Length >= two.Length ? one.Length : two.Length;
            for (int i = 0; i < length; i++)
            {
                int temp = 0;
                if (one.Length - 1 >= i)
                {
                    temp += Convert.ToInt32(one[i]) - 48;
                }
                else
                {
                    temp += 0;
                }
                if (two.Length - 1 >= i)
                {
                    temp += Convert.ToInt32(two[i]) - 48;
                }
                else
                {
                    temp += 0;
                }
                sum += temp.ToString().Last();
            }
            return Convert.ToInt32(string.Join("", sum.ToCharArray().Reverse()));
        }
        public static string Biggest(int[] nums)
        {
            var n = nums.Select(x => x.ToString()).OrderByDescending(x => x);
            return "";
        }
        public static int? ToSeconds(string time)
        {
            var t = time.Split(":");
            int? final = 0;
            if (t.Length == 3)
            {
                for (int i = 0; i < t.Length; i++)
                {
                    if (t[i].Length == 2 & char.IsDigit(t[i][0]) && char.IsDigit(t[i][1]))
                    {
                        int temp = Convert.ToInt32(t[i]);
                        if (i == 0 && temp <= 99)
                        {
                            final += temp * 60 * 60;
                        }
                        else if (i == 1 && temp <= 59)
                        {
                            final += temp * 60;
                        }
                        else if (i == 2 && temp <= 59)
                        {
                            final += temp;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return final;
        }
        public static bool IsNice(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int plus = arr[i] + 1;
                int minus = arr[i] - 1;
                if (arr.Contains(plus) || arr.Contains(minus))
                {
                    //gretat
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static long IpsBetween(string start, string end)
        {
            //start - end
            //if 0, then nothing
            //if negative, then add the absolute value
            //if positve, then 255 - positive value willl be added to running total
            long difference = 0;
            var s = start.Split('.').Select(x => Convert.ToInt32(x)).ToArray();
            var e = end.Split('.').Select(x => Convert.ToInt32(x)).ToArray();
            bool additional = false;
            for (int i = 0; i < s.Length; i++)
            {
                long temp = s[i] - e[i];
                if (temp < 0)
                {
                    difference += Math.Abs(temp);
                    additional = true;
                }
                else if (temp > 0)
                {
                    difference += 255 - temp;
                    additional = true;
                }
                if (temp == 0 && additional)
                {
                    difference += 255;
                }
            }
            return difference;
        }
        public static int[] MergeArrays1(int[] a, int[] b)
        {
            var singlesA = a.GroupBy(x => x).Select(x => new { Value = x.Key, Count = x.Count() }).ToList();
            var singlesB = b.GroupBy(x => x).Select(x => new { Value = x.Key, Count = x.Count() }).ToList();

            var shedA = singlesA.Where(x => !b.Contains(x.Value) || x.Count == (b.Contains(x.Value) ? singlesB.Where(y => y.Value == x.Value).Select(y => y.Count).FirstOrDefault() : 0)).Select(x => x.Value);
            var shedB = singlesB.Where(x => !a.Contains(x.Value) || x.Count == (a.Contains(x.Value) ? singlesA.Where(y => y.Value == x.Value).Select(y => y.Count).FirstOrDefault() : 0)).Select(x => x.Value);

            return shedA.Union(shedB).OrderBy(x => x).ToArray();
            /*if (a.Length == 0 || b.Length == 0)
            {
                return new int[0];
            }
            List<int> merged = new List<int>();
            var singlesA = a.GroupBy(x => x).Select(x => new { Value = x.Key, Count = x.Count() }).ToList();
            var singlesB = b.GroupBy(x => x).Select(x => new { Value = x.Key, Count = x.Count() }).ToList();
            for (int i = 0; i < singlesA.Count; i++)
            {
                if (!b.Contains(singlesA[i].Value))
                {
                    if (merged.Count == 0 || !merged.Contains(singlesA[i].Value))
                    {
                        merged.Add(singlesA[i].Value);
                    }
                }
                else
                {
                    if (singlesA[i].Count == singlesB.Where(x => x.Value == singlesA[i].Value).Select(x => x.Count).FirstOrDefault())
                    {
                        if (merged.Count == 0 || !merged.Contains(singlesA[i].Value))
                        {
                            merged.Add(singlesA[i].Value);
                        }
                    }
                }
            }
            for (int i = 0; i < singlesB.Count; i++)
            {
                if (!a.Contains(singlesB[i].Value))
                {
                    if (merged.Count == 0 || !merged.Contains(singlesB[i].Value))
                    {
                        merged.Add(singlesB[i].Value);
                    }
                }
                else
                {
                    if (singlesB[i].Count == singlesA.Where(x => x.Value == singlesB[i].Value).Select(x => x.Count).FirstOrDefault())
                    {
                        if (merged.Count == 0 || !merged.Contains(singlesB[i].Value))
                        {
                            merged.Add(singlesB[i].Value);
                        }
                    }
                }
            }
            merged.Sort();
            return merged.ToArray();*/
        }
        public static string[] FindDuplicatePhoneNumbers(string[] PhoneNumbers)
        {
            Dictionary<string, string> nums = new Dictionary<string, string> {
                {"2", "ABC"}, {"3", "DEF"}, {"4", "GHI"}, {"5", "JKL"},
                {"6", "MNO"}, {"7", "PRS"}, {"8", "TUV"}, {"9", "WXY"} };
            Dictionary<string, int> phone = new Dictionary<string, int>();
            for (int i = 0; i < PhoneNumbers.Length; i++)
            {
                string temp = PhoneNumbers[i].ToUpper();
                if (temp.Contains("-"))
                {
                    temp = temp.Replace("-", "");
                }
                temp = string.Join("", temp.Select(x => char.IsLetter(x) ? nums.Where(y => y.Value.Contains(x)).Select(y => y.Key).First() : x.ToString())).Insert(3, "-");
                if (phone.Where(x => x.Key == temp).Count() == 0)
                {
                    phone.Add(temp, 0);
                }
                phone[temp] += 1;
            }
            return phone.Where(x => x.Value > 1).Select(x => $"{x.Key}:{x.Value}").OrderBy(x => x).ToArray();

        }
        public static long MinValue(int[] a)
        {
            return Convert.ToInt64(string.Join("", a.Distinct().OrderBy(x => x)));
        }
        public static int Solution(string roman)
        {
            Dictionary<string, int> numbers = new Dictionary<string, int>
            { {"I", 1}, {"V", 5}, {"X", 10}, {"L", 50}, {"C", 100}, {"D", 500}, {"M", 1000} };
            var transform = roman.Select(x => numbers[x.ToString()]).ToArray();
            int finalVal = 0;
            for (int i = 0; i < transform.Length; i++)
            {
                if (i != transform.Length - 1 && transform[i] < transform[i + 1])
                {
                    finalVal -= transform[i];
                }
                else
                {
                    finalVal += transform[i];
                }
            }
            return finalVal;
        }
        public static int TrailingZeros(int n)
        {
            int zeros = (int)Math.Floor((double)(n / 5));
            if (n >= 25)
            {
                zeros += (int)Math.Floor((double)(n / 25));
            }
            return zeros;
        }
        public static int bouncingBall(double h, double bounce, double window)
        {
            if (h <= 0 || bounce <= 0 || bounce > 1 || window > h)
            {
                return -1;
            }
            int seen = 1;
            double height = h * bounce;
            while (height > window)
            {
                height *= bounce;
                seen += 2;
            }
            return seen;
        }
        public static string stat(string strg)
        {
            if (string.IsNullOrEmpty(strg))
            {
                return strg;
            }
            var times = strg.Split(",");
            List<int> seconds = new List<int>();
            for (int i = 0; i < times.Length; i++)
            {
                var mts = times[i].Split("|");
                seconds.Add((Convert.ToInt32(mts[0]) * 3600) + (Convert.ToInt32(mts[1]) * 60) + Convert.ToInt32(mts[2]));
            }
            seconds.Sort();

            //Range
            int range = seconds.Max() - seconds.Min();
            int rangeHour = range / 3600;
            range = range % 3600;
            int rangeMinute = range / 60;
            int rangeSecond = range % 60;

            //Average
            int average = seconds.Sum() / seconds.Count();
            int averageHour = average / 3600;
            average = average % 3600;
            int averageMinute = average / 60;
            int averageSecond = average % 60;

            //Median
            int mid = seconds.Count() / 2;
            int median = 0;
            if (seconds.Count() % 2 == 0)
            {
                median = (seconds[mid - 1] + seconds[mid]) / 2;
            }
            else
            {
                median = seconds[(int)Math.Floor((double)mid)];
            }
            int medianHour = median / 3600;
            median = median % 3600;
            int medianMinute = median / 60;
            int medianSecond = median % 60;

            return $"Range: {(rangeHour < 10 ? "0" : "")}{rangeHour}|{(rangeMinute < 10 ? "0" : "")}{rangeMinute}|{(rangeSecond < 10 ? "0" : "")}{rangeSecond} " +
                   $"Average: {(averageHour < 10 ? "0" : "")}{averageHour}|{(averageMinute < 10 ? "0" : "")}{averageMinute}|{(averageSecond < 10 ? "0" : "")}{averageSecond} " +
                   $"Median: {(medianHour < 10 ? "0" : "")}{medianHour}|{(medianMinute < 10 ? "0" : "")}{medianMinute}|{(medianSecond < 10 ? "0" : "")}{medianSecond}";
        }
        public static int[][] MatrixAddition(int[][] a, int[][] b)
        {
            List<List<int>> matrix = new List<List<int>>();
            for (int i = 0; i < a.Length; i++)
            {
                List<int> temp = new List<int>();
                for (int j = 0; j < a[i].Length; j++)
                {
                    temp.Add(a[i][j] + b[i][j]);
                }
                matrix.Add(temp);
            }
            return matrix.Select(x => x.ToArray()).ToArray();
        }
        public static int NumberOfCarries(int a, int b)
        {
            string aS = string.Join("", a.ToString().ToCharArray().Reverse());
            string bS = string.Join("", b.ToString().ToCharArray().Reverse());
            int difference = Math.Abs(aS.Length - bS.Length);
            if (difference > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('0', difference);
                if (aS.Length < bS.Length)
                {
                    aS += sb;
                }
                else
                {
                    bS += sb;
                }
            }
            int carries = 0;
            int extra = 0;
            for (int i = 0; i < aS.Length; i++)
            {
                if ((Convert.ToInt32(aS[i]) - 48) + (Convert.ToInt32(bS[i]) - 48) + extra > 9)
                {
                    carries += 1;
                    extra = 1;
                }
                else
                {
                    extra = 0;
                }
            }
            return carries;

        }
        public static long SequenceSum(long start, long end, long step)
        {
            if (step < 0)
            {
                long sumNeg = start;
                for (long i = start + step; i >= end; i += step)
                {
                    sumNeg += i;
                }
                return sumNeg;

            }
            if (start > end)
            {
                return 0;
            }
            long sum = start;
            for (long i = start + step; i <= end; i += step)
            {
                sum += i;
            }
            return sum;
        }
        public static List<int> searchText(string text, string pattern, bool behind)
        {
            List<int> indexes = new List<int>();
            if (pattern.Length == 0)
            {
                return indexes;
            }
            if (text.Contains(pattern))
            {
                if (behind)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        int temp = text.IndexOf(pattern);
                        if (temp != -1)
                        {
                            indexes.Add(temp);
                            StringBuilder sb = new StringBuilder();
                            sb.Append('0', pattern.Length);
                            text = text.Remove(temp, pattern.Length);
                            text = text.Insert(temp, sb.ToString());
                            i = -1;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 1; i++)
                    {
                        int temp = text.IndexOf(pattern);
                        if (temp != -1)
                        {
                            indexes.Add(temp);
                            text = text.Remove(temp, 1);
                            text = text.Insert(temp, "0");
                            i = -1;
                        }
                    }
                }
            }
            return indexes;
        }
        public static Int32 Calc(String s)
        {
            if (s.Length == 0)
            {
                return 0;
            }
            string symbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456abcdefghijklmnopqrstuvwxyz";
            string total1 = string.Join("", s.Select(x => (symbols.IndexOf(x) + 65).ToString()));
            string total2 = total1.Replace('7', '1');
            return Convert.ToString(BigInteger.Parse(total1) - BigInteger.Parse(total2)).Where(x => x == '6').Count() * 6;
        }
        public static string[] CapMe(string[] strings)
        {
            return strings.Select(x => x.ToLower()).Select(x => $"{x[0].ToString().ToUpper()}{x.Substring(1)}").ToArray();
        }
        public static int SnakesAndLadders(int[] board, int[] dice)
        {
            int lastIndex = 0;
            for (int i = 0; i < dice.Length; i++)
            {
                int index = lastIndex + dice[i];
                if (board.Length > index)
                {
                    lastIndex = index;
                    if (board[index] != 0)
                    {
                        lastIndex += board[index];
                    }
                    if (lastIndex == board.Length - 1)
                    {
                        break;
                    }
                }
            }
            return lastIndex;
        }
        public static int GetLastDigit(BigInteger n1, BigInteger n2)
        {
            BigInteger num = BigInteger.ModPow(n1, n2, 10);
            BigInteger finalNum = num + 10;
            return Convert.ToInt32(finalNum.ToString().Last()) - 48;
        }
        public static string Solution1(int n)
        {
            string nums = n.ToString();
            string roman = "";
            if (nums.Length == 4)
            {
                int tempNums = Convert.ToInt32(nums[0]) - 48;
                StringBuilder sb = new StringBuilder();
                roman += sb.Append('M', tempNums);
                nums = nums.Remove(0, 1);
            }
            if (nums.Length == 3)
            {
                int tempNums = Convert.ToInt32(nums[0]) - 48;
                if (tempNums == 9)
                {
                    roman += "CM";
                }
                else if (tempNums == 4)
                {
                    roman += "CD";
                }
                else
                {
                    if (tempNums >= 5)
                    {
                        roman += "D";
                        tempNums -= 5;
                    }
                    if (tempNums > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        roman += sb.Append('C', tempNums);
                    }
                }
                nums = nums.Remove(0, 1);
            }
            if (nums.Length == 2)
            {
                int tempNums = Convert.ToInt32(nums[0]) - 48;
                if (tempNums == 9)
                {
                    roman += "XC";
                }
                else if (tempNums == 4)
                {
                    roman += "XL";
                }
                else
                {
                    if (tempNums >= 5)
                    {
                        roman += "L";
                        tempNums -= 5;
                    }
                    if (tempNums > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        roman += sb.Append('X', tempNums);
                    }
                }
                nums = nums.Remove(0, 1);
            }
            if (nums.Length == 1)
            {
                int tempNums = Convert.ToInt32(nums[0]) - 48;
                if (tempNums == 9)
                {
                    roman += "IX";
                }
                else if (tempNums == 4)
                {
                    roman += "IV";
                }
                else
                {
                    if (tempNums >= 5)
                    {
                        roman += "V";
                        tempNums -= 5;
                    }
                    if (tempNums > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        roman += sb.Append('I', tempNums);
                    }
                }
            }
            return roman;
        }
        public static int CountZeros(int n)
        {
            int start = n % 2 == 0 ? 2 : 1;
            BigInteger multiply = start;
            for (int i = start + 2; i <= n; i += 2)
            {
                multiply *= (BigInteger)i;
            }
            var zeros = multiply.ToString().ToCharArray().Reverse().ToArray();
            if (zeros[0] != '0')
            {
                return 0;
            }
            int trailingZeros = 0;
            for (int i = 0; i < zeros.Length; i++)
            {
                if (zeros[i] == '0')
                {
                    trailingZeros++;
                }
                else
                {
                    break;
                }
            }
            return trailingZeros;
        }
        public static string BuildPalindrome(string str)
        {
            string palindrome = string.Join("", str.ToCharArray().Reverse());
            string compare = string.Join("", palindrome.ToCharArray().Reverse());
            int indexAdd = 0;

            while(palindrome != compare)
            {
                palindrome = palindrome.Insert(indexAdd, str[indexAdd].ToString());
                compare = string.Join("", palindrome.ToCharArray().Reverse());
                indexAdd++;
            }
            return palindrome;
        }
        public static int SquareDigitsSequence(int a0)
        {
            List<double> log = new List<double> { a0 };
            for (int i = 0; i < 1; i++)
            {
                var squared = log.Last().ToString().Select(x => Math.Pow(Convert.ToInt32(x) - 48, 2)).Sum();
                if (!log.Contains(squared))
                {
                    log.Add(squared);
                    i = -1;
                }
            }
            return log.Count + 1;
        }
        public static int[] ShakeTree(string[] tree)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('0', tree[0].Length);
            int[] nuts = sb.ToString().Select(x => Convert.ToInt32(x) - 48).ToArray();
            for (int i = 0; i < tree[0].Length; i++)
            {
                if (tree[0][i] == 'o')
                {
                    int row = 1;
                    int column = i;

                    for (int j = 0; j < 1; j++)
                    {
                        if (tree[row][column] == '\\')
                        {
                            column++;
                        }
                        else if (tree[row][column] == '/')
                        {
                            column--;
                        }
                        else
                        {
                            if (tree[row][column] != '_' && row != tree.Length - 1)
                            {
                                row++;
                            }
                        }
                        if (tree[row][column] != '_' && row != tree.Length - 1)
                        {
                            j = -1;
                        }
                        if (row == tree.Length - 1)
                        {
                            nuts[column]++;
                        }
                    }
                }
            }
            return nuts;
        }
        public static long[] OddRow(int n)
        {
            n = n - 1;
            long numberOfnumbersPast = (((long)n + 1) * (long)n) / 2;
            StringBuilder sb = new StringBuilder();
            sb.Append('.', n + 1);
            return sb.ToString().Select(x => (long)((2 * (numberOfnumbersPast++ + 1)) - 1)).ToArray();
        }
        public static string Draw(int[] waves)
        {
            List<string> symbols = new List<string>();
            for (int i = 0; i < waves.Length; i++)
            {
                int solid = waves[i];
                for (int j = 0; j < waves.Max(); j++)
                {
                    if (solid > 0)
                    {
                        if (i == 0)
                        {
                            symbols.Add("■");
                        }
                        else
                        {
                            symbols[j] += "■";
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            symbols.Add("□");
                        }
                        else
                        {
                            symbols[j] += "□";
                        }
                    }
                    solid--;
                }
            }
            symbols.Reverse();
            return string.Join("\n", symbols);
        }
        public static double CalculateHypotenuse(double a, double b)
        {
            // TODO: complete calculateHypotenuse so that it returns the hypotenuse length
            // for a triangle with sides of length a, b, and c, where c is the hypotenuse.
            // The solution should verify that inputs are valid numbers (both above zero).
            if (a > 0 && b > 0)
            {
                return Math.Round(Math.Sqrt((a * a) + (b * b)), 3);
            }
            throw new ArgumentException();
        }
        public static int Distance(string a, string b)
        {
            int index = 0;
            return a.Where(x => x != b[index++]).Count();
        }
        public static int[] nbMonths(int startPriceOld, int startPriceNew, int savingPerMonth, double percentLossByMonth)
        {
            double spendable = startPriceOld - startPriceNew;
            int months = 0;
            double spo = startPriceOld;
            double spn = startPriceNew;
            while (spendable < 0)
            {
                months++;
                if (months != 0 && months % 2 == 0)
                {
                    percentLossByMonth += 0.5;
                }
                spo = spo - (spo * (percentLossByMonth * .01));
                spn = spn - (spn * (percentLossByMonth * .01));

                spendable = (spo + (savingPerMonth * months)) - spn;
            }
            return new int[] { months, (int)Math.Round(spendable)};
        }
        public static string CalculateWinners(string snapshot, string[] penguins)
        {
            var lanes = snapshot.Split("\n").Where(x => x.Contains('P') || x.Contains('p')).ToList();
            Dictionary<string, int> placement = new Dictionary<string, int>();
            for (int i = 0; i < lanes.Count; i++)
            {
                int time = 0;
                var lastStretch = lanes[i].Substring(lanes[i].Contains('P') ? lanes[i].IndexOf('P') + 1 : lanes[i].IndexOf('p') + 1);
                for (int j = 0; j < lastStretch.Length; j++)
                {
                    if (lastStretch[j] == '-')
                    {
                        time += 1;
                    }
                    if (lastStretch[j] == '~')
                    {
                        time += 2;
                    }
                }
                placement.Add(penguins[i], time);
            }
            List<string> prize = new List<string> { "GOLD: ", "SILVER: ", "BRONZE: " };
            int index = 0;
            return string.Join(", ", placement.OrderBy(x => x.Value).Take(3).Select(x => $"{prize[index++]}{x.Key}"));
        }
        public static string SendMessage(string message)
        {
            Dictionary<string, string> buttons = new Dictionary<string, string>();
            buttons.Add("1", ".,?!");
            buttons.Add("2", "abc");
            buttons.Add("3", "def");
            buttons.Add("4", "ghi");
            buttons.Add("5", "jkl");
            buttons.Add("6", "mno");
            buttons.Add("7", "pqrs");
            buttons.Add("8", "tuv");
            buttons.Add("9", "wxyz");
            buttons.Add("*", "'-+=");
            string send = "";
            bool capsOn = false;
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == ' ')
                {
                    if (send.Length > 0 && send.Last() == '0')
                    {
                        send += " ";
                    }
                    send += "0";
                }
                else if (char.IsNumber(message[i]) || message[i] == '*' || message[i] == '#')
                {
                    if (send.Length > 0 && send.Last() == message[i])
                    {
                        send += " ";
                    }
                    send += $"{message[i]}-";
                }
                else
                {
                    var charToAdd = buttons.Where(x => x.Value.Contains(char.ToLower(message[i]))).Select(x => x.Key).First();
                    int amountToAdd = buttons[charToAdd].IndexOf(char.ToLower(message[i])) + 1;

                    if (char.IsLetter(message[i]))
                    {
                        if (char.IsUpper(message[i]) && !capsOn)
                        {
                            send += "#";
                            capsOn = true;
                        }
                        else if (char.IsLower(message[i]) && capsOn)
                        {
                            send += "#";
                            capsOn = false;
                        }
                    }
                    if (send.Length > 0 && send.Last() == Convert.ToChar(charToAdd))
                    {
                        send += " ";
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Convert.ToChar(charToAdd), amountToAdd);
                    send += sb;
                }
            }
            return send;
        }
        public static int[] Encode(string[] cards)
        {
            Dictionary<int, string> codes = new Dictionary<int, string>();
            codes.Add(0, "Ac");
            codes.Add(1, "2c");
            codes.Add(2, "3c");
            codes.Add(3, "4c");
            codes.Add(4, "5c");
            codes.Add(5, "6c");
            codes.Add(6, "7c");
            codes.Add(7, "8c");
            codes.Add(8, "9c");
            codes.Add(9, "Tc");
            codes.Add(10, "Jc");
            codes.Add(11, "Qc");
            codes.Add(12, "Kc");
            codes.Add(13, "Ad");
            codes.Add(14, "2d");
            codes.Add(15, "3d");
            codes.Add(16, "4d");
            codes.Add(17, "5d");
            codes.Add(18, "6d");
            codes.Add(19, "7d");
            codes.Add(20, "8d");
            codes.Add(21, "9d");
            codes.Add(22, "Td");
            codes.Add(23, "Jd");
            codes.Add(24, "Qd");
            codes.Add(25, "Kd");
            codes.Add(26, "Ah");
            codes.Add(27, "2h");
            codes.Add(28, "3h");
            codes.Add(29, "4h");
            codes.Add(30, "5h");
            codes.Add(31, "6h");
            codes.Add(32, "7h");
            codes.Add(33, "8h");
            codes.Add(34, "9h");
            codes.Add(35, "Th");
            codes.Add(36, "Jh");
            codes.Add(37, "Qh");
            codes.Add(38, "Kh");
            codes.Add(39, "As");
            codes.Add(40, "2s");
            codes.Add(41, "3s");
            codes.Add(42, "4s");
            codes.Add(43, "5s");
            codes.Add(44, "6s");
            codes.Add(45, "7s");
            codes.Add(46, "8s");
            codes.Add(47, "9s");
            codes.Add(48, "Ts");
            codes.Add(49, "Js");
            codes.Add(50, "Qs");
            codes.Add(51, "Ks");
            List<int> encoded = new List<int>();
            for (int i = 0; i < cards.Length; i++)
            {
                encoded.Add(codes.Where(x => x.Value == cards[i]).Select(x => x.Key).First());
            }
            return encoded.OrderBy(x => x).ToArray();
        }

        public static string[] Decode(int[] cards)
        {
            Dictionary<int, string> codes = new Dictionary<int, string>();
            codes.Add(0, "Ac");
            codes.Add(1, "2c");
            codes.Add(2, "3c");
            codes.Add(3, "4c");
            codes.Add(4, "5c");
            codes.Add(5, "6c");
            codes.Add(6, "7c");
            codes.Add(7, "8c");
            codes.Add(8, "9c");
            codes.Add(9, "Tc");
            codes.Add(10, "Jc");
            codes.Add(11, "Qc");
            codes.Add(12, "Kc");
            codes.Add(13, "Ad");
            codes.Add(14, "2d");
            codes.Add(15, "3d");
            codes.Add(16, "4d");
            codes.Add(17, "5d");
            codes.Add(18, "6d");
            codes.Add(19, "7d");
            codes.Add(20, "8d");
            codes.Add(21, "9d");
            codes.Add(22, "Td");
            codes.Add(23, "Jd");
            codes.Add(24, "Qd");
            codes.Add(25, "Kd");
            codes.Add(26, "Ah");
            codes.Add(27, "2h");
            codes.Add(28, "3h");
            codes.Add(29, "4h");
            codes.Add(30, "5h");
            codes.Add(31, "6h");
            codes.Add(32, "7h");
            codes.Add(33, "8h");
            codes.Add(34, "9h");
            codes.Add(35, "Th");
            codes.Add(36, "Jh");
            codes.Add(37, "Qh");
            codes.Add(38, "Kh");
            codes.Add(39, "As");
            codes.Add(40, "2s");
            codes.Add(41, "3s");
            codes.Add(42, "4s");
            codes.Add(43, "5s");
            codes.Add(44, "6s");
            codes.Add(45, "7s");
            codes.Add(46, "8s");
            codes.Add(47, "9s");
            codes.Add(48, "Ts");
            codes.Add(49, "Js");
            codes.Add(50, "Qs");
            codes.Add(51, "Ks");
            List<string> decoded = new List<string>();
            Array.Sort(cards);
            for (int i = 0; i < cards.Length; i++)
            {
                decoded.Add(codes[cards[i]]);
            }
            return decoded.ToArray();
        }
        public static void WaveSort(int[] arr)
        {
            var largest = arr.OrderByDescending(x => x).ToList();
            List<int> wave = new List<int>();
            int begin = 0;
            int end = largest.Count - 1;
            bool isOdd = largest.Count % 2 == 0;
            for (int i = 0; i < Math.Floor((double)largest.Count / 2); i++)
            {
                wave.Add(largest[begin]);
                wave.Add(largest[end]);
                begin++;
                end--;
            }
            if (isOdd)
            {
                wave.Add(largest[begin]);
            }
            arr = wave.ToArray();
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
        }
        public static string MakePassword(int len, bool flagUpper, bool flagLower, bool flagDigit)
        {
            string lowerAlpha = "abcdefghijklmnopqrstuvwxyz";
            string upperAlpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numeric = "0123456789";
            string password = "";

            //all upper alpha: flagDigit false and flagLower false
            if (!flagDigit && !flagLower)
            {
                return  upperAlpha.Substring(0, len);
            }
            //all lower alpha: flagDigit false and flagUpper false
            if (!flagDigit && !flagUpper)
            {
                return lowerAlpha.Substring(0, len);
            }
            //all numeric: flagUpper & flagLower false
            if (!flagUpper && !flagLower)
            {
                return numeric.Substring(0, len);
            }
            //combo
            //add one of each required type
            int temp = len;
            if (flagLower)
            {
                password += lowerAlpha[0];
                temp -= 1;
            }
            if (flagUpper)
            {
                password += upperAlpha[0];
                temp -= 1;
            }
            if (flagDigit)
            {
                password += numeric[0];
                temp -= 1;
            }
            //fill in to the required length with appropriate type
            if (temp > 0 && flagLower)
            {
                if (temp > 25)
                {
                    password += lowerAlpha.Substring(1);
                    temp -= 25;
                }
                else
                {
                    password += lowerAlpha.Substring(1, temp);
                    temp -= temp;
                }
            }
            if (temp > 0 && flagUpper)
            {
                if (temp > 25)
                {
                    password += upperAlpha.Substring(1);
                    temp -= 25;
                }
                else
                {
                    password += upperAlpha.Substring(1, temp);
                    temp -= temp;
                }
            }
            if (temp > 0)
            {
                password += numeric.Substring(1, temp);
            }
            return password;
        }
        public static string ReplaceDashesAsOne(string str)
        {
            string one = "";
            bool addDash = true;
            for (int i = 0; i < str.Length; i++)
            {
                if (addDash)
                {
                    if (str[i] == '-')
                    {
                        addDash = false;
                    }
                    one += str[i];
                }
                if (!addDash && str[i] != ' ' && str[i] != '-')
                {
                    if (str[i - 1] == ' ')
                    {
                        one += ' ';
                    }
                    one += str[i];
                    addDash = true;
                }
                if (i == str.Length - 1 && !addDash && str[i] == ' ')
                {
                    one += str[i];
                }
            }
            return one;
        }
        public static string[] GetCard()
        {
            List<int> numBank = new List<int>();
            Random random = new Random();
            while (numBank.Count < 5)
            {
                int bingo = random.Next(1, 15);
                if (!numBank.Contains(bingo))
                {
                    numBank.Add(bingo);
                }
            }
            while (numBank.Count < 10)
            {
                int bingo = random.Next(16, 30);
                if (!numBank.Contains(bingo))
                {
                    numBank.Add(bingo);
                }
            }
            while (numBank.Count < 14)
            {
                int bingo = random.Next(31, 45);
                if (!numBank.Contains(bingo))
                {
                    numBank.Add(bingo);
                }
            }
            while (numBank.Count < 19)
            {
                int bingo = random.Next(46, 60);
                if (!numBank.Contains(bingo))
                {
                    numBank.Add(bingo);
                }
            }
            while (numBank.Count < 24)
            {
                int bingo = random.Next(61, 75);
                if (!numBank.Contains(bingo))
                {
                    numBank.Add(bingo);
                }
            }
            return numBank.Select(x => x <= 15 ? $"B{x}" : x <= 30 ? $"I{x}" : x <= 45 ? $"N{x}" : x <= 60 ? $"G{x}" : $"O{x}").ToArray();
        }
        public static int FourSeven(int num)
        {
            string number = num.ToString();
            string fourToMark = number.Replace("4", "!");
            string sevenToFour = fourToMark.Replace("7", "4");
            string markToSeven = sevenToFour.Replace("!", "7");
            var allElse = markToSeven.Where(x => x != '7' && x != '4').ToList();
            for (int i = 0; i < allElse.Count; i++)
            {
                markToSeven = markToSeven.Replace(allElse[i], '0');
            }
            return Convert.ToInt32(markToSeven);
        }
        public static string reverseAndCombineText(string text)
        {
            if (!text.Contains(" "))
            {
                return text;
            }
            var textArray = text.Split().ToList();
            string combo = "";
            for (int i = 0; i < textArray.Count; i += 2)
            {
                combo += string.Join("", textArray[i].ToCharArray().Reverse());
                if (i != textArray.Count - 1)
                {
                    combo += $"{string.Join("", textArray[i + 1].ToCharArray().Reverse())}";
                    if (i != textArray.Count - 2)
                    {
                        combo += " ";
                    }
                }
                if (i == textArray.Count - 2 && textArray.Count % 2 == 0 && combo.Contains(" "))
                {
                    textArray.Clear();
                    textArray = combo.Split().ToList();
                    combo = "";
                    i = -2;
                }
                if (i == textArray.Count - 1 && textArray.Count % 2 != 0 && combo.Contains(" "))
                {
                    textArray.Clear();
                    textArray = combo.Split().ToList();
                    combo = "";
                    i = -2;
                }
            }
            return combo;
            
        }
        public static string Transform(string s)
        {
            string trans = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (!trans.Contains(s[i]))
                {
                    int count = s.Count(x => x == s[i]);
                    trans += s[i];
                    if (count > 1)
                    {
                        trans += count.ToString();
                    }
                }
            }
            return trans;
        }
        public static string ReplaceAll(string input, string find, string replace)
        {
            if (replace.Length > 0 && input.Length == 0 && find.Length == 0)
            {
                return replace;
            }
            string check = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (find == "")
                {
                    check += replace;
                    check += input[i];
                }
                else
                {
                    check += input[i];
                    if (check.Contains(find))
                    {
                        check = check.Insert(check.IndexOf(find), replace);
                        check = check.Remove(check.IndexOf(find), find.Length);
                    }
                }
            }
            return check;
        }
        public static string Hello4(string name)
        {
            if (name.Length == 0 || name == null)
            {
                return "Hello, World!";
            }
            return $"Hello, {char.ToUpper(name[0])}{name.Remove(0, 1).ToLower()}!";
        }
        public static List<string> StringsInMaxDepth(string s)
        {
            if (s.Length == 0 || !s.Contains('(') || !s.Contains('('))
            {
                return new List<string> { s };
            }
            int level = 0;
            string last = "";
            //index, level (for all open and closes)
            Dictionary<int, int> places = new Dictionary<int, int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    if (last.Length == 0)
                    {
                        last += s[i];
                        level++;
                        places.Add(i, level);
                    }
                    else
                    {
                        if (last == s[i].ToString())
                        {
                            level++;
                            places.Add(i, level);
                        }
                        else
                        {
                            last = s[i].ToString();
                            places.Add(i, level);
                        }
                    }
                }
                else if (s[i] == ')')
                {
                    if (last.Length == 0)
                    {
                        last += s[i];
                        level--;
                        places.Add(i, level);
                    }
                    else
                    {
                        if (last == s[i].ToString())
                        {
                            level--;
                            places.Add(i, level);
                        }
                        else
                        {
                            last = s[i].ToString();
                            places.Add(i, level);
                        }
                    }
                }
            }
            //find the highest value (level) in the dictionary
            var highestLevel = places.Max(x => x.Value);
            //then grab all indexes that have 'highestLevel'
            var indexes = places.Where(x => x.Value == highestLevel).Select(x => x.Key).ToList() ;
            //find the chars in the original string between each index pair from 'indexes'
            List<string> highestValues = new List<string>();
            for (int i = 0; i < indexes.Count; i += 2)
            {
                highestValues.Add(s.Substring(indexes[i] + 1, indexes[i + 1] - indexes[i] - 1));
            }
            return highestValues;
        }
        public static bool ValidateString(string[] dictionary, string word)
        {
            string newWord = word;
            for (int i = 0; i < dictionary.Length; i++)
            {
                if (newWord.Contains(dictionary[i]))
                {
                    newWord = newWord.Remove(newWord.IndexOf(dictionary[i]), dictionary[i].Length);
                }
                if (newWord.Length == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static string ButtonSequences(string seqR, string seqB)
        {
            string sequence = "";
            bool newColor = false;
            for (int i = 0; i < seqR.Length; i++)
            {
                if (seqR[i] == seqB[i] && seqR[i] == '1')
                {
                    if (i == 0)
                    {
                        sequence += 'R';
                        newColor = true;
                    }
                    else if (seqR[i - 1] == '0' && seqB[i - 1] == '0')
                    {
                        sequence += 'R';
                        newColor = true;
                    }
                }

                else if (seqR[i] != seqB[i])
                {
                    if (seqR[i] == '0' && seqB[i] == '1')
                    {
                        sequence += 'B';
                        newColor = true;
                    }
                    else
                    {
                        sequence += 'R';
                        newColor = true;
                    }
                }

                if (newColor)
                {
                    if (sequence.Last() == 'R' && i != seqR.Length - 1 && seqR[i + 1] == '1')
                    {
                        var nextZeroR = seqR.Substring(i).IndexOf('0');
                        if (nextZeroR != -1)
                        {
                            i += seqR.Substring(i).IndexOf('0') - 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (sequence.Last() == 'B' && i != seqB.Length - 1 && seqB[i + 1] == '1')
                    {
                        var nextZeroB = seqB.Substring(i).IndexOf('0');
                        if (nextZeroB != -1)
                        {
                            i += seqB.Substring(i).IndexOf('0') - 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    newColor = false;
                }
            }
            return sequence;
            /*string sequence = "";
            bool defaultR = false;
            for (int i = 0; i < seqR.Length; i++)
            {
                if (seqR[i] == seqB[i] && seqR[i] == '1')
                {
                    if (i == 0)
                    {
                        sequence += 'R';
                        defaultR = true;
                    }
                    else if (seqR[i - 1] == '0' && seqB[i - 1] == '0')
                    {
                        sequence += 'R';
                        defaultR = true;
                    }
                }
                else if (seqR[i] != seqB[i])
                {
                    if (seqR[i] == '0' && seqB[i] == '1')
                    {
                        if (sequence.Length == 0)
                        {
                            sequence += 'B';
                        }
                        else if (seqB[i - 1] != '1')
                        {
                            sequence += 'B';
                            defaultR = false;
                        }
                        else if (defaultR)
                        {
                            sequence += 'B';
                            defaultR = false;
                        }
                    }
                    else
                    {
                        if (sequence.Length == 0)
                        {
                            sequence += 'R';
                            defaultR = false;
                        }
                        else if (seqR[i - 1] != '1')
                        {
                            sequence += 'R';
                            defaultR = false;
                        }
                        else if (seqR[i - 1] == '1' && seqB[i - 1] == '1')
                        {
                            sequence += 'R';
                            defaultR = false;
                        }
                    }
                }
            }
            return sequence;*/
        }
        public static double[] xbonacci(double[] signature, int n)
        {
            if (signature.Length >= n)
            {
                return signature.Take(n).ToArray();
            }
            var nacci = signature.ToList();
            double currentSum = nacci.Sum();
            int indexToDrop = 0;
            for (int i = 0; i < n - signature.Length; i++)
            {
                if (i > 0)
                {
                    currentSum = (currentSum - nacci[indexToDrop]) + nacci.Last();
                    indexToDrop++;
                }
                nacci.Add(currentSum);
            }
            return nacci.ToArray();
        }
        public static int MinDistance(int n)
        {
            List<int> factors = new List<int>();
            for (int i = 1; i < n; i++)
            {
                if (factors.Count > 0 && i >= factors.Last())
                {
                    break;
                }
                if (n % i == 0 && !factors.Contains(i) && !factors.Contains(n / i))
                {
                    if (i == n / i)
                    {
                        factors.Add(i);
                    }
                    else
                    {
                        factors.Add(i);
                        factors.Add(n / i);
                    }
                    /*if (!factors.Contains(i) && !factors.Contains(n / i))
                    {
                        factors.Add(i);
                        factors.Add(n / i);
                    }
                    else
                    {

                    }*/
                }
            }
            factors.Sort();
            int nextIndex = 0;
            return factors.Select(x => ++nextIndex < factors.Count ? factors[nextIndex] - x : 0).Take(factors.Count - 1).Min();
            
        }
        public static int[] MinMinMax(int[] array)
        {
            int min = array.Min();
            int max = array.Max();
            int minAb = min;
            while (minAb < max)
            {
                minAb++;
                if (!array.Contains(minAb))
                {
                    break;
                }
            }
            return new int[] { min, minAb, max};
        }
        public static string Abbreviate(string input)
        {
            string output = "";
            var words = input.Split(" ");
            for (int i = 0; i < words.Length; i++)
            {
                string temp = "";
                string add = "";
                for (int j = 0; j < words[i].Length; j++)
                {
                    if (char.IsLetter(words[i][j]))
                    {
                        temp += words[i][j];
                    }
                    else
                    {
                        if (temp.Length >= 4)
                        {
                            add += $"{temp[0]}{temp.Length - 2}{temp[temp.Length - 1]}{words[i][j]}";
                            temp = "";
                        }
                        else
                        {
                            add += $"{temp}{words[i][j]}";
                            temp = "";
                        }
                    }
                    if (j == words[i].Length - 1 && temp.Length > 0)
                    {
                        if (temp.Length >= 4)
                        {
                            add += $"{temp[0]}{temp.Length - 2}{temp[temp.Length - 1]}";
                        }
                        else
                        {
                            add += temp;
                        }
                    }
                }
                output += $"{add} ";
            }
            return output.Trim();
        }
        public static string ProcessEvents(string events)
        {
            string process = "";
            int position = 0;
            bool up = true;
            bool pause = true;

            for (int i = 0; i < events.Length; i++)
            {
                if (events[i] == '.')
                {
                    if (position == 0 || position == 5)
                    {
                        process += $"{position}";
                    }
                    else if (pause)
                    {
                        process += $"{position}";
                    }
                    else if (up)
                    {
                        position++;
                        process += $"{position}";
                    }
                    else
                    {
                        position--;
                        process += $"{position}";
                    }
                }
                if (events[i] == 'P')
                {
                    if (pause)
                    {
                        pause = false;
                        //now proceed
                    }
                    else
                    {
                        if (position == 5)
                        {
                            up = false;
                        }
                        else if (position == 0)
                        {
                            up = true;
                        }
                        else
                        {
                            pause = true;
                        }
                        //now pause actions
                    }
                    if (pause)
                    {
                        process += $"{position}";
                    }
                    else
                    {
                        if (up)
                        {
                            if (position < 5)
                            {
                                position++;
                            }
                            process += $"{position}";
                        }
                        else
                        {
                            if (position > 0)
                            {
                                position--;
                            }
                            process += $"{position}";
                        }
                    }
                }
                if (events[i] == 'O')
                {
                    if (up)
                    {
                        up = false;
                        //down
                    }
                    else
                    {
                        up = true;
                        //up
                    }
                    if (!pause)
                    {
                        if (up)
                        {
                            if (position < 5)
                            {
                                position++;
                            }
                            process += $"{position}";
                        }
                        else
                        {
                            if (position > 0)
                            {
                                position--;
                            }
                            process += $"{position}";
                        }
                    }
                }
            }
            return process;
        }
        public static List<int> Solve(List<string> arr)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            List<int> matches = new List<int>();
            for (int i = 0; i < arr.Count; i++)
            {
                int index = 0;
                matches.Add(arr[i].ToLower().Where(x => index++ == alpha.IndexOf(x)).Count());
            }
            return matches;
        }
        public static string Encodes(string input)
        {
            string encoded = "";
            string character = input[0].ToString();
            int count = 1;

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i].ToString() != character)
                {
                    encoded += $"{count}{character}";
                    character = input[i].ToString();
                    count = 1;
                }
                else
                {
                    count++;
                }
            }
            encoded += $"{count}{character}";
            return encoded;
        }

        public static string Decodes(string input)
        {
            string decoded = "";
            string num = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    num += input[i].ToString();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(input[i], Convert.ToInt32(num));
                    decoded += sb;
                    num = "";
                }
            }
            return decoded;
        }
        public static string BrailleReader(string[] braille)
        {
            Dictionary<string, string> characters = new Dictionary<string, string>();
            characters.Add("0     ", "a");
            characters.Add("00    ", "b");
            characters.Add("0  0  ", "c");
            characters.Add("0  00 ", "d");
            characters.Add("0   0 ", "e");
            characters.Add("00 0  ", "f");
            characters.Add("00 00 ", "g");
            characters.Add("00  0 ", "h");
            characters.Add(" 0 0  ", "i");
            characters.Add(" 0 00 ", "j");
            characters.Add("0 0   ", "k");
            characters.Add("000   ", "l");
            characters.Add("0 00  ", "m");
            characters.Add("0 000 ", "n");
            characters.Add("0 0 0 ", "o");
            characters.Add("0000  ", "p");
            characters.Add("00000 ", "q");
            characters.Add("000 0 ", "r");
            characters.Add(" 000  ", "s");
            characters.Add(" 0000 ", "t");
            characters.Add("0 0  0", "u");
            characters.Add("000  0", "v");
            characters.Add(" 0 000", "w");
            characters.Add("0 00 0", "x");
            characters.Add("0 0000", "y");
            characters.Add("0 0 00", "z");
            characters.Add("      ", " ");

            string message = "";
            string temp = "";

            for (int i = 0; i < braille[0].Length; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    temp += braille[j][i];
                }

                if (temp.Length == 6)
                {
                    message += characters[temp];
                    temp = "";
                }
            }
            return message;
        }
        public static int PaC(string str)
        {
            string parsed = "";
            if (str.Contains("number"))
            {
                parsed = str.Remove(0, str.IndexOf("number") + 7);
            }
            else if (str.Contains("answer"))
            {
                parsed = str.Remove(0, str.IndexOf("answer") + 7);
            }
            if (parsed.Contains("+"))
            {
                var nums = parsed.Split("+");
                return Convert.ToInt32(nums[0].Trim()) + Convert.ToInt32(nums[1].Trim());
            }
            if (parsed.Contains("-"))
            {
                var nums = parsed.Split("-");
                return Convert.ToInt32(nums[0].Trim()) - Convert.ToInt32(nums[1].Trim());
            }
            return Convert.ToInt32(parsed.Trim());
        }
        public static int[] MergeArrays(int[] a, int[] b)
        {
            var multiplesA = a.Where(x => a.Where(y => y == x).Count() > 1 && b.Contains(x) && a.Where(y => y == x).Count() != b.Where(y => y == x).Count());
            //a has a count of the item more than once, b also contains the item, a's count of the item does not equal b's count of the item
            var multiplesB = b.Where(x => b.Where(y => y == x).Count() > 1 && a.Contains(x) && b.Where(y => y == x).Count() != a.Where(y => y == x).Count());

            var removeMultiples = multiplesA.Union(multiplesB);
            //merge the items to remove from the final merge, removing all duplicates

            var merged = a.Union(b).Where(x => !removeMultiples.Contains(x)).OrderBy(x => x);
            //merge a and b, removing a duplicates, where the items are not in the multiples to be removed

            return merged.ToArray();

        }
        public static string ModifyMultiply(string str, int loc, int num)
        {
            var divide = str.Split(" ");
            string mod = "";
            for (int i = 0; i < num; i++)
            {
                if (i != num - 1)
                {
                    mod += $"{divide[loc]}-";
                }
                else
                {
                    mod += divide[loc];
                }
            }
            return mod;
        }
        public static List<string> OldLadySwallows(List<string> animals)
        {
            if (animals.Contains("fly") && animals.Contains("spider"))
            {
                animals.RemoveAll(item => item == "fly");
            }
            if (animals.Contains("spider") && animals.Contains("bird"))
            {
                animals.RemoveAll(item => item == "spider");
            }
            if (animals.Contains("bird") && animals.Contains("cat"))
            {
                animals.RemoveAll(item => item == "bird");
            }
            if (animals.Contains("cat") && animals.Contains("dog"))
            {
                animals.RemoveAll(item => item == "cat");
            }
            if (animals.Contains("dog") && animals.Contains("goat"))
            {
                animals.RemoveAll(item => item == "dog");
            }
            if (animals.Contains("goat") && animals.Contains("cow"))
            {
                animals.RemoveAll(item => item == "goat");
            }
            if (animals.Contains("cow") && animals.Contains("horse"))
            {
                animals.RemoveAll(item => item == "cow");
            }
            return animals;
            /*List<string> remaining = new List<string>();
            List<string> order = new List<string> { "fly", "spider", "bird", "cat", "dog", "goat", "cow", "horse" }; 
            int eaten = 0;
            for (int i = 0; i < animals.Count; i++)
            {
                if (animals[i] != "horse")
                {
                    if (i != animals.Count - 1 && animals[i + 1] == order[order.IndexOf(animals[i]) + 1])
                    {
                        eaten++;
                    }
                    else if (remaining.Count != 0 && remaining.Last() == order[order.IndexOf(animals[i]) + 1])
                    {
                        eaten++;
                    }
                    else
                    {
                        remaining.Add(animals[i]);
                    }
                }
                else
                {
                    remaining.Add(animals[i]);
                }
                
                if (i == animals.Count - 1 && eaten != 0 && remaining.Count > 1)
                {
                    eaten = 0;
                    i = - 1;
                    animals = remaining;
                    remaining = new List<string>();
                }
                
            }
            return remaining;*/
        }
        public static object[] Alternate(int n, object firstValue, object secondValue)
        {
            if (n == 0)
            {
                return new object[0];
            }
            List<object> alt = new List<object>();
            for (int i = 0; i < n; i++)
            {
                if (i % 2 == 0)
                {
                    alt.Add(firstValue);
                }
                else
                {
                    alt.Add(secondValue);
                }
            }
            return alt.ToArray();
        }
        public static bool HasSurvived(int[] attackers, int[] defenders)
        {
            if (attackers.Length == 0)
            {
                return true;
            }
            if (defenders.Length == 0)
            {
                return false;
            }
            int att = 0;
            int def = 0;
            int loopLength = attackers.Length >= defenders.Length ? attackers.Length : defenders.Length;
            bool attackEqualOrLarger = attackers.Length == loopLength;
            for (int i = 0; i < loopLength; i++)
            {
                if (attackEqualOrLarger)
                {
                    if (i <= defenders.Length - 1)
                    {
                        if (defenders[i] > attackers[i])
                        {
                            def++;
                        }
                        else
                        {
                            att++;
                        }
                    }
                    else
                    {
                        att++;
                    }
                }
                else
                {
                    if (i <= attackers.Length - 1)
                    {
                        if (defenders[i] > attackers[i])
                        {
                            def++;
                        }
                        else
                        {
                            att++;
                        }
                    }
                    else
                    {
                        def++;
                    }
                }
            }
            if (def > att)
            {
                return true;
            }
            if (def == att)
            {
                if (attackers.Sum() <= defenders.Sum())
                {
                    return true;
                }
            }
            return false;
        }
        public static string Correct(string timeString)
        {
            if (string.IsNullOrEmpty(timeString))
            {
                return timeString;
            }
            if (timeString.Where(x => char.IsLetter(x)).Count() > 0)
            {
                return null;
            }
            var timeSegments = timeString.Split(":");
            if (timeSegments.Length != 3)
            {
                return null;
            }
            int seconds = Convert.ToInt32(timeSegments[2]);
            int minutes = Convert.ToInt32(timeSegments[1]);
            int hours = Convert.ToInt32(timeSegments[0]);
            if (seconds > 59)
            {
                minutes += (int)Math.Floor((double)seconds / 60);
                seconds = seconds % 60;
                
            }
            if (minutes > 59)
            {
                hours += (int)Math.Floor((double)minutes / 60);
                minutes = minutes % 60;
                
            }
            if (hours > 23)
            {
                hours = hours % 24;
            }
            timeSegments[2] = seconds < 10 ? $"0{seconds}" : $"{seconds}";
            timeSegments[1] = minutes < 10 ? $"0{minutes}" : $"{minutes}";
            timeSegments[0] = hours < 10 ? $"0{hours}" : $"{hours}";
            return string.Join(":", timeSegments);
        }
        class FileMaster
        {
            public string File { get; set; }

            public FileMaster(string filepath)
            {
                File = filepath;
            }
            public string extension()
            {
                return File.Remove(0, File.IndexOf(".") + 1);
            }
            public string filename()
            {
                int index = 0;
                int lastSlashIndex = Convert.ToInt32(File.Select(x => index++ < File.Length - 1 && x == '/' ? $"{index - 1}" : "").Where(x => x != "").Last());
                string temp = File.Remove(0, lastSlashIndex + 1);
                return temp.Remove(temp.IndexOf("."));
            }
            public string dirpath()
            {
                int index = 0;
                int lastSlashIndex = Convert.ToInt32(File.Select(x => index++ < File.Length - 1 && x == '/' ? $"{index - 1}" : "").Where(x => x != "").Last());
                return File.Remove(lastSlashIndex + 1);
            }
        }
        public static long Mormons(long startingNumber, long reach, long target)
        {
            if (startingNumber >= target)
            {
                return 0;
            }
            long current = startingNumber;
            long missions = 0;
            for (int i = 0; i < 1; i++)
            {
                missions++;

                current = current + (current * reach);

                if (current < target)
                {
                    i = -1;
                }
            }
            return missions;
        }
        public static string LookAndSaySequence(string firstElement, long n)
        {
            
            if (n == 1)
            {
                return firstElement;
            }
            string element = ""; ;
            int count = 0;
            string temp = "";

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < firstElement.Length; j++)
                {
                    if (j != firstElement.Length - 1)
                    {
                        if (firstElement[j] == firstElement[j + 1])
                        {
                            if (firstElement[j].ToString() == element)
                            {
                                if (firstElement[j].ToString() != element)
                                {
                                    element = firstElement[j].ToString();
                                }
                                count++;
                            }
                            else
                            {
                                if (element != "")
                                {
                                    temp += $"{count}{element}";
                                }
                                element = firstElement[j].ToString();
                                count = 2;
                            }
                        }
                        else
                        {
                            if (firstElement[j].ToString() == element)
                            {
                                temp += $"{count}{element}";
                            }
                            else
                            {
                                if (element != "")
                                {
                                    temp += $"{count}{element}";
                                }
                                element = firstElement[j].ToString();
                                count = 1;
                            }
                        }
                    }
                    else
                    {
                        if (element != "" && firstElement[j].ToString() != element)
                        {
                            temp += $"{count}{element}";
                        }
                        if (firstElement[j].ToString() != element)
                        {
                            element = firstElement[j].ToString();
                            count = 1;
                        }
                        temp += $"{count}{element}";
                    }
                }
                firstElement = temp;
                element = "";
                count = 0;
                temp = "";
            }
            return firstElement;
        }
        public static int SumNoDuplicates(int[] arr)
        {
            return arr.Where(x => arr.Where(y => y == x).Count() == 1).Sum();
        }
        public static string DecipherThis(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var splitS = s.Split(" ");

            string alpha = "abcdefghijklmnopqrstuvwxyz";

            List<string> joinS = new List<string>();

            for (int i = 0; i < splitS.Length; i++)
            {
                string temp = Convert.ToInt32(string.Join("", splitS[i].Where(x => char.IsNumber(x)))) <= 90 ?
                    char.ToUpper(alpha[Convert.ToInt32(string.Join("", splitS[i].Where(x => char.IsNumber(x)))) - 65]).ToString() :
                    alpha[Convert.ToInt32(string.Join("", splitS[i].Where(x => char.IsNumber(x)))) - 97].ToString();

                var remaining = splitS[i].Where(x => char.IsLetter(x)).ToList();

                if (remaining.Count >= 2)
                {
                    remaining.Insert(0, remaining.Last());
                    remaining.RemoveAt(remaining.Count - 1);
                    remaining.Add(remaining[1]);
                    remaining.RemoveAt(1);
                }

                if (remaining.Count > 0)
                {
                    temp += string.Join("", remaining);
                }
                joinS.Add(temp);
            }

            return string.Join(" ", joinS);

            /*
            for (int i = 0; i < splitS.Length; i++)
            {
                string temp = char.IsUpper(splitS[i][0]) ?
                    $"{alpha.IndexOf(char.ToLower(splitS[i][0])) + 65}" :
                    $"{alpha.IndexOf(splitS[i][0]) + 97}";

                if (splitS[i].Length <= 2)
                {
                    string remaining = splitS[i].Remove(0, 1);

                    if (splitS[i].Length > 2)
                    {
                        remaining = remaining.Insert(0, remaining.Last().ToString());
                        remaining = remaining.Remove(remaining.Length - 1); 
                        remaining += remaining[1]; 
                        remaining = remaining.Remove(1, 1);
                    }
                    temp += remaining;
                }
                joinS.Add(temp);
            }
            return string.Join(" ", joinS);*/
        }
        public static string last_survivors(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < 1; i++)
            {
                var grouped = str.GroupBy(x => x).Select(x => new { Value = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).ToList();
                string add = "";
                if (grouped.First().Count >= 2)
                {
                    var overTwo = grouped.Where(x => x.Count >= 2).ToList();
                    for (int j = 0; j < overTwo.Count; j++)
                    {
                        int doubles = (int)Math.Floor(Convert.ToDouble(overTwo[j].Count) / 2);
                        int remaining = overTwo[j].Count - 2;

                        char nextLetter = overTwo[j].Value == 'z' ? 'a' : alpha[alpha.IndexOf(overTwo[j].Value) + 1];
                        if (str.Contains(nextLetter))
                        {
                            add += nextLetter;
                        }
                        else
                        {
                            grouped.Add(new { Value = nextLetter, Count = 1 });
                        }

                        StringBuilder sb2 = new StringBuilder();
                        sb2.Append(overTwo[j].Value, remaining);
                        add += sb2;
                    }
                    string temp = "";
                    for (int k = 0; k < grouped.Count; k++)
                    {
                        if (grouped[k].Count < 2)
                        {
                            StringBuilder sb3 = new StringBuilder();
                            sb3.Append(grouped[k].Value, grouped[k].Count);
                            temp += sb3;
                        }
                    }
                    str = "";
                    str = add.Length > 0 ? $"{temp}{add}" : temp;
                    i = -1;
                }
            }
            return str;
        }
        public static object[] CountSel(int[] lst)
        {
            int elements = lst.Length;
            int different = lst.Distinct().Count();
            int unique = lst.Where(x => lst.Where(y => y == x).Count() == 1).Count();
            var frequency = lst.GroupBy(x => x).Select(x => new { Value = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).ThenBy(x => x.Value);
            var highest = frequency.Where(x => frequency.First().Count == x.Count).Select(x => x.Value).ToArray();
            return new object[] { elements, different, unique, new object[] { highest, frequency.First().Count } };
        }
        public static string InsertMissingLetters(string str)
        {
            string alphaAll = "abcdefghijklmnopqrstuvwxyz";
            string alphaIncluded = string.Join("", alphaAll.Where(x => !str.Contains(x))); 
            string letters = "";
            for (int i = 0; i < str.Length; i++)
            {
                letters += str[i];
                string doublCheck = str.Remove(i);
                if (!doublCheck.Contains(str[i]) && str[i] != 'z')
                {
                    string nextLetter = alphaAll[alphaAll.IndexOf(str[i]) + 1].ToString();
                    if(alphaIncluded.Contains(nextLetter))
                    {
                        letters += alphaIncluded.Remove(0, alphaIncluded.IndexOf(nextLetter)).ToUpper();
                    }
                    
                }
            }
            return letters;
        }
        public static string KaCokadekaMe(string word)
        {
            string kaCode = "";
            var code = word.Split(" ");
            for (int i = 0; i < code.Length; i++)
            {
                string temp = "ka";
                for (int j = 0; j < code[i].Length; j++)
                {
                    temp += code[i][j];
                    if (char.ToLower(code[i][j]) == 'a' || char.ToLower(code[i][j]) == 'e' || char.ToLower(code[i][j]) == 'i' || char.ToLower(code[i][j]) == 'o' || char.ToLower(code[i][j]) == 'u')
                    {
                        if (j != code[i].Length - 1)
                        {
                            if (char.ToLower(code[i][j + 1]) != 'a' && char.ToLower(code[i][j + 1]) != 'e' && char.ToLower(code[i][j + 1]) != 'i' && char.ToLower(code[i][j + 1]) != 'o' && char.ToLower(code[i][j + 1]) != 'u')
                            {
                                temp += "ka";
                            }
                        }
                    }
                }
                kaCode += $"{temp} ";
            }
            return kaCode.Trim();
        }
        public static string Ermahgerd(string text)
        {
            string final = "";
            var t = text.ToUpper().Split(" ");
            for (int i = 0; i < t.Length; i++)
            {
                string temp = "";
                for (int j = 0; j < t[i].Length; j++)
                {
                    if (t[i][j] == 'A' || t[i][j] == 'E' || t[i][j] == 'I' || t[i][j] == 'O' || t[i][j] == 'U')
                    {
                        if (j != t[i].Length - 1)
                        {
                            if (t[i][j + 1] == 'A' || t[i][j + 1] == 'E' || t[i][j + 1] == 'I' || t[i][j + 1] == 'O' || t[i][j + 1] == 'U')
                            {
                                temp += "";
                            }
                            else
                            {
                                temp += "ER";
                            }
                        }
                        else
                        {
                            temp += "ER";
                        }
                    }
                    else if (t[i][j] == 'Y' && j != 0 && t[i][j - 1] == 'M')
                    {
                        temp += "AH";
                    }
                    else if (t[i][j] == 'R' && temp.Length > 0 && temp.Last() == 'R')
                    {
                        temp += "";
                    }
                    else if (t[i][j] == 'H' && j != 0)
                    {
                        if (t[i][j - 1] == 'A' || t[i][j - 1] == 'E' || t[i][j - 1] == 'I' || t[i][j - 1] == 'O' || t[i][j - 1] == 'U')
                        {
                            temp += "";
                        }
                        else
                        {
                            temp += t[i][j];
                        }
                    }
                    else
                    {
                        temp += t[i][j];
                    }
                    if (j == t[i].Length - 1 && temp.Length > 4)
                    {
                        if (t[i][j] == 'A' || t[i][j] == 'E' || t[i][j] == 'I' || t[i][j] == 'O' || t[i][j] == 'U')
                        {
                            temp = temp.Remove(temp.Length - 2);
                        }
                        if (!char.IsLetterOrDigit(t[i][j])) 
                        {
                            string special = "";
                            for (int k = t[i].Length - 1; k > 0; k--)
                            {
                                if (!char.IsLetterOrDigit(t[i][k]))
                                {
                                    special += t[i][k];
                                }
                                else
                                {
                                    k = 0;
                                }
                            }
                            if (temp.Length - special.Length > 4)
                            {
                                if (t[i][j - special.Length] == 'A' || t[i][j - special.Length] == 'E' || t[i][j - special.Length] == 'I' || t[i][j - special.Length] == 'O' || t[i][j - special.Length] == 'U')
                                {
                                    temp = $"{temp.Remove(temp.Length - (special.Length + 2))}{string.Join("", special.ToCharArray().Reverse())}";
                                }
                            }
                        }
                    }
                }
                final += $"{temp} ";
            }
            return final.Trim();
        }
        public static string[] words = new string[] { "ABETTOR", "ABETTORS", "ABILITIES", "ABILITY" };
        public static string[] LongestWord(string letters)
        {
            if (string.IsNullOrEmpty(letters))
            {
                return null;
            }
            var bank = words.Select(x => string.Join("", x.Where(y => letters.Contains(y))).Length).ToList();
            foreach (var item in bank)
            {
                Console.WriteLine(item);
            }
            var largest = bank.OrderByDescending(x => x).First();
            if (largest == 0)
            {
                return null;
            }
            int index = 0;
            var longest = bank.Select(x => x == largest && index++ < bank.Count ? $"{index - 1}" : "").Where(x => x != "").Select(x => words[Convert.ToInt32(x)]).OrderBy(x => x).ToArray();
            foreach (var item in longest)
                {
                    Console.WriteLine(item);
                }
            return longest;
        }
        public static string BabySharkLyrics()
        {
            string song = "";
            List<string> sharks = new List<string> { "Baby shark", "Mommy shark", "Daddy shark", "Grandma shark", "Grandpa shark", "Let's go hunt" };
            for(int i = 0; i < sharks.Count; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    song += $"{sharks[i]}, doo doo doo doo doo doo\n";
                }
                song += $"{sharks[i]}!\n";
            }
            song += "Run away,…";
            return song;
        }
        public static int Solve(List<char> xs, int n)
        {
            var dogsCats = xs.GroupBy(x => x).Select(x => new { Animal = x.Key, AnimalCount = x.Count()});
            var dogCount = dogsCats.Where(x => x.Animal == 'D').Count();
            var catCount = dogsCats.Where(x => x.Animal == 'C').Count();
            if (dogCount > catCount)
            {
                return catCount;
            }
            int cats = 0;
            for (int i = 0; i < xs.Count; i++)
            {
                if (xs[i] == 'D')
                {
                    int count = n;
                    bool backwards = false;
                    for (int j = 0; j < n; j++)
                    {
                        if (i > count - 1 && xs[i - count] == 'C')
                        {
                            xs[i - count] = 'X';
                            cats++;
                            backwards = true;
                            j = n;
                        }
                        count--;
                    }
                    if (!backwards)
                    {
                        count = n;
                        for (int k = 0; k < n; k++)
                        {
                            if (i < xs.Count - count && xs[i + count] == 'C')
                            {
                                xs[i + count] = 'X';
                                cats++;
                                k = n;
                            }
                            count--;
                        }
                    }
                }
            }
            return cats;
        }
        public static string AlphabetWar(string[] reinforces, string[] airstrikes)
        {
            Dictionary<int, List<string>> letters = new Dictionary<int, List<string>>();
            for (int i = 0; i < reinforces.Length; i++)
            {
                for (int j = 0; j < reinforces[i].Length; j++)
                {
                    if(!letters.ContainsKey(j))
                    {
                        letters.Add(j, new List<string>());
                    }
                    letters[j].Add(reinforces[i][j].ToString());
                }
            }

            for (int i = 0; i < airstrikes.Length; i++)
            {
                if (airstrikes[i].Contains('*'))
                {
                    int index = 0;
                    var strikeIndex = airstrikes[i].Select(x => index++ <= airstrikes[i].Length - 1 && x == '*' ? $"{index - 1}" : "")
                    .Where(x => x != "")
                    .Select(x => Convert.ToInt32(x)).ToList();
                    List<int> additionalIndex = new List<int>();
                    
                    for (int j = 0; j < strikeIndex.Count; j++)
                    {
                        if (!strikeIndex.Contains(strikeIndex[j] - 1) && !additionalIndex.Contains(strikeIndex[j] - 1) && letters.ContainsKey(strikeIndex[j] - 1) && letters[strikeIndex[j] - 1].Count > 0)
                        {
                            letters[strikeIndex[j] - 1].RemoveAt(0);
                            additionalIndex.Add(strikeIndex[j] - 1);
                        }
                        if (letters.ContainsKey(strikeIndex[j]) && letters[strikeIndex[j]].Count > 0)
                        {
                            letters[strikeIndex[j]].RemoveAt(0);
                        }
                        if (!strikeIndex.Contains(strikeIndex[j] + 1) && !additionalIndex.Contains(strikeIndex[j] + 1) && letters.ContainsKey(strikeIndex[j] + 1) && letters[strikeIndex[j] + 1].Count > 0)
                        {
                            letters[strikeIndex[j] + 1].RemoveAt(0);
                            additionalIndex.Add(strikeIndex[j] + 1);
                        }
                    }
                }
            }

            return string.Join("", letters.Select(x => x.Value.Count > 0 ? x.Value[0] : "_"));
        }
        public static string GreekL33t(string str)
        {
            string alpha = "abcdefghijklmnopqrstuvwxyz";
            string greek = "αβcδεfghιjκlmηθρqπsτμυωχγz";
            return string.Join("", str.ToLower().Select(x => alpha.Contains(x) ? greek[alpha.IndexOf(x)] : x));
        }
        public static IEnumerable<string> OrderByDomain(IEnumerable<string> source)
        {
            
            var domainOnly = source.Select(x => string.Join("", x.ToCharArray().Reverse()))
                .Select(x => x.Remove(0, x.IndexOf('/') + 1))
                .Select(x => x.Remove(x.IndexOf('.')))
                .Select(x => string.Join("", x.ToCharArray().Reverse()))
                .Select(x => x == "com" ? $"1{x}" : x)
                .Select(x => x == "gov" ? $"2{x}" : x)
                .Select(x => x == "org" ? $"3{x}" : x)
                .ToList();

            var ordered = domainOnly.OrderBy(x => x).Select(x => domainOnly.IndexOf(x));
            var sourceList = source.ToList();
            return ordered.Select(x => sourceList[x]);
        }
        public bool InviteMoreWomen(int[] L)
        {
            if(L.Sum() < 0)
            {
                return true;
            }
            return false;

        }
        public static IEnumerable<string> MyLanguages(Dictionary<string, int> results)
        {
            return results.Where(x => x.Value >= 60).OrderByDescending(x => x.Value).Select(x => x.Key);
        }
        public static bool CheckCoupon(string enteredCode, string correctCode, string currentDate, string expirationDate)
        {
            if (enteredCode == correctCode)
            {
                string[] months = new string[] { "january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december" };
                int month = 0;
                int day = 0;
                int year = 0;
                string temp = "";
                for (int i = 0; i < currentDate.Length; i++)
                {
                    if (month == 0 & char.IsLetter(currentDate[i]))
                    {
                        temp += char.ToLower(currentDate[i]);
                    }
                    if (month == 0 && currentDate[i] == ' ')
                    {
                        month = Array.IndexOf(months, temp) + 1;
                        temp = "";
                    }
                    if (month > 0 && day == 0 && char.IsDigit(currentDate[i]))
                    {
                        temp += currentDate[i];
                    }
                    if (currentDate[i] == ',')
                    {
                        day = Convert.ToInt32(temp);
                        temp = "";
                    }
                    if (day > 0 && char.IsDigit(currentDate[i]))
                    {
                        temp += currentDate[i];
                    }
                    if (i == currentDate.Length - 1)
                    {
                        year = Convert.ToInt32(temp);
                        temp = "";
                    }
                }
                long current = new DateTime(year, month, day).Ticks;

                month = 0;
                day = 0;
                year = 0;
                for (int i = 0; i < expirationDate.Length; i++)
                {
                    if (month == 0 & char.IsLetter(expirationDate[i]))
                    {
                        temp += char.ToLower(expirationDate[i]);
                    }
                    if (month == 0 && expirationDate[i] == ' ')
                    {
                        month = Array.IndexOf(months, temp) + 1;
                        temp = "";
                    }
                    if (month > 0 && day == 0 && char.IsDigit(expirationDate[i]))
                    {
                        temp += expirationDate[i];
                    }
                    if (expirationDate[i] == ',')
                    {
                        day = Convert.ToInt32(temp);
                        temp = "";
                    }
                    if (day > 0 && char.IsDigit(expirationDate[i]))
                    {
                        temp += expirationDate[i];
                    }
                    if (i == expirationDate.Length - 1)
                    {
                        year = Convert.ToInt32(temp);
                        temp = "";
                    }
                }
                long expiration = new DateTime(year, month, day).Ticks;

                if (expiration <= current)
                {
                    return true;
                }
            }
            return false;
        }
        public static string Triangle(string row)
        {
            string temp = "";
            string letters = ".abcdefghijklmnopqrstuvwxyz";
            for(int i = 0; i < row.Length; i++)
            {
                if (i != row.Length - 1)
                {
                    var index = letters.IndexOf(row[i]) + letters.IndexOf(row[i + 1]);
                    temp += index > 26 ? letters[index - 26] : letters[index];
                }
                if(i == row.Length - 1 && temp.Length > 1)
                {
                    i = -1;
                    row = temp;
                    temp = "";
                }
            }
            return string.IsNullOrEmpty(temp) ? row : temp;
        }
        public static string Siegfried(int week, string str)
        {
            if (str.Contains("-"))
            {
                str = str.Replace("-", " - ");
            }
            string lower = string.Join("",str.ToLower().Where(x => x == ' ' || char.IsLetterOrDigit(x)));
            if(week > 0)//week 1
            {
                if (lower.Contains("ch"))
                {
                    lower = lower.Replace("ch", "**");
                }
                if (lower.Contains("ci"))
                {
                    lower = lower.Replace("ci", "si");
                }
                if (lower.Contains("ce"))
                {
                    lower = lower.Replace("ce", "se");
                }
                if (lower.Contains("c"))
                {
                    lower = lower.Replace("c", "k");
                }
                if (lower.Contains("**"))
                {
                    lower = lower.Replace("**", "ch");
                }
            }
            if (week > 1)//week 2
            {
                if (lower.Contains("ph"))
                {
                    lower = lower.Replace("ph", "f*");
                }
            }
            if (week > 2)//week 3
            {
                var trail = lower.Split(" ").Where(x => x.Length > 3 && x[x.Length - 1] == 'e').ToList();
                for (int i = 0; i < trail.Count; i++)
                {
                    lower = lower.Replace(trail[i], $"{trail[i].Remove(trail[i].Length - 1)}{"*"}");
                }

                int index = 0;
                var same = lower.Select(x => index != lower.Length - 1 && x == lower[++index] && char.IsLetter(x) ? $"{index}" : "X").Where(x => x != "X").ToList();
                for (int i = 0; i < same.Count; i++)
                {
                    int temp = Convert.ToInt32(same[i]);
                    lower = lower.Insert(temp, "*");
                    lower = lower.Remove(temp + 1, 1);
                }
            }
            if (week > 3)//week 4
            {
                if (lower.Contains("th"))
                {
                    lower = lower.Replace("th", "z*");
                }
                if (lower.Contains("wr"))
                {
                    lower = lower.Replace("wr", "r*");
                }
                if (lower.Contains("wh"))
                {
                    lower = lower.Replace("wh", "v*");
                }
                if (lower.Contains("w"))
                {
                    lower = lower.Replace("w", "v");
                }
            }
            if (week > 4)//week 5
            {
                if (lower.Contains("ou"))
                {
                    lower = lower.Replace("ou", "u*");
                }
                if (lower.Contains("an"))
                {
                    lower = lower.Replace("an", "un");
                }

                var trailing = lower.Split(" ").Where(x => x.Length > 2 && x[x.Length - 1] == 'g' && x[x.Length - 2] == 'n' && x[x.Length - 3] == 'i').ToList();
                for (int i = 0; i < trailing.Count; i++)
                {
                    lower = lower.Replace(trailing[i], $"{trailing[i].Remove(trailing[i].Length - 3)}{"ink"}");
                }

                int index1 = 0;
                string lowerStr = str.ToLower();
                var frontOriginal = lowerStr.Select(x => index1 != lowerStr.Length - 1 && lowerStr[++index1] == 'm' && x == 's' ? $"{index1}" : "X").Where(x => x != "X").ToList();
                int counter = 0;
                for (int i = 0; i < frontOriginal.Count; i++)
                {
                    int temp = Convert.ToInt32(frontOriginal[i]) + counter;
                    str = str.Insert(temp + 1, "**");
                    counter += 2;
                }
                var front = lower.Split(" ").Where(x => x.Length > 1 && x[0] == 's' && x[1] == 'm').ToList();
                for (int i = 0; i < front.Count; i++)
                {
                    lower = lower.Replace(front[i], $"{"schm"}{front[i].Remove(0, 2)}");
                }
            }
            if(week > 0)//restore case
            {
                for(int i = 0; i < str.Length; i++)
                {
                    if(char.IsLetter(str[i]) && char.IsUpper(str[i]))
                    {
                        lower = lower.Insert(i, char.ToUpper(lower[i]).ToString());
                        lower = lower.Remove(i + 1, 1);
                    }
                    if(str[i] != ' ' && !char.IsLetterOrDigit(str[i]) && str[i] != '*')
                    {
                        lower = lower.Insert(i, str[i].ToString());
                    }
                }
                if (lower.Contains("*"))
                {
                    lower = lower.Replace("*", "");
                }
                if (lower.Contains(" - "))
                {
                    lower = lower.Replace(" - ", "-");
                }
            }
            return lower;
        }
        public static int MostFrequentItemCount(int[] collection)
        {
            return collection.GroupBy(x => x).Select(x => collection.Count(y => y == x.Key)).OrderByDescending(x => x).FirstOrDefault();
            
        }
        public static int CountInversions(int[] array)
        {
            var sorted = array.ToList().OrderBy(x => x).ToList();
            int counter = 0; //used for indexing. reset after array.Length has been reached.
            int inversions = 0; //count for each iteration.
            int perfect = 0; //goal is to be array.Length. make sure all have been checked from beginning. reset each time counter is reset to 0.
            for (int i = 0; i < 1; i++)
            {

                if (counter != array.Length - 1)
                {
                    if (array[counter] != sorted[counter])
                    {
                        if (array[counter] > array[counter + 1])
                        {
                            inversions++;
                            (array[counter], array[counter + 1]) = (array[counter + 1], array[counter]);
                        }
                    }
                    else
                    {
                        perfect++;
                    }
                    counter++;
                }
                else
                {
                    if (counter != perfect)
                    {
                        counter = 0;
                        perfect = 0;
                    }
                }
                if (perfect != array.Length - 1)
                {
                    i = - 1;
                }
            }
            return inversions;
        }
        public static string WhatTimeIsIt(double angle)
        {
            if (angle == 0 || angle == 360)
            {
                return "12:00";
            }
            double hour = Math.Floor(angle / 30) == 0 ? 12 : Math.Floor(angle / 30);
            double minutes = Math.Floor((angle % 30) / .5);
            
            return $"{(hour.ToString().Length > 1 ? "" : "0")}{hour}:{(minutes.ToString().Length > 1 ? "" : "0")}{minutes}";
        }
        public static Tuple<char?, int> LongestRepetition(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new Tuple<char?, int>(null, 0);
            }
            int longest = 0;
            string charater = "";
            int count = 1;
            string currentChar = input[0].ToString();
            for (int i = 0; i < input.Length; i++)
            {
                if(i != input.Length - 1)
                {
                    if(input[i] == input[i + 1])
                    {
                        count++;
                        currentChar = input[i].ToString();
                    }
                    else
                    {
                        if(count > longest)
                        {
                            longest = count;
                            charater = currentChar;
                        }
                        count = 1;
                        currentChar = input[i + 1].ToString();
                    }
                }
            }
            if (count > longest)
            {
                longest = count;
                charater = currentChar;
            }
            if (longest == 1)
            {
                longest = 0;
                charater = null;
            }
            return new Tuple<char?, int>(Convert.ToChar(charater), longest);
        }
        public static int[] Beggars(int[] values, int n)
        {
            if (n == 0)
            {
                return new int[0];
            }
            int[] beggings = new int[n];
            int index = 0;
            for(int i = 0; i < values.Length; i++)
            {
                if(index > n - 1)
                {
                    index = 0;
                }
                beggings[index] += values[i];
                index++;
            }
            return beggings;
        }
        public static string Encrypt(string text, int n)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            for (int i = 0; i < n; i++)
            {
                int oddIndex = 0;
                int evenIndex = 0;
                text = $"{string.Join("", text.Select(x => oddIndex++ % 2 != 0 ? x.ToString() : ""))}{string.Join("", text.Select(x => evenIndex++ % 2 == 0 ? x.ToString() : ""))}";
                Console.WriteLine(text);
            }
            return text;
        }

        public static string Decrypt(string encryptedText, int n)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return encryptedText;
            }
            for (int i = 0; i < n; i++)
            {
                int divide = (int)Math.Floor(Convert.ToDouble(encryptedText.Length) / Convert.ToDouble(2));
                int frontOdds = 0;
                int backEvens = divide;
                string temp = "";
                for(int j = 0; j < divide; j++)
                {
                    temp += encryptedText[backEvens];
                    temp += encryptedText[frontOdds];
                    backEvens++;
                    frontOdds++;
                }
                if(encryptedText.Length % 2 != 0)
                {
                    temp += encryptedText[backEvens];
                }
                encryptedText = temp;
                Console.WriteLine(encryptedText);
            }
            return encryptedText;
        }
        public static int[] DataReverse(int[] data)
        {
            int index = 1;
            var divide = string.Join("", string.Join("", data.Select(x => index++ % 8 == 0 ? $"{x} " : $"{x}")).Trim().Split(" ").Reverse()).ToCharArray().Select(x => Convert.ToInt32(x) - 48).ToArray();
            foreach(var item in divide)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(divide);
            return new int[0];

        }
        public static string Decoded(string morseCode)
        {
            Dictionary<string, string> MorseCode = new Dictionary<string, string>();
            string decoded = "";
            morseCode = morseCode.Trim();
            string temp = "";
            for (int i = 0; i < morseCode.Length; i++)
            {
                if (temp.Length > 2 && temp == "   ")
                {
                    decoded += " ";
                    temp = "";
                }
                if (temp.Length > 0 && temp == " " && morseCode[i] != ' ')
                {
                    temp = "";
                }
                if (i != morseCode.Length - 1 && morseCode[i + 1] == ' ' && morseCode[i] != ' ')
                {
                    temp += morseCode[i];
                    decoded += MorseCode.GetValueOrDefault(temp);
                    temp = "";
                }
                else
                {
                    temp += morseCode[i];
                }
            }
            if (temp.Length > 0)
            {
                decoded += MorseCode.GetValueOrDefault(temp);
            }
            return decoded;
        }
        public static long Thirt(long n)
        {
            var nThirteen = n.ToString();
            int indexThirteen = nThirteen.Length - 1;
            List<int> modNumbers = new List<int> { 1, 10, 9, 12, 3, 4 };
            int indexMod = 0;
            int temp = 0;
            int lastTwoDigit = 0;
            for(int i = 0; i < nThirteen.Length; i++)
            {
                temp += (Convert.ToInt32(nThirteen[indexThirteen]) - 48) * modNumbers[indexMod];

                indexThirteen--;
                if(indexMod == modNumbers.Count - 1)
                {
                    indexMod = 0;
                }
                else
                {
                    indexMod++;
                }

                if(i == nThirteen.Length - 1 && nThirteen.Length >= 2)
                {
                    Console.WriteLine(temp);
                    if(temp.ToString().Length == 2)
                    {
                        if(lastTwoDigit == temp)
                        {
                            break;
                        }
                        else
                        {
                            lastTwoDigit = temp;
                        }
                    }
                    nThirteen = temp.ToString();
                    indexThirteen = nThirteen.Length - 1;
                    indexMod = 0;
                    temp = 0;
                    i = - 1;
                }
            }
            return temp;

        }
        public static int OneTwoThree()
        {
            Random rand = new Random();
            return rand.Next(1, 3);
        }
        public static long Int123(ref int a)
        {
            if (a > 123)
            {
                int b = a;
                a = 123 - a;
                return b;
            }
            return 123 - a;
        }
        public static string FormatTime(int hour)
        {
            string time = hour.ToString();
            if(time.Length > 4 || time.Length < 3)
            {
                throw new ArgumentException();
            }
            if(time.Length == 3)
            {
                time = time.Insert(1, ":");
            }
            else if(time.Length == 4)
            {
                time = time.Insert(2, ":");
            }
            return time;
        }
        public static int[] DigitDifferenceSort(int[] a)
        {
            Dictionary<int, List<int>> differences = new Dictionary<int, List<int>>();
            for (int i = 0; i < a.Length; i++)
            {
                var temp = a[i].ToString().ToCharArray();
                if (temp.Length == 1)
                {
                    differences.Add(i, new List<int>());
                    differences[i].Add(a[i]);
                    differences[i].Add(0);
                }
                else
                {
                    Array.Sort(temp);
                    differences.Add(i, new List<int>());
                    differences[i].Add(a[i]);
                    differences[i].Add(Convert.ToInt32(temp[temp.Length - 1]) - Convert.ToInt32(temp[0]));
                }
            }

            return differences.OrderBy(x => x.Value[1]).ThenByDescending(x => Array.IndexOf(differences.Keys.ToArray(), x.Key)).Select(x => x.Value[0]).ToArray();
        }
        public static string[] StreetFighterSelection(string[][] fighters, int[] position, string[] moves)
        {
            List<string> fightersSelected = new List<string>();

            for(int i = 0; i < moves.Length; i++)
            {
                if (moves[i] == "right")
                {
                    if(position[1] + 1 <= fighters[position[0]].Length - 1)
                    {
                        position[1]++;
                    }
                    else
                    {
                        position[1] = 0;
                    }
                    fightersSelected.Add(fighters[position[0]][position[1]]);
                }
                if (moves[i] == "left")
                {
                    if (position[1] - 1 >= 0)
                    {
                        position[1]--;
                    }
                    else
                    {
                        position[1] = fighters[position[0]].Length - 1;
                    }
                    fightersSelected.Add(fighters[position[0]][position[1]]);
                }
                if (moves[i] == "down")
                {
                    if (position[0] + 1 <= fighters.Length - 1)
                    {
                        position[0]++;
                    }
                    fightersSelected.Add(fighters[position[0]][position[1]]);
                }
                if (moves[i] == "up")
                {
                    if (position[0] - 1 >= 0)
                    {
                        position[0]--;
                    }
                    fightersSelected.Add(fighters[position[0]][position[1]]);
                }
            }
            return fightersSelected.ToArray();
        }
        public static string AlphabetWars(string fight)
        {
            string left = "sbpw";
            string right = "zdqm";

            string outcome = "Let's fight again!";

            if (fight.Any(x => left.Contains(x)) || fight.Any(x => right.Contains(x)))
            {
                string stillStanding = "";

                for (int i = 0; i < fight.Length; i++)
                {
                    if (fight[i] == 't')
                    {
                        if (stillStanding.Length > 0 && right.Contains(stillStanding[stillStanding.Length - 1]))
                        {
                            if (stillStanding.Length >= 2 && stillStanding[stillStanding.Length - 2] == 'j')
                            {
                                //do nothing
                            }
                            else
                            {
                                int indexSwitch = right.IndexOf(stillStanding[stillStanding.Length - 1]);
                                stillStanding = stillStanding.Remove(stillStanding.Length - 1);
                                stillStanding += left[indexSwitch];
                            }
                        }
                        
                        if (i != fight.Length - 1 && right.Contains(fight[i + 1]))
                        {
                            if (i != fight.Length - 2 && fight[i + 2] == 'j')
                            {
                                //do nothing
                            }
                            else
                            {
                                int indexSwitch = right.IndexOf(fight[i + 1]);
                                fight = fight.Remove(i + 1, 1);
                                fight = fight.Insert(i + 1, left[indexSwitch].ToString());
                            }
                        }
                    }
                    if (fight[i] == 'j')
                    {
                        if (stillStanding.Length > 0 && left.Contains(stillStanding[stillStanding.Length - 1]))
                        {
                            if (stillStanding.Length >= 2 && stillStanding[stillStanding.Length - 2] == 't')
                            {
                                //do nothing
                            }
                            else
                            {
                                int indexSwitch = left.IndexOf(stillStanding[stillStanding.Length - 1]);
                                stillStanding = stillStanding.Remove(stillStanding.Length - 1);
                                stillStanding += right[indexSwitch];
                            }
                        }
                        if (i != fight.Length - 1 && left.Contains(fight[i + 1]))
                        {
                            if (i != fight.Length - 2 && fight[i + 2] == 't')
                            {
                                //do nothing
                            }
                            else
                            {
                                int indexSwitch = left.IndexOf(fight[i + 1]);
                                fight = fight.Remove(i + 1, 1);
                                fight = fight.Insert(i + 1, right[indexSwitch].ToString());
                            }
                        }
                    }
                    stillStanding += fight[i];
                }

                int leftScore = stillStanding.Where(x => left.Contains(x)).Select(x => left.IndexOf(x) + 1).Sum();
                int rightScore = stillStanding.Where(x => right.Contains(x)).Select(x => right.IndexOf(x) + 1).Sum();

                outcome = leftScore != rightScore ? leftScore > rightScore ? "Left side wins!" : "Right side wins!" : "Let's fight again!";
            }

            return outcome;
        }
        public static string Search(int budget, int[] prices)
        {
            return string.Join(",", prices.Where(x => x <= budget).OrderBy(x => x));
        }
        public static bool HasUniqueChars(string str)
        {
            var dups = str.Where(x => str.Where(y => y == x).Count() > 1);

            return dups.Count() > 0 ? false : true;
        }
        public static int MinPermutation(int n)
        {
            bool negative = false;

            string min = n.ToString();

            if (min.Length == 1)
            {
                return n;
            }

            if (min[0] == '-')
            {
                negative = true;
                min = min.Remove(0, 1);
            }

            min = string.Join("", min.ToCharArray().OrderBy(x => x));

            if (min[0] == '0')
            {
                int zeros = string.Join("", min.Where(x => x == '0')).Count();
                min = min.Remove(0, zeros);
                StringBuilder sb = new StringBuilder();
                sb.Append('0', zeros);
                min = min.Insert(1, sb.ToString());
            }

            return negative ? Convert.ToInt32(min) * -1 : Convert.ToInt32(min);
        }
        public static int KookaCounter(string laughing)
        {
            if (string.IsNullOrEmpty(laughing))
            {
                return 0;
            }
            int birds = 1;
            var h = laughing.Where(x => x == 'h' || x == 'H').ToArray();
            for(int i = 0; i < h.Length; i++)
            {
                if(i != h.Length - 1)
                {
                    if (h[i] != h[i + 1])
                    {
                        birds++;
                    }
                }
            }
            return birds;
        }
        public static Dictionary<string, List<int>> GetPeaks(int[] arr)
        {
            Dictionary<string, List<int>> pp = new Dictionary<string, List<int>>();
            pp.Add("pos", new List<int>());
            pp.Add("peaks", new List<int>());

            if (arr.Length == 0 || arr == null)
            {
                return pp;
            }

            string platNum = "";
            string platPos = "";
            bool inPlat = false;

            for (int i = 0; i < arr.Length; i++)
            {
                if(i != 0 && i != arr.Length - 1)
                {
                    if(arr[i] == arr[i + 1] && arr[i - 1] < arr[i])
                    {
                        platNum = arr[i].ToString();
                        platPos = i.ToString();
                        inPlat = true;
                    }
                    if(inPlat && arr[i - 1] == arr[i] && arr[i + 1] != arr[i])
                    {
                        if (arr[i + 1] < arr[i])
                        {
                            pp["pos"].Add(Convert.ToInt32(platPos));
                            pp["peaks"].Add(Convert.ToInt32(platNum));
                        }
                        platNum = "";
                        platPos = "";
                        inPlat = false;
                    }

                    if(arr[i - 1] < arr[i] && arr[i + 1] < arr[i])
                    {
                        pp["pos"].Add(i);
                        pp["peaks"].Add(arr[i]);
                    }
                }
            }

            return pp;
        }
        public static string Tops(string msg)
        {
            string top = "";
            int counter = 1;
            int last = 1;
            for (int i = 1; i < msg.Length; i = counter + last)
            {
                top += msg[i];
                counter += 4;
                last = i;
            }
            return string.Join("", top.ToCharArray().Reverse());
        }
        public static string Encode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            if (input.All(x => char.IsNumber(x)))
            {
                return input;
            }
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lastLetter = "";
            string encoded = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]))
                {
                    if (i == 0 || lastLetter.Length == 0)
                    {
                        encoded += char.ToUpper(input[i]);
                    }
                    else
                    {
                        int index = alpha.IndexOf(lastLetter) + alpha.IndexOf(char.ToUpper(input[i])) + 2 >= 27 ?
                            (alpha.IndexOf(lastLetter) + alpha.IndexOf(char.ToUpper(input[i])) + 2) - 26 :
                            alpha.IndexOf(lastLetter) + alpha.IndexOf(char.ToUpper(input[i])) + 2;

                        encoded += alpha[index - 1];
                    }
                    lastLetter = char.ToUpper(input[i]).ToString();
                }
                else
                {
                    encoded += input[i];
                }
            }
            return encoded;
        }
        public static string Decode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            if (input.All(x => char.IsNumber(x)))
            {
                return input;
            }
            string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lastLetter = "";
            string decoded = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]))
                {
                    if (i == 0 || lastLetter.Length == 0)
                    {
                        decoded += char.ToUpper(input[i]);
                        lastLetter = char.ToUpper(input[i]).ToString();
                    }
                    else
                    {
                        int index = (alpha.IndexOf(char.ToUpper(input[i])) + 1) - (alpha.IndexOf(lastLetter) + 1) <= 0 ?
                            ((alpha.IndexOf(char.ToUpper(input[i])) + 1) - (alpha.IndexOf(lastLetter) + 1)) + 26 :
                            (alpha.IndexOf(char.ToUpper(input[i])) + 1) - (alpha.IndexOf(lastLetter) + 1);

                        int index2 = Math.Abs((alpha.IndexOf(char.ToUpper(input[i])) + 1) - (alpha.IndexOf(lastLetter) + 1));

                        decoded += alpha[index - 1];

                        lastLetter = alpha[index - 1].ToString();
                    }
                }
                else
                {
                    decoded += input[i];
                }
            }
            return decoded;
        }
        public static string[] DiagonalsOfSquare(string[] array)
        {
            if (array.Length == 0 || array == null)
            {
                return null;
            }
            int letterCount = array[0].Length;
            if(!array.All(x => x.Length == letterCount))
            {
                return null;
            }
            List<string> diagnals = new List<string>();
            List<string> arrayListed = array.ToList();
            Array.Sort(array);
            List<int> originalOrder = new List<int>();
            List<string> tempArray = array.ToList();
            for(int i = 0; i < arrayListed.Count; i++)
            {
                originalOrder.Add(tempArray.IndexOf(arrayListed[i]));
                tempArray[originalOrder.Last()] = "!";
            }
            arrayListed = array.ToList();
            for (int i = 0; i < arrayListed.Count; i++)
            {
                string temp = "";
                for(int j = 0; j < letterCount; j++)
                {
                    temp += arrayListed[j][j];
                }
                diagnals.Add(temp);
                string hold = arrayListed[0];
                arrayListed.RemoveAt(0);
                arrayListed.Add(hold);
            }
            return originalOrder.Select(x => diagnals[x]).ToArray();
            
        }
        public static int ComputeDepth(int n)
        {
            string nums = "";
            int multiple = 1;
            for (int i = 0; i < 1; i++)
            {
                nums += string.Join("", (n * multiple).ToString().Where(x => !nums.Contains(x)).GroupBy(x => x).Select(x => x.Key));
                if(nums.Length < 10)
                {
                    i = - 1;
                    multiple++;
                }
            }
            return multiple;
        }
        public void SortColors(int[] nums)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = 0; j < nums.Length; j++)
                {
                    if (nums[j] < nums[i])
                    {
                        int temp = nums[i];
                        nums[i] = nums[j];
                        nums[j] = temp;
                    }
                }
            }
        }
        public static string AlphabetWar(string b)
        {
            if (!b.Contains('#'))
            {
                return string.Join("", b.Where(x => char.IsLetter(x)));
            }
            List<string> bombAndShelter = new List<string>();
            bool insideBracket = false;
            string sheltered = "";
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] == '[')
                {
                    insideBracket = true;
                }
                if (b[i] == ']')
                {
                    insideBracket = false;
                    if(sheltered.Length > 0)
                    {
                        bombAndShelter.Add(sheltered);
                        sheltered = "";
                    }
                }
                if (b[i] == '#')
                {
                    bombAndShelter.Add(b[i].ToString());
                }
                if (insideBracket && char.IsLetter(b[i]))
                {
                    sheltered += b[i];
                }
            }
            if (bombAndShelter.Count(x => x == "#") == 1)
            {
                return string.Join("", bombAndShelter.Where(x => x != "#"));
            }
            string survivors = "";
            int frontBombs = 0;
            int backBombs = 0;
            for (int i = 0; i < bombAndShelter.Count; i++)
            {
                if (bombAndShelter[i] != "#")
                {
                    if (i != 0 && bombAndShelter[i - 1] == "#")
                    {
                        frontBombs++;
                        if(i != 1 && bombAndShelter[i - 2] == "#")
                        {
                            frontBombs++;
                        }
                    }
                    if (i != bombAndShelter.Count - 1 && bombAndShelter[i + 1] == "#")
                    {
                        backBombs++;
                        if (i != bombAndShelter.Count - 2 && bombAndShelter[i + 2] == "#")
                        {
                            backBombs++;
                        }
                    }
                    if (frontBombs > 1 || backBombs > 1)
                    {
                        //strike
                    }
                    else if (frontBombs > 0 && backBombs > 0)
                    {
                        //strike
                    }
                    else
                    {
                        survivors += bombAndShelter[i];
                    }
                    frontBombs = 0;
                    backBombs = 0;
                }
            }
            return survivors;
        }
        
    }
}
