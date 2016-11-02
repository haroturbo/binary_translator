using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.Focus();
            listBox1.SelectedIndex=0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
             string basepath = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, listBox1.Text);

            string before_trans = System.IO.Path.Combine( basepath , textBox1.Text);
            string after_trans = System.IO.Path.Combine(basepath , textBox2.Text);
            string patch_file = textBox3.Text;
            string range_start = textBox4.Text;
            string[] rang = range_start.Split(Convert.ToChar(0x2d));


                if (System.IO.File.Exists(before_trans) == false) { 
                    MessageBox.Show(before_trans + "翻訳前ファイルが存在しません");
                    return ;
                }
                if (System.IO.File.Exists(after_trans) == false){
                MessageBox.Show(after_trans + "翻訳後ファイルが存在しません");
                    return;
                }
                if (System.IO.File.Exists(patch_file) ==false){

                    MessageBox.Show(after_trans + "ぱっち対象ファイルが存在しません");
                    return;
                }

                if (rang.Length != 2) {
                    MessageBox.Show("範囲を入力して下さい");
                    return;
                }
                int start = Convert.ToInt32(rang[0].Replace("0x",""),16);
               int end = Convert.ToInt32(rang[1].Replace("0x", ""), 16);
                if (start > end) {
                    MessageBox.Show("範囲がおかしいです");
                    return;
                }



                System.IO.FileStream fs = new System.IO.FileStream(patch_file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                
                byte[] bs = new byte[end-start];
                byte[] bin = new byte[fs.Length];

                fs.Read(bin, 0, bin.Length);                             
                fs.Close();
                Array.ConstrainedCopy(bin, start, bs, 0, bs.Length);


            string filestring = "";
            for (int i = 0; i < bs.Length; i++) {
                filestring += bs[i].ToString("X2") + " ";
            }


            System.IO.FileStream bfs = new System.IO.FileStream(before_trans, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] bbs = new byte[bfs.Length];
            bfs.Read(bbs, 0, bbs.Length);
            bfs.Close();
            string[] bt = new string[500];
            int t = 0;
            for (int i = 0; i < bbs.Length; i++)
            {
                if (bbs[i] == 10)
                {
                    t++;
                }
                else if (bbs[i] == 0xd)
                {
                }
                else
                {
                    bt[t] += bbs[i].ToString("X2") + " ";
                }
            }




            System.IO.FileStream afs = new System.IO.FileStream(after_trans, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] abs = new byte[afs.Length];
            afs.Read(abs, 0, abs.Length);
            afs.Close();
            string[] at = new string[500];
            int tt = 0;
            for (int i = 0; i < abs.Length; i++)
            {
                if (abs[i] == 10)
                {
                    tt++;
                }
                else if (abs[i] == 0xd)
                {
                }
                else
                {
                    at[tt] += abs[i].ToString("X2") + " ";
                }
            }

                string logger = "";

            if (t == tt)
            {
                    for (int i = 0; i < t; i++)
                    {

                        if (at[i] != null)
                        {
                            if (bt[i].Length == at[i].Length)
                            {
                                filestring = Microsoft.VisualBasic.Strings.Replace(filestring, bt[i], at[i], 1, 1, Microsoft.VisualBasic.CompareMethod.Binary);
                            }
                            else {

                                logger += (i + 1).ToString() + "行目文字数不一致 前" + (bt[i].Length).ToString() +" 後:"+ (at[i].Length).ToString();
                            }
                        }
                }

                    filestring = filestring.Replace(" ", "");
                }
            else {

                MessageBox.Show("翻訳前てきすとと翻訳後てきすとの行数が一致しません");
                    return;
            }
            
                for (int i = 0; i < filestring.Length; i += 2)
                {
                    byte[] ttt = new byte[1];
                    string num = filestring.Substring(i, 2);
                    ttt[0] = Convert.ToByte(Convert.ToInt16(num, 16));
                    bs[i >> 1] = ttt[0];
                }

                Array.ConstrainedCopy(bs, 0, bin, start, bs.Length);

                System.IO.FileStream ws = new System.IO.FileStream("patched.suprx", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                ws.Write(bin, 0, bin.Length);
                ws.Close();

                MessageBox.Show("翻訳パッチが成功しました!!!!");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string file = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath ,listBox1.Text,"set.txt");
            if (System.IO.File.Exists(file))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(file, System.Text.Encoding.GetEncoding("shift_jis"));

                textBox4.Text = sr.ReadLine();
                textBox3.Text = sr.ReadLine();
                sr.Close();
            }
            else {
                MessageBox.Show(file + "設定用が存在しません");
            }

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
