using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayingCards;
using Pelmanism;

namespace Pelmanism.Model
{
	public interface IMessage
	{
	}

	public class PlayingFlow
	{
		public PlayingFlow()
		{
			Cards = new ObservableCollection<CardStatus>();
		}

		public ObservableCollection<CardStatus> Cards { get; private set; }

		public IEnumerator<IMessage> Run()
		{
			var rand = new Random();

			PlayingCard.CreateRankDeck()
				.Where( _ => _.Rank <= 8 )
				.OrderBy( _ => rand.Next() % 2 == 0 )
				.Select( _ => new CardStatus( _ ) )
				.ForEach( Cards.Add );

			while( Cards.Any() )
			{
				var input1 = new SelectCardMessage();
				yield return input1;

				var card1 = input1.Responce;
				card1.IsOpen = true;

				var input2 = new SelectCardMessage();
				yield return input2;

				var card2 = input2.Responce;
				card2.IsOpen = true;

				yield return new WaitMessage( 1000 );

				if( card1.Card.Rank == card2.Card.Rank )
				{
					Cards.Remove( card1 );
					Cards.Remove( card2 );
				}
				else
				{
					card1.IsOpen = false;
					card2.IsOpen = false;
				}
			}

			yield return new WaitMessage( 600 );
			yield return new TerminateMessage();
		}
	}
}
