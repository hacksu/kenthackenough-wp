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

            int timeDif = dtNow.Hour - dt.Hour - dtNow.Minute + dt.Minute;
            timeDif = 100 - timeDif;


            if (dt.ToLocalTime().Day == dtNow.Day)
            {
                // Same day so lets check the hour
                if (dt.Hour < dtNow.Hour)
                {
                    DateTime tmp = new DateTime();
                    tmp.AddMinutes(dt.Minute);

                    if ((dtNow.Hour - dt.Hour) == 1)
                    {
                        result = "an hour ago";
                        return result;
                    }
                    result = (dtNow.Hour - dt.Hour).ToString() + " hours ago";
                }
                // Count the minutes
                else if (((dtNow.Minute - dt.Minute) > 0) && ((dtNow.Minute - dt.Minute) < 60) && (dtNow.Hour == dt.Hour) || (dtNow.Hour == dt.Hour + 1))
                {
                    if ((dtNow.Minute - dt.Minute) == 1)
                    {
                        result = "a minute ago";
                    }
                    else
                    {
                        result = (dtNow.Minute - dt.Minute).ToString() + " minutes ago";
                    }
                }
                // Looks like this post was just now!
                else
                {
                    result = "just now";
                }
            }
            // Check to see if in same month
            else if (dt.Month == dtNow.Month)
            {
                // Same month lets check the day
                if (dt.ToLocalTime().Day < dtNow.Day)
                {
                    if ((dtNow.ToLocalTime().Day - dt.Day).ToString() == "1")
                    {
                        result = "a day ago";
                        return result;
                    }
                    result = (dtNow.ToLocalTime().Day - dt.Day).ToString() + " days ago";
                }
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
