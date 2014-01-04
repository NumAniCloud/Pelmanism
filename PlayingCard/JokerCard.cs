using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayingCards
{
	public class JokerCard : PlayingCard
	{
		/// <summary>
		/// 派生クラスでオーバーライドされると、このカードがジョーカーかどうかを表す真偽値を取得します。
		/// </summary>
		public override bool IsJoker
		{
			get { return true; }
		}
		/// <summary>
		/// ジョーカーを区別するためのIDを取得または設定します。
		/// </summary>
		public int JokerID { get; set; }

		/// <summary>
		/// ジョーカーどうしを区別するためのIDを指定して、PlayingCards.JokerCard の新しいオブジェクトを生成します。
		/// </summary>
		/// <param name="jokerID">ジョーカーどうしを区別するためのID。</param>
		public JokerCard( int jokerID )
		{
			this.JokerID = jokerID;
		}

		public override string ToString()
		{
			return string.Format( "Joker({0})", JokerID );
		}
	}
}
