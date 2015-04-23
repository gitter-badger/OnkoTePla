using System.Collections.Generic;
using xIT.OnkoTePla.Client.Core.Communication;
using xIT.OnkoTePla.Contracts.Infrastructure;
using xIT.OnkoTePla.Contracts.Patients;
using Xunit;


namespace xIT.OnkoTePla.Client.Core.Test
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
