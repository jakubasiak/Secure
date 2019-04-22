using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Cube.Secure.Converters
{
    public class FileSizeToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                var size = (long)value;
                return this.BytesToString(size);
            }

            return string.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return string.Empty;
            long bytes = Math.Abs(byteCount);
            int place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024.0*1000.0)));
            double num = Math.Round(bytes / Math.Pow(1024.0*1000.0, place), 1);
            return (Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture) + suf[place];
        }
    }
}
