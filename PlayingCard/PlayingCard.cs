using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayingCards
{
	public abstract class PlayingCard
	{
		/// <summary>
		/// 派生クラスでオーバーライドされると、このカードがジョーカーかどうかを表す真偽値を取得します。
		/// </summary>
		public abstract bool IsJoker { get; }

		/// <summary>
		/// トランプ カードのコレクションから、ジョーカーでないものを取り出して新しいコレクションを作成します。
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IEnumerable<RankedCard> GetRanked( IEnumerable<PlayingCard> source )
		{
			return source.Where(_ => !_.IsJoker).Cast<RankedCard>();
		}

		/// <summary>
		/// ジョーカーを含む、トランプ カードのコレクションを生成します。
		/// </summary>
		/// <param name="jokerNum">コレクションが含むジョーカーの枚数。</param>
		/// <returns>ジョーカーを含む、トランプ カードのコレクション。</returns>
		public static IEnumerable<PlayingCard> CreateFullDeck( int jokerNum )
		{
			var deck = new List<PlayingCard>( CreateRankDeck() );
			for (int i = 0; i < jokerNum; i++)
			{
				deck.Add( new JokerCard( i ) );
			}
			return deck;
		}
		/// <summary>
		/// ジョーカーを含まない、52枚のトランプ カードのコレクションを生成します。
		/// </summary>
		/// <returns>ジョーカーを含まないトランプ カードのコレクション。</returns>
		public static IEnumerable<RankedCard> CreateRankDeck()
		{
			List<RankedCard> cards = new List<RankedCard>();
			foreach (var suit in RankedCard.Suits)
			{
				for (int rank = RankedCard.RankMin; rank <= RankedCard.RankMax; rank++)
				{
					cards.Add(new RankedCard(suit, rank));
				}
			}
			return cards;
		}
	}
}
