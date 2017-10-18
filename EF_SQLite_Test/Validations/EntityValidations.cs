using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EF_SQLite_Test.Validations
{
    public partial class EF_SQLite_Test_Model : DbContext
    {
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
            var pis = entityEntry.Entity.GetType().GetProperties();

            //Vaidate max byte
            this.ValidateMaxByte(entityEntry.Entity, pis, result);

            return result.ValidationErrors.Count > 0 ? result : base.ValidateEntity(entityEntry, items);
        }

        private void ValidateMaxByte(object entity, PropertyInfo[] propertyInfos, DbEntityValidationResult result)
        {
            MaxByteAttribute attrMBVA = null;
            var query = propertyInfos.Where(pi =>
            {
                var attr = (MaxByteAttribute[])pi.GetCustomAttributes(typeof(MaxByteAttribute), true);
                if (attr.Length > 0)
                {
                    attrMBVA = attr[0];
                    return true;
                }
                return false;
            }).Select(pi =>
                new { TargetProperty = pi, AttrMaxByte = attrMBVA }
            );
            foreach (var item in query)
            {
                if (!item.AttrMaxByte.IsValid(item.TargetProperty.GetValue(entity)))
                {
                    result.ValidationErrors.Add(new DbValidationError(item.TargetProperty.Name, item.AttrMaxByte.FormatErrorMessage(item.TargetProperty.Name)));
                }
            }
        }
    }
}
