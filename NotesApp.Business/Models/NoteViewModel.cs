using NotesApp.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotesApp.Business
{
    public class NoteViewModel
    {
        [Required]
        [MaxLength(300)]
        public string NoteText { get; set; }
    }

    public class NotesResultView
    {
        public IEnumerable<Note> Notes { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}