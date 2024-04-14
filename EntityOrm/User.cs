using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EntityOrm
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    [Table("User", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class User
    {
        /// <summary>
        /// Логин.
        /// </summary>
        [NotNull]
        [Key]
        [Column("login")]
        public string? Login { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [NotNull]
        [Column("lastName")]
        public string? LastName { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [NotNull]
        [Column("firstName")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [NotNull]
        [Column("middleName")]
        public string? MiddleName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        [NotNull]
        [Column("telephoneNumber")]
        public string? TelephoneNumber { get; set; }

        /// <summary>
        /// Права руководителя.
        /// </summary>
        [Column("isLead")]
        public bool IsLead { get; set; }

        /// <summary>
        /// Пользователи и их права по изменению заявок.
        /// </summary>
        public List<UserRequestRight> UserRequestRight { get; set; } = new List<UserRequestRight>();

        /// <summary>
        /// Пользователи и их роли исполнителей.
        /// </summary>
        public List<UserItRole> UserItRole { get; set; } = new List<UserItRole>();


    }
}
