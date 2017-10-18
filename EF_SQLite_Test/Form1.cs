using EF_SQLite_Test.Models;
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
            var dbContext = new EF_SQLite_Test_Model();
            dbContext.IssueComments.RemoveRange(dbContext.IssueComments);
            try
            {
                for (int id = 1; id <= 99; id++)
                {
                    var entity = dbContext.IssueComments.Create();
                    entity.CommentId = id;
                    entity.Comment = "コメント" + id.ToString();
                    dbContext.IssueComments.Add(entity);
                }                

                dbContext.SaveChanges();
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
            var dbContext = new EF_SQLite_Test_Model();
            var query = dbContext.IssueComments.Where(entity => true);
            this.dataGridView1.DataSource = query.ToList();
        }
    }
}
