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
    
    public partial class grupy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public grupy()
        {
            this.elementy_grup = new ObservableCollection<elementy_grup>();
            this.rodzaje_pomiarow = new ObservableCollection<rodzaje_pomiarow>();
        }
    
        public int idGrupy { get; set; }
        public string nazwa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<elementy_grup> elementy_grup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<rodzaje_pomiarow> rodzaje_pomiarow { get; set; }
    }
}
