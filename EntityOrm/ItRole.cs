using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityOrm
{
    /// <summary>
    /// Роли исполнителей.
    /// </summary>
    [Table("ItRole", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class ItRole
    {
        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        [Key]
        [Column ("id")]
        public int Id { get; set; }

        /// <summary>
        /// Наименнование.
        /// </summary>
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Корпоративный номер.
        /// </summary>
        [Column("corporatePhoneNumber")]
        public string? CorporatePhoneNumber { get; set; }

        /// <summary>
        /// Связь пользователя и ролей исполнителей.
        /// </summary>
        public List<UserItRole> UserItRole { get; set; } = new List<UserItRole>();
    }
}
