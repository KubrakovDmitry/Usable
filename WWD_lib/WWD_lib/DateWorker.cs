using System;

namespace WWD_lib
{
    public abstract class DateWorker
    {
        // вычисление возраста
        public static int CalculateAge(DateTime birth)
        {
            DateTime now = DateTime.Now;        // настоящая дата

            if(DateTime.Compare(birth, now) > 0)
            {
                DateTime num = birth;
                birth = now;
                now = birth;
            }

            int age = now.Year - birth.Year;    // возраст

            // поправка по месяцу и/или дню
            if(now.Month < birth.Month || (now.Month == birth.Month && now.Day < birth.Day))
            {
                age -= 1;
            }

            return age;            
        }

        // вычисление возраста (перегруженный)
        public static int CalculateAge(DateTime birth, DateTime now)
        {
            if (DateTime.Compare(birth, now) > 0)
            {
                DateTime num = birth;
                birth = now;
                now = birth;
            }

            int age = now.Year - birth.Year;    // возраст

            // поправка по месяцу и/или дню
            if (now.Month < birth.Month || (now.Month == birth.Month && now.Day < birth.Day))
            {
                age -= 1;
            }

            return age;
        }

        // вычисление количества дней 
        public static int CaclculateNumberDays(DateTime first, DateTime second)
        {
            TimeSpan interval = second - first;

            if(interval.Days < 0)
            {
                return Math.Abs(interval.Days);
            }
            else
            {
                return interval.Days;
            }
        }
    }
}
