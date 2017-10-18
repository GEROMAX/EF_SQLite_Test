using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_SQLite_Test.Validations
{
    /// <summary>
    /// バイト数検証用の属性
    /// </summary>
    public class MaxByteAttribute : ValidationAttribute
    {
        public int MaxByte { get; set; }
        public Encoding MeasureEncoding { get; set; }

        public MaxByteAttribute(int byteLength, string encodingName = "Shift_JIS")
        {
            this.MaxByte = byteLength;
            this.MeasureEncoding = Encoding.GetEncoding(encodingName);
            this.ErrorMessage = "Maximum byte length of {0} is {1}.";
        }

        public override bool IsValid(object value)
        {
            return this.MaxByte >= this.MeasureEncoding.GetByteCount((string)value);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.MaxByte.ToString());
        }
    }
}
