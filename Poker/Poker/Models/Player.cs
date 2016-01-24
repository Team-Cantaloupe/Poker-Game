namespace Poker.Models
{
    using System.Windows.Forms;
    using System.Drawing;
    using Poker.Interfaces;

    public abstract class Player : IPlayer
    {
        private const int initialPlayerChips = 10000;
        private const double initialPlayerHandPower = 0;
        private const double initialPlayerHandType = -1;
        private const int initialCall = 0;
        private const int initialRaise = 0;

        protected Player()
        {
            this.Panel = new Panel
            {
                BackColor = Color.DarkBlue,
                Height = 150,
                Width = 180
            };
            this.Chips = initialPlayerChips;
            this.HandPower = initialPlayerHandPower;
            this.HandType = initialPlayerHandType;
            this.HasFolded = false;
            this.FoldTurn = false;
            this.Call = initialCall;
            this.Raise = initialRaise;
        }

        public Panel Panel { get; private set; }

        public int Chips { get; set; }

        public double HandPower { get; set; }

        public double HandType { get; set; }

        public bool HasFolded { get; set; }

        public bool FoldTurn { get; set; }

        public int Call { get; set; }

        public int Raise { get; set; }

        public Card Card1 { get; set; }

        public Card Card2 { get; set; }
    }
}
