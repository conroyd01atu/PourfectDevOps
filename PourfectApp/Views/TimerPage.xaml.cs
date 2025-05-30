namespace PourfectApp.Views
{
    public partial class TimerPage : ContentPage
    {
        private System.Timers.Timer timer;
        private int elapsedSeconds = 0;
        private bool isRunning = false;

        public TimerPage()
        {
            InitializeComponent();
            SetupTimer();
        }

        private void SetupTimer()
        {
            timer = new System.Timers.Timer(1000); // 1 second interval
            timer.Elapsed += OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            elapsedSeconds++;

            // Update UI on main thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateTimerDisplay();
                UpdatePhaseLabel();
            });
        }

        private void UpdateTimerDisplay()
        {
            int minutes = elapsedSeconds / 60;
            int seconds = elapsedSeconds % 60;
            TimerLabel.Text = $"{minutes:D2}:{seconds:D2}";
        }

        private void UpdatePhaseLabel()
        {
            // Update phase based on common pour-over timing
            if (elapsedSeconds < 45)
                PhaseLabel.Text = "Bloom phase";
            else if (elapsedSeconds < 90)
                PhaseLabel.Text = "First pour";
            else if (elapsedSeconds < 150)
                PhaseLabel.Text = "Second pour";
            else if (elapsedSeconds < 210)
                PhaseLabel.Text = "Final pour";
            else
                PhaseLabel.Text = "Drawdown";
        }

        private void OnStartClicked(object sender, EventArgs e)
        {
            if (isRunning)
            {
                // Pause
                timer.Stop();
                isRunning = false;
                StartButton.Text = "Resume";
                PhaseLabel.Text = "Paused";
            }
            else
            {
                // Start or Resume
                timer.Start();
                isRunning = true;
                StartButton.Text = "Pause";
            }
        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            timer.Stop();
            isRunning = false;
            elapsedSeconds = 0;
            TimerLabel.Text = "00:00";
            StartButton.Text = "Start";
            PhaseLabel.Text = "Ready to brew";
        }

        private void OnPresetClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string secondsStr)
            {
                if (int.TryParse(secondsStr, out int targetSeconds))
                {
                    // Reset and set to preset time
                    OnResetClicked(sender, e);
                    elapsedSeconds = targetSeconds;
                    UpdateTimerDisplay();

                    // Show countdown message
                    PhaseLabel.Text = $"Preset: {button.Text}";
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Stop timer when leaving page
            if (isRunning)
            {
                timer.Stop();
            }
        }
    }
}