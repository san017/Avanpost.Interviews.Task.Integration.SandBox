using System.ComponentModel.DataAnnotations.Schema;

namespace EntityOrm
{
    /// <summary>
    /// Связь пользователей и ролей исполнителей.
    /// </summary>
    [Table("UserITRole", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class UserItRole
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        [Column("userId")]
        public string? UserId { get; set; }

        /// <summary>
        /// Идентификатор роли исполнителя.
        /// </summary>
        [Column("roleId")]
        public int ItRoleId { get; set; }

        /// <summary>
        /// Пользователи.
        /// </summary>
        public virtual User? User { get; set; }

        /// <summary>
        /// Роли исполнителей.
        /// </summary>
        public virtual ItRole? ItRole { get; set; }
    }
}
