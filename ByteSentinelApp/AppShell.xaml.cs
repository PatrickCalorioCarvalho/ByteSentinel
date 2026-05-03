namespace ByteSentinelApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Shell.SetTabBarBackgroundColor(this, Color.FromArgb("#020617"));
            Shell.SetTabBarForegroundColor(this, Colors.White);
            Shell.SetTabBarTitleColor(this, Colors.White);
            Shell.SetTabBarUnselectedColor(this, Colors.Gray);
        }
    }
}
