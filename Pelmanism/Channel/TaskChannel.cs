using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pelmanism.Model
{
	public abstract class Channel
	{
		public abstract Task RunAsync();
	}

	public class Channel<TMessage> : Channel
	{
		public Channel( IEnumerator<TMessage> coroutine )
		{
			this.coroutine = coroutine;
			this.messageHandlers = new List<Func<TMessage, Task>>();
		}

		public async override Task RunAsync()
		{
			while( coroutine.MoveNext() )
			{
				await Task.WhenAll( messageHandlers.Select( _ => _( coroutine.Current ) ) );
			}
		}

		public void AddMessageHandler<T, TR>( Func<T, Task<TR>> handler ) where T : class, TMessage, IResponse<TR>
		{
			messageHandlers.Add( async m =>
			{
				var obj = m as T;
				if( obj != null )
				{
					obj.Responce = await handler( obj );
				}
			} );
		}

		public void AddMessageHandler<T>( Func<T, Task> handler ) where T : TMessage
		{
			messageHandlers.Add( async m =>
			{
				if( m is T )
				{
					await handler( (T)m );
				}
			} );
		}

		IEnumerator<TMessage> coroutine;

		List<Func<TMessage, Task>> messageHandlers;
	}
}
