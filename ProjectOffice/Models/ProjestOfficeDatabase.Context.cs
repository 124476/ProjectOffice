﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectOffice.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProgectOfficeDatabaseEntities : DbContext
    {
        public ProgectOfficeDatabaseEntities()
            : base("name=ProgectOfficeDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Employe> Employe { get; set; }
        public virtual DbSet<LastStatus> LastStatus { get; set; }
        public virtual DbSet<Portfile> Portfile { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TaskStatus> TaskStatus { get; set; }
    }
}
