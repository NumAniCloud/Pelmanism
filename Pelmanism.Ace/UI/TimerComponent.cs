using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pelmanism.Ace.UI
{
	class TimerComponent : ace.Layer2DComponent
	{
		private int time { get; set; }
		private int endTime { get; set; }
		private Action callback;

		public TimerComponent( int p, Action callback )
		{
			time = 0;
			this.endTime = p * 60 / 1000;
			this.callback = callback;
		}

		protected override void OnUpdated()
		{
			if( time >= endTime )
			{
				callback();
				Vanish();
			}
			++time;
		}
	}
}
