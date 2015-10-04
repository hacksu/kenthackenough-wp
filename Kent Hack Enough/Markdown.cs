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

        public RichTextBox parseMarkdown(string msg)
        {
            RichTextBox result = new RichTextBox();
            
            string tmp;
            
            int i = 0;
            int start = 0;
            int end = 0;
            bool def = true;

            Paragraph para = new Paragraph();
            

            while (i < msg.Length)
            {
                Run textRun = new Run();

                try
                {
                    // Single asterisk - Italic
                    if ((msg[i] == '*') && (msg[i + 1] != '*'))
                    {
                        start = msg.IndexOf('*', i) + 1;
                        end = msg.IndexOf('*', start);
                        tmp = msg.Substring(start, (end - start));
                        //Paragraph para = new Paragraph();
                        //Run textRun = new Run();
                        textRun.Text = tmp + " ";
                        textRun.FontStyle = FontStyles.Italic;

                        para.Inlines.Add(textRun);
                       // result.Blocks.Add(para);

                        i = end + 1;
                        def = false;
                    }
                    // Single underline - Italic
                    else if ((msg[i] == '_') && (msg[i + 1] != '_'))
                    {
                        start = msg.IndexOf('_', i) + 1;
                        end = msg.IndexOf('_', start);
                        tmp = msg.Substring(start, (end - start));

                       // Paragraph para = new Paragraph();
                       // Run textRun = new Run();
                        textRun.Text = tmp + " ";
                        textRun.FontStyle = FontStyles.Italic;

                        para.Inlines.Add(textRun);
                      //  result.Blocks.Add(para);

                        i = end + 1;
                        def = false;
                    }
                    //Double Asterisk or Double underscore
                   else if ((msg[i] == '*') && (msg[i + 1] == '*'))
                    {
                        start = msg.IndexOf('*', i) + 2;
                        end = msg.IndexOf('*', start) - 2;
                        tmp = msg.Substring(start, end);

                        //Bold and italic
                        if (tmp.Contains("_"))
                        {
                            string under;
                            int cur2;

                            cur2 = tmp.IndexOf('_', i) + 2;
                            under = tmp.Substring(cur2, tmp.IndexOf('_', cur2));


                          //  Paragraph para = new Paragraph();
                          //  Run textRun = new Run();
                            textRun.Text = tmp.Remove(cur2, under.Length) + " ";
                            textRun.FontWeight = FontWeights.Bold;

                            para.Inlines.Add(textRun);


                            textRun = null;

                            textRun.Text = under + " ";
                            textRun.FontWeight = FontWeights.Bold;
                            textRun.FontStyle = FontStyles.Italic;
                            para.Inlines.Add(textRun);
                          //  result.Blocks.Add(para);

                            i = i + under.Length;
                            def = false;
                        }
                        else
                        {
                        //    Paragraph para = new Paragraph();
                        //    Run textRun = new Run();
                            textRun.Text = tmp + " ";
                            textRun.FontWeight = FontWeights.Bold;

                            para.Inlines.Add(textRun);
                      //      result.Blocks.Add(para);

                            i = end + start + 2;
                            def = false;
                        }
                    }
                    //Double Underline
                    else if ((msg[i] == '_') && (msg[i + 1] == '_'))
                    {
                        start = msg.IndexOf('_', i) + 2;
                        end = msg.IndexOf('_', start) - 2;
                        tmp = msg.Substring(start, end);
                        //Paragraph para = new Paragraph();
                        //Run textRun = new Run();
                        textRun.Text = tmp + " ";
                        textRun.FontWeight = FontWeights.Bold;

                        para.Inlines.Add(textRun);
                      //  result.Blocks.Add(para);

                        i = end + start + 1;
                        def = false;
                    }
                    //LINK
                    else if ((msg[i] == '['))
                    {
                        start = msg.IndexOf('[', i);
                        end = msg.IndexOf(']', start);

                        if (msg[end + 1] != '(')
                        {
                            break;
                        }
                        else
                        {
                            int startLink = msg.IndexOf('(', end + 1);
                            int endLink = msg.IndexOf(')', startLink);
                            tmp = msg.Substring(start + 1, end - 1);
                          //  Paragraph para = new Paragraph();
                          //  Run textRun = new Run();
                            Hyperlink link = new Hyperlink();
                            textRun.Text = tmp + " ";
                            textRun.FontWeight = FontWeights.Bold;


                            link.TargetName = "_blank";
                            link.NavigateUri = new Uri(msg.Substring(startLink + 1, endLink - startLink - 1));
                            link.Inlines.Add(textRun);

                            para.Inlines.Add(link);
                           // result.Blocks.Add(para);

                            i = endLink + 1;
                            def = false;
                        }

                    }


                    if (def)
                    {
                       // Paragraph para = new Paragraph();

                        para.Inlines.Add(msg);
                        result.Blocks.Add(para);
                        return result;
                    }
                        

                }

                catch (Exception ex)
                {
                   // i++;
                      throw;
                }
                i++;

                    

            }
            result.Blocks.Add(para);
                return result;

        }
    }


}
