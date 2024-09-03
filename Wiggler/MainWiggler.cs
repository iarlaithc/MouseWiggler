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

        private void Start_Click(object sender, EventArgs e)
        {
            MessageBox.Show(CommonStrings.MouseWiggler);
        }
    }
}
