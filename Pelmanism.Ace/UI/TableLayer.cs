using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using ace;
using Pelmanism.Model;
using PlayingCards;

namespace Pelmanism.Ace.UI
{
	class TableLayer : ace.Layer2D
	{
		#region State
		abstract class State
		{
			public State( TableLayer owner )
			{
				this.Owner = owner;
			}
			protected TableLayer Owner { get; set; }
			public abstract void OnUpdate();
		}
		class NeutralState : State
		{
			public NeutralState( TableLayer owner )
				: base( owner )
			{
			}
			public override void OnUpdate()
			{
			}
		}
		class ChooseCardState : State
		{
			private Action<CardStatus> callback;

			public ChooseCardState( TableLayer owner, Action<CardStatus> callback )
				: base( owner )
			{
				this.callback = callback;
			}

			public override void OnUpdate()
			{
				if( Engine.Mouse.LeftButton.ButtonState == MouseButtonState.Push )
				{
					var hit = Owner.Objects
						.OfType<CardObject>()
						.Where( _ => !_.CardStatus.IsOpen )
						.FirstOrDefault( _ => _.IsHit( Engine.Mouse.Position ) );
					if( hit != null )
					{
						callback( hit.CardStatus );
						Owner.state = new NeutralState( Owner );
					}
				}
			}
		}
		#endregion

		private Dictionary<Suit, string> suitToHead = new Dictionary<Suit, string>()
		{
			{Suit.Club, "c"},
			{Suit.Diamond, "d"},
			{Suit.Heart, "h"},
			{Suit.Spade, "s"}
		};
		private State state { get; set; }

		public TableLayer( PlayingFlow model, SteppingChannel<IMessage> channel )
		{
			model.Cards.CollectionChanged += Cards_CollectionChanged;
			channel.AddMessageHandler<SelectCardMessage, CardStatus>( StartChoosingCard );
			channel.AddMessageHandler<WaitMessage>( SetTimer );
			state = new NeutralState( this );
		}

		protected override void OnUpdated()
		{
			state.OnUpdate();
		}

		private void SetTimer( WaitMessage msg, Action callback )
		{
			AddComponent( new TimerComponent( msg.Milisecond, callback ), "Timer" );
		}

		private void StartChoosingCard( SelectCardMessage msg, Action<CardStatus> callback )
		{
			state = new ChooseCardState( this, callback );
		}


		private void Cards_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			switch( e.Action )
			{
			case NotifyCollectionChangedAction.Add:
				AddCardObject( e );
				break;
			case NotifyCollectionChangedAction.Remove:
				RemoveCardObject( e );
				break;
			default:
				break;
			}
		}

		private void RemoveCardObject( NotifyCollectionChangedEventArgs e )
		{
			var target = e.OldItems[0] as CardStatus;
			var obj = Objects.OfType<CardObject>().FirstOrDefault( _ => _.CardStatus.Card == target.Card );

			if( obj != null )
			{
				// Vanishにバグあり
				//obj.Vanish();
				RemoveObject( obj );
			}
		}

		private void AddCardObject( NotifyCollectionChangedEventArgs e )
		{
			int i = e.NewStartingIndex;
			var cardStatus = e.NewItems[0] as CardStatus;
			var obj = new CardObject( cardStatus )
			{
				FrontTexture = GetTexture( cardStatus.Card ),
				BackTexture = Engine.Graphics.CreateTexture2D( "Textures/z01.png" ),
				Position = new Vector2DF( 20 + i % 8 * 90, 20 + i / 8 * 140 ),
				Scale = new Vector2DF( 0.4f, 0.4f )
			};
			AddObject( obj );
		}

		private Texture2D GetTexture( RankedCard card )
		{
			var path = string.Format( "Textures/{0}{1:00}.png", suitToHead[card.Suit], card.Rank );
			return Engine.Graphics.CreateTexture2D( path );
		}
	}
}
