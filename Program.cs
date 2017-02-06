using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace ParsingXml
{
    public class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("hbase_svn_log.xml"); //Loading the xml document from default path

            // Counter to store total number of commits
            int count = 0;

            // Getting node elements
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/log/logentry");

            // List to store each committers commits.  
            List<Max> maxElements = new List<Max>();

            // Getting total count against authors names.
            foreach (XmlNode node in nodes)
            {
                Log logEntry = new Log();

                logEntry.author = node.SelectSingleNode("author").InnerText;
               
                count++;
            }

            string[] authName = new string[count];
            int itr = 0;

            // storing author names in a string array for Comparision
            foreach(XmlNode node in nodes)
            {
                authName[itr] = node.SelectSingleNode("author").InnerText;
                itr++;
            }

            // Nesting 'for' loop to compare string array to itself for finding max for a committer.
            for (int i = 0; i < count; i++)
            {
                int comp = 0; // counting each committer commits 
                for(int j = 0; j < count; j++)
                {
                    if(authName[i].Equals(authName[j]))
                    {
                        comp++;
                    }
                }

                Max maxCurrent = new Max();
                maxCurrent.auth = authName[i];
                maxCurrent.itr = comp;
                maxElements.Add(maxCurrent); // storing the result of inner 'for' loop processing
            }
            // end of nesting 'for' loop

            var unique_items = maxElements.GroupBy(x => x.auth).Select(y => y.First()); // getting unique values from the list
            foreach (Max elem in unique_items)
                Console.WriteLine(elem.auth +" "+ elem.itr);
           
           // Console.WriteLine("Total commits: " + count);
        }

    }

    public class Log
    {
        public string author;
    }

    public class Max
    {
        public string auth;
        public int itr;
    }
}
