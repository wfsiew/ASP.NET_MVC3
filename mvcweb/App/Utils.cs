using System;

namespace mvcweb.App
{
    public class Utils
    {
        public static string GetItemMessage(int total, int pgnum, int pgsize)
        {
            int x = (pgnum - 1) * pgsize + 1;
            int y = pgnum * pgsize;

            if (total < y)
                y = total;

            if (total < 1)
                return "";

            else
                return string.Format("{0:d} to {1:d} of {2:d}", x, y, total);
        }

        public static bool IsNumber(string number)
        {
            bool o = false;

            try
            {
                int val = Convert.ToInt32(number);
                return true;
            }

            catch
            {
                o = true;
            }

            if (o == false)
                return false;

            try
            {
                double val = Convert.ToDouble(number);
                return true;
            }

            catch
            {
            }

            return false;
        }

        public static int GetInt(string s)
        {
            try
            {
                int val = Convert.ToInt32(s);
                return val;
            }

            catch
            {
            }

            return 0;
        }

        public static double GetDouble(string s)
        {
            try
            {
                double val = Convert.ToDouble(s);
                return val;
            }

            catch
            {
            }

            return 0;
        }
    }
}