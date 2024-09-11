using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;
using CommonStrings = Wiggler.Resources.CommonStrings;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Wiggler
{
    public class MainWiggler : Form
    {
        #region Private Variables

        private TextBox _intervalInput;
        private Button _startButton;
        private Button _stopButton;
        private Label _enabledLabel;
        private Panel _headerPanel;
        private Button _windowExitButton;
        private Button _windowMinimizeButton;
        private Label _windowTitleLabel;
        private Timer _sysTimer;

        #endregion

        #region Draggable Custom Window Variables
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        #endregion

        public MainWiggler()
        {
            InitializeComponent();
            AddMainControls();
            _sysTimer = new Timer();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(300, 200);

            // Custom Font
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile("Resources/W95FA.otf");
            Font = new Font(fontCollection.Families[0], 10); 
        }

        private void AddMainControls()
        {
            // Toolbar Panel
            _headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(300, 20),
                BackColor = Color.White,
            };
            _headerPanel.MouseDown += _headerPanel_Draggable;

            // Exit Button
            _windowExitButton = new Button
            {
                Location = new Point(280, 0),
                Size = new Size(20, 20),
                BackColor = Color.Beige,
                Text = CommonStrings.Symbol_X,
            };
            _windowExitButton.Click += _windowExitButton_Click;

            // Minimize Button
            _windowMinimizeButton = new Button
            {
                Location = new Point(260, 0),
                Size = new Size(20, 20),
                BackColor = Color.Beige,
                Text = CommonStrings.Symbol_Dash,
            };
            _windowMinimizeButton.Click += _windowMinimizeButton_Click;

            // Title Label
            _windowTitleLabel = new Label
            {
                Text = CommonStrings.IcsMouseWiggler,
                Location = new Point(0, 0),
                BackColor = Color.Transparent,
                Size = new Size(150, 20),
            };
            _windowTitleLabel.MouseDown += _headerPanel_Draggable;

            // Interval Input
            _intervalInput = new TextBox
            {
                PlaceholderText = CommonStrings.Interval,
                Location = new Point(10, 30),
                Size = new Size(100, 50),
                Text = CommonStrings.INT_1000,
                TabStop = false,
            };

            // Start
            _startButton = new Button
            {
                Text = CommonStrings.Start,
                Location = new Point(120, 30),
                Size = new Size(70, 30)
            };
            _startButton.Click += _startButton_Click;

            // Stop
            _stopButton = new Button
            {
                Text = CommonStrings.Stop,
                Location = new Point(120, 70),
                Size = new Size(70, 30)
            };
            _stopButton.Click += _stopButton_Click;

            // Green / Red
            _enabledLabel = new Label
            {
                Text = CommonStrings.Stopped,
                ForeColor = Color.Red,
                Location = new Point(10, 70),
                Size = new Size(70, 50),
            };

            this.Controls.AddRange(new Control[]
            {
                _headerPanel,
                _enabledLabel,
                _intervalInput,
                _startButton,
                _stopButton,
            });

            _headerPanel.Controls.AddRange(new Control[]
            {
                _windowExitButton,
                _windowMinimizeButton,
                _windowTitleLabel,
            });
        }

        #region Control Events
        private void _headerPanel_Draggable(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void _windowMinimizeButton_Click(object? sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void _windowExitButton_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void _startButton_Click(object? sender, EventArgs e)
        {

            if (int.TryParse(_intervalInput.Text, out Int32 intervalInMilliseconds))
            {
                intervalInMilliseconds = Math.Max(1000, intervalInMilliseconds);

                // begin timer
                _sysTimer.Interval = intervalInMilliseconds;
                _sysTimer.Tick += WiggleCursor;
                _sysTimer.Start();
                _enabledLabel.Text = CommonStrings.Running;
                _enabledLabel.ForeColor = Color.Green;
            }
            else
            {
                _enabledLabel.Text = CommonStrings.Stopped;
                _enabledLabel.ForeColor = Color.Red;
            }
        }

        private void _stopButton_Click(object? sender, EventArgs e)
        {
            // end timer 
            _sysTimer.Stop();

            // Message
            _enabledLabel.Text = CommonStrings.Stopped;
            _enabledLabel.ForeColor = Color.Red;
        }

        private async void WiggleCursor(object? sender, EventArgs e)
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y - 1);
            await Task.Delay(100);
            Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);
            await Task.Delay(100);
        }
        #endregion
    }
}
