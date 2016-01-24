using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poker.Models
{
    public class Card
    {
        private readonly Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");

        public Card(int x, int y, Image image, int tag, PictureBox cardHolder)
        {
            this.CardHolder = cardHolder;
            this.CardHolder.Tag = tag;
            this.CardHolder.Image = image;
            this.CardHolder.Location = new Point(x, y);
        }

        public PictureBox CardHolder { get; set; }
    }
}
