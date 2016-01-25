using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Poker.Interfaces;
using Poker.Models;

namespace Poker
{
    public partial class GameForm : Form
    {
        #region Variables
        private readonly ProgressBar progressBar = new ProgressBar();
        
        private IPlayer player;
        private IPlayer bot1;
        private IPlayer bot2;
        private IPlayer bot3;
        private IPlayer bot4;
        private IPlayer bot5;

        private int call;
        private int foldedPlayers;
        private double type;
        private double rounds = 0;
        private double Raise = 0;

        private bool intsadded;
        private bool changed;

        private int height;
        private int width;
        private int winnersCount;
        private int Flop = 1;
        private int Turn = 2;
        private int River = 3;
        private int End = 4;
        private int maxLeft = 6;
        private int last = 123;
        private int raisedTurn = 1;

        private List<bool?> allPlayersIsActiveCollection = new List<bool?>();
        private List<Type> Win = new List<Type>();
        private List<string> CheckWinners = new List<string>();
        private List<int> ints = new List<int>();

        private bool restart = false;
        private bool raising = false;
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

        private string[] CardImagePaths;
        private Image[] cardDeck;
        private PictureBox[] cardImageHolder;
        private int[] Reserve;

        private Timer timer;
        private Timer updates;

        private int t = 60;
        private int i;
        private int bigBlind;
        private int smallBlind;
        private int up = 10000000;
        private int turnCount = 0;

        #endregion

        public GameForm()
        {
            this.call = GameConstants.DefaultBigBlind;
            this.winnersCount = 0;
            this.foldedPlayers = GameConstants.InitialBotCount;
            this.bigBlind = GameConstants.DefaultBigBlind;
            this.smallBlind = GameConstants.DefaultSmallBlind;
            this.CardImagePaths = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            this.cardDeck = new Image[GameConstants.DeckCardsCount];
            this.cardImageHolder = new PictureBox[GameConstants.DeckCardsCount];
            this.Reserve = new int[GameConstants.AllCardsInGame];
            this.timer = new Timer();
            this.updates = new Timer();

            MaximizeBox = false;
            MinimizeBox = false;
            updates.Start();
            InitializeComponent();

            this.player = new PokerPlayer(-1, "Player", this.playerStatus);
            this.bot1 = new BotPlayer(0, "Bot 1", this.bot1Status);
            this.bot2 = new BotPlayer(1, "Bot 2", this.bot2Status);
            this.bot3 = new BotPlayer(2, "Bot 3", this.bot3Status);
            this.bot4 = new BotPlayer(3, "Bot 4", this.bot4Status);
            this.bot5 = new BotPlayer(4, "Bot 5", this.bot5Status);

            width = this.Width;
            height = this.Height;

            Shuffle();

            potTextBox.Enabled = false;
            playerChipsTextBox.Enabled = false;
            bot1ChipsTextBox.Enabled = false;
            bot2ChipsTextBox.Enabled = false;
            bot3ChipsTextBox.Enabled = false;
            bot4ChipsTextBox.Enabled = false;
            bot5ChipsTextBox.Enabled = false;

            playerChipsTextBox.Text = "Chips : " + player.Chips.ToString();
            bot1ChipsTextBox.Text = "Chips : " + bot1.Chips.ToString();
            bot2ChipsTextBox.Text = "Chips : " + bot2.Chips.ToString();
            bot3ChipsTextBox.Text = "Chips : " + bot3.Chips.ToString();
            bot4ChipsTextBox.Text = "Chips : " + bot4.Chips.ToString();
            bot5ChipsTextBox.Text = "Chips : " + bot5.Chips.ToString();

            timer.Interval = (1 * 1 * 1000);
            timer.Tick += timer_Tick;
            updates.Interval = (1 * 1 * 100);
            updates.Tick += Update_Tick;

            bigBlindTextBox.Visible = true;
            smallBlindTextBox.Visible = true;
            bigBlindButton.Visible = true;
            smallBlindButton.Visible = true;
            bigBlindTextBox.Visible = true;
            smallBlindTextBox.Visible = true;
            bigBlindButton.Visible = true;
            smallBlindButton.Visible = true;
            bigBlindTextBox.Visible = false;
            smallBlindTextBox.Visible = false;
            bigBlindButton.Visible = false;
            smallBlindButton.Visible = false;
            raiseTextBox.Text = (bigBlind * 2).ToString();
        }

        async Task Shuffle()
        {
            allPlayersIsActiveCollection.Add(player.GameEnded);
            allPlayersIsActiveCollection.Add(bot1.GameEnded);
            allPlayersIsActiveCollection.Add(bot2.GameEnded);
            allPlayersIsActiveCollection.Add(bot3.GameEnded);
            allPlayersIsActiveCollection.Add(bot4.GameEnded);
            allPlayersIsActiveCollection.Add(bot5.GameEnded);

            buttonCall.Enabled = false;
            buttonRaise.Enabled = false;
            buttonFold.Enabled = false;
            buttonCheck.Enabled = false;
            MaximizeBox = false;
            MinimizeBox = false;
            
            this.CardDeckInit();

            for (int cardIndex = 0; cardIndex < GameConstants.AllCardsInGame; cardIndex++)
            {
                cardDeck[cardIndex] = Image.FromFile(CardImagePaths[cardIndex]);

                this.ExtractDealthCardsNumbers(cardIndex);

                this.SetCardImageHolder(cardIndex);

                //await Task.Delay(200);
            }

            int positionX = GameConstants.PlayerCardPositionX;
            int positionY = GameConstants.PlayerCardPositionY;

            this.DealPlayersCards(positionX, positionY);

            positionX = GameConstants.TableCardPositionX;
            positionY = GameConstants.TableCardPositionY;
            

            this.DealTableCards(positionX, positionY);
            this.CheckRestartGame();
            
            if (player.Turn)
            {
                buttonRaise.Enabled = true;
                buttonCall.Enabled = true;
                buttonRaise.Enabled = true;
                buttonRaise.Enabled = true;
                buttonFold.Enabled = true;
            }
        }

