using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bytePassion.Lib.WpfLib.ConverterBase;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;


namespace bytePassion.OnkoTePla.Server.WpfUi.Converter
{
    internal class MainPageToIntConverter : GenericValueConverter<MainPage, int>
    {
        protected override int Convert(MainPage page, CultureInfo culture)
        {
            return (int) page;
        }
    }
}
