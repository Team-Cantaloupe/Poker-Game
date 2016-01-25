namespace Poker
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonFold = new System.Windows.Forms.Button();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.buttonCall = new System.Windows.Forms.Button();
            this.buttonRaise = new System.Windows.Forms.Button();
            this.progressBarTimer = new System.Windows.Forms.ProgressBar();
            this.playerChipsTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.addTextBox = new System.Windows.Forms.TextBox();
            this.bot5ChipsTextBox = new System.Windows.Forms.TextBox();
            this.bot4ChipsTextBox = new System.Windows.Forms.TextBox();
            this.bot3ChipsTextBox = new System.Windows.Forms.TextBox();
            this.bot2ChipsTextBox = new System.Windows.Forms.TextBox();
            this.bot1ChipsTextBox = new System.Windows.Forms.TextBox();
            this.potTextBox = new System.Windows.Forms.TextBox();
            this.bOptions = new System.Windows.Forms.Button();
            this.bigBlindButton = new System.Windows.Forms.Button();
            this.smallBlindTextBox = new System.Windows.Forms.TextBox();
            this.smallBlindButton = new System.Windows.Forms.Button();
            this.bigBlindTextBox = new System.Windows.Forms.TextBox();
            this.bot5Status = new System.Windows.Forms.Label();
            this.bot4Status = new System.Windows.Forms.Label();
            this.bot3Status = new System.Windows.Forms.Label();
            this.bot1Status = new System.Windows.Forms.Label();
            this.playerStatus = new System.Windows.Forms.Label();
            this.bot2Status = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.raiseTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonFold
            // 
            this.buttonFold.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonFold.Location = new System.Drawing.Point(302, 660);
            this.buttonFold.Name = "buttonFold";
            this.buttonFold.Size = new System.Drawing.Size(130, 62);
            this.buttonFold.TabIndex = 0;
            this.buttonFold.Text = "Fold";
            this.buttonFold.UseVisualStyleBackColor = true;
            this.buttonFold.Click += new System.EventHandler(this.bFold_Click);
            // 
            // buttonCheck
            // 
            this.buttonCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCheck.Location = new System.Drawing.Point(461, 660);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(134, 62);
            this.buttonCheck.TabIndex = 2;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // buttonCall
            // 
            this.buttonCall.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCall.Location = new System.Drawing.Point(634, 661);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Size = new System.Drawing.Size(126, 62);
            this.buttonCall.TabIndex = 3;
            this.buttonCall.Text = "Call";
            this.buttonCall.UseVisualStyleBackColor = true;
            this.buttonCall.Click += new System.EventHandler(this.bCall_Click);
            // 
            // buttonRaise
            // 
            this.buttonRaise.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonRaise.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRaise.Location = new System.Drawing.Point(802, 661);
            this.buttonRaise.Name = "buttonRaise";
            this.buttonRaise.Size = new System.Drawing.Size(124, 62);
            this.buttonRaise.TabIndex = 4;
            this.buttonRaise.Text = "Raise";
            this.buttonRaise.UseVisualStyleBackColor = true;
            this.buttonRaise.Click += new System.EventHandler(this.bRaise_Click);
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.progressBarTimer.BackColor = System.Drawing.SystemColors.Control;
            this.progressBarTimer.Location = new System.Drawing.Point(302, 631);
            this.progressBarTimer.Maximum = 1000;
            this.progressBarTimer.Name = "progressBarTimer";
            this.progressBarTimer.Size = new System.Drawing.Size(667, 23);
            this.progressBarTimer.TabIndex = 5;
            this.progressBarTimer.Value = 1000;
            // 
            // playerChipsTextBox
            // 
            this.playerChipsTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.playerChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.playerChipsTextBox.Location = new System.Drawing.Point(722, 553);
            this.playerChipsTextBox.Name = "playerChipsTextBox";
            this.playerChipsTextBox.Size = new System.Drawing.Size(163, 23);
            this.playerChipsTextBox.TabIndex = 6;
            this.playerChipsTextBox.Text = "Chips : 0";
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(12, 697);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 25);
            this.addButton.TabIndex = 7;
            this.addButton.Text = "AddChips";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // addTextBox
            // 
            this.addTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addTextBox.Location = new System.Drawing.Point(93, 700);
            this.addTextBox.Name = "addTextBox";
            this.addTextBox.Size = new System.Drawing.Size(125, 20);
            this.addTextBox.TabIndex = 8;
            // 
            // bot5ChipsTextBox
            // 
            this.bot5ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bot5ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bot5ChipsTextBox.Location = new System.Drawing.Point(946, 553);
            this.bot5ChipsTextBox.Name = "bot5ChipsTextBox";
            this.bot5ChipsTextBox.Size = new System.Drawing.Size(152, 23);
            this.bot5ChipsTextBox.TabIndex = 9;
            this.bot5ChipsTextBox.Text = "Chips : 0";
            // 
            // bot4ChipsTextBox
            // 
            this.bot4ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bot4ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bot4ChipsTextBox.Location = new System.Drawing.Point(904, 81);
            this.bot4ChipsTextBox.Name = "bot4ChipsTextBox";
            this.bot4ChipsTextBox.Size = new System.Drawing.Size(123, 23);
            this.bot4ChipsTextBox.TabIndex = 10;
            this.bot4ChipsTextBox.Text = "Chips : 0";
            // 
            // bot3ChipsTextBox
            // 
            this.bot3ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bot3ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bot3ChipsTextBox.Location = new System.Drawing.Point(689, 81);
            this.bot3ChipsTextBox.Name = "bot3ChipsTextBox";
            this.bot3ChipsTextBox.Size = new System.Drawing.Size(125, 23);
            this.bot3ChipsTextBox.TabIndex = 11;
            this.bot3ChipsTextBox.Text = "Chips : 0";
            // 
            // bot2ChipsTextBox
            // 
            this.bot2ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bot2ChipsTextBox.Location = new System.Drawing.Point(276, 81);
            this.bot2ChipsTextBox.Name = "bot2ChipsTextBox";
            this.bot2ChipsTextBox.Size = new System.Drawing.Size(133, 23);
            this.bot2ChipsTextBox.TabIndex = 12;
            this.bot2ChipsTextBox.Text = "Chips : 0";
            // 
            // bot1ChipsTextBox
            // 
            this.bot1ChipsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bot1ChipsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bot1ChipsTextBox.Location = new System.Drawing.Point(181, 553);
            this.bot1ChipsTextBox.Name = "bot1ChipsTextBox";
            this.bot1ChipsTextBox.Size = new System.Drawing.Size(142, 23);
            this.bot1ChipsTextBox.TabIndex = 13;
            this.bot1ChipsTextBox.Text = "Chips : 0";
            // 
            // potTextBox
            // 
            this.potTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.potTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.potTextBox.Location = new System.Drawing.Point(573, 212);
            this.potTextBox.Name = "potTextBox";
            this.potTextBox.Size = new System.Drawing.Size(125, 23);
            this.potTextBox.TabIndex = 14;
            this.potTextBox.Text = "0";
            // 
            // bOptions
            // 
            this.bOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bOptions.Location = new System.Drawing.Point(12, 12);
            this.bOptions.Name = "bOptions";
            this.bOptions.Size = new System.Drawing.Size(75, 36);
            this.bOptions.TabIndex = 15;
            this.bOptions.Text = "BB/SB";
            this.bOptions.UseVisualStyleBackColor = true;
            this.bOptions.Click += new System.EventHandler(this.bOptions_Click);
            // 
            // bigBlindButton
            // 
            this.bigBlindButton.Location = new System.Drawing.Point(12, 254);
            this.bigBlindButton.Name = "bigBlindButton";
            this.bigBlindButton.Size = new System.Drawing.Size(75, 23);
            this.bigBlindButton.TabIndex = 16;
            this.bigBlindButton.Text = "Big Blind";
            this.bigBlindButton.UseVisualStyleBackColor = true;
            this.bigBlindButton.Click += new System.EventHandler(this.bBB_Click);
            // 
            // smallBlindTextBox
            // 
            this.smallBlindTextBox.Location = new System.Drawing.Point(12, 228);
            this.smallBlindTextBox.Name = "smallBlindTextBox";
            this.smallBlindTextBox.Size = new System.Drawing.Size(75, 20);
            this.smallBlindTextBox.TabIndex = 17;
            this.smallBlindTextBox.Text = "250";
            // 
            // smallBlindButton
            // 
            this.smallBlindButton.Location = new System.Drawing.Point(12, 199);
            this.smallBlindButton.Name = "smallBlindButton";
            this.smallBlindButton.Size = new System.Drawing.Size(75, 23);
            this.smallBlindButton.TabIndex = 18;
            this.smallBlindButton.Text = "Small Blind";
            this.smallBlindButton.UseVisualStyleBackColor = true;
            this.smallBlindButton.Click += new System.EventHandler(this.bSB_Click);
            // 
            // bigBlindTextBox
            // 
            this.bigBlindTextBox.Location = new System.Drawing.Point(12, 283);
            this.bigBlindTextBox.Name = "bigBlindTextBox";
            this.bigBlindTextBox.Size = new System.Drawing.Size(75, 20);
            this.bigBlindTextBox.TabIndex = 19;
            this.bigBlindTextBox.Text = "500";
            // 
            // bot5Status
            // 
            this.bot5Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bot5Status.Location = new System.Drawing.Point(946, 579);
            this.bot5Status.Name = "bot5Status";
            this.bot5Status.Size = new System.Drawing.Size(152, 32);
            this.bot5Status.TabIndex = 26;
            // 
            // bot4Status
            // 
            this.bot4Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bot4Status.Location = new System.Drawing.Point(904, 107);
            this.bot4Status.Name = "bot4Status";
            this.bot4Status.Size = new System.Drawing.Size(123, 32);
            this.bot4Status.TabIndex = 27;
            // 
            // bot3Status
            // 
            this.bot3Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bot3Status.Location = new System.Drawing.Point(689, 107);
            this.bot3Status.Name = "bot3Status";
            this.bot3Status.Size = new System.Drawing.Size(125, 32);
            this.bot3Status.TabIndex = 28;
            // 
            // bot1Status
            // 
            this.bot1Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bot1Status.Location = new System.Drawing.Point(181, 579);
            this.bot1Status.Name = "bot1Status";
            this.bot1Status.Size = new System.Drawing.Size(142, 32);
            this.bot1Status.TabIndex = 29;
            // 
            // playerStatus
            // 
            this.playerStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.playerStatus.Location = new System.Drawing.Point(722, 579);
            this.playerStatus.Name = "playerStatus";
            this.playerStatus.Size = new System.Drawing.Size(163, 32);
            this.playerStatus.TabIndex = 30;
            // 
            // bot2Status
            // 
            this.bot2Status.Location = new System.Drawing.Point(276, 107);
            this.bot2Status.Name = "bot2Status";
            this.bot2Status.Size = new System.Drawing.Size(133, 32);
            this.bot2Status.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(621, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pot";
            // 
            // raiseTextBox
            // 
            this.raiseTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.raiseTextBox.Location = new System.Drawing.Point(932, 703);
            this.raiseTextBox.Name = "raiseTextBox";
            this.raiseTextBox.Size = new System.Drawing.Size(108, 20);
            this.raiseTextBox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Poker.Properties.Resources.poker_table___Copy;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1284, 729);
            this.Controls.Add(this.raiseTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bot2Status);
            this.Controls.Add(this.playerStatus);
            this.Controls.Add(this.bot1Status);
            this.Controls.Add(this.bot3Status);
            this.Controls.Add(this.bot4Status);
            this.Controls.Add(this.bot5Status);
            this.Controls.Add(this.bigBlindTextBox);
            this.Controls.Add(this.smallBlindButton);
            this.Controls.Add(this.smallBlindTextBox);
            this.Controls.Add(this.bigBlindButton);
            this.Controls.Add(this.bOptions);
            this.Controls.Add(this.potTextBox);
            this.Controls.Add(this.bot1ChipsTextBox);
            this.Controls.Add(this.bot2ChipsTextBox);
            this.Controls.Add(this.bot3ChipsTextBox);
            this.Controls.Add(this.bot4ChipsTextBox);
            this.Controls.Add(this.bot5ChipsTextBox);
            this.Controls.Add(this.addTextBox);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.playerChipsTextBox);
            this.Controls.Add(this.progressBarTimer);
            this.Controls.Add(this.buttonRaise);
            this.Controls.Add(this.buttonCall);
            this.Controls.Add(this.buttonCheck);
            this.Controls.Add(this.buttonFold);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "GLS Texas Poker";
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Layout_Change);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFold;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.Button buttonCall;
        private System.Windows.Forms.Button buttonRaise;
        private System.Windows.Forms.ProgressBar progressBarTimer;
        private System.Windows.Forms.TextBox playerChipsTextBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.TextBox addTextBox;
        private System.Windows.Forms.TextBox bot5ChipsTextBox;
        private System.Windows.Forms.TextBox bot4ChipsTextBox;
        private System.Windows.Forms.TextBox bot3ChipsTextBox;
        private System.Windows.Forms.TextBox bot2ChipsTextBox;
        private System.Windows.Forms.TextBox bot1ChipsTextBox;
        private System.Windows.Forms.TextBox potTextBox;
        private System.Windows.Forms.Button bOptions;
        private System.Windows.Forms.Button bigBlindButton;
        private System.Windows.Forms.TextBox smallBlindTextBox;
        private System.Windows.Forms.Button smallBlindButton;
        private System.Windows.Forms.TextBox bigBlindTextBox;
        private System.Windows.Forms.Label bot5Status;
        private System.Windows.Forms.Label bot4Status;
        private System.Windows.Forms.Label bot3Status;
        private System.Windows.Forms.Label bot2Status;
        private System.Windows.Forms.Label bot1Status;
        private System.Windows.Forms.Label playerStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox raiseTextBox;



    }
}

