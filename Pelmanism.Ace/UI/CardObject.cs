using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ace;
using Pelmanism.Model;

namespace Pelmanism.Ace.UI
{
	class CardObject : TextureObject2D
	{
		public CardObject( CardStatus cardStatus )
		{
			this.CardStatus = cardStatus;
			cardStatus.PropertyChanged += cardStatus_PropertyChanged;
		}

		void cardStatus_PropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
		{
			if( e.PropertyName == CardStatus.IsOpenName )
			{
				Texture = CardStatus.IsOpen ? FrontTexture : BackTexture;
			}
		}

		protected override void OnStart()
		{
			Texture = CardStatus.IsOpen ? FrontTexture : BackTexture;
		}

		public Texture2D BackTexture { get; set; }
		public Texture2D FrontTexture { get; set; }

		public CardStatus CardStatus { get; set; }

		internal bool IsHit( Vector2DF mouse )
		{
			var size = new Vector2DF( Texture.Size.X, Texture.Size.Y ) * Scale;
			return mouse.X >= Position.X
				&& mouse.X <= Position.X + size.X
				&& mouse.Y >= Position.Y
				&& mouse.Y <= Position.Y + size.Y;
		}
	}
}
