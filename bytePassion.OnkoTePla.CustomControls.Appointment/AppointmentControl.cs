using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using bytePassion.Lib.TimeLib;

namespace bytePassion.OnkoTePla.CustomControls.Appointment
{
    
    public class AppointmentControl : Control
    {

	    public static readonly DependencyProperty StartTimeProperty = 
			DependencyProperty.Register("StartTime", 
										typeof (Time),
										typeof (AppointmentControl), 
										new PropertyMetadata(Time.Dummy));

	    public Time StartTime
	    {
		    get { return (Time) GetValue(StartTimeProperty); }
		    set { SetValue(StartTimeProperty, value); }
	    }

	    public static readonly DependencyProperty EndTimeProperty = 
			DependencyProperty.Register("EndTime", 
										typeof (Time), 
										typeof (AppointmentControl), 
										new PropertyMetadata(default(Time)));

	    public Time EndTime
	    {
		    get { return (Time) GetValue(EndTimeProperty); }
		    set { SetValue(EndTimeProperty, value); }
	    }

        static AppointmentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppointmentControl), new FrameworkPropertyMetadata(typeof(AppointmentControl)));
        }
    }
}
