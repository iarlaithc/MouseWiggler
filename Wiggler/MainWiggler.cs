namespace Wiggler
{
    public partial class MainWiggler : Form
    {
        private TextBox _intervalInput;
        private Button _startButton;
        private Button _stopButton;
        private System.Windows.Forms.Timer _sysTimer;

        public MainWiggler()
        {
            InitializeComponent();
            AddControls();

            _sysTimer = new System.Windows.Forms.Timer();
        }

        private void AddControls()
        {
            // Interval Input
            _intervalInput = new TextBox
            {   
                PlaceholderText = CommonStrings.Interval,
                Location = new Point(50, 10),
                Size = new Size(100, 50),
            };

            this.Controls.Add(_intervalInput);


            // Start
            _startButton = new Button
            {
                Text = CommonStrings.Start,
                Location = new Point(50, 150),
                Size = new Size(100, 30)
            };

            _startButton.Click += Start_Click;

            this.Controls.Add(_startButton);

            // Stop
            _stopButton = new Button
            {
                Text = CommonStrings.Stop,
                Location = new Point(50, 200),
                Size = new Size(100, 30)
            };

            _stopButton.Click += Stop_Click;

            this.Controls.Add(_stopButton);
        }

        private async void Start_Click(object sender, EventArgs e)
        {

            if (int.TryParse(_intervalInput.Text, out Int32 intervalInMilliseconds))
            {
                intervalInMilliseconds = Math.Max(1000, intervalInMilliseconds);

                // begin timer
                _sysTimer.Interval = intervalInMilliseconds;
                _sysTimer.Tick += MoveCursor;
                _sysTimer.Start();
            }
            else
            {
                MessageBox.Show(CommonStrings.InvalidIntervalValue);
            }
        }

        private async void Stop_Click(object sender, EventArgs e)
        {
            // end timer 
            _sysTimer.Stop();
        }

        private async void MoveCursor(object sender, EventArgs e)
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y - 1);
            await Task.Delay(100);
            Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);
            await Task.Delay(100);
        }
    }
}
