//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NotesApp.Business.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Note
    {
        public int NoteId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string NoteText { get; set; }
        public Nullable<System.DateTime> CreatedUTC { get; set; }
    
        public virtual User User { get; set; }
    }
}
