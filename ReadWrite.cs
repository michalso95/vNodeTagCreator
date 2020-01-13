using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace TagCreatorVNode
{
    class ReadWrite
    {

        string pathstr, namestr;
        public int ammount;
        public List<string> tagList = new List<string>();

        public ReadWrite(string name)
        {
            pathstr = name;
            namestr = pathstr.Substring(0, (pathstr.IndexOf(".")));
            Read(pathstr);
        }

        private void Read(string path)
        {
            ammount = 0;
            StringBuilder sb = new StringBuilder();
            StreamReader sr = new StreamReader(path);
            string s;
            do
            {
                s = sr.ReadLine();
                sb.AppendLine(s);
                try
                {
                    s.Replace(".", "-");
                    tagList.Add(s);
                    ammount++;
                }
                catch (Exception)
                {
                    // 
                }

                
            } while (s != null);
            sr.Close();
        }

        public void WriteCSV(List<string> lista)
        {
            string[] text1;
            string outPath = string.Format("{0}CSV.csv", namestr);
            text1 = lista.ToArray();
            text1 = text1.Take(text1.Count() - 1).ToArray();
            System.IO.File.WriteAllLines(outPath, text1);
        }

        public void AddTag(string item)
        {
            string pathAdd = string.Format("{0}CSV.csv", namestr);
            if (!System.IO.File.Exists(pathAdd))
            {
                MessageBox.Show("File doesn't exist");
                return;
            }
            else
            {
                System.IO.File.AppendAllText(pathAdd, item);
            }
        }
    }
}
