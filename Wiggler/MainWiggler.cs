using System.Drawing.Text;
using System.Runtime.InteropServices;
using CommonStrings = Wiggler.Resources.CommonStrings;
using Timer = System.Windows.Forms.Timer;

namespace Wiggler
{
    public class MainWiggler : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        private static readonly Color Win95Background = ColorTranslator.FromHtml("#C0C0C0");
        private static readonly Color Win95DarkShadow = ColorTranslator.FromHtml("#404040");
        private static readonly Color Win95Shadow = ColorTranslator.FromHtml("#808080");
        private static readonly Color Win95TitleBar = ColorTranslator.FromHtml("#000080");

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private TextBox _intervalInput;
        private Button _startButton;
        private Button _stopButton;
        private Label _enabledLabel;
        private Panel _headerPanel;
        private Button _windowExitButton;
        private Button _windowMinimizeButton;
        private Label _windowTitleLabel;
        private readonly Timer _sysTimer;
        private readonly GroupBox _controlGroup;
        private PictureBox _titleIcon;

        public MainWiggler()
        {
            _controlGroup = new GroupBox();
            _sysTimer = new Timer();
            Icon = new Icon(GetType().Assembly.GetManifestResourceStream("Wiggler.Resources.Images.icMW.ico"));
            InitializeComponent();
            AddMainControls(); 
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            MinimizeBox = false;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Win95Background;
            FormBorderStyle = FormBorderStyle.None;
            Size = new Size(264, 180);
            Padding = new Padding(3);
            ShowIcon = true;

            var fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile("Resources/W95FA.otf");
            Font = new Font(fontCollection.Families[0], 10f);
            ResumeLayout();
        }

        private void AddMainControls()
        {
            _controlGroup.Text = CommonStrings.Interval;
            _controlGroup.Location = new Point(5, 30);
            _controlGroup.Size = new Size(252, 144);
            _controlGroup.BackColor = Win95Background;
            _controlGroup.FlatStyle = FlatStyle.System;

            InitializeHeaderPanel();
            InitializeWindowControls();
            InitializeMainControls();

            Controls.Add(_controlGroup);
            Controls.Add(_headerPanel);

            _headerPanel.Controls.AddRange(new Control[]
            {
                _windowExitButton,
                _windowMinimizeButton,
                _windowTitleLabel
            });

            _controlGroup.Controls.AddRange(new Control[]
            {
                _intervalInput,
                _startButton,
                _stopButton,
                _enabledLabel
            });
        }

        private void InitializeHeaderPanel()
        {
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = Win95TitleBar
            };
            _headerPanel.MouseDown += (s, e) => {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };
        }

        private void StyleButton(Button button)
        {
            button.FlatStyle = FlatStyle.System;
            button.BackColor = Win95Background;
            button.UseVisualStyleBackColor = false;
            button.Font = Font;
            button.EnabledChanged += (s, e) => {
                button.ForeColor = button.Enabled ? Color.Black : Win95Shadow;
            };
        }

        private void InitializeWindowControls()
        {
            _titleIcon = new PictureBox
            {
                Image = Icon.ToBitmap(),
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(19, 19),
                Location = new Point(2, 2)
            };
            _headerPanel.Controls.Add(_titleIcon);

            _windowExitButton = new Button
            {
                Size = new Size(22, 22),
                Location = new Point(Width - 29, 2),
                Text = CommonStrings.Symbol_X,
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            StyleButton(_windowExitButton);
            _windowExitButton.Click += WindowExitButton_Click;

            _windowMinimizeButton = new Button
            {
                Size = new Size(22, 22),
                Location = new Point(Width - 53, 2),
                Text = CommonStrings.Symbol_Dash,
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            StyleButton(_windowMinimizeButton);
            _windowMinimizeButton.Click += WindowMinimizeButton_Click;

            _windowTitleLabel = new Label
            {
                Text = CommonStrings.IcsMouseWiggler,
                Location = new Point(22, 3),
                Size = new Size(160, 20),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White,
                Font = new Font(Font.FontFamily, 10f, FontStyle.Bold)
            };
            _windowTitleLabel.MouseDown += (s, e) => {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };
        }

        private void InitializeMainControls()
        {
            _intervalInput = new TextBox
            {
                Location = new Point(12, 24),
                Size = new Size(84, 28),
                Text = CommonStrings.INT_1000,
                BorderStyle = BorderStyle.Fixed3D
            };

            _intervalInput.KeyPress += (s, e) => {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };

            _intervalInput.TextChanged += (s, e) => {
                _sysTimer.Stop();
                _enabledLabel.Text = CommonStrings.Stopped;
                _enabledLabel.ForeColor = Color.Red;
                UpdateButtonStates(false);
            };

            _intervalInput.Leave += (s, e) => {
                if (int.TryParse(_intervalInput.Text, out int value) && value < 1000)
                {
                    _intervalInput.Text = CommonStrings.INT_1000;
                }
            };

            _startButton = new Button
            {
                Location = new Point(108, 24),
                Size = new Size(72, 28),
                Text = CommonStrings.Start
            };
            StyleButton(_startButton);
            _startButton.Click += StartButton_Click;

            _stopButton = new Button
            {
                Location = new Point(108, 60),
                Size = new Size(72, 28),
                Text = CommonStrings.Stop
            };
            StyleButton(_stopButton);
            _stopButton.Click += StopButton_Click;

            _enabledLabel = new Label
            {
                Location = new Point(12, 60),
                Size = new Size(84, 28),
                Text = CommonStrings.Stopped,
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleLeft,
                BorderStyle = BorderStyle.Fixed3D
            };

            UpdateButtonStates(false);
        }

        private void UpdateButtonStates(bool isRunning)
        {
            _startButton.Enabled = !isRunning;
            _stopButton.Enabled = isRunning;
        }

        private void HeaderPanel_Draggable(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void WindowMinimizeButton_Click(object? sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void WindowExitButton_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            if (int.TryParse(_intervalInput.Text, out int intervalInMilliseconds))
            {
                intervalInMilliseconds = Math.Max(1000, intervalInMilliseconds);

                _sysTimer.Interval = intervalInMilliseconds;
                _sysTimer.Tick += WiggleCursor;
                _sysTimer.Start();

                _enabledLabel.Text = CommonStrings.Running;
                _enabledLabel.ForeColor = Color.Green;
                UpdateButtonStates(true);
            }
            else
            {
                _enabledLabel.Text = CommonStrings.Stopped;
                _enabledLabel.ForeColor = Color.Red;
                UpdateButtonStates(false);
            }
        }

        private void StopButton_Click(object? sender, EventArgs e)
        {
            _sysTimer.Stop();
            _enabledLabel.Text = CommonStrings.Stopped;
            _enabledLabel.ForeColor = Color.Red;
            UpdateButtonStates(false);
        }

        private async void WiggleCursor(object? sender, EventArgs e)
        {
            Cursor = new Cursor(Cursor.Current.Handle);

            Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y - 1);
            await Task.Delay(100);

            Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);
            await Task.Delay(100);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Raised);
        }        
    }
}