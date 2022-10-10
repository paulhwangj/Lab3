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
        /// </summary>
        /// <returns>ObservableCollection of entries</returns>
        public ObservableCollection<Entry> GetEntries()
        {
            return db.GetEntries();
        }

        /// <summary>
        /// Finds specific entry in the entries
        /// </summary>
        /// <returns>the entry (if it exists, null otherwise)</returns>
        public Entry FindEntry(int id)
        {
            return db.FindEntry(id);
        }

        /// <summary>
        /// Verifies that all the entry fields are valid
        /// </summary>
        /// <param name="clue">clue to be checked</param>
        /// <param name="answer">answer to be checked</param>
        /// <param name="difficulty">difficulty to be checked</param>
        /// <param name="date">date to be checked</param>
        /// <returns>corresponding error in InvalidFieldError, InvalidFieldError.NoError otherwise</returns>
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
            if (difficulty < 0 || difficulty > MAX_DIFFICULTY)
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
        /// <param name="clue">clue of new entry</param>
        /// <param name="answer">answer of new entry</param>
        /// <param name="difficulty">difficulty of new entry</param>
        /// <param name="date">date of new entry</param>
        /// <returns>corresponding error in InvalidFieldError, InvalidFieldError.NoError otherwise</returns>
        public InvalidFieldError AddEntry(string clue, string answer, int difficulty, string date)
        {
            // verifies that the entry has valid field params
            var result = CheckEntryFields(clue, answer, difficulty, date);
            if (result != InvalidFieldError.NoError)
            {
                return result;
            }
            
            // fields have been verified, create the entry and let's add it to the database
            db.AddEntry(new Entry(clue, answer, difficulty, date, ++latestId));

            return InvalidFieldError.NoError;
        }

        /// <summary>
        /// Deletes an entry
        /// </summary>
        /// <param name="entryId">id of entry to be deleted</param>
        /// <returns>corresponding error if there is one, EntryDeletionError.NoError otherwise</returns>
        public EntryDeletionError DeleteEntry(int entryId)
        {
            var entry = db.FindEntry(entryId);

            if (entry != null) // entry was found in entries
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
            else // entries was not found in entries
            {
                return EntryDeletionError.EntryNotFound;
            }
        }

        /// <summary>
        /// Edits an Entry
        /// </summary>
        /// <param name="clue">new clue for existing entry</param>
        /// <param name="answer">new answer for existing entry</param>
        /// <param name="difficulty">new difficulty for existing entry</param>
        /// <param name="date">new date for existing entry</param>
        /// <param name="id">id for existing entry</param>
        /// <returns>an error if there is one, EntryEditError.NoError otherwise</returns>
        public EntryEditError EditEntry(string clue, string answer, int difficulty, string date, int id)
        {
            var fieldCheck = CheckEntryFields(clue, answer, difficulty, date);
            if (fieldCheck != InvalidFieldError.NoError)
            {
                return EntryEditError.InvalidFieldError;
            }

            var entry = db.FindEntry(id);
            if (entry != null) // entry was found in entries
            {
                bool success = db.EditEntry(new Entry(clue, answer, difficulty, date, id));
                if (!success)
                {
                    return EntryEditError.DBEditError;
                }
                return EntryEditError.NoError;
            }
            else    // entry was not found in entries
            {
                return EntryEditError.EntryNotFound;
            }
        }

        /// <summary>
        /// Ran only once at program start up, it retrieves the next available id by
        /// finding the max id within the database and sets latestId to that number
        /// </summary>
        public void GetNextId() {
            latestId = db.GetNextId();
        }

        /// <summary>
        /// Sorts the entries by clue
        /// </summary>
        public void SortByClue()
        {
            db.SortByClue();
        }

        /// <summary>
        /// Sorts the entries by answer
        /// </summary>
        public void SortByAnswer()
        {
            db.SortByAnswer();
        }
    }
}