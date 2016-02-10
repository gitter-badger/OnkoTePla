using System;
using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.Config;
using bytePassion.OnkoTePla.Core.Repositories.EventStore;
using bytePassion.OnkoTePla.Core.Repositories.Patients;

namespace bytePassion.OnkoTePla.Server.DataAndService.Data
{
	internal class DataCenter : IDataCenter
	{
		private readonly IConfigurationReadRepository readConfig;
		private readonly IConfigurationWriteRepository writeConfig;
		private readonly IPatientReadRepository patientReadRepository;
		private readonly IPatientWriteRepository patientWriteRepository;
		private readonly IEventStore eventStore;

		public DataCenter(IConfigurationReadRepository readConfig,
						  IConfigurationWriteRepository writeConfig,
						  IPatientReadRepository patientReadRepository,
						  IPatientWriteRepository patientWriteRepository,
						  IEventStore eventStore)
		{
			this.readConfig = readConfig;
			this.writeConfig = writeConfig;
			this.patientReadRepository = patientReadRepository;
			this.patientWriteRepository = patientWriteRepository;
			this.eventStore = eventStore;

			practiceVersionCache = new Dictionary<Guid, IDictionary<Date, uint>>();
		}

		
		public IReadOnlyList<Address> GetAllAvailableAddresses()
		{
			return IpAddressCatcher.GetAllAvailableLocalIpAddresses();
		}

		#region patients

		public IEnumerable<Patient> GetAllPatients()
		{
			return patientReadRepository.GetAllPatients();
		}

		#endregion

		#region users

		public IEnumerable<User> GetAllUsers()
		{
			lock (this)
			{
				return readConfig.GetAllUsers();
			}			
		}

		public void AddNewUser(User newUser)
		{
			lock (this)
			{
				writeConfig.AddUser(newUser);
			}
		}

		public void UpdateUser(User updatedUser)
		{
			lock (this)
			{
				writeConfig.UpdateUser(updatedUser);
			}
		}

		public User GetUser(Guid id)
		{
			lock (this)
			{
				return readConfig.GetUserById(id);
			}
		}

		#endregion

		#region therapyPlaceTypes

		public IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypes()
		{
			lock (this)
			{
				return readConfig.GetAllTherapyPlaceTypes();
			}
		}

		public IEnumerable<TherapyPlaceType> GetAllTherapyPlaceTypesPlusDummy()
		{
			lock (this)
			{
				return readConfig.GetAllTherapyPlaceTypes()
								 .Append(TherapyPlaceType.NoType);
			}
		}

		public void AddNewTherapyPlaceType(TherapyPlaceType newTherapyPlaceType)
		{
			lock (this)
			{
				writeConfig.AddTherapyPlaceType(newTherapyPlaceType);
			}
		}

		public void UpdateTherapyPlaceType(TherapyPlaceType updatedTherapyPlaceType)
		{
			lock (this)
			{
				writeConfig.UpdateTherapyPlaceType(updatedTherapyPlaceType);
			}
		}

		public TherapyPlaceType GetTherapyPlaceType(Guid id)
		{
			lock (this)
			{
				return readConfig.GetTherapyPlaceTypeById(id);
			}
		}

		#endregion

		#region medicalPractices

		public IEnumerable<MedicalPractice> GetAllMedicalPractices ()
		{
			lock (this)
			{
				return readConfig.GetAllMedicalPractices();
			}
		}

		public void AddNewMedicalPractice (MedicalPractice newMedicalPractice)
		{
			lock (this)
			{
				writeConfig.AddMedicalPractice(newMedicalPractice);
			}
		}

		public void UpdateMedicalPractice (MedicalPractice updatedMedicalPractice)
		{
			lock (this)
			{
				writeConfig.RemoveMedicalPractice(updatedMedicalPractice.Id);
				writeConfig.AddMedicalPractice(updatedMedicalPractice);
			}
		}

		public void RemoveMedicalPractice(MedicalPractice medicalPracticeToRemove)
		{
			lock (this)
			{
				writeConfig.RemoveMedicalPractice(medicalPracticeToRemove.Id);
			}
		}

		public MedicalPractice GetMedicalPractice(Guid id)
		{
			lock (this)
			{
				return readConfig.GetMedicalPracticeById(id);
			}
		}

		public MedicalPractice GetMedicalPractice(Guid id, uint version)
		{
			lock (this)
			{
				return readConfig.GetMedicalPracticeByIdAndVersion(id, version);
			}
		}

		private readonly IDictionary<Guid, IDictionary<Date, uint>> practiceVersionCache;
		
		public uint GetMedicalPracticeVersion(Guid id, Date date)
		{
			if (!practiceVersionCache.ContainsKey(id))
				practiceVersionCache.Add(id, new Dictionary<Date, uint>());

			var innerCache = practiceVersionCache[id];

			if (!innerCache.ContainsKey(date))
			{
				var eventStream = eventStore.GetEventStreamForADay(new AggregateIdentifier(date, id));
				var practiceVersion = eventStream.Id.PracticeVersion;

				innerCache.Add(date, practiceVersion);				
			}

			return innerCache[date];
		}

		#endregion

	}
}
