//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrzegladBazy.Models
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class zdarzenia_sterownia
    {
        public int id_zdarzenia { get; set; }
        public int id_pracownika_sterowni { get; set; }
        public int id_bramki { get; set; }
        public long data_zdarzenia { get; set; }
        public string opis_zdarzenia { get; set; }
        public int atrybut { get; set; }
        public string termin { get; set; }
        public int id_etapu { get; set; }
        public string opis_bramki { get; set; }
        public Nullable<bool> czy_dotyczy_bramki { get; set; }
    }
}
