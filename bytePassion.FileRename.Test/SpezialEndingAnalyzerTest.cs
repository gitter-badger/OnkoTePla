using System.Collections.Generic;
using bytePassion.FileRename.RenameLogic.NameAnalyzer;
using Xunit;


namespace bytePassion.FileRename.Test
{
	public class SpezialEndingAnalyzerTest
	{

		[Theory]
		[MemberData("TestDataForSpezialEndingRecognitionTest")]
		public void SpezialEndingRecognitionTest (string name, bool containsSpezialEnding)
		{
			var sut = new SpecialEndingAnalyzer();

			var whiteSpaceRecognized = sut.IsMatch(name);

			Assert.Equal(containsSpezialEnding, whiteSpaceRecognized);
		}

		public static readonly IEnumerable<object[]> TestDataForSpezialEndingRecognitionTest = 
			new[]
			{
				new object[]{ "myfile",    false },
				new object[]{ "f-34",      false },
				new object[]{ "file-2344", true  },								
				new object[]{ "file-2baa", true  },
				new object[]{ "file-2bnm", false },
				new object[]{ "file-8476930938", true },
				new object[]{ "file-84a69c209t", true },
				new object[]{ "file-84ac9b209t", true },
				new object[]{ "file-8qutr9r09t", false },				
				new object[]{ "file-84a69r20879t", false },
				new object[]{ "file_84ac9b209t", true },
				new object[]{ "file_8qutr9r09t", false }
			};
	}
}
