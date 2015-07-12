using System;
using System.Globalization;
using System.Windows.Data;


namespace bytePassion.Lib.GenericValueConverter
{
    public abstract class GenericParameterizedValueConverter <TFrom, TTo, TParam> : IValueConverter
    {
	    protected abstract TTo Convert (TFrom value, TParam parameter, CultureInfo culture);
		protected abstract TFrom ConvertBack (TTo value, TParam parameter, CultureInfo culture);

	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
			if (value != null)
				if (value.GetType() != typeof(TFrom))
					throw new ArgumentException("types are not matching");

			if (parameter != null)
				if (parameter.GetType() != typeof(TParam))
					throw new ArgumentException("types are not matching");			

			var conversionResult = Convert((TFrom)value,(TParam) parameter, culture);

			if (conversionResult != null)
				if (conversionResult.GetType() != typeof(TTo)) 
					throw new ArgumentException("types are not matching");

		    return conversionResult;
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
			if (value != null && parameter != null)	// TODO
				if (value.GetType() != typeof(TTo) || parameter.GetType() != typeof(TParam))
					throw new ArgumentException("types are not matching");

			var conversionResult = ConvertBack((TTo)value, (TParam)parameter, culture);

			if (conversionResult != null)
				if (conversionResult.GetType() != typeof(TFrom))
					throw new ArgumentException("types are not matching");

		    return conversionResult;
	    }
    }
}
