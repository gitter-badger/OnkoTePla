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
				new object[]{ "myfile",    false  },
				new object[]{ "f-34",      false  },
				new object[]{ "file-2344", true   },				
				new object[]{ "file-23aa", true   },
				new object[]{ "file-2baa", false   },
			};
	}
}
