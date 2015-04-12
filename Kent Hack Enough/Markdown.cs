using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Kent_Hack_Enough
{
    class Markdown
    {
        public TextBlock parseMarkdown(LiveFeedMessages msg)
        {
            TextBlock result = new TextBlock();
            
            string tmp;
            
            int i = 0;
            int start = 0;
            int end = 0;
            bool def = true;



            while (i < msg.text.Length)
            {

                try
                {
                    // Single asterisk
                    if ((msg.text[i] == '*') && (msg.text[i + 1] != '*'))
                    {
                        start = msg.text.IndexOf('*', i) + 1;
                        end = msg.text.IndexOf('*', start);
                        tmp = msg.text.Substring(start, (end - start));
                        // MessageBox.Show(end.ToString());

                        Run textRun = new Run();
                        textRun.Text = tmp + " ";
                        textRun.FontStyle = FontStyles.Italic;
                        result.Inlines.Add(textRun);

                        i = end + 1;
                        def = false;
                    }
                    // Single underline
                    else if ((msg.text[i] == '_') && (msg.text[i + 1] != '_'))
                    {
                        start = msg.text.IndexOf('_', i - 1);
                        end = msg.text.IndexOf('_', start);
                        tmp = msg.text.Substring(start, end);
                        Run textRun = new Run();
                        textRun.Text = tmp + " ";
                        textRun.FontStyle = FontStyles.Italic;
                        result.Inlines.Add(textRun);

                        i = end + 1;
                        def = false;
                    }
                    // Double Asterisk or Double underscore
                    else if ((msg.text[i] == '*') && (msg.text[i + 1] == '*'))
                    {
                        start = msg.text.IndexOf('*', i) + 2;
                        end = msg.text.IndexOf('*', start) - 2;
                        tmp = msg.text.Substring(start, end);

                        // Bold and italic
                        if (tmp.Contains("_"))
                        {
                            string under;
                            int cur2;

                            cur2 = tmp.IndexOf('_', i) + 2;
                            under = tmp.Substring(cur2, tmp.IndexOf('_', cur2));


                            Run textRun = new Run();
                            textRun.Text = tmp.Remove(cur2, under.Length) + " ";
                            textRun.FontWeight = FontWeights.Bold;
                            result.Inlines.Add(textRun);

                            textRun = null;

                            textRun.Text = under + " ";
                            textRun.FontWeight = FontWeights.Bold;
                            textRun.FontStyle = FontStyles.Italic;
                            result.Inlines.Add(textRun);

                            i = i + under.Length;
                            def = false;
                        }
                        else
                        {
                            Run textRun = new Run();
                            textRun.Text = tmp + " ";
                            textRun.FontWeight = FontWeights.Bold;
                            result.Inlines.Add(textRun);

                            i = end + start + 2;
                            def = false;
                        }
                    }
                    // Double Underline
                    else if ((msg.text[i] == '_') && (msg.text[i + 1] == '_'))
                    {
                        start = msg.text.IndexOf('_', i + 1);
                        end = msg.text.IndexOf('_', start);
                        tmp = msg.text.Substring(start, end);
                        Run textRun = new Run();
                        textRun.Text = tmp + " ";
                        textRun.FontWeight = FontWeights.Bold;
                        result.Inlines.Add(textRun);

                        i = end + start + 1;
                        def = false;
                    }
                }
                    
                catch (Exception)
                {
                   // i++;
                    //   throw;
                }
                i++;
                
            }
            

            if (def)
                {
                    result.Text = msg.text;
                }
            return result;
        }
    }


}
