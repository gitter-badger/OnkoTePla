using System;
using System.Collections.Generic;
using bytePassion.FileRename.RenameLogic.Helper;


namespace bytePassion.FileRename.RenameLogic.NameAnalyzer
{
	public class NonCaseSensitiveStringAnalyzer : INameAnalyzer
	{
		private readonly string searchString;		

		public NonCaseSensitiveStringAnalyzer(string searchString)
		{
			this.searchString = searchString.ToLower();
			
		}

		public bool IsMatch(string name)
		{			
			return name.ToLower().Contains(searchString);			
		}

		public IEnumerable<Tuple<int, int>> ReplacementIndecies(string name)
		{
			return IndexSearcher.GetIndecies(name.ToLower(), searchString);
		}
	}
}
