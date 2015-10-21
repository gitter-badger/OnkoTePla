using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace bytePassion.Lib.WpfLib.Commands
{

	public class UpdateCommandInformation : IDisposable
	{
		public event EventHandler UpdateOfCanExecuteChangedRequired;

		private IDictionary<INotifyPropertyChanged, IReadOnlyList<string>> propertysLists;

		public UpdateCommandInformation ()
		{
			propertysLists = new ConcurrentDictionary<INotifyPropertyChanged, IReadOnlyList<string>>();
		}

		public UpdateCommandInformation (INotifyPropertyChanged notifyingObject, params string[] properties)
		{
			propertysLists = new ConcurrentDictionary<INotifyPropertyChanged, IReadOnlyList<string>>();
			AddUpdateInformation(notifyingObject, properties);
		}

		public void AddUpdateInformation (INotifyPropertyChanged notifyingObject, params string[] properties)
		{
			propertysLists.Add(notifyingObject, properties.ToList());

			notifyingObject.PropertyChanged += OnPropertyChanged;
		}

		private void OnPropertyChanged (object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			var propertyList = propertysLists[(INotifyPropertyChanged)sender];
			if (propertyList.Contains(propertyChangedEventArgs.PropertyName))
			{
				UpdateOfCanExecuteChangedRequired?.Invoke(this, new EventArgs());
			}
		}

		private void DeregisterEventHandler ()
		{
			foreach (var notifyingObject in propertysLists.Keys)
				notifyingObject.PropertyChanged -= OnPropertyChanged;
		}


		private bool disposed = false;
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~UpdateCommandInformation ()
		{
			Dispose(false);
		}

		private void Dispose (bool disposing)
		{
			if (!disposed)
				if (disposing)
					DeregisterEventHandler();

			disposed = true;
		}
	}
}