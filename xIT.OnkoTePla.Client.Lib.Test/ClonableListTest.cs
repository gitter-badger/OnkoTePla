using System;
using Xunit;


namespace xIT.OnkoTePla.Client.Lib.Test
{
	public class ClonableListTest
	{
		private class GenericClonableMock : IGenericClonable<GenericClonableMock>
		{
			public GenericClonableMock Clone()
			{
				throw new NotImplementedException();
			}
		}

		[Fact]
		public void FirstTest()
		{
			var testTarget = new ClonableList<GenericClonableMock>
							{
								new GenericClonableMock()
							};

			Assert.Equal(1, testTarget.Count);
		}
	}
}
