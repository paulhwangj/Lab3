
namespace Lab2Solution
{

    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            EntriesLV.ItemsSource = MauiProgram.ibl.GetEntries();
        }

        void AddEntry(System.Object sender, System.EventArgs e)
        {
            String clue = clueENT.Text;
            String answer = answerENT.Text;
            String date = dateENT.Text;

            int difficulty;
            bool validDifficulty = int.TryParse(difficultyENT.Text, out difficulty);
            if (validDifficulty)
            {
                InvalidFieldError invalidFieldError = MauiProgram.ibl.AddEntry(clue, answer, difficulty, date);
                if(invalidFieldError != InvalidFieldError.NoError)
                {
                    DisplayAlert("An error has occurred while adding an entry", $"{invalidFieldError}", "OK");
                }
            }
            else
            {
                DisplayAlert("Error with Add:", "Invalid Difficulty (0-2)", "OK");
            }
        }

        void DeleteEntry(System.Object sender, System.EventArgs e)
        {
            Entry selectedEntry = EntriesLV.SelectedItem as Entry;
            EntryDeletionError entryDeletionError = MauiProgram.ibl.DeleteEntry(selectedEntry.Id);
            if(entryDeletionError != EntryDeletionError.NoError)
            {
                DisplayAlert("An error has occurred while deleting an entry", $"{entryDeletionError}", "OK");
            }
        }

        void EditEntry(System.Object sender, System.EventArgs e)
        {

            Entry selectedEntry = EntriesLV.SelectedItem as Entry;
            // TODO: delete these?
            selectedEntry.Clue = clueENT.Text;
            selectedEntry.Answer = answerENT.Text;
            selectedEntry.Date = dateENT.Text;


            int difficulty;
            bool validDifficulty = int.TryParse(difficultyENT.Text, out difficulty);
            if (validDifficulty)
            {
                // TODO: delete below?
                // EntryEditError entryEditError = MauiProgram.ibl.EditEntry(selectedEntry.Clue, selectedEntry.Answer, difficulty, selectedEntry.Date, selectedEntry.Id);
                var entryEditError = MauiProgram.ibl.EditEntry(clueENT.Text, answerENT.Text, difficulty, dateENT.Text, selectedEntry.Id);
                if(entryEditError != EntryEditError.NoError)
                {
                    DisplayAlert("Error with Edit:", entryEditError.ToString(), "OK");
                }
            }
            else
            {
                DisplayAlert("Error with Edit:", "Invalid Difficulty (0-2)", "OK");
            }
        }

        void EntriesLV_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
        {
            Entry selectedEntry = e.SelectedItem as Entry;
            clueENT.Text = selectedEntry.Clue;
            answerENT.Text = selectedEntry.Answer;
            difficultyENT.Text = selectedEntry.Difficulty.ToString();
            dateENT.Text = selectedEntry.Date;

        }

        // TODO: implement below
        void SortByClue(System.Object sender, System.EventArgs e)
        {

        }

        void SortByAnswer(System.Object sender, System.EventArgs e)
        {

        }




    }
}

