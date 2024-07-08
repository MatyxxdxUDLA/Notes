namespace Notes.Models;

internal class MRNote
{
    public string MRFilename { get; set; }
    public string MRText { get; set; }
    public DateTime MRDate { get; set; }

    public MRNote()
    {
        MRFilename = $"{Path.GetRandomFileName()}.notes.txt";
        MRDate = DateTime.Now;
        MRText = "";
    }

    public void Save() =>
    File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, MRFilename), MRText);

    public void Delete() =>
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, MRFilename));

    public static MRNote Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new()
            {
                MRFilename = Path.GetFileName(filename),
                MRText = File.ReadAllText(filename),
                MRDate = File.GetLastWriteTime(filename)
            };
    }

    public static IEnumerable<MRNote> LoadAll()
    {
        // Get the folder where the notes are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.notes.txt files.
        return Directory

                // Select the file names from the directory
                .EnumerateFiles(appDataPath, "*.notes.txt")

                // Each file name is used to load a note
                .Select(filename => MRNote.Load(Path.GetFileName(filename)))

                // With the final collection of notes, order them by date
                .OrderByDescending(note => note.MRDate);
    }
}
