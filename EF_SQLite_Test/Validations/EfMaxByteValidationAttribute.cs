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
    public class EfMaxByteAttribute : ValidationAttribute
    {
        public int MaxByte { get; set; }
        public bool AllowJP { get; set; }
        public Encoding MeasureEncoding { get; set; }

        public int MaxLength
        {
            get
            {
                return this.AllowJP ? this.MaxByte / 2 : this.MaxByte;
            }
        }

        public EfMaxByteAttribute(int byteLength, bool allowJP = true, string encodingName = "Shift_JIS")
        {
            this.MaxByte = byteLength;
            this.AllowJP = allowJP;
            this.MeasureEncoding = Encoding.GetEncoding(encodingName);
            this.ErrorMessage = "{0} は{1}文字以内で入力してください。";
        }

        public override bool IsValid(object value)
        {
            return this.MaxByte >= this.MeasureEncoding.GetByteCount((string)value ?? "");
        }

        public override string FormatErrorMessage(string name)
        {
            var lengthHelp = (this.AllowJP ? this.MaxLength.ToString() : "半角" + this.MaxLength.ToString());
            return string.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, lengthHelp);
        }
    }
}
