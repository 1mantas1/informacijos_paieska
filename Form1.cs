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

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] filePaths = Directory.GetFiles(@"D:\failai\");
            int fileIndex = 0;
            foreach (string filesNames in filePaths)
            {
                failu_indeksai[fileIndex] = filesNames;
                turinys[fileIndex] = File.ReadAllText(filesNames);
                fileIndex++;
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          richTextBox1.Clear();
          string paieskosZodziai = textBox1.Text;
          string[] ieskotiZodziu = Regex.Split(paieskosZodziai,"and");
          Paieska(ieskotiZodziu);
        }

        public void Paieska(string[] searchWords){
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
