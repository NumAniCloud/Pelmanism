using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pelmanism.Model
{
	public class SelectCardMessage : IMessage, IResponse<CardStatus>
	{
		public CardStatus Responce { get; set; }
	}

	public class WaitMessage : IMessage
	{
		public int Milisecond { get; private set; }

		public WaitMessage( int milisecond )
		{
			this.Milisecond = milisecond;
		}
	}

	public class TerminateMessage : IMessage
	{
	}
}
