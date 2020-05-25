using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SignalR
{
  public  class ChatDbContent : IdentityDbContext<UserModel>
    {
        public DbSet<ChatMessage> Message { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ConversationRoom> Rooms { get; set; }


        public ChatDbContent(DbContextOptions options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            //    modelBuilder.Entity<ChatMessage>()
            //        .Property(f => f.Id)
            //        .ValueGeneratedOnAdd();



         




        }

    }
}


