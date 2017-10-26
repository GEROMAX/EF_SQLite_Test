using EF_SQLite_Test.Models;
using EF_SQLite_Test.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EF_SQLite_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Context.IssueComments.RemoveRange(this.Context.IssueComments);
            try
            {
                for (int id = 1; id <= 99; id++)
                {
                    var entity = this.Context.IssueComments.Create();
                    entity.CommentId = id;
                    entity.Comment = "コメント" + id.ToString();
                    this.Context.IssueComments.Add(entity);
                }

                this.Context.SaveChanges();
            }
            catch (DbEntityValidationException exception)
            {
                var sb = new StringBuilder();
                foreach (var validation in exception.EntityValidationErrors)
                {
                    foreach (var error in validation.ValidationErrors)
                    {
                        sb.AppendLine(error.ErrorMessage);
                    }
                }
                MessageBox.Show(sb.ToString());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var query = this.Context.IssueComments.Where(entity => true);
            this.dataGridView1.DataSource = query.ToList();
        }

        private EF_SQLite_Test_Model Context { get; set; } = new EF_SQLite_Test_Model();
        private ViewManager Manager { get; set; }

        //private void btnInsert_Click(object sender, EventArgs e)
        //{
        //    var sb = new StringBuilder();
        //    try
        //    {
        //        this.Context.Issues.Add((Issue)this.Manager.SetValueToEntity());
        //        this.Context.SaveChanges();
        //    }
        //    catch (DbEntityValidationException exception)
        //    {
        //        foreach (var validation in exception.EntityValidationErrors)
        //        {
        //            foreach (var error in validation.ValidationErrors)
        //            {
        //                sb.AppendLine(error.ErrorMessage);
        //            }
        //        }
        //    }
        //    catch (DbUpdateException exception)
        //    {
        //        sb.AppendLine(this.GetLastError(exception).Message);
        //    }
        //    catch (Exception exception)
        //    {
        //        sb.Append(exception.ToString());
        //    }

        //    if (sb.Length > 0)
        //    {
        //        MessageBox.Show(sb.ToString());
        //    }
        //    else
        //    {
        //        MessageBox.Show("登録しました");
        //        this.DialogResult = DialogResult.OK;
        //        this.Close();
        //    }
        //}

        //this.Manager = new ViewManager(entity);
        //this.Manager.Add(this.textBox1, this.label1, Mitumori1.見積NO);
        //this.Manager.Add(this.dateTimePicker1, this.label2, Mitumori1.申込日);
        //this.Manager.Add(this.textBox2, this.label3, Mitumori1.保険会社コード);
        //this.Manager.Add(this.comboBox1, this.label4, Mitumori1.短期区分);
        //this.Manager.Add(this.comboBox2, this.label5, Mitumori1.集金区分);
        //this.Manager.Add(this.textBox3, this.label6, Mitumori1.タイトル);
        //this.Manager.Add(this.dateTimePicker2, this.label7, Mitumori1.契約変更日);
        //this.Manager.SetValueToControl();
    }
}
