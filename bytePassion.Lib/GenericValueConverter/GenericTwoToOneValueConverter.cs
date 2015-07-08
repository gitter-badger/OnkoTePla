using System;
using System.Globalization;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace funkwerk.phx.smp.map
{
    public abstract class GenericTwoToOneValueConverter <TFrom1, TFrom2, TTo> : IMultiValueConverter
    {
	    protected abstract TTo Convert (TFrom1 value1, TFrom2 value2, CultureInfo culture);
	    protected abstract object[] ConvertBack (TTo value, CultureInfo culture);	   	   

	    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	    {
			if (values.Length != 2)
				throw new ArgumentException("There sould be two values for conversion, but there are " + values.Length);

			if (values[0].GetType() != typeof(TFrom1))
				if (!(values[0] is TFrom1))
					throw new ArgumentException("Type of first given value is " + values[0].GetType() + ", but should be " + typeof(TFrom1));

			if (values[1].GetType() != typeof(TFrom2))
				if (!(values[1] is TFrom2))
					throw new ArgumentException("Type of second given value is " + values[1].GetType() + ", but should be " + typeof(TFrom2));

			var conversionResult = Convert((TFrom1)values[0], (TFrom2)values[1], culture);

			if (conversionResult.GetType() != typeof(TTo))
				throw new ArgumentException("Type of computedResult is " + conversionResult.GetType() + ", but should be " + typeof(TTo));

			return conversionResult;
	    }

	    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	    {
			if (value.GetType() != typeof(TTo))
				if (!(value is TTo))
					throw new ArgumentException("types are not matching");


			var conversionResult = ConvertBack((TTo)value, culture);

			if (conversionResult.Length != 2)
				throw new ArgumentException("There sould be two converted values, but there are " + conversionResult.Length);

			// TODO typeTest for conversionResult

			return conversionResult;
	    }
    }
}
