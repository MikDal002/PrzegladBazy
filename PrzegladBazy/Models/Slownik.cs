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
    
    public partial class Slownik
    {
        public int slowId { get; set; }
        public short RecNo { get; set; }
        public string LongGate { get; set; }
        public string description { get; set; }
        public byte idStacji { get; set; }
        public string rodzajBramki { get; set; }
        public string rodzajPomiaru { get; set; }
        public int gateId { get; set; }
        public Nullable<short> minimum { get; set; }
        public Nullable<short> maksimum { get; set; }
        public Nullable<bool> zdarz { get; set; }
        public Nullable<int> rodz_dla_zdarz { get; set; }
    }
}
