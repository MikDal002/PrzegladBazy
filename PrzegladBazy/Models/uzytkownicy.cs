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
    
    public partial class uzytkownicy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public uzytkownicy()
        {
            this.pomiary = new ObservableCollection<pomiary>();
            this.pomiary1 = new ObservableCollection<pomiary>();
            this.pomiary2 = new ObservableCollection<pomiary>();
        }
    
        public string login { get; set; }
        public string username { get; set; }
        public string haslo { get; set; }
        public int poziom { get; set; }
        public int id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<pomiary> pomiary { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<pomiary> pomiary1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<pomiary> pomiary2 { get; set; }
    }
}
