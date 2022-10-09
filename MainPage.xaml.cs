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
            if(selectedEntry != null) // we will delete an entry as long as something was selected
            {
                EntryDeletionError entryDeletionError = MauiProgram.ibl.DeleteEntry(selectedEntry.Id);
                if(entryDeletionError != EntryDeletionError.NoError)
                {
                    DisplayAlert("An error has occurred while deleting an entry", $"{entryDeletionError}", "OK");
                }
            }
            else
            {
                DisplayAlert("Delete Entry Message:", "EntryNotFound", "Ok");
            }
        }

        void EditEntry(System.Object sender, System.EventArgs e)
        {
            Entry selectedEntry = EntriesLV.SelectedItem as Entry;
            if(selectedEntry != null) // we will edit an entry as long as something was selected
            {
                int difficulty;
                bool validDifficulty = int.TryParse(difficultyENT.Text, out difficulty);
                if (validDifficulty)
                {
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
            else
            {
                DisplayAlert("Edit Entry Message:", "EntryNotFound", "Ok");
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
        void SortByClue(System.Object sender, System.EventArgs e)
        {
            MauiProgram.ibl.SortByClue();
        }

        void SortByAnswer(System.Object sender, System.EventArgs e)
        {
            MauiProgram.ibl.SortByAnswer();
        }
    }
}

