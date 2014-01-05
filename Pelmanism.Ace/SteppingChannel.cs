using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pelmanism.Ace
{
	public interface ISteppingChannel
	{
		void Update();
	}

	public class SteppingChannel<TMessage> : ISteppingChannel
	{
		private IEnumerator<TMessage> coroutine { get; set; }
		private List<Action<TMessage>> messageHandlers { get; set; }
		private bool IsWaiting { get; set; }
		private bool IsFinished { get; set; }

		public SteppingChannel( IEnumerator<TMessage> coroutine )
		{
			this.coroutine = coroutine;
			this.messageHandlers = new List<Action<TMessage>>();
			IsWaiting = false;
			IsFinished = false;
		}

		public void Update()
		{
			if( !IsWaiting && !IsFinished )
			{
				IsFinished = !coroutine.MoveNext();
				messageHandlers.ForEach( m => m( coroutine.Current ) );
				IsWaiting = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T">メッセージの型。</typeparam>
		/// <typeparam name="TResult">返り値の型。</typeparam>
		/// <param name="handler">メッセージと、コールバック デリゲートを引数に取るイベント ハンドラ。</param>
		public void AddMessageHandler<T, TResult>( Action<T, Action<TResult>> handler )
			where T : class, TMessage, Model.IResponse<TResult>
		{
			messageHandlers.Add( m =>
			{
				var obj = m as T;
				if( obj != null )
				{
					handler( obj, r => OnResumed( () => obj.Responce = r ) );
				}
			} );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T">メッセージの型。</typeparam>
		/// <param name="handler">メッセージと、コールバック デリゲートを引数に取るイベント ハンドラ。</param>
		public void AddMessageHandler<T>( Action<T, Action> handler ) where T : class, TMessage
		{
			messageHandlers.Add( m =>
			{
				var obj = m as T;
				if( obj != null )
				{
					handler( obj, () => OnResumed() );
				}
			} );
		}

		private void OnResumed( Action additional = null )
		{
			if( IsWaiting )
			{
				IsWaiting = false;
				if( additional != null )
				{
					additional();
				}
			}
			else
			{
				throw new InvalidOperationException( "イベントの待機中でないタイミングにイベントが発行されました。" );
			}
		}
	}
}
