using System;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Utils;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Contracts.Domain.Events;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;


namespace bytePassion.OnkoTePla.Client.NetworkTest.ViewModels.MainWindow
{
	internal class MainWindowViewModel : IMainWindowViewModel
    {       
        public MainWindowViewModel()
        {
            DoSomeThing = new Command(DoIt);
            Text = "no Text";                       
        }

        private void DoIt()
        {
	        var e = new AppointmentDeleted(new AggregateIdentifier(new Date(1,2,2015), Guid.NewGuid(), 123),
										  12, Guid.NewGuid(), Guid.NewGuid(), 
										  TimeTools.GetCurrentTimeStamp(), ActionTag.UndoAction, 
										  Guid.NewGuid());

	        DomainEvent domainEvent = e;

			var s = DomainEventSerialization.Serialize(Converter.ChangeTo(domainEvent, typeof(DomainEvent)));
		}

        private string text;
        public ICommand DoSomeThing { get; }

        public string Text
        {
            get { return text; }
            private set { PropertyChanged.ChangeAndNotify(this, ref text, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
