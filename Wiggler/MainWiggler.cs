namespace Wiggler
{
    public partial class MainWiggler : Form
    {
        private Button startButton;

        public MainWiggler()
        {
            InitializeComponent();
            AddControls();
        }

        private void AddControls()
        {
            startButton = new Button
            {
                Text = CommonStrings.Start,
                Location = new Point(50, 30),
                Size = new Size(100, 30)
            };

            startButton.Click += Start_Click;

            this.Controls.Add(startButton);
        }

        private async void Start_Click(object sender, EventArgs e)
        {
            Int32 i = 0;
            while (i < 10)
            {
                await MoveCursor();
                i++;

                //this freezes the wiggler app window while running
            }
        }

        private async Task MoveCursor()
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y - 1);
            Task.Delay(200).Wait();
            Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);
            Task.Delay(200).Wait();
        }
    }
}