        private void DealTableCards(int positionX, int positionY)
        {
            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");

            for (int index = GameConstants.FirstBoardCardIndex; index < GameConstants.AllCardsInGame; index++)
            {
                DealBoardCards(index, positionX, positionY, backImage);

                positionX += GameConstants.BoardCardsOffset;

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
        }

        private void DealPlayersCards(int cardPositionX, int cardPositionY)
        {
            for (int index = 0; index < GameConstants.NumberOfPlayerCards; index += 2)
            {
                if (index < 2)
                {
                    this.DealCardsToCurrentPlayer(player, index, cardPositionX, cardPositionY, 0, 1, AnchorStyles.Bottom);
                }

                if (bot1.Chips > 0)
                {
                    foldedPlayers--;
                    if (2 <= index && index < 4)
                    {
                        cardPositionX = 15;
                        cardPositionY = 420;

                        DealCardsToCurrentPlayer(bot1, index, cardPositionX, cardPositionY, 2, 3, AnchorStyles.Bottom, AnchorStyles.Left);
                    }
                }

                if (bot2.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 4 && index < 6)
                    {
                        cardPositionX = 75;
                        cardPositionY = 65;

                        DealCardsToCurrentPlayer(bot2, index, cardPositionX, cardPositionY, 4, 5, AnchorStyles.Top, AnchorStyles.Left);
                    }
                }

                if (bot3.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 6 && index < 8)
                    {
                        cardPositionX = 590;
                        cardPositionY = 25;

                        DealCardsToCurrentPlayer(bot3, index, cardPositionX, cardPositionY, 6, 7, AnchorStyles.Top);
                    }
                }

                if (bot4.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 8 && index < 10)
                    {
                        cardPositionX = 1115;
                        cardPositionY = 65;

                        this.DealCardsToCurrentPlayer(bot4, index, cardPositionX, cardPositionY, 8, 9, AnchorStyles.Top, AnchorStyles.Right);
                    }
                }

