using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityOrm
{
    [Table("RequestRight", Schema = "AvanpostIntegrationTestTaskSchema")]
    public class RequestRight
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

       public List<UserRequestRight> UserRequestRight { get; set; } = new List<UserRequestRight>();
    }
}
