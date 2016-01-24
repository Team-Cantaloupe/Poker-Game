﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Poker.Interfaces;
using Poker.Models;

namespace Poker
{
    public partial class GameForm : Form
    {
        #region Variables
        ProgressBar progressBar = new ProgressBar();
        public int Nm;

        private readonly IPlayer player = new PokerPlayer();
        private readonly IPlayer bot1 = new BotPlayer();
        private readonly IPlayer bot2 = new BotPlayer();
        private readonly IPlayer bot3 = new BotPlayer();
        private readonly IPlayer bot4 = new BotPlayer();
        private readonly IPlayer bot5 = new BotPlayer();

        private int call = 500;
        int foldedPlayers = 5;
        private double type;
        private double rounds = 0;
        private double Raise = 0;

        bool Pturn = true, B1turn = false, B2turn = false, B3turn = false, B4turn = false, B5turn = false;
        bool PFturn = false, B1Fturn = false, B2Fturn = false, B3Fturn = false, B4Fturn = false, B5Fturn = false;
        bool pFolded, b1Folded, b2Folded, b3Folded, b4Folded, b5Folded;

        private bool intsadded;
        private bool changed;

        private int height;
        private int width;
        private int winners = 0;
        private int Flop = 1;
        private int Turn = 2;
        private int River = 3;
        private int End = 4;
        private int maxLeft = 6;
        private int last = 123;
        private int raisedTurn = 1;

        List<bool?> allPlayersIsActiveCollection = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> ints = new List<int>();

        private bool restart = false;
        bool raising = false;
        Poker.Type sorted;

        /*string[] CardImagePaths ={
                   "Assets\\Cards\\33.png","Assets\\Cards\\22.png",
                    "Assets\\Cards\\29.png","Assets\\Cards\\21.png",
                    "Assets\\Cards\\36.png","Assets\\Cards\\17.png",
                    "Assets\\Cards\\40.png","Assets\\Cards\\16.png",
                    "Assets\\Cards\\5.png","Assets\\Cards\\47.png",
                    "Assets\\Cards\\37.png","Assets\\Cards\\13.png",
                    
                    "Assets\\Cards\\12.png",
                    "Assets\\Cards\\8.png","Assets\\Cards\\18.png",
                    "Assets\\Cards\\15.png","Assets\\Cards\\27.png"};*/

        string[] CardImagePaths = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        private readonly Image[] cardDeck = new Image[52];
        private readonly PictureBox[] cardImageHolder = new PictureBox[52];
        private readonly int[] Reserve = new int[17];

        private readonly Timer timer = new Timer();
        private readonly Timer updates = new Timer();

        private int t = 60;
        private int i;
        private int bigBlind = 500;
        private int smallBlind = 250;
        private int up = 10000000;
        private int turnCount = 0;

        #endregion

        public GameForm()
        {
            //allPlayersIsActiveCollection.Add(PFturn); allPlayersIsActiveCollection.Add(B1Fturn); allPlayersIsActiveCollection.Add(B2Fturn); allPlayersIsActiveCollection.Add(B3Fturn); allPlayersIsActiveCollection.Add(B4Fturn); allPlayersIsActiveCollection.Add(B5Fturn);
            call = bigBlind;
            MaximizeBox = false;
            MinimizeBox = false;
            updates.Start();
            InitializeComponent();
            width = this.Width;
            height = this.Height;
            Shuffle();
            tbPot.Enabled = false;
            tbChips.Enabled = false;
            tbBotChips1.Enabled = false;
            tbBotChips2.Enabled = false;
            tbBotChips3.Enabled = false;
            tbBotChips4.Enabled = false;
            tbBotChips5.Enabled = false;
            tbChips.Text = "Chips : " + player.Chips.ToString();
            tbBotChips1.Text = "Chips : " + bot1.Chips.ToString();
            tbBotChips2.Text = "Chips : " + bot2.Chips.ToString();
            tbBotChips3.Text = "Chips : " + bot3.Chips.ToString();
            tbBotChips4.Text = "Chips : " + bot4.Chips.ToString();
            tbBotChips5.Text = "Chips : " + bot5.Chips.ToString();
            timer.Interval = (1 * 1 * 1000);
            timer.Tick += timer_Tick;
            updates.Interval = (1 * 1 * 100);
            updates.Tick += Update_Tick;
            tbBB.Visible = true;
            tbSB.Visible = true;
            bBB.Visible = true;
            bSB.Visible = true;
            tbBB.Visible = true;
            tbSB.Visible = true;
            bBB.Visible = true;
            bSB.Visible = true;
            tbBB.Visible = false;
            tbSB.Visible = false;
            bBB.Visible = false;
            bSB.Visible = false;
            tbRaise.Text = (bigBlind * 2).ToString();
        }

        async Task Shuffle()
        {
            allPlayersIsActiveCollection.Add(PFturn);
            allPlayersIsActiveCollection.Add(B1Fturn);
            allPlayersIsActiveCollection.Add(B2Fturn);
            allPlayersIsActiveCollection.Add(B3Fturn);
            allPlayersIsActiveCollection.Add(B4Fturn);
            allPlayersIsActiveCollection.Add(B5Fturn);

            buttonCall.Enabled = false;
            buttonRaise.Enabled = false;
            buttonFold.Enabled = false;
            buttonCheck.Enabled = false;
            MaximizeBox = false;
            MinimizeBox = false;
            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");
            int horizontal = 580, vertical = 480;
            this.CardDeckInit();

            for (i = 0; i < GameConstants.AllCardsInGame; i++)
            {
                cardDeck[i] = Image.FromFile(CardImagePaths[i]);
                var charsToRemove = new string[] { "Assets\\Cards\\", ".png" };
                foreach (var c in charsToRemove)
                {
                    CardImagePaths[i] = CardImagePaths[i].Replace(c, string.Empty);
                }

                Reserve[i] = int.Parse(CardImagePaths[i]) - 1;
                cardImageHolder[i] = new PictureBox();
                cardImageHolder[i].SizeMode = PictureBoxSizeMode.StretchImage;
                cardImageHolder[i].Height = 130;
                cardImageHolder[i].Width = 80;
                this.Controls.Add(cardImageHolder[i]);
                cardImageHolder[i].Name = "pb" + i.ToString();
                // await Task.Delay(200);
            }

            for (int index = 0; index < GameConstants.NumberOfPlayerCards; index += 2)
            {
                if (index < 2)
                {
                    DealCardsToCurrentPlayer(player, index, horizontal, vertical, AnchorStyles.Bottom);
                }

                if (bot1.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 2 && index < 4)
                    {
                        horizontal = 15;
                        vertical = 420;

                        DealCardsToCurrentPlayer(bot1, index, horizontal, vertical, AnchorStyles.Bottom, AnchorStyles.Left);
                    }
                }

                if (bot2.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 4 && index < 6)
                    {
                        horizontal = 75;
                        vertical = 65;

                        DealCardsToCurrentPlayer(bot2, index, horizontal, vertical, AnchorStyles.Top, AnchorStyles.Left);
                    }
                }

                if (bot3.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 6 && index < 8)
                    {
                        horizontal = 590;
                        vertical = 25;

                        DealCardsToCurrentPlayer(bot3, index, horizontal, vertical, AnchorStyles.Top);
                    }
                }

