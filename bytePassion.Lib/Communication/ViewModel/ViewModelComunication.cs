using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bytePassion.Lib.Communication.MessageBus;


namespace bytePassion.Lib.Communication.ViewModel
{
	public class ViewModelComunication<TMessageBase>
	{
		private readonly IMessageBus<TMessageBase> viewModelMessageBus;

		public ViewModelComunication(IMessageBus<TMessageBase> viewModelMessageBus)
		{
			this.viewModelMessageBus = viewModelMessageBus;
		}
	}
}
