using System;

namespace bytePassion.OnkoTePla.Contracts.Config
{
	public class ClientUserData
	{
        public ClientUserData (string name, Guid id)
		{
			Name = name;			
			Id = id;	        
		}		

		public string Name { get; }		
		public Guid   Id   { get; }				
	}
}