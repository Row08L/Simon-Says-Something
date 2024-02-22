using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Diagnostics;

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        SoundPlayer blueSound = new SoundPlayer(Properties.Resources.blue);
        SoundPlayer greenSound = new SoundPlayer(Properties.Resources.green);
        SoundPlayer redSound = new SoundPlayer(Properties.Resources.red);
        SoundPlayer yellowSound = new SoundPlayer(Properties.Resources.yellow);
        SoundPlayer mistakeSound = new SoundPlayer(Properties.Resources.mistake);

        Stopwatch colorTimer = new Stopwatch();
        Stopwatch antiBuffer = new Stopwatch();
        Random random = new Random();
        List<int> playerColors = new List<int>();
        List<int> compColors = new List<int>();
        public static int turnNumber = 1;
        int numberOfCorrectGuesses = 0;
        int viewTime = 1000;
        bool playerControls = false;
        bool wKeyDown = false;
        bool sKeyDown = false;
        bool spaceKeyDown = false;
        Font drawFont = new Font("Arial", 16);
        SolidBrush drawBrushWhite = new SolidBrush(Color.White);

        public GameScreen()
        {
            InitializeComponent();
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            greenButton.TabStop = false;
            blueButton.TabStop = false;
            redButton.TabStop = false;
            yellowButton.TabStop = false;
            greenButton.Click += Button_Click;
            blueButton.Click += Button_Click;
            redButton.Click += Button_Click;
            yellowButton.Click += Button_Click;
            Thread.Sleep(500);
            ComputerTurn();
        }

        private void ComputerTurn()
        {
            this.ActiveControl = null;
            numberOfCorrectGuesses = 0;
            compColors.Clear();
            playerColors.Clear();
            for (int i = 1; i <= turnNumber; i++)
            {
                int colorChoice = random.Next(1, 5);
                colorTimer.Reset();
                colorTimer.Start();
                while(colorTimer.Elapsed.TotalMilliseconds <= viewTime)
                {
                    if (colorTimer.Elapsed.TotalMilliseconds <= 700)
                    {
                        switch (colorChoice)
                        {
                            case 1:
                                redButton.BackColor = Color.White;
                                redSound.Play();
                                Refresh();
                                break;
                            case 2:
                                blueButton.BackColor = Color.White;
                                blueSound.Play();
                                Refresh();
                                break;
                            case 3:
                                yellowButton.BackColor = Color.White;
                                yellowSound.Play();
                                Refresh();
                                break;
                            case 4:
                                greenButton.BackColor = Color.White;
                                greenSound.Play();
                                Refresh();
                                break;
                        }
                    }
                    else
                    {
                        Reset();
                    }
                    
                }
                colorTimer.Stop();
                compColors.Add(colorChoice);
            }
            playerControls = true;
            antiBuffer.Restart();
            this.ActiveControl = null;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            if (playerControls == true && antiBuffer.Elapsed.TotalMilliseconds > 20)
            {
                Button clickedButton = sender as Button;
                bool right = false;
                switch (clickedButton.Name)
                {
                    // red
                    case "redButton":
                        redSound.Play();
                        playerColors.Add(1);
                        right = Comparer(playerColors, compColors);
                        break;
                    // blue
                    case "blueButton":
                        blueSound.Play();
                        playerColors.Add(2);
                        right = Comparer(playerColors, compColors);
                        break;
                    // yellow
                    case "yellowButton":
                        yellowSound.Play();
                        playerColors.Add(3);
                        right = Comparer(playerColors, compColors);
                        break;
                    // green
                    case "greenButton":
                        greenSound.Play();
                        playerColors.Add(4);
                        right = Comparer(playerColors, compColors);
                        break;
                }
                if (right == false)
                {
                    mistakeSound.Play();
                    Form1.ChangeScreen(this, new GameOverScreen());
                }
                else
                {
                    numberOfCorrectGuesses++;
                }
                if (numberOfCorrectGuesses == turnNumber)
                {
                    turnNumber++;
                    playerControls = false;
                    ComputerTurn();
                }
            }
        }

        void Reset()
        {
            redButton.BackColor = Color.DarkRed;
            blueButton.BackColor = Color.DarkBlue;
            yellowButton.BackColor = Color.Goldenrod;
            greenButton.BackColor = Color.ForestGreen;
            Refresh();
        }
        static bool Comparer (List<int> playerColorList, List<int> compColorList)
        {
            bool a = true;
            int count = 0;
            foreach (int color in playerColorList)
            {
                if (color != compColorList[count])
                {
                    a = false;
                }
                else
                {
                    count++;
                }
            }
            return a;
        }

        public void GameOver()
        {
            //TODO: Play a game over sound

            //TODO: close this screen and open the GameOverScreen

        }

        void ShowColors (List<int> colors)
        {
            playerControls = false;
            for (int i = 1; i <= turnNumber; i++)
            {
                colorTimer.Reset();
                colorTimer.Start();
                while (colorTimer.Elapsed.TotalMilliseconds <= viewTime)
                {
                    if (colorTimer.Elapsed.TotalMilliseconds <= 700)
                    {
                        switch (colors[i - 1])
                        {
                            case 1:
                                redSound.Play();
                                redButton.BackColor = Color.White;
                                Refresh();
                                break;
                            case 2:
                                blueButton.BackColor = Color.White;
                                blueSound.Play();
                                Refresh();
                                break;
                            case 3:
                                yellowButton.BackColor = Color.White;
                                yellowSound.Play();
                                Refresh();
                                break;
                            case 4:
                                greenButton.BackColor = Color.White;
                                greenSound.Play();
                                Refresh();
                                break;
                        }
                    }
                    else
                    {
                        Reset();
                    }
                }

                colorTimer.Stop();
            }
            playerControls = true;
            antiBuffer.Restart();
        }

        private void GameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.Space:
                    spaceKeyDown = true;
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.Space:
                    spaceKeyDown = false;
                    break;
            }
        }

        private void ticks_Tick(object sender, EventArgs e)
        {
            if(spaceKeyDown == true)
            {
                ShowColors(compColors);
            }
            
            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //greenButton.Visible = false;
            //redButton.Visible = false;
            //yellowButton.Visible = false;
            //blueButton.Visible = false;

            Bitmap colorGame = new Bitmap(this.Width, this.Height);
            Graphics GcolorGame = this.CreateGraphics();
            GcolorGame = Graphics.FromImage(colorGame);
            string number = "";
            for (int i = 0; i < compColors.Count; i++)
            {
                number += " _ ";

            }
            for (int i = 0; i < playerColors.Count; i++)
            {
                number = number.Substring(0, i) + " * " + number.Substring(i + 1);
            }
            SizeF textSize = e.Graphics.MeasureString(number, drawFont);
            GcolorGame.DrawString(number, drawFont, drawBrushWhite, new PointF((this.Width /2) - textSize.Width / 2, this.Height - (textSize.Height + 10)));
            e.Graphics.DrawImage(colorGame, new Point(0,0));
        }
    }
}
