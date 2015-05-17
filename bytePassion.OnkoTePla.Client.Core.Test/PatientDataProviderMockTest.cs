using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Communication;
using bytePassion.OnkoTePla.Contracts.Patients;
using Xunit;


namespace bytePassion.OnkoTePla.Client.Core.Test
{
	public class PatientDataProviderMockTest
	{
		[Fact]
		public void Test()
		{
			var testTarget = new PatientDataProviderMock();

			var patients = testTarget.GetPatients() as List<Patient>;
			if (patients != null)
			{
				var firstPatient = patients[0];								
			}
		}
	}
}
