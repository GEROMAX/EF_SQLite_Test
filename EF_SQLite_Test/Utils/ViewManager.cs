using EF_SQLite_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EF_SQLite_Test.Utils
{
    public class ViewManager
    {
        public EntityBase TargetEntity { get; set; }

        public List<ViewManagerItem> Items { get; set; } = new List<ViewManagerItem>();

        private Dictionary<string, PropertyInfo> PropertyMap { get; set; } = new Dictionary<string, PropertyInfo>();

        public ViewManager(EntityBase entity)
        {
            this.TargetEntity = entity;
            this.createPropertyMap();
        }

        private void createPropertyMap()
        {
            foreach (PropertyInfo pi in this.TargetEntity.GetType().GetProperties())
            {
                this.PropertyMap.Add(pi.Name, pi);
            }
        }

        public void Add(Control ctrl, Label lbl, string itemName)
        {
            this.Items.Add(new ViewManagerItem(ctrl, lbl, this.PropertyMap[itemName]));
        }

        public void SetValueToControl()
        {
            this.Items.ForEach(item => item.SetValueToControl(this.TargetEntity));
        }

        public EntityBase SetValueToEntity()
        {
            return this.setValue(this.TargetEntity);
        }

        public void SetValueToEntity(EntityBase entity)
        {
            this.setValue(entity);
        }

        private EntityBase setValue(EntityBase entity)
        {
            this.Items.ForEach(item => item.SetValueToEntity(entity));
            return entity;
        }
    }
}
