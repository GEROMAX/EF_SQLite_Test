using EF_SQLite_Test.Models;
using EF_SQLite_Test.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EF_SQLite_Test.Utils
{
    public class ViewManagerItem
    {
        public Control TargetControl { get; set; }
        public Label ItemLabel { get; set; }
        public PropertyInfo TargetProperty { get; set; }

        public ViewManagerItem(Control ctrl, Label lbl, PropertyInfo pi)
        {
            this.TargetControl = ctrl;
            this.ItemLabel = lbl;
            this.TargetProperty = pi;
        }

        public void SetValueToControl(EntityBase entity)
        {
            this.setLabel();
            this.setValueToControl((dynamic)this.TargetControl, entity);
        }

        private void setLabel()
        {
            var displayAttr = (DisplayAttribute)this.TargetProperty.GetCustomAttribute<DisplayAttribute>();
            this.ItemLabel.Text = (displayAttr != null) ? displayAttr.Name : "不明";
        }

        private void setValueToControl(TextBox txt, EntityBase entity)
        {
            var byteAttr = this.TargetProperty.GetCustomAttribute<EfMaxByteAttribute>();
            if (null != byteAttr)
            {
                txt.MaxLength = byteAttr.MaxLength;
            }
            txt.Text = this.getString((dynamic)this.TargetProperty.GetValue(entity));
        }

        private void setValueToControl(DateTimePicker dtp, EntityBase entity)
        {
            var value = this.TargetProperty.GetValue(entity);
            dtp.Checked = (null != value);
            if (dtp.Checked)
            {
                dtp.Value = dtp.MinDate <= (DateTime)value ? (DateTime)value : DateTime.Now;
            }
        }

        private void setValueToControl(ComboBox cmb, EntityBase entity)
        {
            var type = this.TargetProperty.PropertyType;
            if (type.IsEnum)
            {
                cmb.DataSource = Enum.GetValues(type);
            }
            else if (this.typeIsNullable(type))
            {
                //Null許容Enumはめんどくさい。。
                var lst = new List<KeyValuePair<string, object>>();
                var names = Enum.GetNames(type.GetProperties()[1].PropertyType);
                var values = Enum.GetValues(type.GetProperties()[1].PropertyType);
                lst.Add(new KeyValuePair<string, object>("", -1));
                for (int i = 0; i < names.Count(); i++)
                {
                    lst.Add(new KeyValuePair<string, object>(names[i], values.GetValue(i)));
                }
                cmb.DisplayMember = "key";
                cmb.ValueMember = "value";
                cmb.DataSource = lst;
            }
            var value = this.TargetProperty.GetValue(entity);
            if (null != value)
            {
                if (type.IsEnum)
                {
                    cmb.SelectedItem = this.TargetProperty.GetValue(entity);
                }
                else
                {
                    cmb.SelectedValue = this.TargetProperty.GetValue(entity);
                }
            }
            else
            {
                cmb.SelectedValue = -1;
            }
        }

        private void setValueToControl(NumericUpDown nud, EntityBase entity)
        {
            var rangeAttr = this.TargetProperty.GetCustomAttribute<RangeAttribute>();
            nud.Minimum = (rangeAttr.Minimum != null) ? Convert.ToDecimal(rangeAttr.Minimum) : decimal.MinValue;
            nud.Maximum = (rangeAttr.Maximum != null) ? Convert.ToDecimal(rangeAttr.Maximum) : decimal.MaxValue;
            nud.Value = Convert.ToDecimal(this.TargetProperty.GetValue(entity));
        }

        private bool typeIsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private string getString(decimal value)
        {
            return value.ToString();
        }

        private string getString(string value)
        {
            return value;
        }

        public void SetValueToEntity(EntityBase entity)
        {
            this.setValueToEntity((dynamic)this.TargetControl, entity);
        }

        private void setValueToEntity(TextBox txt, EntityBase entity)
        {
            var value = txt.Text;

            if (this.TargetProperty.PropertyType.Equals(typeof(string)))
            {
                this.TargetProperty.SetValue(entity, value);
            }
            else if (this.TargetProperty.PropertyType.Equals(typeof(decimal)))
            {
                value = string.IsNullOrWhiteSpace(value) ? "0" : value;
                this.TargetProperty.SetValue(entity, decimal.Parse(value));
            }
            else if (this.TargetProperty.PropertyType.Equals(typeof(decimal?)))
            {
                value = string.IsNullOrWhiteSpace(value) ? null : value;
                var typedValue = (value == null) ? null : (decimal?)decimal.Parse(value);
                this.TargetProperty.SetValue(entity, typedValue);
            }
            else if (this.TargetProperty.PropertyType.Equals(typeof(int)))
            {
                value = string.IsNullOrWhiteSpace(value) ? "0" : value;
                this.TargetProperty.SetValue(entity, int.Parse(value));
            }
            else
            {
                throw new NotImplementedException(string.Format("Set value entity string to {0} is not support now.", this.TargetProperty.PropertyType.ToString()));
            }
        }

        private void setValueToEntity(DateTimePicker dtp, EntityBase entity)
        {
            if (dtp.ShowCheckBox)
            {
                this.TargetProperty.SetValue(entity, dtp.Checked ? (DateTime?)dtp.Value : null);
            }
            else
            {
                this.TargetProperty.SetValue(entity, dtp.Value);
            }
        }

        private void setValueToEntity(ComboBox cmb, EntityBase entity)
        {
            var type = this.TargetProperty.PropertyType;
            if (type.IsEnum)
            {
                this.TargetProperty.SetValue(entity, Enum.Parse(type, cmb.SelectedValue.ToString()));
            }
            else if (this.typeIsNullable(type))
            {
                if (cmb.SelectedValue.Equals(-1))
                {
                    this.TargetProperty.SetValue(entity, null);
                }
                else
                {
                    this.TargetProperty.SetValue(entity, Enum.Parse(type.GetProperties()[1].PropertyType, cmb.SelectedValue.ToString()));
                }
            }
            else
            {
                throw new NotImplementedException(string.Format("Set value entity combo to {0} is not support now.", type.ToString()));
            }
        }

        private void setValueToEntity(NumericUpDown nud, EntityBase entity)
        {
            if (this.TargetProperty.PropertyType.Equals(typeof(int)))
            {
                this.TargetProperty.SetValue(entity, Convert.ToInt32(nud.Value));
            }
            else if (this.TargetProperty.PropertyType.Equals(typeof(decimal)))
            {
                this.TargetProperty.SetValue(entity, nud.Value);
            }
            else
            {
                throw new NotImplementedException(string.Format("Set value entity decimal to {0} is not support now.", this.TargetProperty.PropertyType.ToString()));
            }
        }
    }
}
