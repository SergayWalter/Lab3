﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kyrs
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KyrsEntities1 : DbContext
    {
        private static KyrsEntities1 _context;

        public static KyrsEntities1 GetContext()
        {
            if (_context == null)
                _context = new KyrsEntities1();
            return _context;
        }
        public KyrsEntities1()
            : base("name=KyrsEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<Preparation> Preparation { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<Role_> Role_ { get; set; }
        public virtual DbSet<Type_> Type_ { get; set; }
    }
}