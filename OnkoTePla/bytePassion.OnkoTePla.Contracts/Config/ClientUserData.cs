using System;
using System.Collections.Generic;

namespace bytePassion.OnkoTePla.Contracts.Config
{
	public class ClientUserData
	{
        public ClientUserData (string name, Guid id, IReadOnlyList<Guid> listOfAccessablePractices)
		{
			Name = name;			
			Id = id;
	        ListOfAccessablePractices = listOfAccessablePractices;
		}		
		
		public string Name { get; }		
		public Guid   Id   { get; }
		
		public IReadOnlyList<Guid> ListOfAccessablePractices { get; } 				
	}
}