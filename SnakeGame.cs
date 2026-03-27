using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        // Spielfeld-Einstellungen
        private List<Point> snake = new List<Point>();
        private Point food = new Point();
        private string direction = "right";
private System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();        private int score = 0;
        private int gridSize = 20; // Größe eines Quadrats

        public Form1()
        {
            // Fenster-Einstellungen
            this.Text = "C# Snake mit Freunden";
            this.Width = 600;
            this.Height = 400;
            this.DoubleBuffered = true; // Verhindert Flackern der Grafik

            StartGame();

            // Timer-Logik (Herzschlag des Spiels)
            gameTimer.Interval = 100; // Geschwindigkeit in Millisekunden
            gameTimer.Tick += UpdateGame;
            gameTimer.Start();

            // Tasten-Steuerung
            this.KeyDown += ChangeDirection;
        }

        private void StartGame()
        {
            snake.Clear();
            snake.Add(new Point(5, 5)); // Startposition
            score = 0;
            SpawnFood();
        }

        private void SpawnFood()
        {
            Random rand = new Random();
            food = new Point(rand.Next(0, this.Width / gridSize - 1), rand.Next(0, this.Height / gridSize - 2));
        }

        private void UpdateGame(object sender, EventArgs e)
        {
            // 1. Neuen Kopf berechnen
            Point head = snake.First();
            Point newHead = new Point(head.X, head.Y);

            if (direction == "right") newHead.X++;
            if (direction == "left") newHead.X--;
            if (direction == "up") newHead.Y--;
            if (direction == "down") newHead.Y++;

            // 2. Kollision prüfen (Wand oder sich selbst)
            if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= this.Width / gridSize || newHead.Y >= this.Height / gridSize || snake.Contains(newHead))
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over! Score: " + score);
                StartGame();
                gameTimer.Start();
                return;
            }

            snake.Insert(0, newHead); // Kopf hinzufügen

            // 3. Essen prüfen
            if (newHead == food)
            {
                score++;
                SpawnFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1); // Letztes Teil entfernen (Bewegung)
            }

            this.Invalidate(); // Zeichnet das Fenster neu -> ruft OnPaint auf
        }

        protected override void OnPaint(PaintEventArgs e)
{
    Graphics g = e.Graphics;

    // 1. Hintergrund komplett grün füllen
    g.Clear(Color.DarkGreen);

    // 2. Schwarze Gitterlinien zeichnen
    Pen gridPen = new Pen(Color.Black, 1);
    
    // Vertikale Linien
    for (int x = 0; x <= this.Width; x += gridSize)
    {
        g.DrawLine(gridPen, x, 0, x, this.Height);
    }
    
    // Horizontale Linien
    for (int y = 0; y <= this.Height; y += gridSize)
    {
        g.DrawLine(gridPen, 0, y, this.Width, y);
    }

    // 3. Apfel zeichnen (etwas kleiner als das Feld, damit man das Gitter noch sieht)
    g.FillEllipse(Brushes.Red, food.X * gridSize + 1, food.Y * gridSize + 1, gridSize - 2, gridSize - 2);

    // 4. Schlange zeichnen
    foreach (var part in snake)
    {
        // Hellgrün für die Schlange, damit sie sich vom dunklen Hintergrund abhebt
        g.FillRectangle(Brushes.LimeGreen, part.X * gridSize + 1, part.Y * gridSize + 1, gridSize - 2, gridSize - 2);
    }
}

        private void ChangeDirection(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && direction != "down") direction = "up";
            if (e.KeyCode == Keys.Down && direction != "up") direction = "down";
            if (e.KeyCode == Keys.Left && direction != "right") direction = "left";
            if (e.KeyCode == Keys.Right && direction != "left") direction = "right";
        }
    }
}