﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class wizualizacja2Entities : DbContext
    {
        public wizualizacja2Entities()
            : base("name=wizualizacja2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<pomiary> pomiary { get; set; }
        public virtual DbSet<Slownik> Slownik { get; set; }
        public virtual DbSet<atrybuty_zdarzen> atrybuty_zdarzen { get; set; }
        public virtual DbSet<grupy> grupy { get; set; }
        public virtual DbSet<grupy_raportowe> grupy_raportowe { get; set; }
        public virtual DbSet<komunikaty> komunikaty { get; set; }
        public virtual DbSet<miejsca_pomiaru> miejsca_pomiaru { get; set; }
        public virtual DbSet<multicont> multicont { get; set; }
        public virtual DbSet<pracownik_sterowni> pracownik_sterowni { get; set; }
        public virtual DbSet<rodzaje_bramek_zdarzen> rodzaje_bramek_zdarzen { get; set; }
        public virtual DbSet<rodzaje_pomiarow> rodzaje_pomiarow { get; set; }
        public virtual DbSet<slownik_bramek_zewnetrznych> slownik_bramek_zewnetrznych { get; set; }
        public virtual DbSet<uprawnienia> uprawnienia { get; set; }
        public virtual DbSet<uprawnienia_sterowni> uprawnienia_sterowni { get; set; }
        public virtual DbSet<uzytkownicy> uzytkownicy { get; set; }
        public virtual DbSet<wielkosci_mierzone> wielkosci_mierzone { get; set; }
        public virtual DbSet<zdarzenia_sterownia> zdarzenia_sterownia { get; set; }
        public virtual DbSet<zewnetrzne_zrodla_zdarzen> zewnetrzne_zrodla_zdarzen { get; set; }
        public virtual DbSet<zmiany> zmiany { get; set; }
        public virtual DbSet<elementy_grup> elementy_grup { get; set; }
        public virtual DbSet<elementy_grup_raportowych> elementy_grup_raportowych { get; set; }
        public virtual DbSet<objetosci_zbiornikow> objetosci_zbiornikow { get; set; }
        public virtual DbSet<potwierdzone_komunikaty> potwierdzone_komunikaty { get; set; }
    }
}