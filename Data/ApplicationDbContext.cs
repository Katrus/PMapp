using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMApp.Models;

namespace PMApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Building> Buildings { get; set; }

        public DbSet<Tax_Parcel> Tax_Parcels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<PMApp.Models.Vehicle> Vehicle { get; set; }

        public DbSet<PMApp.Models.Tenant> Tenant { get; set; }

        public DbSet<PMApp.Models.Unit> Unit { get; set; }

        public DbSet<PMApp.Models.Rent> Rent { get; set; }

        public DbSet<PMApp.Models.Repair_History> Repair_History { get; set; }

        public DbSet<PMApp.Models.Move_in> Move_in { get; set; }

        public DbSet<PMApp.Models.Move_out> Move_out { get; set; }

        public DbSet<PMApp.Models.Contractor> Contractor { get; set; }

        public DbSet<PMApp.Models.Infractions> Infractions { get; set; }

        public DbSet<PMApp.Models.Checklist> Checklist { get; set; }

        public DbSet<PMApp.Models.PdfFile> PdfFile { get; set; }
    }
}
