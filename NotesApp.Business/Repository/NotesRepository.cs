using NotesApp.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotesApp.Business
{
    public class NotesRepository : INotesRepository
    {
        public bool AddNote(Note note, string ipAddress)
        {
            try
            {
                using (var db = new NotesAppEntities())
                {
                    db.Notes.Add(note);
                    var result = db.Users.FirstOrDefault(s => s.UserId == note.UserId);
                    result.IpAddress = ipAddress;
                    db.AuthenticationLogs.Add(
                        new AuthenticationLog
                        {
                            CreatedUTC = DateTime.UtcNow,
                            UserId = note.UserId,
                            LogType = LogTypeEnum.AddedNote.ToString()
                        });
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Note> GetAllNotes(int userId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                using (var db = new NotesAppEntities())
                {
                    List<Note> notes;
                    if (db.Users.Where(s => s.UserId == userId).FirstOrDefault().UserType == UserTypeEnum.User.ToString())
                    {
                        notes = db.Notes.Where(s => s.UserId == userId).ToList();
                    }
                    else
                    {
                        notes = db.Notes.Include("User").ToList();
                    }
                    if (fromDate != null && toDate == null)
                    {
                        notes = notes.Where(s => s.CreatedUTC.Value.Date >= fromDate.Value.Date).ToList();
                    }
                    else if (fromDate != null && toDate != null)
                    {
                        notes = notes.Where(s => s.CreatedUTC.Value.Date >= fromDate.Value.Date && s.CreatedUTC.Value.Date <= toDate.Value.Date).ToList();
                    }
                    return notes.OrderByDescending(s=>s.CreatedUTC).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}