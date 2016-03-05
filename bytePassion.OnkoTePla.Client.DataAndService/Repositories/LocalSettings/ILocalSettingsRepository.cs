using System;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.Types.Repository;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.LocalSettings
{
	public interface ILocalSettingsRepository : IPersistable
	{
		bool              IsAutoConnectionEnabled     { get; set; }
		AddressIdentifier AutoConnectionClientAddress { get; set; }
		AddressIdentifier AutoConnectionServerAddress { get; set; }

		Guid LastUsedMedicalPracticeId { get; set; }
		Guid LastLoggedInUserId        { get; set; }
	}
}