                if (bot4.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 8 && index < 10)
                    {
                        horizontal = 1115;
                        vertical = 65;

                        DealCardsToCurrentPlayer(bot4, index, horizontal, vertical, AnchorStyles.Top, AnchorStyles.Right);
                    }
                }

                if (bot5.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 10 && index < 12)
                    {
                        horizontal = 1160;
                        vertical = 420;

                        DealCardsToCurrentPlayer(bot5, index, horizontal, vertical, AnchorStyles.Bottom, AnchorStyles.Right);
                    }
                }
            }

            horizontal = 410;
            vertical = 265;

            for (int index = GameConstants.FirstBoardCardIndex; index < GameConstants.AllCardsInGame; index++)
            {
                DealBoardCard(index, horizontal, vertical, backImage);

                horizontal += GameConstants.BoardCardsOffset;

                //if (bot1.Chips <= 0)
                //{
                //    B1Fturn = true;
                //    cardImageHolder[2].Visible = false;
                //    cardImageHolder[3].Visible = false;
                //}
                //else
                //{
                //    B1Fturn = false;
                //    if (i == 3)
                //    {
                //        if (cardImageHolder[3] != null)
                //        {
                //            cardImageHolder[2].Visible = true;
                //            cardImageHolder[3].Visible = true;
                //        }
                //    }
                //}
                //if (bot2.Chips <= 0)
                //{
                //    B2Fturn = true;
                //    cardImageHolder[4].Visible = false;
                //    cardImageHolder[5].Visible = false;
                //}
                //else
                //{
                //    B2Fturn = false;
                //    if (i == 5)
                //    {
                //        if (cardImageHolder[5] != null)
                //        {
                //            cardImageHolder[4].Visible = true;
                //            cardImageHolder[5].Visible = true;
                //        }
                //    }
                //}
                //if (bot3.Chips <= 0)
                //{
                //    B3Fturn = true;
                //    cardImageHolder[6].Visible = false;
                //    cardImageHolder[7].Visible = false;
                //}
                //else
                //{
                //    B3Fturn = false;
                //    if (i == 7)
                //    {
                //        if (cardImageHolder[7] != null)
                //        {
                //            cardImageHolder[6].Visible = true;
                //            cardImageHolder[7].Visible = true;
                //        }
                //    }
                //}
                //if (bot4.Chips <= 0)
                //{
                //    B4Fturn = true;
                //    cardImageHolder[8].Visible = false;
                //    cardImageHolder[9].Visible = false;
                //}
                //else
                //{
                //    B4Fturn = false;
                //    if (i == 9)
                //    {
                //        if (cardImageHolder[9] != null)
                //        {
                //            cardImageHolder[8].Visible = true;
                //            cardImageHolder[9].Visible = true;
                //        }
                //    }
                //}
                //if (bot5.Chips <= 0)
                //{
                //    B5Fturn = true;
                //    cardImageHolder[10].Visible = false;
                //    cardImageHolder[11].Visible = false;
                //}
                //else
                //{
                //    B5Fturn = false;
                //    if (i == 11)
                //    {
                //        if (cardImageHolder[11] != null)
                //        {
                //            cardImageHolder[10].Visible = true;
                //            cardImageHolder[11].Visible = true;
                //        }
                //    }
                //}
                if (index == 16)
                {
                    if (!restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }
                    timer.Start();
                }
            }

            if (foldedPlayers == 5)
            {
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                foldedPlayers = 5;
            }
            if (i == 17)
            {
                buttonRaise.Enabled = true;
                buttonCall.Enabled = true;
                buttonRaise.Enabled = true;
                buttonRaise.Enabled = true;
                buttonFold.Enabled = true;
            }
        }

        private void DealBoardCard(int index, int horizontal, int vertical, Image cardImage)
        {
            cardImageHolder[index].Tag = Reserve[index];
            cardImageHolder[index].Anchor = AnchorStyles.None;
            cardImageHolder[index].Image = cardImage;
            cardImageHolder[index].Location = new Point(horizontal, vertical);
        }

        private void DealCardsToCurrentPlayer(IPlayer currentPlayer, int cardIndex, int x, int y, AnchorStyles style1, AnchorStyles style2 = AnchorStyles.None)
        {
            cardImageHolder[cardIndex].Anchor = (style1 | style2);
            currentPlayer.Card1 = new Card(x, y, cardDeck[cardIndex], Reserve[cardIndex], cardImageHolder[cardIndex]);
            x += currentPlayer.Card1.CardHolder.Width;
            cardImageHolder[cardIndex + 1].Anchor = (style1 | style2);
            currentPlayer.Card2 = new Card(x, y, cardDeck[cardIndex + 1], Reserve[cardIndex + 1], cardImageHolder[cardIndex + 1]);
            this.Controls.Add(currentPlayer.Panel);
            currentPlayer.Panel.Location = new Point(currentPlayer.Card1.CardHolder.Left - 10, currentPlayer.Card1.CardHolder.Top - 10);
            currentPlayer.Panel.Visible = false;
        }

        private void CardDeckInit()
        {
            Random rand = new Random();
            for (int cardIndex = CardImagePaths.Length; cardIndex > 0; cardIndex--)
            {
                int randomNumber = rand.Next(cardIndex);
                var cardImagePath = CardImagePaths[randomNumber];
                CardImagePaths[randomNumber] = CardImagePaths[cardIndex - 1];
                CardImagePaths[cardIndex - 1] = cardImagePath;
            }
        }

        async Task Turns()
        {
            #region Rotating
            if (!PFturn)
            {
                if (Pturn)
                {
                    FixCall(pStatus, player.Call, player.Raise, 1);
                    //MessageBox.Show("Player's Turn");
                    pbTimer.Visible = true;
                    pbTimer.Value = 1000;
                    t = 60;
                    up = 10000000;
                    timer.Start();
                    buttonRaise.Enabled = true;
                    buttonCall.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    turnCount++;
                    FixCall(pStatus, player.Call, player.Raise, 2);
                }
            }
            if (PFturn || !Pturn)
            {
                await AllIn();
                if (PFturn && !pFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        allPlayersIsActiveCollection.RemoveAt(0);
                        allPlayersIsActiveCollection.Insert(0, null);
                        maxLeft--;
                        pFolded = true;
                    }
                }

                await CheckRaise(0, 0);
                pbTimer.Visible = false;
                buttonRaise.Enabled = false;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                timer.Stop();
                B1turn = true;
                if (!B1Fturn)
                {
                    if (B1turn)
                    {
                        FixCall(b1Status, bot1.Call, bot1.Raise, 1);
                        FixCall(b1Status, bot1.Call, bot1.Raise, 2);
                        Rules(2, 3, "Bot 1", bot1.HandType, bot1.HandPower, B1Fturn);
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, bot1.Chips, ref B1turn, ref  B1Fturn, b1Status, 0, bot1.HandPower, bot1.HandType);
                        turnCount++;
                        last = 1;
                        B1turn = false;
                        B2turn = true;
                    }
                }
                if (B1Fturn && !b1Folded)
                {
                    allPlayersIsActiveCollection.RemoveAt(1);
                    allPlayersIsActiveCollection.Insert(1, null);
                    maxLeft--;
                    b1Folded = true;
                }
                if (B1Fturn || !B1turn)
                {
                    await CheckRaise(1, 1);
                    B2turn = true;
                }
                if (!B2Fturn)
                {
                    if (B2turn)
                    {
                        FixCall(b2Status, bot2.Call, bot2.Raise, 1);
                        FixCall(b2Status, bot2.Call, bot2.Raise, 1);
                        Rules(4, 5, "Bot 2", bot2.HandType, bot2.HandPower, B2Fturn);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, bot2.Chips, ref B2turn, ref  B2Fturn, b2Status, 1, bot2.HandPower, bot2.HandType);
                        turnCount++;
                        last = 2;
                        B2turn = false;
                        B3turn = true;
                    }
                }
                if (B2Fturn && !b2Folded)
                {
                    allPlayersIsActiveCollection.RemoveAt(2);
                    allPlayersIsActiveCollection.Insert(2, null);
                    maxLeft--;
                    b2Folded = true;
                }
                if (B2Fturn || !B2turn)
                {
                    await CheckRaise(2, 2);
                    B3turn = true;
                }
                if (!B3Fturn)
                {
                    if (B3turn)
                    {
                        FixCall(b3Status, bot3.Call, bot3.Raise, 1);
                        FixCall(b3Status, bot3.Call, bot3.Raise, 2);
                        Rules(6, 7, "Bot 3", bot3.HandType, bot3.HandPower, B3Fturn);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, bot3.Chips, ref B3turn, ref  B3Fturn, b3Status, 2, bot3.HandPower, bot3.HandType);
                        turnCount++;
                        last = 3;
                        B3turn = false;
                        B4turn = true;
                    }
                }
                if (B3Fturn && !b3Folded)
                {
                    allPlayersIsActiveCollection.RemoveAt(3);
                    allPlayersIsActiveCollection.Insert(3, null);
                    maxLeft--;
                    b3Folded = true;
                }
                if (B3Fturn || !B3turn)
                {
                    await CheckRaise(3, 3);
                    B4turn = true;
                }
                if (!B4Fturn)
                {
                    if (B4turn)
                    {
                        FixCall(b4Status, bot4.Call, bot4.Raise, 1);
                        FixCall(b4Status, bot4.Call, bot4.Raise, 2);
                        Rules(8, 9, "Bot 4", bot4.HandType, bot4.HandPower, B4Fturn);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, bot4.Chips, ref B4turn, ref  B4Fturn, b4Status, 3, bot4.HandPower, bot4.HandType);
                        turnCount++;
                        last = 4;
                        B4turn = false;
                        B5turn = true;
                    }
                }
                if (B4Fturn && !b4Folded)
                {
                    allPlayersIsActiveCollection.RemoveAt(4);
                    allPlayersIsActiveCollection.Insert(4, null);
                    maxLeft--;
                    b4Folded = true;
                }
                if (B4Fturn || !B4turn)
                {
                    await CheckRaise(4, 4);
                    B5turn = true;
                }
                if (!B5Fturn)
                {
                    if (B5turn)
                    {
                        FixCall(b5Status, bot5.Call, bot5.Raise, 1);
                        FixCall(b5Status, bot5.Call, bot5.Raise, 2);
                        Rules(10, 11, "Bot 5", bot5.HandType, bot5.HandPower, B5Fturn);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, bot5.Chips, ref B5turn, ref  B5Fturn, b5Status, 4, bot5.HandPower, bot5.HandType);
                        turnCount++;
                        last = 5;
                        B5turn = false;
                    }
                }
                if (B5Fturn && !b5Folded)
                {
                    allPlayersIsActiveCollection.RemoveAt(5);
                    allPlayersIsActiveCollection.Insert(5, null);
                    maxLeft--;
                    b5Folded = true;
                }
                if (B5Fturn || !B5turn)
                {
                    await CheckRaise(5, 5);
                    Pturn = true;
                }
                if (PFturn && !pFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        allPlayersIsActiveCollection.RemoveAt(0);
                        allPlayersIsActiveCollection.Insert(0, null);
                        maxLeft--;
                        pFolded = true;
                    }
                }
            #endregion
                await AllIn();
                if (!restart)
                {
                    await Turns();
                }
                restart = false;
            }
        }

        void Rules(int c1, int c2, string currentText, double currentHandType, double handPower, bool foldedTurn)
        {
            //if (c1 == 0 && c2 == 1)
            //{
            //}
            if (!foldedTurn || c1 == 0 && c2 == 1 && pStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false, vf = false;
                int[] Straight1 = new int[5];
                int[] Straight = new int[7];
                Straight[0] = Reserve[c1];
                Straight[1] = Reserve[c2];
                Straight1[0] = Straight[2] = Reserve[12];
                Straight1[1] = Straight[3] = Reserve[13];
                Straight1[2] = Straight[4] = Reserve[14];
                Straight1[3] = Straight[5] = Reserve[15];
                Straight1[4] = Straight[6] = Reserve[16];
                var a = Straight.Where(o => o % 4 == 0).ToArray();
                var b = Straight.Where(o => o % 4 == 1).ToArray();
                var c = Straight.Where(o => o % 4 == 2).ToArray();
                var d = Straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(Straight); Array.Sort(st1); Array.Sort(st2); Array.Sort(st3); Array.Sort(st4);
                #endregion
                for (i = 0; i < 16; i++)
                {
                    if (Reserve[i] == int.Parse(cardImageHolder[c1].Tag.ToString()) && Reserve[i + 1] == int.Parse(cardImageHolder[c2].Tag.ToString()))
                    {
                        //Pair from Hand currentHandType = 1

                        rPairFromHand(currentHandType, handPower);

                        #region Pair or Two Pair from Table currentHandType = 2 || 0
                        rPairTwoPair(currentHandType, handPower);
                        #endregion

                        #region Two Pair currentHandType = 2
                        rTwoPair(currentHandType, handPower);
                        #endregion

                        #region Three of a kind currentHandType = 3
                        rThreeOfAKind(currentHandType, handPower, Straight);
                        #endregion

                        #region Straight currentHandType = 4
                        rStraight(currentHandType, handPower, Straight);
                        #endregion

                        #region Flush currentHandType = 5 || 5.5
                        rFlush(currentHandType, handPower, ref vf, Straight1);
                        #endregion

                        #region Full House currentHandType = 6
                        rFullHouse(currentHandType, handPower, ref done, Straight);
                        #endregion

                        #region Four of a Kind currentHandType = 7
                        rFourOfAKind(currentHandType, handPower, Straight);
                        #endregion

                        #region Straight Flush currentHandType = 8 || 9
                        rStraightFlush(currentHandType, handPower, st1, st2, st3, st4);
                        #endregion

                        #region High Card currentHandType = -1
                        rHighCard(currentHandType, handPower);
                        #endregion
                    }
                }
            }
        }
        private void rStraightFlush(double currentHandType, double handPower, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (currentHandType >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        currentHandType = 8;
                        handPower = (st1.Max()) / 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        currentHandType = 9;
                        handPower = (st1.Max()) / 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        currentHandType = 8;
                        handPower = (st2.Max()) / 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        currentHandType = 9;
                        handPower = (st2.Max()) / 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        currentHandType = 8;
                        handPower = (st3.Max()) / 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        currentHandType = 9;
                        handPower = (st3.Max()) / 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        currentHandType = 8;
                        handPower = (st4.Max()) / 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        currentHandType = 9;
                        handPower = (st4.Max()) / 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rFourOfAKind(double currentHandType, double handPower, int[] Straight)
        {
            if (currentHandType >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 && Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        currentHandType = 7;
                        handPower = (Straight[j] / 4) * 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 7 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        currentHandType = 7;
                        handPower = 13 * 4 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 7 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rFullHouse(double currentHandType, double handPower, ref bool done, int[] Straight)
        {
            if (currentHandType >= -1)
            {
                type = handPower;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                currentHandType = 6;
                                handPower = 13 * 2 + currentHandType * 100;
                                Win.Add(new Type() { Power = handPower, Current = 6 });
                                sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                            if (fh.Max() / 4 > 0)
                            {
                                currentHandType = 6;
                                handPower = fh.Max() / 4 * 2 + currentHandType * 100;
                                Win.Add(new Type() { Power = handPower, Current = 6 });
                                sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }
                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                handPower = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                handPower = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }
                if (currentHandType != 6)
                {
                    handPower = type;
                }
            }
        }

        private void rFlush(double currentHandType, double handPower, ref bool vf, int[] Straight1)
        {
            if (currentHandType >= -1)
            {
                var f1 = Straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = Straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = Straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = Straight1.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i] / 4 > f1.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i + 1] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f1.Max() / 4 && Reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = f1.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i] / 4 > f1.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentHandType = 5;
                            handPower = f1.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i + 1] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentHandType = 5;
                            handPower = f1.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 5)
                {
                    if (Reserve[i] % 4 == f1[0] % 4 && Reserve[i] / 4 > f1.Min() / 4)
                    {
                        currentHandType = 5;
                        handPower = Reserve[i] + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[i + 1] % 4 == f1[0] % 4 && Reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        currentHandType = 5;
                        handPower = Reserve[i + 1] + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f1.Min() / 4 && Reserve[i + 1] / 4 < f1.Min())
                    {
                        currentHandType = 5;
                        handPower = f1.Max() + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i] / 4 > f2.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i + 1] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f2.Max() / 4 && Reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = f2.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i] / 4 > f2.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentHandType = 5;
                            handPower = f2.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i + 1] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentHandType = 5;
                            handPower = f2.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 5)
                {
                    if (Reserve[i] % 4 == f2[0] % 4 && Reserve[i] / 4 > f2.Min() / 4)
                    {
                        currentHandType = 5;
                        handPower = Reserve[i] + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[i + 1] % 4 == f2[0] % 4 && Reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        currentHandType = 5;
                        handPower = Reserve[i + 1] + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f2.Min() / 4 && Reserve[i + 1] / 4 < f2.Min())
                    {
                        currentHandType = 5;
                        handPower = f2.Max() + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i] / 4 > f3.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i + 1] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f3.Max() / 4 && Reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = f3.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i] / 4 > f3.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentHandType = 5;
                            handPower = f3.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i + 1] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentHandType = 5;
                            handPower = f3.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 5)
                {
                    if (Reserve[i] % 4 == f3[0] % 4 && Reserve[i] / 4 > f3.Min() / 4)
                    {
                        currentHandType = 5;
                        handPower = Reserve[i] + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[i + 1] % 4 == f3[0] % 4 && Reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        currentHandType = 5;
                        handPower = Reserve[i + 1] + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f3.Min() / 4 && Reserve[i + 1] / 4 < f3.Min())
                    {
                        currentHandType = 5;
                        handPower = f3.Max() + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i] / 4 > f4.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i + 1] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f4.Max() / 4 && Reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = f4.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i] / 4 > f4.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentHandType = 5;
                            handPower = f4.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            currentHandType = 5;
                            handPower = Reserve[i + 1] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            currentHandType = 5;
                            handPower = f4.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 5)
                {
                    if (Reserve[i] % 4 == f4[0] % 4 && Reserve[i] / 4 > f4.Min() / 4)
                    {
                        currentHandType = 5;
                        handPower = Reserve[i] + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[i + 1] % 4 == f4[0] % 4 && Reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        currentHandType = 5;
                        handPower = Reserve[i + 1] + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f4.Min() / 4 && Reserve[i + 1] / 4 < f4.Min())
                    {
                        currentHandType = 5;
                        handPower = f4.Max() + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                //ace
                if (f1.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentHandType = 5.5;
                        handPower = 13 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        currentHandType = 5.5;
                        handPower = 13 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f2.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentHandType = 5.5;
                        handPower = 13 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        currentHandType = 5.5;
                        handPower = 13 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f3.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentHandType = 5.5;
                        handPower = 13 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        currentHandType = 5.5;
                        handPower = 13 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f4.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        currentHandType = 5.5;
                        handPower = 13 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        currentHandType = 5.5;
                        handPower = 13 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rStraight(double currentHandType, double handPower, int[] Straight)
        {
            if (currentHandType >= -1)
            {
                var op = Straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            currentHandType = 4;
                            handPower = op.Max() + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            currentHandType = 4;
                            handPower = op[j + 4] + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }
                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        currentHandType = 4;
                        handPower = 13 + currentHandType * 100;
                        Win.Add(new Type() { Power = handPower, Current = 4 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rThreeOfAKind(double currentHandType, double handPower, int[] Straight)
        {
            if (currentHandType >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            currentHandType = 3;
                            handPower = 13 * 3 + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            currentHandType = 3;
                            handPower = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        private void rTwoPair(double currentHandType, double handPower)
        {
            if (currentHandType >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (Reserve[i] / 4 != Reserve[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            if (tc - k >= 12)
                            {
                                if (Reserve[i] / 4 == Reserve[tc] / 4 && Reserve[i + 1] / 4 == Reserve[tc - k] / 4 ||
                                    Reserve[i + 1] / 4 == Reserve[tc] / 4 && Reserve[i] / 4 == Reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[i] / 4 == 0)
                                        {
                                            currentHandType = 2;
                                            handPower = 13 * 4 + (Reserve[i + 1] / 4) * 2 + currentHandType * 100;
                                            Win.Add(new Type() { Power = handPower, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i + 1] / 4 == 0)
                                        {
                                            currentHandType = 2;
                                            handPower = 13 * 4 + (Reserve[i] / 4) * 2 + currentHandType * 100;
                                            Win.Add(new Type() { Power = handPower, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i + 1] / 4 != 0 && Reserve[i] / 4 != 0)
                                        {
                                            currentHandType = 2;
                                            handPower = (Reserve[i] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + currentHandType * 100;
                                            Win.Add(new Type() { Power = handPower, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void rPairTwoPair(double currentHandType, double handPower)
        {
            if (currentHandType >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }
                        if (tc - k >= 12)
                        {
                            if (Reserve[tc] / 4 == Reserve[tc - k] / 4)
                            {
                                if (Reserve[tc] / 4 != Reserve[i] / 4 && Reserve[tc] / 4 != Reserve[i + 1] / 4 && currentHandType == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[i + 1] / 4 == 0)
                                        {
                                            currentHandType = 2;
                                            handPower = (Reserve[i] / 4) * 2 + 13 * 4 + currentHandType * 100;
                                            Win.Add(new Type() { Power = handPower, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i] / 4 == 0)
                                        {
                                            currentHandType = 2;
                                            handPower = (Reserve[i + 1] / 4) * 2 + 13 * 4 + currentHandType * 100;
                                            Win.Add(new Type() { Power = handPower, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i + 1] / 4 != 0)
                                        {
                                            currentHandType = 2;
                                            handPower = (Reserve[tc] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + currentHandType * 100;
                                            Win.Add(new Type() { Power = handPower, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i] / 4 != 0)
                                        {
                                            currentHandType = 2;
                                            handPower = (Reserve[tc] / 4) * 2 + (Reserve[i] / 4) * 2 + currentHandType * 100;
                                            Win.Add(new Type() { Power = handPower, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                                if (currentHandType == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (Reserve[i] / 4 > Reserve[i + 1] / 4)
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                currentHandType = 0;
                                                handPower = 13 + Reserve[i] / 4 + currentHandType * 100;
                                                Win.Add(new Type() { Power = handPower, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentHandType = 0;
                                                handPower = Reserve[tc] / 4 + Reserve[i] / 4 + currentHandType * 100;
                                                Win.Add(new Type() { Power = handPower, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                currentHandType = 0;
                                                handPower = 13 + Reserve[i + 1] + currentHandType * 100;
                                                Win.Add(new Type() { Power = handPower, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                currentHandType = 0;
                                                handPower = Reserve[tc] / 4 + Reserve[i + 1] / 4 + currentHandType * 100;
                                                Win.Add(new Type() { Power = handPower, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                    }
                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void rPairFromHand(double currentHandType, double handPower)
        {
            if (currentHandType >= -1)
            {
                bool msgbox = false;
                if (Reserve[i] / 4 == Reserve[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (Reserve[i] / 4 == 0)
                        {
                            currentHandType = 1;
                            handPower = 13 * 4 + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            currentHandType = 1;
                            handPower = (Reserve[i + 1] / 4) * 4 + currentHandType * 100;
                            Win.Add(new Type() { Power = handPower, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                    msgbox = true;
                }
                for (int tc = 16; tc >= 12; tc--)
                {
                    if (Reserve[i + 1] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[i + 1] / 4 == 0)
                            {
                                currentHandType = 1;
                                handPower = 13 * 4 + Reserve[i] / 4 + currentHandType * 100;
                                Win.Add(new Type() { Power = handPower, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentHandType = 1;
                                handPower = (Reserve[i + 1] / 4) * 4 + Reserve[i] / 4 + currentHandType * 100;
                                Win.Add(new Type() { Power = handPower, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                    if (Reserve[i] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[i] / 4 == 0)
                            {
                                currentHandType = 1;
                                handPower = 13 * 4 + Reserve[i + 1] / 4 + currentHandType * 100;
                                Win.Add(new Type() { Power = handPower, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                currentHandType = 1;
                                handPower = (Reserve[tc] / 4) * 4 + Reserve[i + 1] / 4 + currentHandType * 100;
                                Win.Add(new Type() { Power = handPower, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }
        private void rHighCard(double currentHandType, double handPower)
        {
            if (currentHandType == -1)
            {
                if (Reserve[i] / 4 > Reserve[i + 1] / 4)
                {
                    currentHandType = -1;
                    handPower = Reserve[i] / 4;
                    Win.Add(new Type() { Power = handPower, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    currentHandType = -1;
                    handPower = Reserve[i + 1] / 4;
                    Win.Add(new Type() { Power = handPower, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                if (Reserve[i] / 4 == 0 || Reserve[i + 1] / 4 == 0)
                {
                    currentHandType = -1;
                    handPower = 13;
                    Win.Add(new Type() { Power = handPower, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        void Winner(double current, double Power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (cardImageHolder[j].Visible)
                    cardImageHolder[j].Image = cardDeck[j];
            }
            if (current == sorted.Current)
            {
                if (Power == sorted.Power)
                {
                    winners++;
                    CheckWinners.Add(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }
                    if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }
                    if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }
                    if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }
                    if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }
                    if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }
                    if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }
                    if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }
                    if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }
                    if (current == 9)
                    {
                        MessageBox.Show(currentText + " Royal Flush ! ");
                    }
                }
            }
            if (currentText == lastly)//lastfixed
            {
                if (winners > 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        player.Chips += int.Parse(tbPot.Text) / winners;
                        tbChips.Text = player.Chips.ToString();
                        //pPanel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips1.Text = bot1.Chips.ToString();
                        //b1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips2.Text = bot2.Chips.ToString();
                        //b2Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips3.Text = bot3.Chips.ToString();
                        //b3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips4.Text = bot4.Chips.ToString();
                        //b4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5.Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips5.Text = bot5.Chips.ToString();
                        //b5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        player.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //pPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b2Panel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5.Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //b5Panel.Visible = true;
                    }
                }
            }
        }
        async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (raising)
            {
                turnCount = 0;
                raising = false;
                raisedTurn = currentTurn;
                changed = true;
            }
            else
            {
                if (turnCount >= maxLeft - 1 || !changed && turnCount == maxLeft)
                {
                    if (currentTurn == raisedTurn - 1 || !changed && turnCount == maxLeft || raisedTurn == 0 && currentTurn == 5)
                    {
                        changed = false;
                        turnCount = 0;
                        Raise = 0;
                        call = 0;
                        raisedTurn = 123;
                        rounds++;
                        if (!PFturn)
                            pStatus.Text = "";
                        if (!B1Fturn)
                            b1Status.Text = "";
                        if (!B2Fturn)
                            b2Status.Text = "";
                        if (!B3Fturn)
                            b3Status.Text = "";
                        if (!B4Fturn)
                            b4Status.Text = "";
                        if (!B5Fturn)
                            b5Status.Text = "";
                    }
                }
            }
            if (rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (cardImageHolder[j].Image != cardDeck[j])
                    {
                        cardImageHolder[j].Image = cardDeck[j];
                        player.Call = 0; player.Raise = 0;
                        bot1.Call = 0; bot1.Raise = 0;
                        bot2.Call = 0; bot2.Raise = 0;
                        bot3.Call = 0; bot3.Raise = 0;
                        bot4.Call = 0; bot4.Raise = 0;
                        bot5.Call = 0; bot5.Raise = 0;
                    }
                }
            }
            if (rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (cardImageHolder[j].Image != cardDeck[j])
                    {
                        cardImageHolder[j].Image = cardDeck[j];
                        player.Call = 0; player.Raise = 0;
                        bot1.Call = 0; bot1.Raise = 0;
                        bot2.Call = 0; bot2.Raise = 0;
                        bot3.Call = 0; bot3.Raise = 0;
                        bot4.Call = 0; bot4.Raise = 0;
                        bot5.Call = 0; bot5.Raise = 0;
                    }
                }
            }
            if (rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (cardImageHolder[j].Image != cardDeck[j])
                    {
                        cardImageHolder[j].Image = cardDeck[j];
                        player.Call = 0; player.Raise = 0;
                        bot1.Call = 0; bot1.Raise = 0;
                        bot2.Call = 0; bot2.Raise = 0;
                        bot3.Call = 0; bot3.Raise = 0;
                        bot4.Call = 0; bot4.Raise = 0;
                        bot5.Call = 0; bot5.Raise = 0;
                    }
                }
            }
            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!pStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", player.HandType, player.HandPower, PFturn);
                }
                if (!b1Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", bot1.HandType, bot1.HandPower, B1Fturn);
                }
                if (!b2Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", bot2.HandType, bot2.HandPower, B2Fturn);
                }
                if (!b3Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", bot3.HandType, bot3.HandPower, B3Fturn);
                }
                if (!b4Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", bot4.HandType, bot4.HandPower, B4Fturn);
                }
                if (!b5Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", bot5.HandType, bot5.HandPower, B5Fturn);
                }
                Winner(player.HandType, player.HandPower, "Player", player.Chips, fixedLast);
                Winner(bot1.HandType, bot1.HandPower, "Bot 1", bot1.Chips, fixedLast);
                Winner(bot2.HandType, bot2.HandPower, "Bot 2", bot2.Chips, fixedLast);
                Winner(bot3.HandType, bot3.HandPower, "Bot 3", bot3.Chips, fixedLast);
                Winner(bot4.HandType, bot4.HandPower, "Bot 4", bot4.Chips, fixedLast);
                Winner(bot5.HandType, bot5.HandPower, "Bot 5", bot5.Chips, fixedLast);
                restart = true;
                Pturn = true;
                PFturn = false;
                B1Fturn = false;
                B2Fturn = false;
                B3Fturn = false;
                B4Fturn = false;
                B5Fturn = false;
                if (player.Chips <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        player.Chips = f2.a;
                        bot1.Chips += f2.a;
                        bot2.Chips += f2.a;
                        bot3.Chips += f2.a;
                        bot4.Chips += f2.a;
                        bot5.Chips += f2.a;
                        PFturn = false;
                        Pturn = true;
                        buttonRaise.Enabled = true;
                        buttonFold.Enabled = true;
                        buttonCheck.Enabled = true;
                        buttonRaise.Text = "Raise";
                    }
                }
                player.Panel.Visible = false;
                bot1.Panel.Visible = false;
                bot2.Panel.Visible = false;
                bot3.Panel.Visible = false;
                bot4.Panel.Visible = false;
                bot5.Panel.Visible = false;

                player.Call = 0; player.Raise = 0;
                bot1.Call = 0; bot1.Raise = 0;
                bot2.Call = 0; bot2.Raise = 0;
                bot3.Call = 0; bot3.Raise = 0;
                bot4.Call = 0; bot4.Raise = 0;
                bot5.Call = 0; bot5.Raise = 0;
                last = 0;
                call = bigBlind;
                Raise = 0;
                CardImagePaths = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                allPlayersIsActiveCollection.Clear();
                rounds = 0;
                player.HandPower = 0;
                player.HandType = -1;
                type = 0;
                bot1.HandPower = 0;
                bot2.HandPower = 0;
                bot3.HandPower = 0;
                bot4.HandPower = 0;
                bot5.HandPower = 0;
                bot1.HandType = -1;
                bot2.HandType = -1;
                bot3.HandType = -1;
                bot4.HandType = -1;
                bot5.HandType = -1;
                ints.Clear();
                CheckWinners.Clear();
                winners = 0;
                Win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    cardImageHolder[os].Image = null;
                    cardImageHolder[os].Invalidate();
                    cardImageHolder[os].Visible = false;
                }
                tbPot.Text = "0";
                pStatus.Text = "";
                await Shuffle();
                await Turns();
            }
        }
        void FixCall(Label status, int cCall, int cRaise, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        cRaise = int.Parse(changeRaise);
                    }
                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        cCall = int.Parse(changeCall);
                    }
                    if (status.Text.Contains("Check"))
                    {
                        cRaise = 0;
                        cCall = 0;
                    }
                }
                if (options == 2)
                {
                    if (cRaise != Raise && cRaise <= Raise)
                    {
                        call = Convert.ToInt32(Raise) - cRaise;
                    }
                    if (cCall != call || cCall <= call)
                    {
                        call = call - cCall;
                    }
                    if (cRaise == Raise && Raise > 0)
                    {
                        call = 0;
                        buttonCall.Enabled = false;
                        buttonCall.Text = "Callisfuckedup";
                    }
                }
            }
        }
        async Task AllIn()
        {
            #region All in
            if (player.Chips <= 0 && !intsadded)
            {
                if (pStatus.Text.Contains("Raise"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
                if (pStatus.Text.Contains("Call"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
            }
            intsadded = false;
            if (bot1.Chips <= 0 && !B1Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot1.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot2.Chips <= 0 && !B2Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot2.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot3.Chips <= 0 && !B3Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot3.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot4.Chips <= 0 && !B4Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot4.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot5.Chips <= 0 && !B5Fturn)
            {
                if (!intsadded)
                {
                    ints.Add(bot5.Chips);
                    intsadded = true;
                }
            }
            if (ints.ToArray().Length == maxLeft)
            {
                await Finish(2);
            }
            else
            {
                ints.Clear();
            }
            #endregion

            var abc = allPlayersIsActiveCollection.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = allPlayersIsActiveCollection.IndexOf(false);
                if (index == 0)
                {
                    player.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = player.Chips.ToString();
                    player.Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    bot1.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot1.Chips.ToString();
                    bot1.Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    bot2.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot2.Chips.ToString();
                    bot2.Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    bot3.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot3.Chips.ToString();
                    bot3.Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    bot4.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot4.Chips.ToString();
                    bot4.Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    bot5.Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot5.Chips.ToString();
                    bot5.Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    cardImageHolder[j].Visible = false;
                }
                await Finish(1);
            }
            intsadded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= End)
            {
                await Finish(2);
            }
            #endregion


        }
        async Task Finish(int n)
        {
            if (n == 2)
            {
                FixWinners();
            }
            player.Panel.Visible = false;
            bot1.Panel.Visible = false;
            bot2.Panel.Visible = false;
            bot3.Panel.Visible = false;
            bot4.Panel.Visible = false;
            bot5.Panel.Visible = false;
            call = bigBlind; Raise = 0;
            foldedPlayers = 5;
            type = 0;
            rounds = 0;
            bot1.HandPower = 0;
            bot2.HandPower = 0;
            bot3.HandPower = 0;
            bot4.HandPower = 0;
            bot5.HandPower = 0;
            player.HandPower = 0;
            player.HandType = -1;
            Raise = 0;
            bot1.HandType = -1;
            bot2.HandType = -1;
            bot3.HandType = -1;
            bot4.HandType = -1;
            bot4.HandType = -1;
            B1turn = false; B2turn = false; B3turn = false; B4turn = false; B5turn = false;
            B1Fturn = false; B2Fturn = false; B3Fturn = false; B4Fturn = false; B5Fturn = false;
            pFolded = false; b1Folded = false; b2Folded = false; b3Folded = false; b4Folded = false; b5Folded = false;
            PFturn = false; Pturn = true; restart = false; raising = false;
            player.Call = 0; player.Raise = 0;
            bot1.Call = 0; bot1.Raise = 0;
            bot2.Call = 0; bot2.Raise = 0;
            bot3.Call = 0; bot3.Raise = 0;
            bot4.Call = 0; bot4.Raise = 0;
            bot5.Call = 0; bot5.Raise = 0;
            height = 0; width = 0; winners = 0; Flop = 1; Turn = 2; River = 3; End = 4; maxLeft = 6;
            last = 123; raisedTurn = 1;
            allPlayersIsActiveCollection.Clear();
            CheckWinners.Clear();
            ints.Clear();
            Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            tbPot.Text = "0";
            t = 60; up = 10000000; turnCount = 0;
            pStatus.Text = "";
            b1Status.Text = "";
            b2Status.Text = "";
            b3Status.Text = "";
            b4Status.Text = "";
            b5Status.Text = "";
            if (player.Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    player.Chips = f2.a;
                    bot1.Chips += f2.a;
                    bot2.Chips += f2.a;
                    bot3.Chips += f2.a;
                    bot4.Chips += f2.a;
                    bot5.Chips += f2.a;
                    PFturn = false;
                    Pturn = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    buttonCheck.Enabled = true;
                    buttonRaise.Text = "Raise";
                }
            }
            CardImagePaths = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                cardImageHolder[os].Image = null;
                cardImageHolder[os].Invalidate();
                cardImageHolder[os].Visible = false;
            }
            await Shuffle();
            //await Turns();
        }
        void FixWinners()
        {
            Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!pStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", player.HandType, player.HandPower, PFturn);
            }
            if (!b1Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", bot1.HandType, bot1.HandPower, B1Fturn);
            }
            if (!b2Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", bot2.HandType, bot2.HandPower, B2Fturn);
            }
            if (!b3Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", bot3.HandType, bot3.HandPower, B3Fturn);
            }
            if (!b4Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", bot4.HandType, bot4.HandPower, B4Fturn);
            }
            if (!b5Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", bot5.HandType, bot5.HandPower, B5Fturn);
            }
            Winner(player.HandType, player.HandPower, "Player", player.Chips, fixedLast);
            Winner(bot1.HandType, bot1.HandPower, "Bot 1", bot1.Chips, fixedLast);
            Winner(bot2.HandType, bot2.HandPower, "Bot 2", bot2.Chips, fixedLast);
            Winner(bot3.HandType, bot3.HandPower, "Bot 3", bot3.Chips, fixedLast);
            Winner(bot4.HandType, bot4.HandPower, "Bot 4", bot4.Chips, fixedLast);
            Winner(bot5.HandType, bot5.HandPower, "Bot 5", bot5.Chips, fixedLast);
        }
        void AI(int c1, int c2, int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower, double botCurrent)
        {
            if (!sFTurn)
            {
                if (botCurrent == -1)
                {
                    HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 0)
                {
                    PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 1)
                {
                    PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 2)
                {
                    TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 3)
                {
                    ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 4)
                {
                    Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 6)
                {
                    FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 7)
                {
                    FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
            }
            if (sFTurn)
            {
                cardImageHolder[c1].Visible = false;
                cardImageHolder[c2].Visible = false;
            }
        }
        private void HighCard(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 20, 25);
        }
        private void PairTable(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 16, 25);
        }
        private void PairHand(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (botPower <= 199 && botPower >= 140)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 6, rRaise);
            }
            if (botPower <= 139 && botPower >= 128)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 7, rRaise);
            }
            if (botPower < 128 && botPower >= 101)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 9, rRaise);
            }
        }
        private void TwoPair(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (botPower <= 290 && botPower >= 246)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 3, rRaise);
            }
            if (botPower <= 244 && botPower >= 234)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
            if (botPower < 234 && botPower >= 201)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
        }
        private void ThreeOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (botPower <= 390 && botPower >= 330)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower <= 327 && botPower >= 321)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower < 321 && botPower >= 303)//7 2
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
        }
        private void Straight(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (botPower <= 480 && botPower >= 410)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            if (botPower <= 409 && botPower >= 407)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            if (botPower < 407 && botPower >= 404)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
        }
        private void Flush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fCall, fRaise);
        }
        private void FullHouse(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (botPower <= 626 && botPower >= 620)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
            if (botPower < 620 && botPower >= 602)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
        }
        private void FourOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fkCall, fkRaise);
            }
        }
        private void StraightFlush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sfCall, sfRaise);
            }
        }

        private void Fold(ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            raising = false;
            sStatus.Text = "Fold";
            sTurn = false;
            sFTurn = true;
        }
        private void Check(ref bool cTurn, Label cStatus)
        {
            cStatus.Text = "Check";
            cTurn = false;
            raising = false;
        }
        private void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            raising = false;
            sTurn = false;
            sChips -= call;
            sStatus.Text = "Call " + call;
            tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
        }
        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(Raise);
            sStatus.Text = "Raise " + Raise;
            tbPot.Text = (int.Parse(tbPot.Text) + Convert.ToInt32(Raise)).ToString();
            call = Convert.ToInt32(Raise);
            raising = true;
            sTurn = false;
        }
        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }
        private void HP(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (call <= 0)
            {
                Check(ref sTurn, sStatus);
            }
            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(sChips, n))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
                if (rnd == 2)
                {
                    if (call <= RoundN(sChips, n1))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (rnd == 3)
            {
                if (Raise == 0)
                {
                    Raise = call * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (Raise <= RoundN(sChips, n))
                    {
                        Raise = call * 2;
                        Raised(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }
        private void PH(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (call <= 0)
                {
                    Check(ref sTurn, sStatus);
                }
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (Raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n) && call <= RoundN(sChips, n1))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n) && Raise >= (RoundN(sChips, n)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= (RoundN(sChips, n)) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1 - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (Raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n - rnd) && call <= RoundN(sChips, n1 - rnd))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n - rnd) && Raise >= (RoundN(sChips, n - rnd)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= (RoundN(sChips, n - rnd)) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n - rnd);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
                if (call <= 0)
                {
                    Raise = RoundN(sChips, r - rnd);
                    Raised(ref sChips, ref sTurn, sStatus);
                }
            }
            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }
        void Smooth(ref int botChips, ref bool botTurn, ref bool botFTurn, Label botStatus, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (call <= 0)
            {
                Check(ref botTurn, botStatus);
            }
            else
            {
                if (call >= RoundN(botChips, n))
                {
                    if (botChips > call)
                    {
                        Call(ref botChips, ref botTurn, botStatus);
                    }
                    else if (botChips <= call)
                    {
                        raising = false;
                        botTurn = false;
                        botChips = 0;
                        botStatus.Text = "Call " + botChips;
                        tbPot.Text = (int.Parse(tbPot.Text) + botChips).ToString();
                    }
                }
                else
                {
                    if (Raise > 0)
                    {
                        if (botChips >= Raise * 2)
                        {
                            Raise *= 2;
                            Raised(ref botChips, ref botTurn, botStatus);
                        }
                        else
                        {
                            Call(ref botChips, ref botTurn, botStatus);
                        }
                    }
                    else
                    {
                        Raise = call * 2;
                        Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }
            if (botChips <= 0)
            {
                botFTurn = true;
            }
        }

        #region UI
        private async void timer_Tick(object sender, object e)
        {
            if (pbTimer.Value <= 0)
            {
                PFturn = true;
                await Turns();
            }
            if (t > 0)
            {
                t--;
                pbTimer.Value = (t / 6) * 100;
            }
        }
        private void Update_Tick(object sender, object e)
        {
            if (player.Chips <= 0)
            {
                tbChips.Text = "Chips : 0";
            }
            if (bot1.Chips <= 0)
            {
                tbBotChips1.Text = "Chips : 0";
            }
            if (bot2.Chips <= 0)
            {
                tbBotChips2.Text = "Chips : 0";
            }
            if (bot3.Chips <= 0)
            {
                tbBotChips3.Text = "Chips : 0";
            }
            if (bot4.Chips <= 0)
            {
                tbBotChips4.Text = "Chips : 0";
            }
            if (bot5.Chips <= 0)
            {
                tbBotChips5.Text = "Chips : 0";
            }
            tbChips.Text = "Chips : " + player.Chips.ToString();
            tbBotChips1.Text = "Chips : " + bot1.Chips.ToString();
            tbBotChips2.Text = "Chips : " + bot2.Chips.ToString();
            tbBotChips3.Text = "Chips : " + bot3.Chips.ToString();
            tbBotChips4.Text = "Chips : " + bot4.Chips.ToString();
            tbBotChips5.Text = "Chips : " + bot5.Chips.ToString();
            if (player.Chips <= 0)
            {
                Pturn = false;
                PFturn = true;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                buttonCheck.Enabled = false;
            }
            if (up > 0)
            {
                up--;
            }
            if (player.Chips >= call)
            {
                buttonCall.Text = "Call " + call.ToString();
            }
            else
            {
                buttonCall.Text = "All in";
                buttonRaise.Enabled = false;
            }
            if (call > 0)
            {
                buttonCheck.Enabled = false;
            }
            if (call <= 0)
            {
                buttonCheck.Enabled = true;
                buttonCall.Text = "Call";
                buttonCall.Enabled = false;
            }
            if (player.Chips <= 0)
            {
                buttonRaise.Enabled = false;
            }
            int parsedValue;

            if (tbRaise.Text != "" && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (player.Chips <= int.Parse(tbRaise.Text))
                {
                    buttonRaise.Text = "All in";
                }
                else
                {
                    buttonRaise.Text = "Raise";
                }
            }
            if (player.Chips < call)
            {
                buttonRaise.Enabled = false;
            }
        }
        private async void bFold_Click(object sender, EventArgs e)
        {
            pStatus.Text = "Fold";
            Pturn = false;
            PFturn = true;
            await Turns();
        }
        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                Pturn = false;
                pStatus.Text = "Check";
            }
            else
            {
                //pStatus.Text = "All in " + Chips;

                buttonCheck.Enabled = false;
            }
            await Turns();
        }
        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", player.HandType, player.HandPower, PFturn);
            if (player.Chips >= call)
            {
                player.Chips -= call;
                tbChips.Text = "Chips : " + player.Chips.ToString();
                if (tbPot.Text != "")
                {
                    tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                }
                else
                {
                    tbPot.Text = call.ToString();
                }
                Pturn = false;
                pStatus.Text = "Call " + call;
                player.Call = call;
            }
            else if (player.Chips <= call && call > 0)
            {
                tbPot.Text = (int.Parse(tbPot.Text) + player.Chips).ToString();
                pStatus.Text = "All in " + player.Chips;
                player.Chips = 0;
                tbChips.Text = "Chips : " + player.Chips.ToString();
                Pturn = false;
                buttonFold.Enabled = false;
                player.Call = player.Chips;
            }
            await Turns();
        }
        private async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", player.HandType, player.HandPower, PFturn);
            int parsedValue;
            if (tbRaise.Text != "" && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (player.Chips > call)
                {
                    if (Raise * 2 > int.Parse(tbRaise.Text))
                    {
                        tbRaise.Text = (Raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the currentHandType raise !");
                        return;
                    }
                    else
                    {
                        if (player.Chips >= int.Parse(tbRaise.Text))
                        {
                            call = int.Parse(tbRaise.Text);
                            Raise = int.Parse(tbRaise.Text);
                            pStatus.Text = "Raise " + call.ToString();
                            tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                            buttonCall.Text = "Call";
                            player.Chips -= int.Parse(tbRaise.Text);
                            raising = true;
                            last = 0;
                            player.Raise = Convert.ToInt32(Raise);
                        }
                        else
                        {
                            call = player.Chips;
                            Raise = player.Chips;
                            tbPot.Text = (int.Parse(tbPot.Text) + player.Chips).ToString();
                            pStatus.Text = "Raise " + call.ToString();
                            player.Chips = 0;
                            raising = true;
                            last = 0;
                            player.Raise = Convert.ToInt32(Raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }
            Pturn = false;
            await Turns();
        }
        private void bAdd_Click(object sender, EventArgs e)
        {
            if (tbAdd.Text == "") { }
            else
            {
                player.Chips += int.Parse(tbAdd.Text);
                bot1.Chips += int.Parse(tbAdd.Text);
                bot2.Chips += int.Parse(tbAdd.Text);
                bot3.Chips += int.Parse(tbAdd.Text);
                bot4.Chips += int.Parse(tbAdd.Text);
                bot5.Chips += int.Parse(tbAdd.Text);
            }
            tbChips.Text = "Chips : " + player.Chips.ToString();
        }
        private void bOptions_Click(object sender, EventArgs e)
        {
            tbBB.Text = bigBlind.ToString();
            tbSB.Text = smallBlind.ToString();
            if (tbBB.Visible == false)
            {
                tbBB.Visible = true;
                tbSB.Visible = true;
                bBB.Visible = true;
                bSB.Visible = true;
            }
            else
            {
                tbBB.Visible = false;
                tbSB.Visible = false;
                bBB.Visible = false;
                bSB.Visible = false;
            }
        }
        private void bSB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (tbSB.Text.Contains(",") || tbSB.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                tbSB.Text = smallBlind.ToString();
                return;
            }
            if (!int.TryParse(tbSB.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                tbSB.Text = smallBlind.ToString();
                return;
            }
            if (int.Parse(tbSB.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                tbSB.Text = smallBlind.ToString();
            }
            if (int.Parse(tbSB.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            if (int.Parse(tbSB.Text) >= 250 && int.Parse(tbSB.Text) <= 100000)
            {
                smallBlind = int.Parse(tbSB.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        private void bBB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (tbBB.Text.Contains(",") || tbBB.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                tbBB.Text = bigBlind.ToString();
                return;
            }
            if (!int.TryParse(tbSB.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                tbSB.Text = bigBlind.ToString();
                return;
            }
            if (int.Parse(tbBB.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                tbBB.Text = bigBlind.ToString();
            }
            if (int.Parse(tbBB.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }
            if (int.Parse(tbBB.Text) >= 500 && int.Parse(tbBB.Text) <= 200000)
            {
                bigBlind = int.Parse(tbBB.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        private void Layout_Change(object sender, LayoutEventArgs e)
        {
            width = this.Width;
            height = this.Height;
        }
        #endregion
    }
}