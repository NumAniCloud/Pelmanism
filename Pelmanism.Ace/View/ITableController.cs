using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pelmanism.Model;

namespace Pelmanism.Ace.View
{
	interface ITableController
	{
		void StartChooseCard( Action<CardStatus> callback );
	}
}
