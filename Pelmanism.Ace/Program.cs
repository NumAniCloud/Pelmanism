using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ace;
using Pelmanism.Model;

namespace Pelmanism.Ace
{
	class Program
	{
		public static bool IsFinished = false;

		static void Main( string[] args )
		{
			var option = new EngineOption { GraphicsType = GraphicsType.DirectX11, IsFullScreen = false };
			Engine.Initialize( "ペルマニズム", 800, 600, option );

			var model = new PlayingFlow();
			var channel = new SteppingChannel<IMessage>( model.Run() );
			channel.AddMessageHandler<TerminateMessage>( Terminate );

			var scene = new Scene();
			var layer = new UI.TableLayer( model, channel );

			scene.AddLayer(layer);
			Engine.ChangeScene( scene );

			while( Engine.DoEvents() && !IsFinished )
			{
				Engine.Update();
				channel.Update();
			}

			Engine.Terminate();
		}

		private static void Terminate( TerminateMessage msg, Action callback )
		{
			IsFinished = true;
			callback();
		}
	}
}
