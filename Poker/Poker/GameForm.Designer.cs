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
            this.pbTimer = new System.Windows.Forms.ProgressBar();
            this.playerChipsTextBox = new System.Windows.Forms.TextBox();
            this.bAdd = new System.Windows.Forms.Button();
            this.AddTextBox = new System.Windows.Forms.TextBox();
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
            this.b5Status = new System.Windows.Forms.Label();
            this.b4Status = new System.Windows.Forms.Label();
            this.b3Status = new System.Windows.Forms.Label();
            this.b1Status = new System.Windows.Forms.Label();
            this.pStatus = new System.Windows.Forms.Label();
            this.b2Status = new System.Windows.Forms.Label();
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
            // pbTimer
            // 
            this.pbTimer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pbTimer.BackColor = System.Drawing.SystemColors.Control;
            this.pbTimer.Location = new System.Drawing.Point(302, 631);
            this.pbTimer.Maximum = 1000;
            this.pbTimer.Name = "pbTimer";
            this.pbTimer.Size = new System.Drawing.Size(667, 23);
            this.pbTimer.TabIndex = 5;
            this.pbTimer.Value = 1000;
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
            // bAdd
            // 
            this.bAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bAdd.Location = new System.Drawing.Point(12, 697);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(75, 25);
            this.bAdd.TabIndex = 7;
            this.bAdd.Text = "AddChips";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // AddTextBox
            // 
            this.AddTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddTextBox.Location = new System.Drawing.Point(93, 700);
            this.AddTextBox.Name = "AddTextBox";
            this.AddTextBox.Size = new System.Drawing.Size(125, 20);
            this.AddTextBox.TabIndex = 8;
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
            // b5Status
            // 
            this.b5Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b5Status.Location = new System.Drawing.Point(946, 579);
            this.b5Status.Name = "b5Status";
            this.b5Status.Size = new System.Drawing.Size(152, 32);
            this.b5Status.TabIndex = 26;
            // 
            // b4Status
            // 
            this.b4Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b4Status.Location = new System.Drawing.Point(904, 107);
            this.b4Status.Name = "b4Status";
            this.b4Status.Size = new System.Drawing.Size(123, 32);
            this.b4Status.TabIndex = 27;
            // 
            // b3Status
            // 
            this.b3Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b3Status.Location = new System.Drawing.Point(689, 107);
            this.b3Status.Name = "b3Status";
            this.b3Status.Size = new System.Drawing.Size(125, 32);
            this.b3Status.TabIndex = 28;
            // 
            // b1Status
            // 
            this.b1Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b1Status.Location = new System.Drawing.Point(181, 579);
            this.b1Status.Name = "b1Status";
            this.b1Status.Size = new System.Drawing.Size(142, 32);
            this.b1Status.TabIndex = 29;
            // 
            // pStatus
            // 
            this.pStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pStatus.Location = new System.Drawing.Point(722, 579);
            this.pStatus.Name = "pStatus";
            this.pStatus.Size = new System.Drawing.Size(163, 32);
            this.pStatus.TabIndex = 30;
            // 
            // b2Status
            // 
            this.b2Status.Location = new System.Drawing.Point(276, 107);
            this.b2Status.Name = "b2Status";
            this.b2Status.Size = new System.Drawing.Size(133, 32);
            this.b2Status.TabIndex = 31;
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
            this.Controls.Add(this.b2Status);
            this.Controls.Add(this.pStatus);
            this.Controls.Add(this.b1Status);
            this.Controls.Add(this.b3Status);
            this.Controls.Add(this.b4Status);
            this.Controls.Add(this.b5Status);
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
            this.Controls.Add(this.AddTextBox);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.playerChipsTextBox);
            this.Controls.Add(this.pbTimer);
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
        private System.Windows.Forms.ProgressBar pbTimer;
        private System.Windows.Forms.TextBox playerChipsTextBox;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.TextBox AddTextBox;
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
        private System.Windows.Forms.Label b5Status;
        private System.Windows.Forms.Label b4Status;
        private System.Windows.Forms.Label b3Status;
        private System.Windows.Forms.Label b1Status;
        private System.Windows.Forms.Label pStatus;
        private System.Windows.Forms.Label b2Status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox raiseTextBox;



    }
}

