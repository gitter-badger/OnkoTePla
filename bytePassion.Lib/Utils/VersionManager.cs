using System;
using System.Collections.Generic;
using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.Utils
{	
    public class VersionManager<T> : INotifyPropertyChanged
    {
	    public event EventHandler<T> CurrentVersionChanged; 

	    private readonly uint maximalSavedVersions;

		private readonly LinkedList<T> versions;
	    private LinkedListNode<T> currentVersionPointer;	    

	    private bool undoPossible;
	    private bool redoPossible;

	    public VersionManager(uint maximalSavedVersions)
	    {
		    this.maximalSavedVersions = maximalSavedVersions;
			versions = new LinkedList<T>();
		    currentVersionPointer = null;

		    UndoPossible = false;
		    RedoPossible = false;
	    }

		public VersionManager (uint maximalSavedVersions, T initialVersion) 
			: this(maximalSavedVersions)
		{
			AddnewVersion(initialVersion);
		}

	    public void FixCurrentVersion()
	    {
		    lock (this)
		    {
			    versions.Clear();
			    versions.AddFirst(new LinkedListNode<T>(currentVersionPointer.Value));
			    currentVersionPointer = versions.First;

				CheckIfUndoAndRedoIsPossible();
		    }		    
	    }

	    public bool UndoPossible
	    {
		    get { return undoPossible; }
		    private set { PropertyChanged.ChangeAndNotify(this, ref undoPossible, value); }
	    }
	   
	    public bool RedoPossible
	    {
		    get { return redoPossible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref redoPossible, value); }
	    }		

	    public void AddnewVersion(T newVersion)
	    {
		    lock (this)
		    {
			    var newVersionNode = new LinkedListNode<T>(newVersion);

			    RemoveAllFromEndTo(currentVersionPointer);
			    versions.AddLast(newVersionNode);
			    currentVersionPointer = newVersionNode;

			    if (versions.Count == maximalSavedVersions+1)
				    versions.RemoveFirst();

			    CheckIfUndoAndRedoIsPossible();
		    }
	    }

	    public T CurrentVersion
	    {
		    get
		    {
			    lock (this)
			    {
				    return currentVersionPointer != null // TODO c# 6.0: .?
						? currentVersionPointer.Value 
						: default(T);
			    }
		    }
	    }

	    public void Undo()
	    {
		    lock (this)
		    {
			    if (UndoPossible)
			    {
				    currentVersionPointer = currentVersionPointer.Previous;
				    CheckIfUndoAndRedoIsPossible();

				    RaiseCurrentVersionChanged();
			    } 
				else
					throw new InvalidOperationException("undo not possible");
		    }
	    }	    

	    public void Redo()
	    {
		    lock (this)
		    {
			    if (RedoPossible)
			    {
				    currentVersionPointer = currentVersionPointer.Next;
				    CheckIfUndoAndRedoIsPossible();

					RaiseCurrentVersionChanged();
			    }
				else
					throw new InvalidOperationException("redo not possible");
		    }
	    }

		private void RaiseCurrentVersionChanged ()
		{
			var handler = CurrentVersionChanged;
			if (handler != null)
				handler(this, CurrentVersion);
		}

	    private void CheckIfUndoAndRedoIsPossible()
	    {		   
		    UndoPossible = currentVersionPointer.Previous != null;
		    RedoPossible = currentVersionPointer.Next     != null;		   
	    }

		private void RemoveAllFromEndTo (LinkedListNode<T> node)
		{
			while (versions.Last != node)
				versions.RemoveLast();
		}

	    public event PropertyChangedEventHandler PropertyChanged;
    }
}
