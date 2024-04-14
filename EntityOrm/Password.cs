using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EntityOrm
{
    /// <summary>
    /// Пароли пользователей.
    /// </summary>
    [Table("Passwords", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class Password
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя.(По совместительству его логин.)
        /// </summary>
        [NotNull]
        [Column("userId")]
        public string? UserId { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        [NotNull]
        [Column("password")]
        public string? PasswordUser { get; set; }
    }
}
