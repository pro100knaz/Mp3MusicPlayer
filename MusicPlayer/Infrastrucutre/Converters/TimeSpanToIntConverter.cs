using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicPlayer.Infrastrucutre.Converters
{
    internal class TimeSpanToIntConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (!(v is int value)) return v;

            return TimeSpan.FromSeconds(value);

        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            if (!(v is TimeSpan value)) return v;

            return value.Minutes * 60 + value.Seconds;
        }
    }
}
