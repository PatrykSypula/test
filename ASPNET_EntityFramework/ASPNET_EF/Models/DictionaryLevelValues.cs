using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASPNET_EF.Models
{
    public class DictionaryLevelValues
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(2)")]
        public string DictionaryLevel { get; set; }

        public List<Dictionaries> Dictionaries { get; set; } = new List<Dictionaries>();

    }
}
