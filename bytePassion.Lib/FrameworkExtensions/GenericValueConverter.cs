using System;
using System.Globalization;
using System.Windows.Data;


namespace bytePassion.Lib.FrameworkExtensions
{
    public abstract class GenericValueConverter <TFrom, TTo> : IValueConverter
    {
	    protected abstract TTo Convert (TFrom value, CultureInfo culture);
	    protected abstract TFrom ConvertBack (TTo value, CultureInfo culture);

	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    if (value.GetType() != typeof (TFrom))		   
				if (!(value is TFrom))
					throw new ArgumentException("Type of given value is " + value.GetType() + ", but should be " + typeof (TFrom));
		   

		    var conversionResult = Convert((TFrom)value, culture);

			if (conversionResult.GetType() != typeof(TTo))   
				throw new ArgumentException("Type of computedResult is " + conversionResult.GetType() + ", but should be " + typeof(TTo));

		    return conversionResult;
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    if (value.GetType() != typeof (TTo))
				if (!(value is TTo))
					throw new ArgumentException("types are not matching");
		    

		    var conversionResult = ConvertBack((TTo) value, culture);

			if (conversionResult.GetType() != typeof(TFrom))
				throw new ArgumentException("types are not matching");

		    return conversionResult;
	    }
    }
}
