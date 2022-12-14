using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserControlExample
{
    public partial class Form1 : Form
    {
        // Variables
        Timer timer = new Timer();
        Cell[,] grid;
        int[] gameRecordsData = new int[3]; // index 0 = wins, 1 = loses, 2 = elapsedTime
        int elapsedTime = 0;
        int numberOfMines = 10;
        int buttonClicks = 0;

        /// <summary>
        /// Main method
        /// </summary>
        public Form1()
        {
            // Fills grid
            FillGrid();

            // Timer interval and tick
            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerTick);
        }

        /// <summary>
        /// Fills the grid
        /// </summary>
        public void FillGrid()
        {
            // Random
            Random rand = new Random();

            // Clears the form and initializes
            this.Controls.Clear();
            InitializeComponent();

            // Sets the grid to a 10 by 10
            grid = new Cell[10, 10];

            // Rows
            for(int row = 0; row < grid.GetLength(0); row++)
            {
                // Columns
                for(int col = 0; col < grid.GetLength(1); col++)
                {
                    // Sets the panels and buttons
                    grid[row, col] = new Cell(row, col);
                    grid[row, col].Location = new Point(row * 24, (col * 24) + 24);
                    grid[row, col].ButtonHasBeenClicked += new EventHandler(CellClicked);
                    grid[row, col].PanelColor = Color.Silver;
                    this.Controls.Add(grid[row, col]);
                }
            }

            // Up to the number of mines
            for (int i = 0; i < numberOfMines; i++)
            {
                // Random variables
                int ranY = rand.Next(10);
                int ranX = rand.Next(10);

                // Random location and black color
                grid[ranX, ranY].PanelColor = Color.Black;
            }

            // Checks for bombs
            CheckForBomb();
        }

        /// <summary>
        /// Checks for bombs
        /// </summary>
        private void CheckForBomb()
        {
            // Rows
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                // Columns
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    // Calls the check directions
                    CheckLeftForBomb(grid[row, col]);
                    CheckRightForBomb(grid[row, col]);
                    CheckUpForBomb(grid[row, col]);
                    CheckDownForBomb(grid[row, col]);
                    CheckUpLeftForBomb(grid[row, col]);
                    CheckUpRightForBomb(grid[row, col]);
                    CheckDownLeftForBomb(grid[row, col]);
                    CheckDownRightForBomb(grid[row, col]);

                    // Creates a label, sets the texts, and adds
                    Label temp = new Label();
                    temp.Text = grid[row, col].NeighboringBombs.ToString();
                    grid[row, col].MyPanel.Controls.Add(temp);
                }
            }
        }

        /// <summary>
        /// Clicks the cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CellClicked(object sender, EventArgs e)
        {
            // If the button clicks are equal to 0
            if (buttonClicks == 0)
            {
                timer.Start();
            }

            // Cell variable
            Cell cell = ((Cell)sender);

            // Calls the checks method to click
            CheckLeft(cell);
            CheckRight(cell);
            CheckUp(cell);
            CheckDown(cell);

            // Adds to button clicks
            buttonClicks++;

            // If lose
            if (Lose(cell))
            {
                // Sets to false
                StatCheckEndGame(false);
            }

            // If win
            if (Win())
            {
                // Sets to true
                StatCheckEndGame(true);
            }
        }

        /// <summary>
        /// Checks down to perform click
        /// </summary>
        /// <param name="cell"></param>
        private void CheckDown(Cell cell)
        {
            if (cell.Row < grid.GetLength(0) - 1)
            {
                if (cell.NeighboringBombs == 0)
                {

                    grid[cell.Row + 1, cell.Col].MyButton.PerformClick();
                }
            }
        }

        /// <summary>
        /// Checks up to perform click
        /// </summary>
        /// <param name="cell"></param>
        private void CheckUp(Cell cell)
        {
            if (cell.Row > 0)
            {
                if (cell.NeighboringBombs == 0)
                {
                    grid[cell.Row - 1, cell.Col].MyButton.PerformClick();
                }
            }
        }

        /// <summary>
        /// Checks right to perform click
        /// </summary>
        /// <param name="cell"></param>
        private void CheckRight(Cell cell)
        {
            if (cell.Col < grid.GetLength(1) - 1)
            {
                if (cell.NeighboringBombs == 0)
                {
                    grid[cell.Row, cell.Col + 1].MyButton.PerformClick();
                }
            }
        }

        /// <summary>
        /// Checks left to perform click
        /// </summary>
        /// <param name="cell"></param>
        private void CheckLeft(Cell cell)
        {
            if (cell.Col > 0)
            {
                if (cell.NeighboringBombs == 0)
                {
                    grid[cell.Row, cell.Col - 1].MyButton.PerformClick();
                }
            }
        }
         /// <summary>
         /// Checks down for bomb and adds to the text
         /// </summary>
         /// <param name="cell"></param>
        private void CheckDownForBomb(Cell cell)
        {
            if (cell.Row < grid.GetLength(0) - 1)
            {
                if (grid[cell.Row + 1, cell.Col].PanelColor == Color.Black)
                {
                    cell.NeighboringBombs += 1;
                }
            }
        }

        /// <summary>
        /// Checks up for bomb and adds to the text
        /// </summary>
        /// <param name="cell"></param>
        private void CheckUpForBomb(Cell cell)
        {
            if (cell.Row > 0)
            {
                if (grid[cell.Row - 1, cell.Col].PanelColor == Color.Black)
                {
                    cell.NeighboringBombs += 1;
                }
            }
        }

        /// <summary>
        /// Checks right for bomb and adds to the text
        /// </summary>
        /// <param name="cell"></param>
        private void CheckRightForBomb(Cell cell)
        {
            if (cell.Col < grid.GetLength(1) - 1)
            {
                if (grid[cell.Row, cell.Col + 1].PanelColor == Color.Black)
                {
                    cell.NeighboringBombs += 1;
                }
            }
        }

        /// <summary>
        /// Checks left for bomb and adds to the text
        /// </summary>
        /// <param name="cell"></param>
        private void CheckLeftForBomb(Cell cell)
        {
            if (cell.Col > 0)
            {
                if (grid[cell.Row, cell.Col - 1].PanelColor == Color.Black)
                {
                    cell.NeighboringBombs += 1;
                }
            }
        }

        /// <summary>
        /// Checks upper right for bomb and adds to the text
        /// </summary>
        /// <param name="cell"></param>
        public void CheckUpRightForBomb(Cell cell)
        {
            if (cell.Row > 0 && cell.Col < grid.GetLength(1) - 1)
            {
                if (grid[cell.Row - 1, cell.Col + 1].PanelColor == Color.Black)
                {
                    cell.NeighboringBombs += 1;
                }
            }
        }

        /// <summary>
        /// Checks upper left for bomb and adds to the text
        /// </summary>
        /// <param name="cell"></param>
        public void CheckUpLeftForBomb(Cell cell)
        {
            if (cell.Row > 0 && cell.Col > 0)
            {
                if (grid[cell.Row - 1, cell.Col - 1].PanelColor == Color.Black)
                {
                    cell.NeighboringBombs += 1;
                }
            }
        }

        /// <summary>
        /// Checks lower right for bomb and adds to the text
        /// </summary>
        /// <param name="cell"></param>
        public void CheckDownRightForBomb(Cell cell)
        {
            if (cell.Row < grid.GetLength(0) - 1 && cell.Col < grid.GetLength(1) - 1)
            {
                if (grid[cell.Row + 1, cell.Col + 1].PanelColor == Color.Black)
                {
                    cell.NeighboringBombs += 1;
                }
            }
        }

        /// <summary>
        /// Checks lower left for bomb and adds to the text
        /// </summary>
        /// <param name="cell"></param>
        public void CheckDownLeftForBomb(Cell cell)
        {
            if (cell.Row < grid.GetLength(0) - 1 && cell.Col > 0)
            {
                if (grid[cell.Row + 1, cell.Col - 1].PanelColor == Color.Black)
                {
                    cell.NeighboringBombs += 1;
                }
            }
        }

        /// <summary>
        /// Adds to time and displays
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTick(object sender, EventArgs e)
        {
            elapsedTime++;
            toolStripStatusLabel1.Text = $"Timer: {elapsedTime}";
        }

        /// <summary>
        /// Reads the records text file
        /// </summary>
        private void ReadRecords()
        {
            try
            {
                // Variables
                StreamReader reader = new StreamReader("Records.txt");
                string line = reader.ReadLine();
                int count = 0;

                Console.WriteLine("Reading from file");
                
                // While line is not null
                while (line != null)
                {
                    // Writes, parses, adds to count, and sets line
                    Console.WriteLine(line);
                    gameRecordsData[count] = int.Parse(line);
                    count++;
                    line = reader.ReadLine();
                }
                // Closes
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Writes to the records text file
        /// </summary>
        private void WriteRecords()
        {
            try
            {
                // Variables
                StreamWriter writer = new StreamWriter("Records.txt");
                
                Console.WriteLine("Writing to file");
                
                // For each line in the game records
                foreach (int i in gameRecordsData)
                {
                    // Writes into the file
                    writer.WriteLine(i);
                }
                // Closes
                writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Stats for the game
        /// </summary>
        /// <param name="hasWon"></param>
        private void StatCheckEndGame(bool hasWon)
        {
            // Variable
            Label label = new Label();

            // Reads the records 
            ReadRecords();

            // For winning displays text and adds
            if (hasWon == true)
            {
                label.Text = "You Win!";
                gameRecordsData[0]++;
            }
            // For losing displays text and adds
            else
            {
                label.Text = "You Lose!";
                gameRecordsData[1]++;
            }

            // Shows on the form the result
            label.Size = new Size(100, 100);
            label.Font = new Font("Arial", 24);
            label.Location = new Point(245, 50); 
            this.Controls.Add(label);

            // Math to get the average time
            int totalGames = gameRecordsData[0] + gameRecordsData[1] - 1;
            gameRecordsData[2] = (totalGames * gameRecordsData[2] + elapsedTime) / (totalGames + 1);

            // Writes to the records text
            WriteRecords();
        }

        /// <summary>
        /// Loses
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool Lose(Cell cell)
        {
            // Based on the color
            if (cell.PanelColor == Color.Black)
            {
                Console.WriteLine("You Lose!");
                
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Wins
        /// </summary>
        /// <returns></returns>
        private bool Win()
        {
            // Based on the amount of clicks
            if (buttonClicks == 90)
            {
                Console.WriteLine("You Win!");
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Makes a new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillGrid();
            buttonClicks = 0;
            timer.Stop();
            elapsedTime = 0;
        }

        /// <summary>
        /// Views records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadRecords();
            MessageBox.Show("Wins: " + gameRecordsData[0] + "\nLoses: " + gameRecordsData[1] 
                                + "\nAverage Time: " + gameRecordsData[2]);

        }

        /// <summary>
        /// Closes the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Shows instructions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. Left click to unvail a tile  " +
                "\n2. Win if you click all tiles without bombs" +
                "\n3. Lose if a bomb is clicked");
        }

        /// <summary>
        /// Shows the about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. Tyler Schumacher wrote the code  " +
                "\n2. Written on April 14, 2020  " +
                "\n3. Written for CS3020");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //FillGrid();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
