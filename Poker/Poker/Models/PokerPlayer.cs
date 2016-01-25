namespace Poker.Models
{
    using System.Windows.Forms;

    public class PokerPlayer : Player
    {
        public PokerPlayer(Label status) 
            : base(status)
        {
            this.Turn = true;
        }
    }
}
