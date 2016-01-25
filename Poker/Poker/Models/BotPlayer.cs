using System.Windows.Forms;

namespace Poker.Models
{
    public class BotPlayer : Player
    {
        public BotPlayer(Label status) 
            : base(status)
        {
            this.Turn = false;
        }
    }
}
