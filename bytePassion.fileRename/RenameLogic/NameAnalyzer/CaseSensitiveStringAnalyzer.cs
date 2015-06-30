using System;
using System.Collections.Generic;
using bytePassion.FileRename.RenameLogic.Helper;


namespace bytePassion.FileRename.RenameLogic.NameAnalyzer
{
	public class CaseSensitiveStringAnalyzer : INameAnalyzer
	{
		private readonly string searchString;		

		public CaseSensitiveStringAnalyzer(string searchString)
		{
			this.searchString = searchString;
			
		}

		public bool IsMatch(string name)
		{			
			return name.Contains(searchString);			
		}

		public IEnumerable<Tuple<int, int>> ReplacementIndecies(string name)
		{
			return IndexSearcher.GetIndecies(name, searchString);
		}
	}
}
