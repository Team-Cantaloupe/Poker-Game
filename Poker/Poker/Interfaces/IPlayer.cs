namespace Poker.Interfaces
{
    using Poker.Models;
    using System.Windows.Forms;

    public interface IPlayer
    {
        Panel Panel { get; }

        int Chips { get; set; }

        double HandPower { get; set; }

        double HandType { get; set; }

        bool HasFolded { get; }

        bool FoldTurn { get; }

        bool GameEnded { get; set; }

        int Call { get; set; }

        int Raise { get; set; }

        Card Card1 { get; set; }

        Card Card2 { get; set; }
    }
}
