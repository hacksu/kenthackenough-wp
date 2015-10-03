using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kent_Hack_Enough
{
    class Dates
    {

        public string parseDate(DateTime dt)
        {
            DateTime dtNow = new DateTime();
            dt = dt.ToLocalTime();

            string result = null;

            dtNow = DateTime.Now.ToLocalTime();




            //int timeDif = dt.Hour - dtNow.Hour;
            ////timeDif = 100 - timeDif;
            //double span;
            //if(timeDif < 12)
            //{
            //    span = 24 - TimeSpan.FromHours(dt.Hour).TotalHours + TimeSpan.FromHours(dtNow.Hour).TotalHours;
            //}
            //else
            //{
            //    span = TimeSpan.FromHours(dt.Hour).TotalHours - TimeSpan.FromHours(dtNow.Hour).TotalHours;
            //}

            double span = -1;



            if(dt.Day == dtNow.Day)
            {
                if (dt.Minute >= 40)
                {
                    span = TimeSpan.FromHours(dtNow.Hour).TotalHours - TimeSpan.FromHours(dt.Hour).TotalHours - 1;
                }
                else
                {
                    span = TimeSpan.FromHours(dtNow.Hour).TotalHours - TimeSpan.FromHours(dt.Hour).TotalHours;
                }
               
            }else if(dt.Day + 1 == dtNow.Day)
            {
                if (dt.Minute >= 40)
                {
                    span = 24 - TimeSpan.FromHours(dt.Hour).TotalHours + TimeSpan.FromHours(dtNow.Hour).TotalHours - 1;
                }
                else
                {
                    span = 24 - TimeSpan.FromHours(dt.Hour).TotalHours + TimeSpan.FromHours(dtNow.Hour).TotalHours;
                }
                
            }



            if (span < 24 && span != -1 && dtNow.Hour != dt.Hour)
            {
                // Same day so lets check the hour
               // if (dt.Hour < dtNow.Hour)
               // {
                    DateTime tmp = new DateTime();
                    tmp.AddMinutes(dt.Minute);

                    if ((dtNow.Hour - dt.Hour) == 1)
                    {
                        result = "an hour ago";
                        return result;
                    }
                    result = span + " hours ago";
             //   }
            }
            // Count the minutes
            else if (((dtNow.Minute - dt.Minute) > 0) && ((dtNow.Minute - dt.Minute) < 60) && (dtNow.Hour == dt.Hour))
            {
                if ((dtNow.Minute - dt.Minute) <= 1)
                {
                    result = "Just now";
                }
                else
                {
                    result = (dtNow.Minute - dt.Minute).ToString() + " minutes ago";
                }
            }
            // Check to see if in same month
            else if (dt.Month == dtNow.Month)
            {
                // Same month lets check the day
                //   if (dt.ToLocalTime().Day < dtNow.Day)
                // {
                if ((dtNow.ToLocalTime().Day - dt.Day).ToString() == "1")
                {
                    result = "a day ago";
                    return result;
                }
                result = dtNow.Day - dt.Day + " days ago";
                //}
            }
            else if(dt.Month == dtNow.Month - 1)
            {
                result = (DateTime.DaysInMonth(dtNow.Year, dt.Month) - dt.Day + 1).ToString() + " days ago";
            }
            else if (dt.Month != dtNow.Month && dt.Year == dtNow.Year)
            {
                if ((dtNow.ToLocalTime().Month - dt.Month).ToString() == "1")
                {
                    result = "a month ago";
                    return result;
                }
                result = (dtNow.ToLocalTime().Month - dt.Month).ToString() + " months ago";
            }
            // Check the year
            else if (dt.Year < dtNow.Year)
            {
                if ((dtNow.ToLocalTime().Year - dt.Year).ToString() == "1")
                {
                    result = "a year ago";
                    return result;
                }
                result = (dtNow.ToLocalTime().Year - dt.Year).ToString() + " years ago";
            }
            else
            {

            }

            return result;

        }
    }
}
