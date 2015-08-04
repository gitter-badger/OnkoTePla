using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bytePassion.Lib.Communication.MessageBus;


namespace bytePassion.Lib.Communication.ViewModel
{
	public interface IViewModelMessageHandler<in TViewModelMessage> : IMessageHandler<TViewModelMessage>
	{
	}
}
