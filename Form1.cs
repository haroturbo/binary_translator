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
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
            string before_trans = textBox1.Text;
            string after_trans = textBox2.Text;
            string patch_file = textBox3.Text;
            string range_start = textBox4.Text;
            string[] rang = range_start.Split(Convert.ToChar(0x2d));

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
            string[] bt = new string[100];
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
            string[] at = new string[100];
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

            if (t == tt)
            {
                    for (int i = 0; i < t; i++)
                    {

                        if (at[i] != null)
                        {
                            if (bt[i].Length == at[i].Length)
                            {
                                filestring = Microsoft.VisualBasic.Strings.Replace(filestring,bt[i], at[i], 1, 1, Microsoft.VisualBasic.CompareMethod.Binary);
                            }
                        }
                }

                    filestring = filestring.Replace(" ", "");
                }
            else {

                MessageBox.Show("翻訳前てきすとと翻訳後てきすとの行数が一致しません");
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


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }

        }
    }
}
