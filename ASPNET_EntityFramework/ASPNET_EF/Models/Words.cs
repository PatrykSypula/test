using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET_EF.Models
{
    public class Words
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string WordPolish { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string WordTranslated { get; set; }
        [Column(TypeName = "int")]
        public int DictionaryId { get; set; }

        public Dictionaries Dictionary { get; set; }
    }
}
