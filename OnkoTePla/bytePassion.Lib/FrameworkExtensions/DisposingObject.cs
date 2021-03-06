﻿using System;

namespace bytePassion.Lib.FrameworkExtensions
{
	public abstract class DisposingObject : IDisposable
	{
		protected abstract void CleanUp();

		private bool disposed = false;
		

		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~DisposingObject ()
		{
			Dispose(false);
		}

		private void Dispose (bool disposing)
		{
			if (!disposed)
				if (disposing)
					CleanUp();

			disposed = true;
		}
	}
}
