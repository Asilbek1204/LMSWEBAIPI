using Microsoft.EntityFrameworkCore;
using LMS.Data.Entities;

namespace LMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User -> Teacher: One-to-One
            builder.Entity<Teacher>()
                .HasOne(t => t.User)                      // Teacher bir User ga bog'lanadi
                .WithOne(u => u.TeacherProfile)           // User bir TeacherProfile ga bog'lanadi  
                .HasForeignKey<Teacher>(t => t.UserId)    // Bog'lanish UserId orqali amalga oshadi
                .OnDelete(DeleteBehavior.Cascade);        // User o'chirilsa, Teacher ham o'chadi

            // User -> Student: One-to-One  
            builder.Entity<Student>()
                .HasOne(s => s.User)                      // Student bir User ga bog'lanadi
                .WithOne(u => u.StudentProfile)           // User bir StudentProfile ga bog'lanadi
                .HasForeignKey<Student>(s => s.UserId)    // Bog'lanish UserId orqali amalga oshadi
                .OnDelete(DeleteBehavior.Cascade);        // User o'chirilsa, Student ham o'chadi

            // Teacher -> Course: One-to-Many
            builder.Entity<Course>()
                .HasOne(c => c.Teacher)                   // Course bir Teacher ga bog'lanadi
                .WithMany(t => t.Courses)                 // Teacher ko'p Course lar ga bog'lanadi
                .HasForeignKey(c => c.TeacherId)          // Bog'lanish TeacherId orqali
                .OnDelete(DeleteBehavior.Restrict);       // Teacher o'chirilsa, Course lar o'chmaydi


            // Student -> Enrollment: One-to-Many

            builder.Entity<Enrollment>()
                .HasOne(e => e.Student)                   // Enrollment bir Student ga bog'lanadi
                .WithMany(s => s.Enrollments)             // Student ko'p Enrollment lar ga bog'lanadi
                .HasForeignKey(e => e.StudentId)          // Bog'lanish StudentId orqali
                .OnDelete(DeleteBehavior.Cascade);        // Student o'chirilsa, Enrollment lar ham o'chadi


            // Course -> Category: Many-to-One
            builder.Entity<Course>()
                .HasOne(c => c.Category)                  // Course bir Category ga bog'lanadi
                .WithMany(cat => cat.Courses)             // Category ko'p Course lar ga bog'lanadi
                .HasForeignKey(c => c.CategoryId)         // Bog'lanish CategoryId orqali
                .OnDelete(DeleteBehavior.Restrict);       // Category o'chirilsa, Course lar o'chmaydi

            // Course -> Enrollment: One-to-Many
            builder.Entity<Enrollment>()
                .HasOne(e => e.Course)                    // Enrollment bir Course ga bog'lanadi
                .WithMany(c => c.Enrollments)             // Course ko'p Enrollment lar ga bog'lanadi
                .HasForeignKey(e => e.CourseId)           // Bog'lanish CourseId orqali
                .OnDelete(DeleteBehavior.Cascade);        // Course o'chirilsa, Enrollment lar ham o'chadi

            // Course -> Module: One-to-Many
            builder.Entity<Module>()
                .HasOne(m => m.Course)                    // Module bir Course ga bog'lanadi
                .WithMany(c => c.Modules)                 // Course ko'p Module lar ga bog'lanadi
                .HasForeignKey(m => m.CourseId)           // Bog'lanish CourseId orqali
                .OnDelete(DeleteBehavior.Cascade);        // Course o'chirilsa, Module lar ham o'chadi

            // Har bir email va username unique bo'lishi kerak
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            // Har bir student ID unique bo'lishi kerak
            builder.Entity<Student>()
                .HasIndex(s => s.StudentId)
                .IsUnique();

            // Har bir kategoriya nomi unique bo'lishi kerak
            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Bir o'quvchi bir kursga faqat bir marta yozilishi mumkin
            builder.Entity<Enrollment>()
                .HasIndex(e => new { e.CourseId, e.StudentId })
                .IsUnique();
        }
    }
}