                if (bot5.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 10 && index < 12)
                    {
                        cardPositionX = 1160;
                        cardPositionY = 420;

                        this.DealCardsToCurrentPlayer(bot5, index, cardPositionX, cardPositionY, 10, 11, AnchorStyles.Bottom, AnchorStyles.Right);
                    }
                }
            }
        }

        private void CheckRestartGame()
        {
            if (foldedPlayers == 5)
            {
                DialogResult dialogResult = MessageBox.Show(GameConstants.PlayAgainMessage,
                    GameConstants.PlayerWonMessage,
                    MessageBoxButtons.YesNo);
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
        }

        private void SetCardImageHolder(int cardIndex)
        {
            cardImageHolder[cardIndex] = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Height = GameConstants.CardImageHolderHeight,
                Width = GameConstants.CardImageHolderWidth,
                Name = "pb" + i.ToString()
            };

            this.Controls.Add(cardImageHolder[cardIndex]);
        }

        private void ExtractDealthCardsNumbers(int cardIndex)
        {
            string directoryPathRemove = "Assets\\Cards\\";
            string fileExtension = ".png";
            CardImagePaths[cardIndex] = CardImagePaths[cardIndex].Replace(directoryPathRemove, string.Empty);
            CardImagePaths[cardIndex] = CardImagePaths[cardIndex].Replace(fileExtension, string.Empty);
            Reserve[cardIndex] = int.Parse(CardImagePaths[cardIndex]) - 1;
        }

        private void DealBoardCards(int index, int horizontal, int vertical, Image cardImage)
        {
            cardImageHolder[index].Tag = Reserve[index];
            cardImageHolder[index].Anchor = AnchorStyles.None;
            cardImageHolder[index].Image = cardImage;
            cardImageHolder[index].Location = new Point(horizontal, vertical);
        }

        private void DealCardsToCurrentPlayer(IPlayer currentPlayer, int cardIndex, int x, int y, int firstCardBoardNumber, int secondCardBoardNumber, AnchorStyles style1, AnchorStyles style2 = AnchorStyles.None)
        {
            cardImageHolder[cardIndex].Anchor = (style1 | style2);
            currentPlayer.Card1 = new Card(x, y, cardDeck[cardIndex], Reserve[cardIndex], cardImageHolder[cardIndex], firstCardBoardNumber);
            x += currentPlayer.Card1.CardHolder.Width;
            cardImageHolder[cardIndex + 1].Anchor = (style1 | style2);
            currentPlayer.Card2 = new Card(x, y, cardDeck[cardIndex + 1], Reserve[cardIndex + 1], cardImageHolder[cardIndex + 1], secondCardBoardNumber);
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

            if (!player.GameEnded)
            {
                if (player.Turn)
                {
                    FixCall(player, 1);
                    progressBarTimer.Visible = true;
                    progressBarTimer.Value = 1000;
                    t = 60;
                    up = 10000000;
                    timer.Start();
                    buttonRaise.Enabled = true;
                    buttonCall.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    turnCount++;
                    FixCall(player, 2);
                }
            }
            if (player.GameEnded || !player.Turn)
            {
                await AllIn();
                if (player.GameEnded && !player.HasFolded)
                {
                    if (!buttonCall.Text.Contains("All in")|| !buttonRaise.Text.Contains("All in"))
                    {
                        this.RemovePlayerFromGame(player, 0);
                    }
                }

                await CheckRaise(0, 0);
                progressBarTimer.Visible = false;
                buttonRaise.Enabled = false;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                timer.Stop();
                bot1.Turn = true;

                if (!bot1.GameEnded)
                {
                    this.CheckBotActions(bot1, bot2, 1);
                }

                if (!bot2.GameEnded)
                {
                    this.CheckBotActions(bot2, bot3, 2);
                }

                if (!bot3.GameEnded)
                {
                        this.CheckBotActions(bot3, bot4, 3);
                }
               
                if (!bot4.GameEnded)
                {
                        this.CheckBotActions(bot4, bot5, 4);
                }

                if (!bot5.GameEnded)
                {
                    this.CheckBotActions(bot5, player, 5);
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

        private void RemovePlayerFromGame(IPlayer gamePlayer, int position)
        {
            allPlayersIsActiveCollection.RemoveAt(position);
            allPlayersIsActiveCollection.Insert(position, null);
            maxLeft--;
            gamePlayer.HasFolded = true;
        }

        private void CheckBotActions(IPlayer bot, IPlayer nextBot, int botIndex)
        {
            if (bot.Turn)
            {
                FixCall(bot, 1);
                FixCall(bot, 2);
                Rules(bot1);
                MessageBox.Show(string.Format("{0}'s Turn", bot.Name));
                AI(bot1);
                turnCount++;
                last = botIndex;
                bot.Turn = false;
                nextBot.Turn = true;
            }
            if (bot.GameEnded && !bot.HasFolded)
            {
                this.RemovePlayerFromGame(bot, botIndex);
            }
            if (bot.GameEnded || !bot.Turn)
            {
                CheckRaise(botIndex, botIndex);
                nextBot.Turn = true;
            }
        }

        void Rules(IPlayer gamePlayer)
        {
            if (!gamePlayer.GameEnded || gamePlayer.Card1.CardBoardNumber == 0 && gamePlayer.Card2.CardBoardNumber == 1 && gamePlayer.Status.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false, vf = false;
                int[] Straight1 = new int[5];
                int[] Straight = new int[7];
                Straight[0] = Reserve[gamePlayer.Card1.CardBoardNumber];
                Straight[1] = Reserve[gamePlayer.Card2.CardBoardNumber];
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
                    if (Reserve[i] == int.Parse(cardImageHolder[gamePlayer.Card1.CardBoardNumber].Tag.ToString()) && Reserve[i + 1] == int.Parse(cardImageHolder[gamePlayer.Card2.CardBoardNumber].Tag.ToString()))
                    {
                        //Pair from Hand currentHandType = 1

                        rPairFromHand(gamePlayer.HandType, gamePlayer.HandPower);

                        #region Pair or Two Pair from Table currentHandType = 2 || 0
                        rPairTwoPair(gamePlayer.HandType, gamePlayer.HandPower);
                        #endregion

                        #region Two Pair currentHandType = 2
                        rTwoPair(gamePlayer.HandType, gamePlayer.HandPower);
                        #endregion

                        #region Three of a kind currentHandType = 3
                        rThreeOfAKind(gamePlayer.HandType, gamePlayer.HandPower, Straight);
                        #endregion

                        #region Straight currentHandType = 4
                        rStraight(gamePlayer.HandType, gamePlayer.HandPower, Straight);
                        #endregion

                        #region Flush currentHandType = 5 || 5.5
                        rFlush(gamePlayer.HandType, gamePlayer.HandPower, ref vf, Straight1);
                        #endregion

                        #region Full House currentHandType = 6
                        rFullHouse(gamePlayer.HandType, gamePlayer.HandPower, ref done, Straight);
                        #endregion

                        #region Four of a Kind currentHandType = 7
                        rFourOfAKind(gamePlayer.HandType, gamePlayer.HandPower, Straight);
                        #endregion

                        #region Straight Flush currentHandType = 8 || 9
                        rStraightFlush(gamePlayer.HandType, gamePlayer.HandPower, st1, st2, st3, st4);
                        #endregion

                        #region High Card currentHandType = -1
                        rHighCard(gamePlayer.HandType, gamePlayer.HandPower);
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

        void Winner(IPlayer gamePlayer, string lastly)
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
            if (gamePlayer.HandType == sorted.Current)
            {
                if (gamePlayer.HandPower == sorted.Power)
                {
                    winnersCount++;
                    CheckWinners.Add(gamePlayer.Name);
                    if (gamePlayer.HandType == -1)
                    {
                        MessageBox.Show(gamePlayer.Name + " High Card ");
                    }
                    if (gamePlayer.HandType == 1 || gamePlayer.HandType == 0)
                    {
                        MessageBox.Show(gamePlayer.Name + " Pair ");
                    }
                    if (gamePlayer.HandType == 2)
                    {
                        MessageBox.Show(gamePlayer.Name + " Two Pair ");
                    }
                    if (gamePlayer.HandType == 3)
                    {
                        MessageBox.Show(gamePlayer.Name + " Three of a Kind ");
                    }
                    if (gamePlayer.HandType == 4)
                    {
                        MessageBox.Show(gamePlayer.Name + " Straight ");
                    }
                    if (gamePlayer.HandType == 5 || gamePlayer.HandType == 5.5)
                    {
                        MessageBox.Show(gamePlayer.Name + " Flush ");
                    }
                    if (gamePlayer.HandType == 6)
                    {
                        MessageBox.Show(gamePlayer.Name + " Full House ");
                    }
                    if (gamePlayer.HandType == 7)
                    {
                        MessageBox.Show(gamePlayer.Name + " Four of a Kind ");
                    }
                    if (gamePlayer.HandType == 8)
                    {
                        MessageBox.Show(gamePlayer.Name + " Straight Flush ");
                    }
                    if (gamePlayer.HandType == 9)
                    {
                        MessageBox.Show(gamePlayer.Name + " Royal Flush ! ");
                    }
                }
            }
            if (gamePlayer.Name == lastly)//lastfixed
            {
                if (winnersCount > 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        player.Chips += int.Parse(potTextBox.Text) / winnersCount;
                        playerChipsTextBox.Text = player.Chips.ToString();
                        //pPanel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1.Chips += int.Parse(potTextBox.Text) / winnersCount;
                        bot1ChipsTextBox.Text = bot1.Chips.ToString();
                        //b1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2.Chips += int.Parse(potTextBox.Text) / winnersCount;
                        bot2ChipsTextBox.Text = bot2.Chips.ToString();
                        //b2Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3.Chips += int.Parse(potTextBox.Text) / winnersCount;
                        bot3ChipsTextBox.Text = bot3.Chips.ToString();
                        //b3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4.Chips += int.Parse(potTextBox.Text) / winnersCount;
                        bot4ChipsTextBox.Text = bot4.Chips.ToString();
                        //b4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5.Chips += int.Parse(potTextBox.Text) / winnersCount;
                        bot5ChipsTextBox.Text = bot5.Chips.ToString();
                        //b5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winnersCount == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        player.Chips += int.Parse(potTextBox.Text);
                        //await Finish(1);
                        //pPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1.Chips += int.Parse(potTextBox.Text);
                        //await Finish(1);
                        //b1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2.Chips += int.Parse(potTextBox.Text);
                        //await Finish(1);
                        //b2Panel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3.Chips += int.Parse(potTextBox.Text);
                        //await Finish(1);
                        //b3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4.Chips += int.Parse(potTextBox.Text);
                        //await Finish(1);
                        //b4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5.Chips += int.Parse(potTextBox.Text);
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
                        if (!player.GameEnded)
                            player.Status.Text = "";
                        if (!bot1.GameEnded)
                            bot1.Status.Text = "";
                        if (!bot2.GameEnded)
                            bot2.Status.Text = "";
                        if (!bot3.GameEnded)
                            bot3.Status.Text = "";
                        if (!bot4.GameEnded)
                            bot4.Status.Text = "";
                        if (!bot5.GameEnded)
                            bot5.Status.Text = "";
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
                if (!player.Status.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(player);
                }
                if (!bot1.Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(bot1);
                }
                if (!bot2.Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(bot2);
                }
                if (!bot3.Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(bot3);
                }
                if (!bot4.Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(bot4);
                }
                if (!bot5.Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(bot5);
                }
                Winner(player, fixedLast);
                Winner(bot1, fixedLast);
                Winner(bot2, fixedLast);
                Winner(bot3, fixedLast);
                Winner(bot4, fixedLast);
                Winner(bot5, fixedLast);
                restart = true;
                player.Turn = true;
                player.GameEnded = false;
                bot1.GameEnded = false;
                bot2.GameEnded = false;
                bot3.GameEnded = false;
                bot4.GameEnded = false;
                bot5.GameEnded = false;
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
                        player.GameEnded = false;
                        player.Turn = true;
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
                winnersCount = 0;
                Win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    cardImageHolder[os].Image = null;
                    cardImageHolder[os].Invalidate();
                    cardImageHolder[os].Visible = false;
                }
                potTextBox.Text = "0";
                player.Status.Text = "";
                await Shuffle();
                await Turns();
            }
        }
        void FixCall(IPlayer gamePlayer, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (gamePlayer.Status.Text.Contains("Raise"))
                    {
                        var changeRaise = gamePlayer.Status.Text.Substring(6);
                        gamePlayer.Raise = int.Parse(changeRaise);
                    }
                    if (gamePlayer.Status.Text.Contains("Call"))
                    {
                        var changeCall = gamePlayer.Status.Text.Substring(5);
                        gamePlayer.Call = int.Parse(changeCall);
                    }
                    if (gamePlayer.Status.Text.Contains("Check"))
                    {
                        gamePlayer.Raise = 0;
                        gamePlayer.Call = 0;
                    }
                }
                if (options == 2)
                {
                    if (gamePlayer.Raise != Raise && gamePlayer.Raise <= Raise)
                    {
                        call = Convert.ToInt32(Raise) - gamePlayer.Raise;
                    }
                    if (gamePlayer.Call != call || gamePlayer.Call <= call)
                    {
                        call = call - gamePlayer.Call;
                    }
                    if (gamePlayer.Raise == Raise && Raise > 0)
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
                if (player.Status.Text.Contains("Raise"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
                if (player.Status.Text.Contains("Call"))
                {
                    ints.Add(player.Chips);
                    intsadded = true;
                }
            }
            intsadded = false;
            if (bot1.Chips <= 0 && !bot1.GameEnded)
            {
                if (!intsadded)
                {
                    ints.Add(bot1.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot2.Chips <= 0 && !bot2.GameEnded)
            {
                if (!intsadded)
                {
                    ints.Add(bot2.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot3.Chips <= 0 && !bot3.GameEnded)
            {
                if (!intsadded)
                {
                    ints.Add(bot3.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot4.Chips <= 0 && !bot4.GameEnded)
            {
                if (!intsadded)
                {
                    ints.Add(bot4.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot5.Chips <= 0 && !bot5.GameEnded)
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
                    player.Chips += int.Parse(potTextBox.Text);
                    playerChipsTextBox.Text = player.Chips.ToString();
                    player.Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    bot1.Chips += int.Parse(potTextBox.Text);
                    playerChipsTextBox.Text = bot1.Chips.ToString();
                    bot1.Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    bot2.Chips += int.Parse(potTextBox.Text);
                    playerChipsTextBox.Text = bot2.Chips.ToString();
                    bot2.Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    bot3.Chips += int.Parse(potTextBox.Text);
                    playerChipsTextBox.Text = bot3.Chips.ToString();
                    bot3.Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    bot4.Chips += int.Parse(potTextBox.Text);
                    playerChipsTextBox.Text = bot4.Chips.ToString();
                    bot4.Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    bot5.Chips += int.Parse(potTextBox.Text);
                    playerChipsTextBox.Text = bot5.Chips.ToString();
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
            bot1.Turn = false; bot2.Turn = false; bot3.Turn = false; bot3.Turn = false; bot5.Turn = false;
            bot1.GameEnded = false; bot2.GameEnded = false; bot3.GameEnded = false; bot4.GameEnded = false; bot5.GameEnded = false;
            player.HasFolded = false; bot1.HasFolded = false; bot2.HasFolded = false; bot3.HasFolded = false; bot4.HasFolded = false; bot5.HasFolded = false;
            player.GameEnded = false; player.Turn = true; restart = false; raising = false;
            player.Call = 0; player.Raise = 0;
            bot1.Call = 0; bot1.Raise = 0;
            bot2.Call = 0; bot2.Raise = 0;
            bot3.Call = 0; bot3.Raise = 0;
            bot4.Call = 0; bot4.Raise = 0;
            bot5.Call = 0; bot5.Raise = 0;
            height = 0; width = 0; winnersCount = 0; Flop = 1; Turn = 2; River = 3; End = 4; maxLeft = 6;
            last = 123; raisedTurn = 1;
            allPlayersIsActiveCollection.Clear();
            CheckWinners.Clear();
            ints.Clear();
            Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            potTextBox.Text = "0";
            t = 60; up = 10000000; turnCount = 0;
            playerStatus.Text = "";
            bot1Status.Text = "";
            bot2Status.Text = "";
            bot3Status.Text = "";
            bot4Status.Text = "";
            bot5Status.Text = "";
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
                    player.GameEnded = false;
                    player.Turn = true;
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
            if (!playerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(player);
            }
            if (!bot1Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(bot1);
            }
            if (!bot2Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(bot2);
            }
            if (!bot3Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(bot3);
            }
            if (!bot4Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(bot4);
            }
            if (!bot5Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(bot5);
            }
            Winner(player, fixedLast);
            Winner(bot1, fixedLast);
            Winner(bot2, fixedLast);
            Winner(bot3, fixedLast);
            Winner(bot4, fixedLast);
            Winner(bot5, fixedLast);
        }
        void AI(IPlayer gamePlayer)
        {
            if (!gamePlayer.GameEnded)
            {
                if (gamePlayer.HandType == -1)
                {
                    HighCard(gamePlayer);
                }
                if (gamePlayer.HandType == 0)
                {
                    PairTable(gamePlayer);
                }
                if (gamePlayer.HandType == 1)
                {
                    PairHand(gamePlayer);
                }
                if (gamePlayer.HandType == 2)
                {
                    TwoPair(gamePlayer);
                }
                if (gamePlayer.HandType == 3)
                {
                    ThreeOfAKind(gamePlayer);
                }
                if (gamePlayer.HandType == 4)
                {
                    Straight(gamePlayer);
                }
                if (gamePlayer.HandType == 5 || gamePlayer.HandType == 5.5)
                {
                    Flush(gamePlayer);
                }
                if (gamePlayer.HandType == 6)
                {
                    FullHouse(gamePlayer);
                }
                if (gamePlayer.HandType == 7)
                {
                    FourOfAKind(gamePlayer);
                }
                if (gamePlayer.HandType == 8 || gamePlayer.HandType == 9)
                {
                    StraightFlush(gamePlayer);
                }
            }
            if (gamePlayer.GameEnded)
            {
                cardImageHolder[gamePlayer.Card1.CardBoardNumber].Visible = false;
                cardImageHolder[gamePlayer.Card2.CardBoardNumber].Visible = false;
            }
        }
        private void HighCard(IPlayer gamePlayer)
        {
            HP(gamePlayer, 20, 25);
        }
        private void PairTable(IPlayer gamePlayer)
        {
            HP(gamePlayer, 16, 25);
        }
        private void PairHand(IPlayer gamePlayer)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (gamePlayer.HandPower <= 199 && gamePlayer.HandPower >= 140)
            {
                PH(gamePlayer, rCall, 6, rRaise);
            }
            if (gamePlayer.HandPower <= 139 && gamePlayer.HandPower >= 128)
            {
                PH(gamePlayer, rCall, 7, rRaise);
            }
            if (gamePlayer.HandPower < 128 && gamePlayer.HandPower >= 101)
            {
                PH(gamePlayer, rCall, 9, rRaise);
            }
        }
        private void TwoPair(IPlayer gamePlayer)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (gamePlayer.HandPower <= 290 && gamePlayer.HandPower >= 246)
            {
                PH(gamePlayer, rCall, 3, rRaise);
            }
            if (gamePlayer.HandPower <= 244 && gamePlayer.HandPower >= 234)
            {
                PH(gamePlayer, rCall, 4, rRaise);
            }
            if (gamePlayer.HandPower < 234 && gamePlayer.HandPower >= 201)
            {
                PH(gamePlayer, rCall, 4, rRaise);
            }
        }
        private void ThreeOfAKind(IPlayer gamePlayer)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (gamePlayer.HandPower <= 390 && gamePlayer.HandPower >= 330)
            {
                Smooth(gamePlayer, tCall, tRaise);
            }
            if (gamePlayer.HandPower <= 327 && gamePlayer.HandPower >= 321)//10  8
            {
                Smooth(gamePlayer, tCall, tRaise);
            }
            if (gamePlayer.HandPower < 321 && gamePlayer.HandPower >= 303)//7 2
            {
                Smooth(gamePlayer, tCall, tRaise);
            }
        }
        private void Straight(IPlayer gamePlayer)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (gamePlayer.HandPower <= 480 && gamePlayer.HandPower >= 410)
            {
                Smooth(gamePlayer, sCall, sRaise);
            }
            if (gamePlayer.HandPower <= 409 && gamePlayer.HandPower >= 407)//10  8
            {
                Smooth(gamePlayer, sCall, sRaise);
            }
            if (gamePlayer.HandPower < 407 && gamePlayer.HandPower >= 404)
            {
                Smooth(gamePlayer, sCall, sRaise);
            }
        }
        private void Flush(IPlayer gamePlayer)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(gamePlayer, fCall, fRaise);
        }
        private void FullHouse(IPlayer gamePlayer)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (gamePlayer.HandPower <= 626 && gamePlayer.HandPower>= 620)
            {
                Smooth(gamePlayer, fhCall, fhRaise);
            }
            if (gamePlayer.HandPower < 620 && gamePlayer.HandPower >= 602)
            {
                Smooth(gamePlayer, fhCall, fhRaise);
            }
        }
        private void FourOfAKind(IPlayer gamePlayer)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (gamePlayer.HandPower <= 752 && gamePlayer.HandPower >= 704)
            {
                Smooth(gamePlayer, fkCall, fkRaise);
            }
        }
        private void StraightFlush(IPlayer gamePlayer)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (gamePlayer.HandPower <= 913 && gamePlayer.HandPower >= 804)
            {
                Smooth(gamePlayer, sfCall, sfRaise);
            }
        }

        private void Fold(IPlayer gamePlayer)
        {
            raising = false;
            gamePlayer.Status.Text = "Fold";
            gamePlayer.Turn = false;
            gamePlayer.GameEnded = true;
        }
        private void Check(IPlayer gamePlayer)
        {
            gamePlayer.Status.Text = "Check";
            gamePlayer.Turn = false;
            raising = false;
        }
        private void Call(IPlayer gamePlayer)
        {
            raising = false;
            gamePlayer.Turn = false;
            gamePlayer.Chips -= call;
            gamePlayer.Status.Text = "Call " + call;
            potTextBox.Text = (int.Parse(potTextBox.Text) + call).ToString();
        }
        private void Raised(IPlayer gamePlayer)
        {
            gamePlayer.Chips -= Convert.ToInt32(Raise);
            gamePlayer.Status.Text = "Raise " + Raise;
            potTextBox.Text = (int.Parse(potTextBox.Text) + Convert.ToInt32(Raise)).ToString();
            call = Convert.ToInt32(Raise);
            raising = true;
            gamePlayer.Turn = false;
        }
        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }
        private void HP(IPlayer gamePlayer, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (call <= 0)
            {
                Check(gamePlayer);
            }
            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(gamePlayer.Chips, n))
                    {
                        Call(gamePlayer);
                    }
                    else
                    {
                        Fold(gamePlayer);
                    }
                }
                if (rnd == 2)
                {
                    if (call <= RoundN(gamePlayer.Chips, n1))
                    {
                        Call(gamePlayer);
                    }
                    else
                    {
                        Fold(gamePlayer);
                    }
                }
            }
            if (rnd == 3)
            {
                if (Raise == 0)
                {
                    Raise = call * 2;
                    Raised(gamePlayer);
                }
                else
                {
                    if (Raise <= RoundN(gamePlayer.Chips, n))
                    {
                        Raise = call * 2;
                        Raised(gamePlayer);
                    }
                    else
                    {
                        Fold(gamePlayer);
                    }
                }
            }
            if (gamePlayer.Chips <= 0)
            {
                player.GameEnded = true;
            }
        }
        private void PH(IPlayer gamePlayer, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (call <= 0)
                {
                    Check(gamePlayer);
                }
                if (call > 0)
                {
                    if (call >= RoundN(gamePlayer.Chips, n1))
                    {
                        Fold(gamePlayer);
                    }
                    if (Raise > RoundN(gamePlayer.Chips, n))
                    {
                        Fold(gamePlayer);
                    }
                    if (!gamePlayer.GameEnded)
                    {
                        if (call >= RoundN(gamePlayer.Chips, n) && call <= RoundN(gamePlayer.Chips, n1))
                        {
                            Call(gamePlayer);
                        }
                        if (Raise <= RoundN(gamePlayer.Chips, n) && Raise >= (RoundN(gamePlayer.Chips, n)) / 2)
                        {
                            Call(gamePlayer);
                        }
                        if (Raise <= (RoundN(gamePlayer.Chips, n)) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(gamePlayer.Chips, n);
                                Raised(gamePlayer);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(gamePlayer);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(gamePlayer.Chips, n1 - rnd))
                    {
                        Fold(gamePlayer);
                    }
                    if (Raise > RoundN(gamePlayer.Chips, n - rnd))
                    {
                        Fold(gamePlayer);
                    }
                    if (!gamePlayer.GameEnded)
                    {
                        if (call >= RoundN(gamePlayer.Chips, n - rnd) && call <= RoundN(gamePlayer.Chips, n1 - rnd))
                        {
                            Call(gamePlayer);
                        }
                        if (Raise <= RoundN(gamePlayer.Chips, n - rnd) && Raise >= (RoundN(gamePlayer.Chips, n - rnd)) / 2)
                        {
                            Call(gamePlayer);
                        }
                        if (Raise <= (RoundN(gamePlayer.Chips, n - rnd)) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(gamePlayer.Chips, n - rnd);
                                Raised(gamePlayer);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(gamePlayer);
                            }
                        }
                    }
                }
                if (call <= 0)
                {
                    Raise = RoundN(gamePlayer.Chips, r - rnd);
                    Raised(gamePlayer);
                }
            }
            if (gamePlayer.Chips <= 0)
            {
                gamePlayer.GameEnded = true;
            }
        }
        void Smooth(IPlayer gamePlayer, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (call <= 0)
            {
                Check(gamePlayer);
            }
            else
            {
                if (call >= RoundN(gamePlayer.Chips, n))
                {
                    if (gamePlayer.Chips > call)
                    {
                        Call(gamePlayer);
                    }
                    else if (gamePlayer.Chips <= call)
                    {
                        raising = false;
                        gamePlayer.Turn = false;
                        gamePlayer.Chips = 0;
                        gamePlayer.Status.Text = "Call " + gamePlayer.Chips;
                        potTextBox.Text = (int.Parse(potTextBox.Text) + gamePlayer.Chips).ToString();
                    }
                }
                else
                {
                    if (Raise > 0)
                    {
                        if (gamePlayer.Chips >= Raise * 2)
                        {
                            Raise *= 2;
                            Raised(gamePlayer);
                        }
                        else
                        {
                            Call(gamePlayer);
                        }
                    }
                    else
                    {
                        Raise = call * 2;
                        Raised(gamePlayer);
                    }
                }
            }
            if (gamePlayer.Chips <= 0)
            {
                gamePlayer.GameEnded = true;
            }
        }

        #region UI
        private async void timer_Tick(object sender, object e)
        {
            if (progressBarTimer.Value <= 0)
            {
                player.GameEnded = true;
                await Turns();
            }
            if (t > 0)
            {
                t--;
                progressBarTimer.Value = (t / 6) * 100;
            }
        }
        private void Update_Tick(object sender, object e)
        {
            if (player.Chips <= 0)
            {
                playerChipsTextBox.Text = "Chips : 0";
            }
            if (bot1.Chips <= 0)
            {
                bot1ChipsTextBox.Text = "Chips : 0";
            }
            if (bot2.Chips <= 0)
            {
                bot2ChipsTextBox.Text = "Chips : 0";
            }
            if (bot3.Chips <= 0)
            {
                bot3ChipsTextBox.Text = "Chips : 0";
            }
            if (bot4.Chips <= 0)
            {
                bot4ChipsTextBox.Text = "Chips : 0";
            }
            if (bot5.Chips <= 0)
            {
                bot5ChipsTextBox.Text = "Chips : 0";
            }
            playerChipsTextBox.Text = "Chips : " + player.Chips.ToString();
            bot1ChipsTextBox.Text = "Chips : " + bot1.Chips.ToString();
            bot2ChipsTextBox.Text = "Chips : " + bot2.Chips.ToString();
            bot3ChipsTextBox.Text = "Chips : " + bot3.Chips.ToString();
            bot4ChipsTextBox.Text = "Chips : " + bot4.Chips.ToString();
            bot5ChipsTextBox.Text = "Chips : " + bot5.Chips.ToString();
            if (player.Chips <= 0)
            {
                player.Turn = false;
                player.GameEnded = true;
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

            if (raiseTextBox.Text != "" && int.TryParse(raiseTextBox.Text, out parsedValue))
            {
                if (player.Chips <= int.Parse(raiseTextBox.Text))
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
            playerStatus.Text = "Fold";
            player.Turn = false;
            player.GameEnded = true;
            await Turns();
        }
        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                player.Turn = false;
                playerStatus.Text = "Check";
            }
            else
            {
                //playerStatus.Text = "All in " + Chips;

                buttonCheck.Enabled = false;
            }
            await Turns();
        }
        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(player);
            if (player.Chips >= call)
            {
                player.Chips -= call;
                playerChipsTextBox.Text = "Chips : " + player.Chips.ToString();
                if (potTextBox.Text != "")
                {
                    potTextBox.Text = (int.Parse(potTextBox.Text) + call).ToString();
                }
                else
                {
                    potTextBox.Text = call.ToString();
                }
                player.Turn = false;
                playerStatus.Text = "Call " + call;
                player.Call = call;
            }
            else if (player.Chips <= call && call > 0)
            {
                potTextBox.Text = (int.Parse(potTextBox.Text) + player.Chips).ToString();
                playerStatus.Text = "All in " + player.Chips;
                player.Chips = 0;
                playerChipsTextBox.Text = "Chips : " + player.Chips.ToString();
                player.Turn = false;
                buttonFold.Enabled = false;
                player.Call = player.Chips;
            }
            await Turns();
        }
        private async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(player);
            int parsedValue;
            if (raiseTextBox.Text != "" && int.TryParse(raiseTextBox.Text, out parsedValue))
            {
                if (player.Chips > call)
                {
                    if (Raise * 2 > int.Parse(raiseTextBox.Text))
                    {
                        raiseTextBox.Text = (Raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the currentHandType raise !");
                        return;
                    }
                    else
                    {
                        if (player.Chips >= int.Parse(raiseTextBox.Text))
                        {
                            call = int.Parse(raiseTextBox.Text);
                            Raise = int.Parse(raiseTextBox.Text);
                            playerStatus.Text = "Raise " + call.ToString();
                            potTextBox.Text = (int.Parse(potTextBox.Text) + call).ToString();
                            buttonCall.Text = "Call";
                            player.Chips -= int.Parse(raiseTextBox.Text);
                            raising = true;
                            last = 0;
                            player.Raise = Convert.ToInt32(Raise);
                        }
                        else
                        {
                            call = player.Chips;
                            Raise = player.Chips;
                            potTextBox.Text = (int.Parse(potTextBox.Text) + player.Chips).ToString();
                            playerStatus.Text = "Raise " + call.ToString();
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
            player.Turn = false;
            await Turns();
        }
        private void bAdd_Click(object sender, EventArgs e)
        {
            if (addTextBox.Text == "") { }
            else
            {
                player.Chips += int.Parse(addTextBox.Text);
                bot1.Chips += int.Parse(addTextBox.Text);
                bot2.Chips += int.Parse(addTextBox.Text);
                bot3.Chips += int.Parse(addTextBox.Text);
                bot4.Chips += int.Parse(addTextBox.Text);
                bot5.Chips += int.Parse(addTextBox.Text);
            }
            playerChipsTextBox.Text = "Chips : " + player.Chips.ToString();
        }
        private void bOptions_Click(object sender, EventArgs e)
        {
            bigBlindTextBox.Text = bigBlind.ToString();
            smallBlindTextBox.Text = smallBlind.ToString();
            if (bigBlindTextBox.Visible == false)
            {
                bigBlindTextBox.Visible = true;
                smallBlindTextBox.Visible = true;
                bigBlindButton.Visible = true;
                smallBlindButton.Visible = true;
            }
            else
            {
                bigBlindTextBox.Visible = false;
                smallBlindTextBox.Visible = false;
                bigBlindButton.Visible = false;
                smallBlindButton.Visible = false;
            }
        }
        private void bSB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (smallBlindTextBox.Text.Contains(",") || smallBlindTextBox.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                smallBlindTextBox.Text = smallBlind.ToString();
                return;
            }
            if (!int.TryParse(smallBlindTextBox.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                smallBlindTextBox.Text = smallBlind.ToString();
                return;
            }
            if (int.Parse(smallBlindTextBox.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                smallBlindTextBox.Text = smallBlind.ToString();
            }
            if (int.Parse(smallBlindTextBox.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            if (int.Parse(smallBlindTextBox.Text) >= 250 && int.Parse(smallBlindTextBox.Text) <= 100000)
            {
                smallBlind = int.Parse(smallBlindTextBox.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        private void bBB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (bigBlindTextBox.Text.Contains(",") || bigBlindTextBox.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                bigBlindTextBox.Text = bigBlind.ToString();
                return;
            }
            if (!int.TryParse(smallBlindTextBox.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                smallBlindTextBox.Text = bigBlind.ToString();
                return;
            }
            if (int.Parse(bigBlindTextBox.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                bigBlindTextBox.Text = bigBlind.ToString();
            }
            if (int.Parse(bigBlindTextBox.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }
            if (int.Parse(bigBlindTextBox.Text) >= 500 && int.Parse(bigBlindTextBox.Text) <= 200000)
            {
                bigBlind = int.Parse(bigBlindTextBox.Text);
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