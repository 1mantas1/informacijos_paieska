using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace informacijos_paieska.Classes
{
    public class Turinys
    {
        public int failoIndeksas { get; set; }
        public string failoTurinys { get; set; }
        public string[] failoturinysPazodiui { get; set; }
        public string[] zodis { get; set; }
        public int[] zodziaiCount { get; set; }

        public Turinys()
        {

        }
        public void darbasSuTuriniu()
        {
            string[] turinioZodziai = failoTurinys.Split(' ', '\n', ' ', '.', ',', '-', '?', '|', '✓', '!', '/', ':', '\'', '\"', '(', ')', ';', '`', '’');
            turinioZodziai = turinioZodziai.Where(x => !string.IsNullOrWhiteSpace(x))
                             .ToArray();
            //MessageBox.Show("Yra " + turinioZodziai.Length.ToString() + " Žodžių faile");
            //Surenkami visi unikalūs žodžiai
            for (int i = 0; i < turinioZodziai.Length; i++)
            {
                turinioZodziai[i] = turinioZodziai[i].Trim().ToLower();
            }
            this.failoturinysPazodiui = turinioZodziai.ToArray();
            this.zodis = turinioZodziai.Distinct().ToArray();
            zodziaiCount = new int[zodis.Length];
            //MessageBox.Show("Yra " + zodis.Length.ToString() + " uniklaių faile");
            //suskaičiuojama kiek kiekvienas žodis pasikartoja faile

            zodis = zodis.Except(new List<string> { string.Empty }).ToArray();

            for (int i = 0; i < zodis.Length; i++)
            {
                for (int j = 0; j < turinioZodziai.Length; j++)
                {
                    if (zodis[i] == turinioZodziai[j])
                    {
                        zodziaiCount[i]++;
                    }
                }
            }
        }
        //Lemmatizer lemmatizer = new Lemmatizer();
        public void listWords(RichTextBox ats)
        {

            darbasSuTuriniu();
            for (int i = 0; i < zodis.Length; i++)
            {
                ats.Text += zodis[i] + " ; " + zodziaiCount[i] + Environment.NewLine;
            }

        }
    }
}
