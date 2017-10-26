using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EF_SQLite_Test.Models
{
    public abstract class EntityBase
    {
        public void CopyTo<T>(T entity) where T : EntityBase
        {
            if (!this.GetType().Equals(entity.GetType()))
            {
                throw new NotSupportedException("Not support different class copy now.");
            }

            entity.GetType().GetProperties().ToList().FindAll(pi => pi.CanWrite && pi.CanRead).ForEach(pi => pi.SetValue(entity, pi.GetValue(this)));
        }
    }
}
