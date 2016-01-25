namespace Poker.Models
{
    using System.Windows.Forms;

    public class PokerPlayer : Player
    {
        public PokerPlayer(int playerNumber, string name, Label status) 
            : base(playerNumber,name, status)
        {
            this.Turn = true;
        }
    }
}
