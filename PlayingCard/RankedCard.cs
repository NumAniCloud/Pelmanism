using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayingCards
{
	/// <summary>
	/// 4 種類のスートと 13 段階のランクからなるトランプ カードを表すクラス。
	/// </summary>
	public class RankedCard : PlayingCard
	{
		/// <summary>
		/// トランプ カードのランクの最小値。
		/// </summary>
		public const int RankMin = 1;
		/// <summary>
		/// トランプ カードのランクの最大値。
		/// </summary>
		public const int RankMax = 13;
		/// <summary>
		/// トランプ カードのスートのコレクション。
		/// </summary>
		public static readonly IEnumerable<Suit> Suits;
		/// <summary>
		/// トランプ カードのランクを表す文字列のコレクション。
		/// </summary>
		public static readonly IEnumerable<string> NumberNames;

		int rank_;
		/// <summary>
		/// トランプ カードのスートを取得または設定します。
		/// </summary>
		public Suit Suit { get; set; }
		/// <summary>
		/// トランプ カードのランクを取得または設定します。
		/// </summary>
		public int Rank
		{
			get { return rank_; }
			set
			{
				if (value < 1 || value > 13)
				{
					throw new AggregateException( "ランクが範囲外です。" );
				}
				rank_ = value;
			}
		}
		/// <summary>
		/// 派生クラスでオーバーライドされると、このカードがジョーカーかどうかを表す真偽値を取得します。
		/// </summary>
		public override bool IsJoker
		{
			get { return false; }
		}

		static RankedCard()
		{
			Suits = Enum.GetValues(typeof(Suit)) as Suit[];
			var names = Enumerable.Range(RankMin, RankMax).Select(_ => _.ToString()).ToArray();
			names[0] = "A";
			names[10] = "J";
			names[11] = "Q";
			names[12] = "K";
			NumberNames = names;
		}
		/// <summary>
		/// スートとランクを指定して、PlayingCards.RankedCard の新しいオブジェクトを生成します。
		/// </summary>
		/// <param name="suit">トランプ カードのスート。</param>
		/// <param name="rank">トランプ カードのランク。</param>
		public RankedCard(Suit suit, int rank)
		{
			this.Suit = suit;
			this.Rank = rank;
		}

		public override string ToString()
		{
			return string.Format("{0}-{1}", Suit, NumberNames.ElementAt(Rank - 1));
		}
	}
}
