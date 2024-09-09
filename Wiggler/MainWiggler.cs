namespace Wiggler
{
    public partial class MainWiggler : Form
    {
        private TextBox _intervalInput;
        private Button _startButton;
        private Button _stopButton;
        private Label _enabledLabel;
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
                Location = new Point(10, 30),
                Size = new Size(100, 50),
            };

            // Start
            _startButton = new Button
            {
                Text = CommonStrings.Start,
                Location = new Point(120, 30),
                Size = new Size(70, 30)
            };
            _startButton.Click += Start_Click;

            // Stop
            _stopButton = new Button
            {
                Text = CommonStrings.Stop,
                Location = new Point(120, 70),
                Size = new Size(70, 30)
            };
            _stopButton.Click += Stop_Click;

            // Green / Red
            _enabledLabel = new Label
            {
                Text = CommonStrings.Stopped,
                ForeColor = Color.Red,
                Location = new Point(10, 70),
                Size = new Size(70, 50),
            };

            Control[] controls = new Control[] { _enabledLabel, _intervalInput, _startButton, _stopButton };
            this.Controls.AddRange(controls);
        }

        private void Start_Click(object sender, EventArgs e)
        {

            if (int.TryParse(_intervalInput.Text, out Int32 intervalInMilliseconds))
            {
                intervalInMilliseconds = Math.Max(1000, intervalInMilliseconds);

                // begin timer
                _sysTimer.Interval = intervalInMilliseconds;
                _sysTimer.Tick += MoveCursor;
                _sysTimer.Start();

                // add message
                _enabledLabel.Text = CommonStrings.Running;
                _enabledLabel.ForeColor = Color.Green;
            }
            else
            {
                MessageBox.Show(CommonStrings.InvalidIntervalValue);
                // add message
                _enabledLabel.Text = CommonStrings.Stopped;
                _enabledLabel.ForeColor = Color.Red;
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            // end timer 
            _sysTimer.Stop();

            // Message
            _enabledLabel.Text = CommonStrings.Stopped;
            _enabledLabel.ForeColor = Color.Red;
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
