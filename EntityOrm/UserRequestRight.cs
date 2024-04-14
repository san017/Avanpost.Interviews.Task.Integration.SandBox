using System.ComponentModel.DataAnnotations.Schema;

namespace EntityOrm
{
    /// <summary>
    /// Имеющиеся права по изменению заявок у пользователя.
    /// </summary>
    [Table("UserRequestRight", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class UserRequestRight
    {  
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        [Column("userId")]
        public string? UserId { get; set; }

        /// <summary>
        /// Идентификатор права по изменению заявок.
        /// </summary>
        [Column("rightId")]
        public int RequestRightId { get; set; }

        /// <summary>
        /// Пользователи.
        /// </summary>
        public virtual User? User { get; set; }

        /// <summary>
        /// Права по изменению заявок.
        /// </summary>
        public virtual RequestRight? RequestRight { get; set; }
    }
}
