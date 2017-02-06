using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace BugReports
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Region contains dictionary to convert alphabetic months into numerics.
            #region 
            Dictionary<string, string> months = new Dictionary<string, string>()
            {
                { "Jan", "01"},
                { "Feb", "02"},
                { "Mar", "03"},
                { "Apr", "04"},
                { "May", "05"},
                { "Jun", "06"},
                { "Jul", "07"},
                { "Aug", "08"},
                { "Sep", "09"},
                { "Oct", "10"},
                { "Nov", "11"},
                { "Dec", "12"},
            };
            #endregion 

            XmlDocument doc = new XmlDocument(); // initializing Xml instance

            List<string> resolved = new List<string>(); // dynamic list to store resolved dates of bug reports 
            List<string> created = new List<string>(); // dynamic list to store creation dates of bug reports 
            List<double> maxTime = new List<double>(); // dynamic list to store bug resolution time.

            int count = 0; // to store the total number of resolved bug reports

            // iterating through all the files
            foreach (string file in Directory.EnumerateFiles("hbaseBugReport", "*.xml"))
            {
                doc.Load(file);
                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/rss/channel/item");

                foreach (XmlNode node in nodes)
                {
                    
                    if (node.SelectSingleNode("resolved") == null) // Checking if the bug report is unresolved
                    {
                       // Console.WriteLine("No resolve date found");
                    }
                    else
                    {
                        count++;
                        resolved.Add(node.SelectSingleNode("resolved").InnerText);
                        created.Add(node.SelectSingleNode("created").InnerText);
                        //Console.WriteLine(resolve);
                    }
     
                }
            }

            // converting dynamic lists into arrays.
            string[] resolvedArray = resolved.ToArray(); 
            string[] createdArray = created.ToArray();

            for (int i = 0; i < count; i++)
            {
                // Splitting "Created" dates for comparison
                string[] tempCreated = createdArray[i].Split(' ');

                string dayCre = tempCreated[1];
                string monthCre = null;
                foreach (var month in months) // getting numeric month from dictionary
                {
                    if (tempCreated[2].Contains(month.Key))
                    {
                        monthCre = month.Value;
                    }
                }
                string yearCre = tempCreated[3];
                string[] timeSplitCre = tempCreated[4].Split(':');
                string hourCre = timeSplitCre[0];
                string minCre = timeSplitCre[1];
                string secCre = timeSplitCre[2];

                // Splitting "Resolved" dates for comparison 
                string[] tempResolved = resolvedArray[i].Split(' ');

                string dayRes = tempResolved[1];
                string monthRes = null;
                foreach (var month in months) // getting numeric month from dictionary
                {
                    if(tempResolved[2].Contains(month.Key))
                    {
                        monthRes = month.Value;
                    }
                }
                string yearRes = tempResolved[3];
                string[] timeSplitRes = tempResolved[4].Split(':');
                string hourRes = timeSplitRes[0];
                string minRes = timeSplitRes[1];
                string secRes = timeSplitRes[2];

                DateTime createdDate = new DateTime(Int32.Parse(yearCre), Int32.Parse(monthCre), Int32.Parse(dayCre), Int32.Parse(hourCre), Int32.Parse(minCre), Int32.Parse(secCre)); // (year, month, day, hour, minute, second)
                DateTime resolvedDate = new DateTime(Int32.Parse(yearRes), Int32.Parse(monthRes), Int32.Parse(dayRes), Int32.Parse(hourRes), Int32.Parse(minRes), Int32.Parse(secRes)); // (year, month, day, hour, minute, second)
                
                
               // Subtracting Bug Report "created" date from "resolved" date to find "Bug Resolution Time" 
               maxTime.Add(resolvedDate.Subtract(createdDate).TotalSeconds);
            }

            foreach (double elem in maxTime) // Displaying bug resolution time for each bug report.
                Console.WriteLine("Bug resolution time: "+ elem);

            Console.WriteLine("Maximum bug resolution time (in seconds): "+ maxTime.Max());
            Console.WriteLine("Total number of resolved bug reports: " + count);
        }

    }
}
