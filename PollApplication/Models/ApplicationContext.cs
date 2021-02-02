using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PollApplication.Models
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<PollAnswer> Answers { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";
 
            string adminEmail = "admin@gmail.com";
            string adminPassword = Encoding.ASCII.GetString(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes("12345678")));
            
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });

            modelBuilder.Entity<Variant>().HasOne(p => p.Question).WithMany(v => v.Variants).HasForeignKey("QuestionId");
            modelBuilder.Entity<Question>().HasOne(q => q.LinkedPoll).WithMany(p => p.Questions).
                HasForeignKey("LinkedPollId");
            modelBuilder.Entity<PollAnswer>().HasOne(a => a.LinkedUser).WithMany(u => u.Answers).HasForeignKey("UserId");

            modelBuilder.Entity<Poll>().HasData(new Poll() {Id=1, Name = "Age Poll", Count = 1});
            modelBuilder.Entity<Question>().HasData(new Question()
                {Id = 1, LinkedPollId = 1, QuestionText = "How old are you?", QuestionIndex = 1});
            modelBuilder.Entity<Variant>().HasData(new Variant[]
            {
                new Variant() {Id = 1, QuestionId = 1, Text = "15-30", VariantIndex = 1},
                new Variant() {Id = 2, QuestionId = 1, Text = "30-45", VariantIndex = 2},
                new Variant() {Id = 3, QuestionId = 1, Text = "45-60", VariantIndex = 3}
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
