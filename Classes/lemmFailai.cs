using LemmaSharp.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace informacijos_paieska.Classes
{
    class lemmFailai
    {
        public int fileIndex { get; set; }
        public string fileName { get; set; }
        public string fileContent { get; set; }

        List<Zodis> zodziaiList = new List<Zodis>();

        private string[] eil;
        public void readIndexFile()
        {
            string[] tmpSplitArray = new string[2];
            fileContent = File.ReadAllText(fileName);
            eil = File.ReadAllLines(fileName);
            for (int i = 0; i < eil.Length; i++)
            {
                tmpSplitArray = eil[i].Split(';');
                zodziaiList.Add(
                    new Zodis()
                    {
                        id = i,
                        word = tmpSplitArray[0],
                        wordDuplicateCount = Convert.ToInt32(tmpSplitArray[1])
                    });
            }
        }
        public bool isInFile(string str, RichTextBox ats)
        {
            var dataFilepath = @"D:\failai\LemFile\full7z-multext-en.lem";
            var lemuotiFailai = @"D:\failai\lemuotiFailai\";

            var stream = File.OpenRead(dataFilepath);
            var lemmatizer = new Lemmatizer(stream);
            string[] tmpZodziai;
            str = str.ToLower();
            int[] element = new int[2];
            if (str.Contains("and"))
            {
                
                tmpZodziai = Regex.Split(str, "and");
                if (zodziaiList.Any(x => x.word.Contains(lemmatizer.Lemmatize(tmpZodziai[0].Trim()))) && zodziaiList.Any(x => x.word.Contains(lemmatizer.Lemmatize(tmpZodziai[1].Trim()))))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                tmpZodziai = str.Split(' ');
                if (tmpZodziai.Length > 0)
                {
                    if (tmpZodziai.Length == 1)
                    {
                        if (zodziaiList.Any(x => x.word.Contains(lemmatizer.Lemmatize(tmpZodziai[0].Trim()))))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (tmpZodziai.Length == 2)
                    {
                        if (zodziaiList.Any(x => x.word.Contains(lemmatizer.Lemmatize(tmpZodziai[0].Trim())) || x.word.Contains(lemmatizer.Lemmatize(tmpZodziai[1].Trim()))))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (tmpZodziai.Length == 3)
                    {
                        if (zodziaiList.Any(x => x.word.Contains(lemmatizer.Lemmatize(tmpZodziai[0].Trim())) || x.word.Contains(lemmatizer.Lemmatize(tmpZodziai[1].Trim())) || x.word.Contains(lemmatizer.Lemmatize(tmpZodziai[2].Trim()))))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
