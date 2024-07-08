using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class MRNoteViewModel : ObservableObject, IQueryAttributable
{
    private Models.MRNote _note;

    public string MRText
    {
        get => _note.MRText;
        set
        {
            if (_note.MRText != value)
            {
                _note.MRText = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime MRDate => _note.MRDate;

    public string MRIdentifier => _note.MRFilename;

    public ICommand MRSaveCommand { get; private set; }
    public ICommand MRDeleteCommand { get; private set; }

    public MRNoteViewModel()
    {
        _note = new Models.MRNote();
        MRSaveCommand = new AsyncRelayCommand(MRSave);
        MRDeleteCommand = new AsyncRelayCommand(MRDelete);
    }

    public MRNoteViewModel(Models.MRNote note)
    {
        _note = note;
        MRSaveCommand = new AsyncRelayCommand(MRSave);
        MRDeleteCommand = new AsyncRelayCommand(MRDelete);
    }

    private async Task MRSave()
    {
        _note.MRDate = DateTime.Now;
        _note.Save();
        await Shell.Current.GoToAsync($"..?saved={_note.MRFilename}");
    }

    private async Task MRDelete()
    {
        _note.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_note.MRFilename}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _note = Models.MRNote.Load(query["load"].ToString());
            MRRefreshProperties();
        }
    }

    public void MRReload()
    {
        _note = Models.MRNote.Load(_note.MRFilename);
        MRRefreshProperties();
    }

    private void MRRefreshProperties()
    {
        OnPropertyChanged(nameof(MRText));
        OnPropertyChanged(nameof(MRDate));
    }
}
