using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class MRAboutViewModel
{
    public string MRTitle => AppInfo.Name;
    public string MRVersion => AppInfo.VersionString;
    public string MRMoreInfoUrl => "https://aka.ms/maui";
    public string MRMessage => "This app is written in XAML and C# with .NET MAUI.";
    public ICommand MRShowMoreInfoCommand { get; }

    public MRAboutViewModel()
    {
        MRShowMoreInfoCommand = new AsyncRelayCommand(ShowMoreInfo);
    }

    async Task ShowMoreInfo() =>
        await Launcher.Default.OpenAsync(MRMoreInfoUrl);
}