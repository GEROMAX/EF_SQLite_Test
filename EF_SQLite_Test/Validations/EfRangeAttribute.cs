using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SQLite_Test.Validations
{
    public class EfRangeAttribute : RangeAttribute
    {
        public EfRangeAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
        }

        public EfRangeAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
        }

        public EfRangeAttribute(Type type, string minimum, string maximum) : base(type, minimum, maximum)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("{0} は {1} から {2} までの範囲で指定してください。", name, this.Minimum.ToString(), this.Maximum.ToString());
        }
    }
}
