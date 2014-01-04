using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayingCards;
using MvvmHelper;
using System.ComponentModel;

namespace Pelmanism.Model
{
	public class CardStatus : INotifyPropertyChanged
	{
		public CardStatus( RankedCard card )
		{
			this.Card = card;
			IsOpen = false;
		}

		#region IsOpen

		public bool IsOpen
		{
			get { return _IsOpen; }
			set
			{
				_IsOpen = value;
				PropertyChanged.Raise( this, IsOpenName );
			}
		}

		private bool _IsOpen;
		public static readonly string IsOpenName = PropertyName<CardStatus>.Get( _ => _.IsOpen );

		#endregion

		public RankedCard Card { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
