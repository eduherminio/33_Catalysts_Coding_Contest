using System;
using System.Globalization;
using System.IO;

namespace CatalystContest
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            if (!Directory.Exists("Output"))
            {
                Directory.CreateDirectory("Output");
            }

            SolveLevel1();
            SolveLevel2();
            SolveLevel3();
            SolveLevel4();
            //SolveLevel5();

            Console.WriteLine("***END***");
        }

        private static void SolveLevel1()
        {
            Contest_lvl1 contest = new Contest_lvl1();
            contest.Run("level1_example.in");
            contest.Run("level1_1.in");
            contest.Run("level1_2.in");
            contest.Run("level1_3.in");
            contest.Run("level1_4.in");
            contest.Run("level1_5.in");
        }

        private static void SolveLevel2()
        {
            Contest_lvl2 contest = new Contest_lvl2();
            contest.Run("level2_example.in");
            contest.Run("level2_1.in");
            contest.Run("level2_2.in");
            contest.Run("level2_3.in");
            contest.Run("level2_4.in");
            contest.Run("level2_5.in");
        }

        private static void SolveLevel3()
        {
            Contest_lvl3 contest = new Contest_lvl3();
            contest.Run("level3_example.in");
            contest.Run("level3_1.in");
            contest.Run("level3_2.in");
            contest.Run("level3_3.in");
            contest.Run("level3_4.in");
            contest.Run("level3_5.in");
        }

        private static void SolveLevel4()
        {
            Contest_lvl4 contest = new Contest_lvl4();
            contest.Run("level4_example.in");
            contest.Run("level4_1.in");
            contest.Run("level4_2.in");
            contest.Run("level4_3.in");
            contest.Run("level4_4.in");
            contest.Run("level4_5.in");
        }

        private static void SolveLevel5()
        {
            Contest_lvl5_incomplete contest = new Contest_lvl5_incomplete();
            contest.Run("level5_example.in");
            contest.Run("level5_1.in");
            contest.Run("level5_2.in");
            contest.Run("level5_3.in");
            contest.Run("level5_4.in");
            contest.Run("level5_5.in");
        }
    }
}
