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

        string pathstr;
        public int ammount;
        public List<string> tagList = new List<string>();

        public ReadWrite()
        {
        }

        public void SetPath(string path)
        {
            this.pathstr = path;
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
                    //s.Replace(".", "-");
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

        public void WriteCSV(List<string> lista, string filename)
        {
            string[] text1;
            text1 = lista.ToArray();
            text1 = text1.Take(text1.Count() - 1).ToArray();
            System.IO.File.WriteAllLines(filename, text1);
        }

        public void AddTag(string item, string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("File doesn't exist. \n Do you want to create that file?", "File not found", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.No)
                    return;
                else
                {
                    System.IO.File.AppendAllText(filename, item);
                    MessageBox.Show("One tag has been added");
                }
            }
            else
            {
                System.IO.File.AppendAllText(filename, item);
                MessageBox.Show("One tag has been added");
            }
        }
    }
}
