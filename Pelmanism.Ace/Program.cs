using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ace;
using Pelmanism.Ace.View;
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
			var channel = new Channel<IMessage>( model.Run() );

			var scene = new Scene();
			var layer = new UI.TableLayer( model );
			var view = new TableView( channel, layer );

			scene.AddLayer(layer);
			Engine.ChangeScene( scene );

			var channelTask = channel.RunAsync();

			while( Engine.DoEvents() && !IsFinished )
			{
				Engine.Update();
			}

			Engine.Terminate();
		}
	}
}
