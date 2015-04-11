using System.Collections.Generic;
using xIT.OnkoTePla.Contracts.DataObjects;


namespace xIT.OnkoTePla.Contracts.Communication
{
	public interface IPatientInfoProvider
	{
		IReadOnlyList<Patient> GetPatients();
	}
}
