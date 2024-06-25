using DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {
        }

        // establish the entities in DB.
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // defines the properties in table
            modelBuilder.Entity<Message>(tb =>
            {
                // define primary key
                tb.HasKey(col => col.Id);
                // auto increment id and generated when Add a register.
                tb.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd();

                tb.Property(col => col.Text).HasMaxLength(200);
                tb.Property(col => col.User).HasMaxLength(20);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
