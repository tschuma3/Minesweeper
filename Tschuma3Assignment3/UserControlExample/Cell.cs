using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserControlExample
{
    public partial class Cell : UserControl
    {

        public EventHandler ButtonHasBeenClicked;
        Button myButton = new Button();
        Panel myPanel = new Panel();

        // Position within the grid
        int row;
        int col;

        // Bombs
        int neighboringBombs = 0;

        /// <summary>
        /// The cells
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public Cell(int row, int col)
        {
            // Sets rows and columns
            this.Row = row;
            this.Col = col;

            // Sets up the button
            myButton.Size = new Size(24, 24);
            myButton.Click += new EventHandler(OnButtonClick);
            this.Controls.Add(myButton);

            // Sets up the panel
            myPanel.Size = new Size(24, 24);
            myPanel.BackColor = Color.HotPink;
            this.Controls.Add(myPanel);

            // Initializes the components
            InitializeComponent();
        }

        // Triggers the cascade effect and makes button invisible, revealing the panel
        public void OnButtonClick(object sender, EventArgs e)
        {
            if (myButton.Text != "0")
            {
                myButton.Visible = false;

                myButton.Text = "0";

                if (ButtonHasBeenClicked != null)
                {
                    this.ButtonHasBeenClicked(this, e);
                }
            }
        }

        /// <summary>
        /// Gets and sets the panel colors
        /// </summary>
        public Color PanelColor
        {
            get
            {
                return myPanel.BackColor;
            }
            set
            {
                myPanel.BackColor = value;
            }
        }
        /// <summary>
        /// Gets and sets the rows
        /// </summary>
        public int Row { get => row; set => row = value; }
       /// <summary>
       /// Gets and sets the columns
       /// </summary>
        public int Col { get => col; set => col = value; }
        /// <summary>
        /// Gets the buttons
        /// </summary>
        public Button MyButton { get => myButton; }
        /// <summary>
        /// Gets the panels
        /// </summary>
        public Panel MyPanel { get => myPanel; }
        /// <summary>
        /// Gets and sets the neighboring bombs
        /// </summary>
        public int NeighboringBombs { get => neighboringBombs; set => neighboringBombs = value; }
    }
}
