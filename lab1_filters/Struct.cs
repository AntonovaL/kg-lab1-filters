using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1_filters
{
        public partial class Struct : Form
        {
            private int[] row;
        private DataGridViewTextBoxColumn[] arCol;


        public Struct()
            {
                InitializeComponent();
            }
        

      
        public int GetRad()
            {
            return Form1.n;
            }

        public int GetVal(int x, int y)
            {
                if (x < Form1.n && y < Form1.n)
                    return (int)dataGridView1[x, y].Value;
                return 0;
            }
        

        private void button1_Click(object sender, EventArgs e)
        {
            {
                if (textBox1.Text != null)
                {
                    dataGridView1.Columns.Clear();
                    Form1.n = Convert.ToInt32(textBox1.Text);
                    int n1 = Form1.n % 2;
                    if (Form1.n > 0 && n1 == 1)
                    {
                        dataGridView1.AllowUserToAddRows = true;
                        dataGridView1.AllowUserToDeleteRows = true;
                        row = new int[Form1.n];
                        arCol = new DataGridViewTextBoxColumn[Form1.n];
                        for (int i = 0; i <Form1.n; i++)
                        {
                            arCol[i] = new DataGridViewTextBoxColumn();
                            arCol[i].Width = 50;
                        }
                        dataGridView1.Columns.AddRange(arCol);
                        for (int i = 0; i < Form1.n; i++)
                        {
                            dataGridView1.Rows.Add(row);
                        }
                        for (int i = 0; i < Form1.n; i++)
                            for (int j = 0; j < Form1.n; j++)
                                dataGridView1[i, j].Value = 0;
                        dataGridView1.AllowUserToAddRows = false;
                        dataGridView1.AllowUserToDeleteRows = false;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                if (Form1.n > 0)
                {
                    Form1.matrix = new bool[Form1.n, Form1.n];
                    for (int i = 0; i < Form1.n; i++)
                        for (int j = 0; j < Form1.n; j++)
                        {
                            if (Convert.ToInt32(dataGridView1[i, j].Value) > 0)
                                Form1.matrix[i, j] = true;
                            else
                                Form1.matrix[i, j] = false;
                        }

                }

            Close();
        }
    }
}
