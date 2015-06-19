using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using bytePassion.FileRename.Enums;


namespace bytePassion.FileRename.RenameLogic
{
	public static class ReplaceFunc
	{
		public static Func<string, string> CreateDirectoryReplaceFunc(SearchType searchType, ReplaceType replaceType,
																	  string searchString, string replaceString,
																	  bool caseSensivity)
		{
			return directoryName =>
			{
				var replacement = GetReplacement(replaceType, replaceString);
				return Replace(directoryName, searchType, searchString, replacement, caseSensivity);
			};
		}


		public static Func<string, string> CreateFileReplaceFunc(SearchType searchType, ReplaceType replaceType,
															     string searchString, string replaceString,
																 bool caseSensivity)
		{
			return filename =>
			{
				var extension            = Path.GetExtension(filename);
			    var nameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
				var replacement          = GetReplacement(replaceType, replaceString);

				var replacedFilename = Replace(nameWithoutExtension, 
											   searchType, searchString, 
											   replacement, caseSensivity);

				return replacedFilename + extension;
			};
		}


		private static string GetReplacement(ReplaceType replaceType, string replaceString)
		{			
			switch (replaceType)
			{
				case ReplaceType.Characters: return replaceString; 
				case ReplaceType.WhiteSpace: return " ";
				case ReplaceType.Delete:	 return "";	

				default: throw new ArgumentException();
			}			
		}		

		private static string Replace (string fileOrDirectoryName,
									   SearchType searchType, string searchString, 
									   string replace, 
									   bool caseSensivity)
		{						
			switch (searchType)
			{
				case SearchType.WhiteSpace:
					return ReplaceStringByStringCaseSensitive(fileOrDirectoryName, " ", replace);

				case SearchType.Characters:
				{
					if (caseSensivity)										
						return ReplaceStringByStringCaseSensitive(fileOrDirectoryName, searchString, replace);				
					else					
						return ReplaceStringByStringNonCaseSensitive(fileOrDirectoryName, searchString.ToLower(), replace);										
				}

				case SearchType.Special:
				{
					return ReplaceSpecialEnding(fileOrDirectoryName, replace);
				}
			}

			throw new ArgumentException();
		}


		private static string ReplaceSpecialEnding(string name, string replacement)
		{
			int i = name.Length - 1;
			while (i > 2)
			{
				char c = name[i];

				if (c == '-' || c == '_')
					break;

				i--;
			}

			return name.Substring(0, i) + replacement;
		}


		private static string ReplaceStringByStringCaseSensitive(string name, string searchString, string replacement)
		{
			return name.Replace(searchString, replacement);
		}

		private static string ReplaceStringByStringNonCaseSensitive(string name, string lowerSearchString, string replacement)
		{
			var startIndices = GetReplacementIndices(name, lowerSearchString);

			var builder = new StringBuilder(name.Length);

			int stringPointer = 0;

			foreach (int i in startIndices)
			{
				builder.Append(name.Substring(stringPointer, i-stringPointer));
				stringPointer = i+lowerSearchString.Length;
				builder.Append(replacement);
			}

			builder.Append(name.Substring(stringPointer));

			return builder.ToString();
		}

		private static IEnumerable<int> GetReplacementIndices (string s, string lowerSearchString)
		{
			var resultList = new List<int>();

			var lowerString = s.ToLower();			

			int i = 0;
			while (i < lowerString.Length - lowerSearchString.Length + 1)
			{
				var matchFound = true;

				for (int j = 0; j < lowerSearchString.Length; j++)
				{
					if (lowerString[i + j] != lowerSearchString[j])
					{
						matchFound = false;
						break;
					}
				}

				if (matchFound)
				{
					resultList.Add(i);
					i += lowerSearchString.Length;
				}
				else
					i++;

			}
			return resultList;
		}
	}
}
