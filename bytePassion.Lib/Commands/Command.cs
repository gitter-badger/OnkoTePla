using System;
using System.Windows.Input;

namespace bytePassion.Lib.Commands
{
	public class Command : ICommand, IDisposable
	{
		private readonly Func<bool> canExecute;
		private readonly Action execute;
		private readonly UpdateCommandInformation updateCommandInformation;


		public Command (Action execute, Func<bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public Command (Action execute, Func<bool> canExecute, UpdateCommandInformation updateCommandInformation)
		{
			this.execute = execute;
			this.canExecute = canExecute;
			this.updateCommandInformation = updateCommandInformation;

			updateCommandInformation.UpdateOfCanExecuteChangedRequired += CanExecuteChangedRequired;
		}

		private void CanExecuteChangedRequired (object sender, EventArgs eventArgs)
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		public bool CanExecute (object parameter=null)
		{
			return canExecute == null || canExecute();
		}

		public void Execute (object parameter=null)
		{
			execute();
		}
		
		public event EventHandler CanExecuteChanged;		

		private bool disposed = false;
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Command ()
		{
			Dispose(false);
		}

		private void Dispose (bool disposing)
		{
			if (!disposed)
				if (disposing)
					if (updateCommandInformation != null)
					{
						updateCommandInformation.UpdateOfCanExecuteChangedRequired -= CanExecuteChangedRequired;
						updateCommandInformation.Dispose();
					}

			disposed = true;
		}
	}
}
