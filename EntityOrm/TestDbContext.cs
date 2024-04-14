using Microsoft.EntityFrameworkCore;

namespace EntityOrm
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    public class TestDbContext : DbContext
    {
        public TestDbContext()
        {

        }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserItRole>()
                  .HasKey(nameof(UserItRole.UserId),nameof(UserItRole.ItRoleId));

            modelBuilder.Entity<UserRequestRight>()
                 .HasKey(nameof(UserRequestRight.UserId), nameof(UserRequestRight.RequestRightId));
        }


        /// <summary>
        /// Пользователи.
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// Пароли.
        /// </summary>
        public virtual DbSet<Password> Passwords { get; set; }

        /// <summary>
        /// Исполнительные роли.
        /// </summary>
        public virtual DbSet<ItRole> ItRoles { get; set; }

        /// <summary>
        /// Права по изменению заявок.
        /// </summary>
        public virtual DbSet<RequestRight> RequestRights { get; set; }

        /// <summary>
        /// Исполнительные роли у пользователей.
        /// </summary>
        public virtual DbSet<UserItRole> UserItRoles { get; set; }

        /// <summary>
        /// Права у пользователей по изменению заявок.
        /// </summary>
        public virtual DbSet<UserRequestRight> UserRequestRights { get; set; }
    }
}
