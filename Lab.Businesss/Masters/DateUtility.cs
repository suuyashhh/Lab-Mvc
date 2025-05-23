﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Businesss.Masters
{
    public class DateUtility
    {
        public string MonthYear { get; set; }
        public string DateValue { get; set; }

        public static string GetFormatedDate(string strDate, int flag)
        {
            string retDate = "";

            if (strDate != null)
            {
                if (strDate.Trim().Length == 0)
                {
                    retDate = "";
                }
                else
                {
                    try
                    {
                        if (flag == 1)
                        {
                            retDate = DateTime.ParseExact(strDate, "dd/MM/yyyy", null).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                        }
                        else if (flag == 4)
                        {
                            retDate = DateTime.ParseExact(strDate, "dd/MM/yyyy", null).ToString("yyyyMMdd");
                        }
                        else if (flag == 3)
                        {
                            retDate = DateTime.ParseExact(strDate, "yyyy-MM-dd", null).ToString("dd/MM/yyyy");
                        }
                        else if(flag == 8)
                        {
                            retDate = DateTime.ParseExact(strDate, "yyyyMMdd", null).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            retDate = DateTime.ParseExact(strDate, "yyyyMMdd", null).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    catch
                    {
                        retDate = "";
                    }
                }
            }
            return retDate;
        }

        public static string GetCurrentDate()
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo indiaZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, indiaZone);
            return indianTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
           
        }

        public static string GetCurrDateForGenId()
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo indiaZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, indiaZone);
            return indianTime.ToString("yyMMdd", CultureInfo.InvariantCulture);

        }
        public static string GetCurrentMonthStartDate()
        {
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);

            return startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }


    }
}
