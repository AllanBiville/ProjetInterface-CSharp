using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ProjetInterface
{
    public partial class Form1 : Form
    {
        
        public int phase;
        // L : Listing
        // E : Listing + erreurs
        // R : execution - console visible uniqement
        // H : Aide
        public Form1()
        {
            InitializeComponent();
            phase ='E';
        }
        public void reorganiser()
        {
            int top1 = button1.Bottom + 5;
            int width = this.ClientRectangle.Width;
            int height = this.ClientRectangle.Height - top1;

            richTextBox1.Top = top1;
            richTextBox1.Left = 5;
            richTextBox1.Width = width - 10;
            richTextBox1.Height = height - (height / 4) -35;

            richTextBox2.Top = top1 + height / 2 + 100;
            richTextBox2.Left = 5;
            richTextBox2.Width = width - 10;
            richTextBox2.Height = height -  (height/2) -100;

            richTextBox3.Top = top1;
            richTextBox3.Left = 5;
            richTextBox3.Width = width - 10;
            richTextBox3.Height = height - 5;

            richTextBox4.Top = top1;
            richTextBox4.Left = 5;
            richTextBox4.Width = width - 10;
            richTextBox4.Height = height - 5;

            switch (phase)
            {
                case 'L':
                    richTextBox1.Visible = true; 
                    richTextBox2.Visible = false; 
                    richTextBox3.Visible = false; 
                    richTextBox4.Visible = false; 
                    break;
                case 'E': 
                    richTextBox1.Visible = true; 
                    richTextBox2.Visible = true;
                    richTextBox3.Visible = false;
                    richTextBox4.Visible = false; 
                    break;
                case 'R': 
                    richTextBox1.Visible = false; 
                    richTextBox2.Visible = false;
                    richTextBox3.Visible = true; 
                    richTextBox4.Visible = false; 
                    break;
                case 'H': 
                    richTextBox1.Visible = false; 
                    richTextBox2.Visible = false; 
                    richTextBox3.Visible = false; 
                    richTextBox4.Visible = true; 
                    break;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            reorganiser();
        }
        private void button1_Click(object sender, EventArgs e)
        {
           if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text += "\nFichier = " + openFileDialog1.FileName;
                richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
            // richTextBox1.Text += "test\n";

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void button3_Click(object sender, EventArgs e) // edit
        {
            if (phase == 'E')
            {
                phase = 'L';
            } else
            {
                phase = 'E';
            }
            reorganiser();
        }


        private async void button4_Click(object sender, EventArgs e) // Help
        {
           
            using var client = new HttpClient();

            var result = await client.GetAsync("https://zacharyhuguet.alwaysdata.net/lang/");
            richTextBox4.Text += result;
            phase = 'H';
            reorganiser();
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            phase = 'R';
            reorganiser();
            run();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            phase = 'E';
            reorganiser();
        }
        public void clear() // vider
        {
            richTextBox3.Text = "";
        }
        public void write(string str) // afficher
        {
            richTextBox3.Text += str;
        }
        public void nl() // sauter ligne
        {
            richTextBox3.Text += "\n";
        }
        public void writenl(string str) // sauter ligne
        {
            richTextBox3.Text += str + "\n";
        }
        public void erreur(string str)
        {
            richTextBox2.Text += str+"\n";
        }
        public void run()
        {

            clear();
                writenl("==========================================");
                writenl("                               CODE SOURCE");
                writenl("==========================================");
            nl();
            Parser.Compiler(openFileDialog1.FileName);
            // Parser.Compiler(richTextBox1.Text);    
            Parser.leProgramme.afficher();

                writenl("==========================================");
                writenl("                           RESULTAT DU CODE");
                writenl("==========================================");
            nl();
            Parser.lesVariables = new Variables();
                Parser.leProgramme.Executer();
                nl(); write("Fin Normale"); nl();
        }
    }
}
