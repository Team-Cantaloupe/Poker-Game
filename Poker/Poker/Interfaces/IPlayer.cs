namespace Poker.Interfaces
{
    using System.Windows.Forms;

    public interface IPlayer
    {
        Panel Panel { get; }

        int Chips { get; set; }

        double HandPower { get; set; }

        double HandType { get; set; }

        bool HasFolded { get; }

        bool FoldTurn { get; }

        int Call { get; set; }

        int Raise { get; set; }
    }
}
