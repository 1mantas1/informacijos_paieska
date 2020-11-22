using LemmaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using XLemmatizer;

namespace informacijos_paieska
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // static ILemmatizer lmtz = new LemmatizerPrebuiltCompact(LanguagePrebuilt.English);

        public class Turinys
        {
            public int failoIndeksas { get; set; }
            public string failoTurinys { get; set; }
            public string[] zodis { get; set; }
            public int[] zodziaiCount { get; set; }

            public Turinys()
            {

            }
            public void darbasSuTuriniu()
            {
                string[] turinioZodziai = failoTurinys.Split(' ', '\n', ' ', '.', ',', '-', '?', '|', '✓', '!', '/', ':', '\'', '\"', '(', ')', ';');
                turinioZodziai = turinioZodziai.Where(x => !string.IsNullOrWhiteSpace(x))
                                 .ToArray();
                //MessageBox.Show("Yra " + turinioZodziai.Length.ToString() + " Žodžių faile");
                //Surenkami visi unikalūs žodžiai
                for (int i = 0; i < turinioZodziai.Length; i++)
                {
                    turinioZodziai[i] = turinioZodziai[i].Trim().ToLower();
                }
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
            Lemmatizer lemmatizer = new Lemmatizer();
            public void listWords(RichTextBox ats)
            {
                
                darbasSuTuriniu();
                for (int i = 0; i < zodis.Length; i++)
                {
                    ats.Text += zodis[i] + " ; " + zodziaiCount[i] + " ; " + lemmatizer.(zodis[i]) + Environment.NewLine;
                }

            }
        }

        public string[] turinys = new string[30];
        public string[] failu_indeksai = new string[30];

        List<Turinys> failai = new List<Turinys>();

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] filePaths = Directory.GetFiles(@"D:\failai\");
            int fileIndex = 0;
            foreach (string filesNames in filePaths)
            {
                failu_indeksai[fileIndex] = filesNames;
                turinys[fileIndex] = File.ReadAllText(filesNames);
                failai.Add(new Turinys() { failoIndeksas = fileIndex, failoTurinys = turinys[fileIndex] });
                failai.ElementAt(fileIndex).listWords(richTextBox1);
                fileIndex++;
            }
            infoBottomLabel.Text = "Failai Nustaikyti";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            string paieskosZodziai = textBox1.Text;
            string[] ieskotiZodziu = Regex.Split(paieskosZodziai, " ");
            for (int i = 0; i < ieskotiZodziu.Length; i++)
            {
                ieskotiZodziu[i] = ieskotiZodziu[i].Trim();
            }
            // Paieska(ieskotiZodziu);
        }

        public void Paieska(string[] searchWords)
        {
            for (int i = 0; i < turinys.Length; i++)
            {
                if (turinys[i] != null)
                {
                    richTextBox1.Text += Environment.NewLine + failu_indeksai[i] + Environment.NewLine + Environment.NewLine;
                    foreach (string word in searchWords)
                    {
                        if (turinys[i].Contains(word))
                            richTextBox1.Text += word + "[" + Regex.Matches(turinys[i], word).Count + "]" + " ";
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            richTextBox1.Clear();
        }

    }
}
