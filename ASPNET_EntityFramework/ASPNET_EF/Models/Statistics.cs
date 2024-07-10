using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNET_EF.Models
{
    public class Statistics
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public DateTime SessionDate { get; set; }
        [Column(TypeName = "int")]
        public int GoodAnswers { get; set; }
        [Column(TypeName = "int")]
        public int AllAnswers { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Percentage { get; set; }
        [Column(TypeName = "int")]
        public int DictionaryId { get; set; }
        [Column(TypeName = "int")]
        public int UserId { get; set; }

        public Dictionaries Dictionary { get; set; }
    }
}
