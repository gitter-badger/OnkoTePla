using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bytePassion.FileRename.Enums;


namespace bytePassion.FileRename.RenameLogic
{
	public static class IsMatchFunc
	{
		public static Func<string, bool> Create(SearchType searchType, bool caseSensitiv, string searchString)
		{			
			switch (searchType)
			{
				case SearchType.Characters: return CreateFuncToSearchForCharacterSubsequence(caseSensitiv, searchString);
				case SearchType.WhiteSpace: return CreateFuncToSearchForWhitespaces();
				case SearchType.Special:    return CreateFuncToSearchForSpezialEnding();
			}

			throw new ArgumentException();
		}		

		private static Func<string, bool> CreateFuncToSearchForCharacterSubsequence(bool caseSensitiv, string searchString)
		{			
			if (caseSensitiv)
				return s => SplitSearchString(s).Any(s.Contains);
			else
				return s => SplitSearchString(s).Any(searchPart => s.ToLower().Contains(searchPart.ToLower()));	
		}

		private static IEnumerable<string> SplitSearchString(string searchString)
		{
			if (IsMultipleSearch(searchString))
			{
				return searchString.Split(',')
								   .Select(part => part.Substring(1, part.Length - 2))
								   .ToList();
			}
			else
			{
				return new List<string> {searchString};
			}
		}

		private static bool IsMultipleSearch (string searchString)
		{
			var parts = searchString.Split(',');

			if (parts.Length < 2) return false;

			return parts.Select(substring => substring.Trim())
						.All(trimmedPart => trimmedPart[0] == '"' && 
											trimmedPart[trimmedPart.Length - 1] == '"');
		}

		private static Func<string, bool> CreateFuncToSearchForWhitespaces()
		{
			return s => s.Contains(' ');
		}	

		private static Func<string, bool> CreateFuncToSearchForSpezialEnding()
		{
			// conditions for special ending
			//	- the special ending beginns with '-' or '_'
			//	- minimal length 4
			//	- maximal length 10
			//	- more numbers than letters

			return s =>
			{				
				var fileNameWithoutFileExtension= Path.GetFileNameWithoutExtension(s);

				if (fileNameWithoutFileExtension == null)
					return false;

				if (fileNameWithoutFileExtension.Length < 5)
					return false;

				int i = fileNameWithoutFileExtension.Length - 1;
				while (i > 2)
				{
					char c = fileNameWithoutFileExtension[i];

					if (c == ' ')
						return false;

					if (c == '-' || c == '_')
						break;

					i--;
				}
				if (i == 2)
					return false;

				var specialEnding = fileNameWithoutFileExtension.Substring(i);

				if (specialEnding.Length > 4 || specialEnding.Length < 10)
				{
					var numberCount = 0;					
					var letterCount = 0;

					foreach (char c in specialEnding)
					{
						if (c > 'a' && c < 'z')						
							letterCount++;
						

						if (c > '0' && c < '9')
							numberCount++;
						
					}

					if (numberCount*2 > letterCount)
						return true;
				}

				return false;
			};
		} 
	}
}
