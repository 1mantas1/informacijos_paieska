using informacijos_paieska.Classes;
using LemmaSharp.Classes;
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

namespace informacijos_paieska
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string[] turinys = new string[30];
        public string[] failu_indeksai = new string[30];
        List<Turinys> failai = new List<Turinys>();

        List<indeksuotiFailai> indexFiles = new List<indeksuotiFailai>();
        List<lemmFailai> lemmFiles = new List<lemmFailai>();
        List<biggram> bigramFiles = new List<biggram>();
        private void Form1_Load(object sender, EventArgs e)
        {

            //Failų apdorojimas | suindeksavimas visais 3 būdais.
            string[] filePaths = Directory.GetFiles(@"D:\failai\");
            string[] indeksuotiFailai = Directory.GetFiles(@"D:\failai\indeksuotiFailai\");
            string[] lemuotiFailai = Directory.GetFiles(@"D:\failai\lemuotiFailai\");
            string[] bigramFailai = Directory.GetFiles(@"D:\failai\bigramFailai\");

            int fileIndex = 0;
            foreach (string filesNames in filePaths)
            {
                failu_indeksai[fileIndex] = filesNames;
                turinys[fileIndex] = File.ReadAllText(filesNames);
                failai.Add(new Turinys() { failoIndeksas = fileIndex, failoTurinys = turinys[fileIndex] });
                failai.ElementAt(fileIndex).darbasSuTuriniu();
                fileIndex++;
            }
            fileIndex = 0;
            infoBottomLabel.Text = "Failai Nustaikyti";
            indexButton_Click(sender, e);
            lemButton_Click(sender, e);
            bigramButton_Click(sender, e);
            moveFilesAfterConvertion();
            // Failų apdorojimo pabaiga

            //Indeksuotų failų paėmimas
            foreach (string filesNames in indeksuotiFailai)
            {
                indexFiles.Add(new indeksuotiFailai()
                {
                    fileIndex = fileIndex,
                    fileName = filesNames
                });
                indexFiles.ElementAt(fileIndex).readIndexFile();
                fileIndex++;
            }
            //Darbo pabaiga su indeksuotais failais 
            fileIndex = 0;
            //Lemmuotų failų paėmimas
            foreach (string filesNames in lemuotiFailai)
            {
                lemmFiles.Add(new lemmFailai()
                {
                    fileIndex = fileIndex,
                    fileName = filesNames
                });
                lemmFiles.ElementAt(fileIndex).readIndexFile();
                fileIndex++;
            }
            //Darbo pabaiga su Lemuotais failais 
            fileIndex = 0;
            //Biguotu failų paėmimas
            foreach (string filesNames in bigramFailai)
            {
                bigramFiles.Add(new biggram()
                {
                    fileIndex = fileIndex,
                    fileName = filesNames
                });
                bigramFiles.ElementAt(fileIndex).readBigramFile();
                fileIndex++;
            }
            //Darbo pabaiga su Biguotais failais 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            indexlistView.Items.Clear();
            string paieskosZodziai = textBox1.Text.Trim();
            indexlistView.View = View.Details;
            richTextBox1.Clear();
            foreach (var file in indexFiles)
            {
                if (file.isInFile(paieskosZodziai,richTextBox1))
                {
                    indexlistView.Items.Add(new ListViewItem(new[] { file.fileIndex.ToString(), file.fileName.ToString() }));
                }
                
            }
            indexlistView.GridLines = true;
            // Paieska(ieskotiZodziu);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            richTextBox1.Clear();
        }

        private void lemButton_Click(object sender, EventArgs e)
        {
            var dataFilepath = @"D:\failai\LemFile\full7z-multext-en.lem";
            var lemuotiFailai = @"D:\failai\lemuotiFailai\";

            var stream = File.OpenRead(dataFilepath);
            var lemmatizer = new Lemmatizer(stream);
            string failoVardas = "";

            for (int i = 0; i < failai.Count; i++)
            {

                failoVardas = failu_indeksai[failai.ElementAt(i).failoIndeksas].ToString();
                string[] workingFileName = failoVardas.Split('\\');
                failoVardas = lemuotiFailai + workingFileName[workingFileName.Length - 1];
                using (StreamWriter file = new StreamWriter(failoVardas)){

                    failoVardas = "";
                    for(int j=0;j<failai.ElementAt(i).zodis.Length;j++)
                    {
                        file.WriteLine(lemmatizer.Lemmatize(failai.ElementAt(i).zodis[j]) +';'+ failai.ElementAt(i).zodziaiCount[j]);
                    }
                }

            }
            infoBottomLabel.Text = "Failai sėkmingai Lemuoti";
        }
        private void indexButton_Click(object sender, EventArgs e)
        {
            var indeksuotiFailai = @"D:\failai\indeksuotiFailai\";
            string failoVardas = "";

            for (int i = 0; i < failai.Count; i++)
            {

                failoVardas = failu_indeksai[failai.ElementAt(i).failoIndeksas].ToString();
                string[] workingFileName = failoVardas.Split('\\');
                failoVardas = indeksuotiFailai + workingFileName[workingFileName.Length - 1];
                using (StreamWriter file = new StreamWriter(failoVardas))
                {
                    failoVardas = "";
                    for (int j = 0; j < failai.ElementAt(i).zodis.Length; j++)
                    {
                        file.WriteLine(failai.ElementAt(i).zodis[j] + ';' + failai.ElementAt(i).zodziaiCount[j]);
                    }
                }

            }
            infoBottomLabel.Text = "Failai sėkmingai Suindeksuoti";
        }

        private void bigramButton_Click(object sender, EventArgs e)
        {
            var bigramosFailai = @"D:\failai\bigramFailai\";
            string failoVardas = "";

            for (int i = 0; i < failai.Count; i++)
            {

                failoVardas = failu_indeksai[failai.ElementAt(i).failoIndeksas].ToString();
                string[] workingFileName = failoVardas.Split('\\');
                failoVardas = bigramosFailai + workingFileName[workingFileName.Length - 1];

                using (StreamWriter file = new StreamWriter(failoVardas))
                {
                    failoVardas = "";
                    for (int j = 0; j < failai.ElementAt(i).zodis.Length-1; j++)
                    {
                        file.WriteLine(failai.ElementAt(i).zodis[j] + ' ' + failai.ElementAt(i).zodis[j+1]);
                    }
                }

            }
            infoBottomLabel.Text = "Failai sėkmingai Bigramuoti";
        }

        private void moveFilesAfterConvertion()
        {
            var originalPath = "";
            var apdorotiFailai = @"D:\failai\apdorotiFailai\";
            string failoVardas = "";
            for (int i = 0; i < failai.Count; i++)
            {
                originalPath = failu_indeksai[failai.ElementAt(i).failoIndeksas].ToString();
                string[] workingFileName = originalPath.Split('\\');
                failoVardas = apdorotiFailai + workingFileName[workingFileName.Length - 1];
                File.Move(originalPath, failoVardas);
            }
            infoBottomLabel.Text = "Visi Failai Apdoroti";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lemmListView.Items.Clear();
            string paieskosZodziai = textBox1.Text.Trim();
            lemmListView.View = View.Details;
            richTextBox1.Clear();
            foreach (var file in lemmFiles)
            {
                if (file.isInFile(paieskosZodziai, richTextBox1))
                {
                    lemmListView.Items.Add(new ListViewItem(new[] { file.fileIndex.ToString(), file.fileName.ToString() }));
                }

            }
            lemmListView.GridLines = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            biggramListView.Items.Clear();
            string paieskosZodziai = textBox1.Text.Trim();
            biggramListView.View = View.Details;
            richTextBox1.Clear();
            foreach (var file in bigramFiles)
            {
                if (file.isInFile(paieskosZodziai, richTextBox1))
                {
                    biggramListView.Items.Add(new ListViewItem(new[] { file.fileIndex.ToString(), file.fileName.ToString() }));
                }

            }
            biggramListView.GridLines = true;
        }

        private void label4_Click(object sender, EventArgs e)
        {
          
        }
    }
}
