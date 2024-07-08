using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class MRNotesViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.MRNoteViewModel> AllNotes { get; }
    public ICommand MRNewCommand { get; }
    public ICommand MRSelectNoteCommand { get; }

    public MRNotesViewModel()
    {
        AllNotes = new ObservableCollection<ViewModels.MRNoteViewModel>(Models.MRNote.LoadAll().Select(n => new MRNoteViewModel(n)));
        MRNewCommand = new AsyncRelayCommand(MRNewNoteAsync);
        MRSelectNoteCommand = new AsyncRelayCommand<ViewModels.MRNoteViewModel>(MRSelectNoteAsync);
    }

    private async Task MRNewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task MRSelectNoteAsync(ViewModels.MRNoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.MRIdentifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            MRNoteViewModel matchedNote = AllNotes.Where((n) => n.MRIdentifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            MRNoteViewModel matchedNote = AllNotes.Where((n) => n.MRIdentifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.MRReload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new MRNoteViewModel(Models.MRNote.Load(noteId)));
        }
    }
}
