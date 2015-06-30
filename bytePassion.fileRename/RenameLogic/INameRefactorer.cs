using System;
using System.Collections.Generic;


namespace bytePassion.FileRename.RenameLogic
{

	public interface INameRefactorer
	{
		string GetRefactoredName(string name, IEnumerable<Tuple<int, int>> replacementIndecies);
	}

}