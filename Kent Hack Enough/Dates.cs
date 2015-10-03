using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Kent_Hack_Enough
{
    class Dates
    {

        public RichTextBox parseDate(DateTime dt)
        {
            DateTime dtNow = new DateTime();
            dt = dt.ToLocalTime();

            Paragraph para = new Paragraph();
            RichTextBox result = new RichTextBox();

            dtNow = DateTime.Now.ToLocalTime();
           

            double span = -1;



            if (dt.Day == dtNow.Day)
            {
                if (dt.Minute >= 40)
                {
                    span = TimeSpan.FromHours(dtNow.Hour).TotalHours - TimeSpan.FromHours(dt.Hour).TotalHours - 1;
                }
                else
                {
                    span = TimeSpan.FromHours(dtNow.Hour).TotalHours - TimeSpan.FromHours(dt.Hour).TotalHours;
                }

            }
            else if (dt.Day + 1 == dtNow.Day)
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
                DateTime tmp = new DateTime();
                tmp.AddMinutes(dt.Minute);

                if ((dtNow.Hour - dt.Hour) == 1)
                {
                    para.Inlines.Add("an hour ago");
                    return returnResult(para);
                }
                para.Inlines.Add(span + " hours ago");
            }
            // Count the minutes
            else if (((dtNow.Minute - dt.Minute) > 0) && ((dtNow.Minute - dt.Minute) < 60) && (dtNow.Hour == dt.Hour))
            {
                if ((dtNow.Minute - dt.Minute) <= 1)
                {
                    para.Inlines.Add("Just now");
                    return returnResult(para);
                }
                else
                {
                    para.Inlines.Add((dtNow.Minute - dt.Minute).ToString() + " minutes ago");
                    return returnResult(para);
                }
            }
            // Check to see if in same month
            else if (dt.Month == dtNow.Month)
            {
                if ((dtNow.ToLocalTime().Day - dt.Day).ToString() == "1")
                {
                    para.Inlines.Add("a day ago");
                    return returnResult(para);
                }
                para.Inlines.Add(dtNow.Day - dt.Day + " days ago");
            }
            else if (dt.Month == dtNow.Month - 1)
            {
                para.Inlines.Add((DateTime.DaysInMonth(dtNow.Year, dt.Month) - dt.Day + 1).ToString() + " days ago");
            }
            else if (dt.Month != dtNow.Month && dt.Year == dtNow.Year)
            {
                if ((dtNow.ToLocalTime().Month - dt.Month).ToString() == "1")
                {
                    para.Inlines.Add("a month ago");
                    return returnResult(para);
                }
                para.Inlines.Add((dtNow.ToLocalTime().Month - dt.Month).ToString() + " months ago");
            }
            // Check the year
            else if (dt.Year < dtNow.Year)
            {
                if ((dtNow.ToLocalTime().Year - dt.Year).ToString() == "1")
                {
                    para.Inlines.Add("a year ago");
                    return returnResult(para);
                }
                para.Inlines.Add((dtNow.ToLocalTime().Year - dt.Year).ToString() + " years ago");
            }

            return returnResult(para);
        }

        private RichTextBox returnResult(Paragraph para)
        {
            RichTextBox result = new RichTextBox();
            result.Blocks.Add(para);
            return result;
        }
    }
}
