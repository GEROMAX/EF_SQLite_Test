using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SQLite_Test.Validations
{
    public class EfRequiredAttribute : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0} は必須です。", name);
        }
    }
}
