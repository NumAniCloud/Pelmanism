using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pelmanism.Model;
using Pelmanism;

namespace Pelmanism.Ace.View
{
	class TableView
	{
		private ITableController controller;

		public TableView( Channel<IMessage> channel, ITableController tableController )
		{
			this.controller = tableController;

			channel.AddMessageHandler<SelectCardMessage, CardStatus>( SelectCard );
			channel.AddMessageHandler<WaitMessage>( Wait );
			channel.AddMessageHandler<TerminateMessage>( Terminate );
		}

		private Task Terminate( TerminateMessage arg )
		{
			Program.IsFinished = true;
			return Task.Delay( 0 );
		}

		private async Task Wait( WaitMessage arg )
		{
			await Task.Delay( arg.Milisecond );
		}

		private async Task<CardStatus> SelectCard( SelectCardMessage arg )
		{
			bool completed = false;
			CardStatus card = null;

			controller.StartChooseCard( result =>
			{
				completed = true;
				card = result;
			} );

			await Task.Run( () =>
			{
				while( !completed ) ;
			} );

			return card;
		}
	}
}
