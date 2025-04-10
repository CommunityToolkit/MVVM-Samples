namespace MvvmSampleMAUI;

public partial class App : Application
{
    readonly AppShell _appShell;
    
    public App(AppShell appShell)
    {
        InitializeComponent();

        _appShell = appShell;
    }

    protected override Window CreateWindow(IActivationState? activationState) => new(_appShell);
}