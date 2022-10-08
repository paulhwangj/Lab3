using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lab2Solution
{

    /// <summary>
    /// Handles the BusinessLogic
    /// </summary>
    public class BusinessLogic : IBusinessLogic
    {
        const int MAX_CLUE_LENGTH = 250;
        const int MAX_ANSWER_LENGTH = 25;
        const int MAX_DIFFICULTY = 2;
        int latestId = 0;

        IDatabase db;                     // the actual database that does the hardwork

        public BusinessLogic()
        {
            db = new RelationalDatabase();
            GetNextId();
        }


        /// <summary>
        /// Returns the entries that have all the entries in the DB populated in it
        /// This also could have been a property
        /// </summary>
        /// <returns>ObservableCollection of entrties</returns>
        public ObservableCollection<Entry> GetEntries()
        {
            return db.GetEntries();
        }

        public Entry FindEntry(int id)
        {
            return db.FindEntry(id);
        }

        /// <summary>
        /// Verifies that all the entry fields are valid
        /// </summary>
        /// <param name="clue"></param>
        /// <param name="answer"></param>
        /// <param name="difficulty"></param>
        /// <param name="date"></param>
        /// <returns>an error if there is an error, InvalidFieldError.None otherwise</returns>

        private InvalidFieldError CheckEntryFields(string clue, string answer, int difficulty, string date)
        {
            if (clue == null || clue.Length < 1 || clue.Length > MAX_CLUE_LENGTH)
            {
                return InvalidFieldError.InvalidClueLength;
            }
            if (answer == null || answer.Length < 1 || answer.Length > MAX_ANSWER_LENGTH)
            {
                return InvalidFieldError.InvalidAnswerLength;
            }
            if (difficulty < 1 || difficulty > MAX_DIFFICULTY)
            {
                return InvalidFieldError.InvalidDifficulty;
            }
            if (date == null || date.Length < 1 || date.Length > 11)
            {
                return InvalidFieldError.InvalidDate;
            }
            return InvalidFieldError.NoError;
        }


        /// <summary>
        /// Adds an entry
        /// </summary>
        /// <param name="clue"></param>
        /// <param name="answer"></param>
        /// <param name="difficulty"></param>
        /// <param name="date"></param>
        /// <returns>an error if there is an error, InvalidFieldError.None otherwise</returns>
        public InvalidFieldError AddEntry(string clue, string answer, int difficulty, string date)
        {

            var result = CheckEntryFields(clue, answer, difficulty, date);
            if (result != InvalidFieldError.NoError)
            {
                return result;
            }
            Entry entry = new Entry(clue, answer, difficulty, date, ++latestId);    // TODO: delete the id from entry?
            db.AddEntry(entry);

            return InvalidFieldError.NoError;
        }

        /// <summary>
        /// Deletes an entry
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns>an erreor if there is one, EntryDeletionError.NoError otherwise</returns>
        public EntryDeletionError DeleteEntry(int entryId)
        {

            var entry = db.FindEntry(entryId);

            if (entry != null)
            {
                bool success = db.DeleteEntry(entry);
                if (success)
                {
                    return EntryDeletionError.NoError;

                }
                else
                {
                    return EntryDeletionError.DBDeletionError;
                }
            }
            else
            {
                return EntryDeletionError.EntryNotFound;
            }
        }

        /// <summary>
        /// Edits an Entry
        /// </summary>
        /// <param name="clue"></param>
        /// <param name="answer"></param>
        /// <param name="difficulty"></param>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <returns>an error if there is one, EntryEditError.None otherwise</returns>
        public EntryEditError EditEntry(string clue, string answer, int difficulty, string date, int id)
        {

            var fieldCheck = CheckEntryFields(clue, answer, difficulty, date);
            if (fieldCheck != InvalidFieldError.NoError)
            {
                return EntryEditError.InvalidFieldError;
            }

            var entry = db.FindEntry(id);
            entry.Clue = clue;
            entry.Answer = answer;
            entry.Difficulty = difficulty;
            entry.Date = date;

            // TODO: test that this works by entering in an invalid date (since date isn't checked in businesslogic)
            bool success = db.EditEntry(entry);
            if (!success)
            {
                return EntryEditError.DBEditError;
            }

            return EntryEditError.NoError;
        }

        /// <summary>
        /// Ran only once at program start up, it retrieves the next available Id by
        /// finding the max id + 1 within the database and sets latestId to that number
        /// </summary>
        public void GetNextId() {
            latestId = db.GetNextId();
        }
    }
}