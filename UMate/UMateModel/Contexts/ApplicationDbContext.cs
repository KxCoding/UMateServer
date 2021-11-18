using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UMateModel.Entities.Timetable;
using UMateModel.Entities.UMateBoard;
using UMateModel.Entities.Place;
using UMateModel.Models;

namespace UMateModel.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<Timetable> Timetable { get; set; }

        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer("Server=tcp:practice1008.database.windows.net,1433;Initial Catalog=UMate;Persist Security Info=False;User ID=test;Password=abcd1234@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }


        // Common
        public DbSet<University> University { get; set; }

        /// <summary>
        /// 게시판
        /// </summary>
        public DbSet<Board> Board { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<LikeComment> LikeComment { get; set; }
        public DbSet<LikePost> LikePost { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostImage> PostImage { get; set; }
        public DbSet<ScrapPost> ScrapPost { get; set; }


        /// <summary>
        /// 강의평가 게시판
        /// </summary>
        public DbSet<Professor> Professor { get; set; }
        public DbSet<LectureInfo> LectureInfo { get; set; }
        public DbSet<LectureReview> LectureReview { get; set; }
        public DbSet<TestInfo> TestInfo { get; set; }
        public DbSet<Example> Example { get; set; }

        /// Place
        public DbSet<Place> Place { get; set; }
        public DbSet<PlaceBookmark> PlaceBookmark { get; set; }
    }
}
