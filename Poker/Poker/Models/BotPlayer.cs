using System.Windows.Forms;

namespace Poker.Models
{
    public class BotPlayer : Player
    {
        public BotPlayer(int playerNumber, string name, Label status) 
            : base(playerNumber, name, status)
        {
            this.Turn = false;
        }
    }
}
