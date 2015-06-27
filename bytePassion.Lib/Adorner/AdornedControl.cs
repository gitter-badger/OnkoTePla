using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace bytePassion.Lib.Adorner
{
   
    public class AdornedControl : ContentControl
	{
		#region private Enum (Showstate)

		private enum ShowState
		{
			Visible,
			Hidden,
			FadingIn,
			FadingOut,
		}

		#endregion
		
		#region constructor and static contstructor

		// TODO: eigener DataContext für den AdornerContent
	    public AdornedControl()
	    {
		    adornerShowState = ShowState.Hidden;
		    closeAdornerTimer = new DispatcherTimer();

		    Focusable = false; // By default don't want 'AdornedControl' to be focusable.

		    DataContextChanged += (sender, eventArgs) => UpdateAdornerContentDataContext();

			closeAdornerTimer.Tick += (sender, eventArgs) =>
			                          {
				                          closeAdornerTimer.Stop();
				                          FadeOutAdorner();
			                          };
		   	    
			closeAdornerTimer.Interval = TimeSpan.FromSeconds(CloseAdornerTimeOut);
        }

		static AdornedControl ()
		{
			CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), ShowAdornerCommandBinding);
			CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), FadeOutAdornerCommandBinding);
			CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), HideAdornerCommandBinding);
			CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), FadeInAdornerCommandBinding);
		}

		#endregion

		#region Dependency Properties

		public static readonly DependencyProperty IsAdornerVisibleProperty =
            DependencyProperty.Register("IsAdornerVisible", 
										typeof(bool), 
										typeof(AdornedControl),
										new FrameworkPropertyMetadata(IsAdornerVisible_PropertyChanged));

        public static readonly DependencyProperty AdornerContentProperty =
            DependencyProperty.Register("AdornerContent", 
										typeof(FrameworkElement), 
										typeof(AdornedControl),
										new FrameworkPropertyMetadata(AdornerContent_PropertyChanged));

        public static readonly DependencyProperty HorizontalAdornerPlacementProperty =
            DependencyProperty.Register("HorizontalAdornerPlacement", 
										typeof(AdornerPlacement), 
										typeof(AdornedControl),
										new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        public static readonly DependencyProperty VerticalAdornerPlacementProperty =
            DependencyProperty.Register("VerticalAdornerPlacement", 
										typeof(AdornerPlacement), 
										typeof(AdornedControl),
										new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        public static readonly DependencyProperty AdornerOffsetXProperty =
            DependencyProperty.Register("AdornerOffsetX", 
										typeof(double), 
										typeof(AdornedControl));

        public static readonly DependencyProperty AdornerOffsetYProperty =
            DependencyProperty.Register("AdornerOffsetY", 
										typeof(double), 
										typeof(AdornedControl));

        public static readonly DependencyProperty IsMouseOverShowEnabledProperty =
            DependencyProperty.Register("IsMouseOverShowEnabled", 
										typeof(bool), 
										typeof(AdornedControl),
										new FrameworkPropertyMetadata(true, IsMouseOverShowEnabled_PropertyChanged));

        public static readonly DependencyProperty FadeInTimeProperty =
            DependencyProperty.Register("FadeInTime", 
										typeof(double), 
										typeof(AdornedControl),
										new FrameworkPropertyMetadata(0.25));

        public static readonly DependencyProperty FadeOutTimeProperty =
            DependencyProperty.Register("FadeOutTime", 
										typeof(double), 
										typeof(AdornedControl),
										new FrameworkPropertyMetadata(1.0));

        public static readonly DependencyProperty CloseAdornerTimeOutProperty =
            DependencyProperty.Register("CloseAdornerTimeOut", 
										typeof(double), 
										typeof(AdornedControl),
										new FrameworkPropertyMetadata(2.0, CloseAdornerTimeOut_PropertyChanged));       

        #endregion Dependency Properties

		#region DependencyPropertyChangedEventHandler

		private static void IsAdornerVisible_PropertyChanged (DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var adornedControl = (AdornedControl)o;
			adornedControl.ShowOrHideAdornerInternal();
		}
		
		private static void IsMouseOverShowEnabled_PropertyChanged (DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var adornedControl = (AdornedControl)o;					//
			adornedControl.closeAdornerTimer.Stop();				// ???????????????????????????????
			adornedControl.HideAdorner();							//
		}
		
		private static void CloseAdornerTimeOut_PropertyChanged (DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var adornedControl = (AdornedControl)o;
			adornedControl.closeAdornerTimer.Interval = TimeSpan.FromSeconds(adornedControl.CloseAdornerTimeOut);
		}
		
		private static void AdornerContent_PropertyChanged (DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var adornedControl = (AdornedControl)o;
			adornedControl.ShowOrHideAdornerInternal();

			var oldAdornerContent = (FrameworkElement)e.OldValue;
			if (oldAdornerContent != null)
			{
				oldAdornerContent.MouseEnter -= adornedControl.adornerContent_MouseEnter;
				oldAdornerContent.MouseLeave -= adornedControl.adornerContent_MouseLeave;
			}

			var newAdornerContent = (FrameworkElement)e.NewValue;
			if (newAdornerContent != null)
			{
				newAdornerContent.MouseEnter += adornedControl.adornerContent_MouseEnter;
				newAdornerContent.MouseLeave += adornedControl.adornerContent_MouseLeave;
			}
		}
		
		private void adornerContent_MouseEnter (object sender, MouseEventArgs e) { MouseEnterLogic(); }		
		private void adornerContent_MouseLeave (object sender, MouseEventArgs e) { MouseLeaveLogic(); }

		#endregion

		#region Commands

		public static readonly RoutedCommand ShowAdornerCommand    = new RoutedCommand("ShowAdorner",    typeof(AdornedControl));
        public static readonly RoutedCommand FadeInAdornerCommand  = new RoutedCommand("FadeInAdorner",  typeof(AdornedControl));
        public static readonly RoutedCommand HideAdornerCommand    = new RoutedCommand("HideAdorner",    typeof(AdornedControl));
        public static readonly RoutedCommand FadeOutAdornerCommand = new RoutedCommand("FadeOutAdorner", typeof(AdornedControl));

        #endregion Commands

		#region Actions (Show/Hide/FadeIn/FadeOut)

		public void ShowAdorner() { IsAdornerVisible = true;  }      
        public void HideAdorner() { IsAdornerVisible = false; }
    
        public void FadeInAdorner()
        {
            if (adornerShowState == ShowState.Visible ||
                adornerShowState == ShowState.FadingIn)
            {
                // Already visible or fading in.
                return;
            }

            ShowAdorner();

            if (adornerShowState != ShowState.FadingOut)
            {
                adornerContentFrameworkElement.Opacity = 0.0;
            }

            DoubleAnimation doubleAnimation = new DoubleAnimation(1.0, new Duration(TimeSpan.FromSeconds(FadeInTime)));

			doubleAnimation.Completed += (sender, eventArgs) => adornerShowState = ShowState.Visible;
            doubleAnimation.Freeze();
                
            adornerContentFrameworkElement.BeginAnimation(OpacityProperty, doubleAnimation);

            adornerShowState = ShowState.FadingIn;
        }

        
        public void FadeOutAdorner()
        {
            if (adornerShowState == ShowState.FadingOut || adornerShowState == ShowState.Hidden)            
                return;                        

            DoubleAnimation fadeOutAnimation = new DoubleAnimation(0.0, new Duration(TimeSpan.FromSeconds(FadeOutTime)));
	        fadeOutAnimation.Completed += (sender, eventArgs) =>
	                                      {
		                                      if (adornerShowState == ShowState.FadingOut)
		                                      {
			                                      // Still fading out => it wasn't aborted.
			                                      HideAdorner();
		                                      }
	                                      };
            fadeOutAnimation.Freeze();

            adornerContentFrameworkElement.BeginAnimation(OpacityProperty, fadeOutAnimation);

            adornerShowState = ShowState.FadingOut;
        }
		#endregion

		#region CLR-Propertywrapper for DependencyProperties

		public bool IsAdornerVisible
        {
            get { return (bool)GetValue(IsAdornerVisibleProperty); }
            set { SetValue(IsAdornerVisibleProperty, value);}
        }
        
        public FrameworkElement AdornerContent
        {
            get { return (FrameworkElement)GetValue(AdornerContentProperty); }
            set { SetValue(AdornerContentProperty, value); }
        }
        
        public AdornerPlacement HorizontalAdornerPlacement
        {
            get { return (AdornerPlacement)GetValue(HorizontalAdornerPlacementProperty); }
            set { SetValue(HorizontalAdornerPlacementProperty, value); }
        }
        
        public AdornerPlacement VerticalAdornerPlacement
        {
            get { return (AdornerPlacement)GetValue(VerticalAdornerPlacementProperty); }
            set { SetValue(VerticalAdornerPlacementProperty, value); }
        }
       
        public double AdornerOffsetX
        {
            get { return (double)GetValue(AdornerOffsetXProperty); }
            set { SetValue(AdornerOffsetXProperty, value); }
        }
       
        public double AdornerOffsetY
        {
            get { return (double)GetValue(AdornerOffsetYProperty); }
            set { SetValue(AdornerOffsetYProperty, value); }
        }
        
        public bool IsMouseOverShowEnabled
        {
            get { return (bool)GetValue(IsMouseOverShowEnabledProperty); }
            set { SetValue(IsMouseOverShowEnabledProperty, value); }
        }
       
        public double FadeInTime
        {
            get { return (double)GetValue(FadeInTimeProperty); }
            set { SetValue(FadeInTimeProperty, value); }
        }
        
        public double FadeOutTime
        {
            get { return (double) GetValue(FadeOutTimeProperty); }
            set { SetValue(FadeOutTimeProperty, value); }
        }
        
        public double CloseAdornerTimeOut
        {
            get { return (double)GetValue(CloseAdornerTimeOutProperty); }
            set { SetValue(CloseAdornerTimeOutProperty, value); }
        }
       
		#endregion

		#region CommandBindings + ExecutionMethods
		
		private static readonly CommandBinding ShowAdornerCommandBinding    = new CommandBinding(ShowAdornerCommand,   ShowAdornerCommand_Executed);
        private static readonly CommandBinding FadeInAdornerCommandBinding  = new CommandBinding(FadeInAdornerCommand, FadeInAdornerCommand_Executed);
        private static readonly CommandBinding HideAdornerCommandBinding    = new CommandBinding(HideAdornerCommand,   HideAdornerCommand_Executed);
        private static readonly CommandBinding FadeOutAdornerCommandBinding = new CommandBinding(FadeInAdornerCommand, FadeOutAdornerCommand_Executed);

		
		private static void ShowAdornerCommand_Executed (object target, ExecutedRoutedEventArgs e)
		{
			((AdornedControl)target).ShowAdorner();
		}
		
		private static void FadeInAdornerCommand_Executed (object target, ExecutedRoutedEventArgs e)
		{
			((AdornedControl)target).FadeOutAdorner();
		}
		
		private static void HideAdornerCommand_Executed (object target, ExecutedRoutedEventArgs e)
		{
			((AdornedControl)target).HideAdorner();
		}
		
		private static void FadeOutAdornerCommand_Executed (object target, ExecutedRoutedEventArgs e)
		{
			((AdornedControl)target).FadeOutAdorner();
		}

		#endregion

		#region Private Data Members

		private ShowState adornerShowState;        
        private AdornerLayer adornerLayer;      
        private FrameworkElementAdorner adornerContentFrameworkElement;   
     
        private readonly DispatcherTimer closeAdornerTimer;
        
        #endregion

        #region Private/Internal Functions               
       
        private void UpdateAdornerContentDataContext()
        {
            if (AdornerContent != null)
            {
                AdornerContent.DataContext = DataContext;
            }
        }       

		private void ShowOrHideAdornerInternal ()
		{
			if (IsAdornerVisible)
			{
				CreateAndShowAdorner();
			}
			else
			{
				HideAndDestroyAdorner();
			}
		}
        
        private void CreateAndShowAdorner()
        {
            if (adornerContentFrameworkElement != null)            
                return;
            

            if (AdornerContent != null)
            {
                if (adornerLayer == null)                
                    adornerLayer = AdornerLayer.GetAdornerLayer(this);

                if (adornerLayer == null)
					throw new Exception();

				FrameworkElement adornedControl = this; 

				adornerContentFrameworkElement = new FrameworkElementAdorner(AdornerContent, adornedControl,
																			 HorizontalAdornerPlacement, VerticalAdornerPlacement,
																			 AdornerOffsetX, AdornerOffsetY);
				adornerLayer.Add(adornerContentFrameworkElement);

				UpdateAdornerContentDataContext();
            }

            adornerShowState = ShowState.Visible;
        }
       
        private void HideAndDestroyAdorner()
        {
            if (adornerLayer == null || adornerContentFrameworkElement == null)           
                return;
                       
            closeAdornerTimer.Stop();
            adornerLayer.Remove(adornerContentFrameworkElement);
            adornerContentFrameworkElement.DisconnectAdornerContent();

            adornerContentFrameworkElement = null;
            adornerLayer = null;
           
            adornerShowState = ShowState.Hidden;
        }

        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ShowOrHideAdornerInternal();
        }
        
        protected override void OnMouseEnter(MouseEventArgs e) { MouseEnterLogic(); }        
        protected override void OnMouseLeave(MouseEventArgs e) { MouseLeaveLogic(); }

        private void MouseEnterLogic()
        {
            if (!IsMouseOverShowEnabled)           
                return;
            
            closeAdornerTimer.Stop();
            FadeInAdorner();
        }
    
        private void MouseLeaveLogic()
        {
            if (!IsMouseOverShowEnabled)            
                return;            

            closeAdornerTimer.Start();
        }
     
        #endregion
    }
}
