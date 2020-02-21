using NotesApp.Business.Models;
using System;
using System.Collections.Generic;

namespace NotesApp.Business
{
    public interface INotesRepository
    {
        List<Note> GetAllNotes(int userId, DateTime? fromDate, DateTime? toDate);

        bool AddNote(Note note, string ip);
    }
}