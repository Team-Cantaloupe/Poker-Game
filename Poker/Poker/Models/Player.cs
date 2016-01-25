using System;
using System.Windows.Forms.VisualStyles;

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
        private const int initialPlayerCall = 0;
        private const int initialPlayerRaise = 0;
        private const int CardPanelWidth = 180;
        private const int CardPanelHeight = 150;

        private Label status;

        protected Player(Label status)
        {
            this.Panel = new Panel
            {
                BackColor = Color.DarkBlue,
                Width = CardPanelWidth,
                Height = CardPanelHeight
            };
            this.Chips = initialPlayerChips;
            this.HandPower = initialPlayerHandPower;
            this.HandType = initialPlayerHandType;
            this.HasFolded = false;
            this.GameEnded = false;
            this.Status = status;
            this.Call = initialPlayerCall;
            this.Raise = initialPlayerRaise;
        }

        public Panel Panel { get; private set; }

        public int Chips { get; set; }

        public double HandPower { get; set; }

        public double HandType { get; set; }

        public bool HasFolded { get; set; }

        public bool Turn { get; set; }

        public bool GameEnded { get; set; }

        public int Call { get; set; }

        public int Raise { get; set; }

        public Label Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (value == null)
                {
                   throw new ArgumentNullException("Status cannot be null.");
                }

                this.status = value;
            }
        }

        public Card Card1 { get; set; }

        public Card Card2 { get; set; }
    }
}
