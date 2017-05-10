using System.ComponentModel.DataAnnotations;

namespace Example.Ef
{
    public class Test
    {
        [Key]
        public int Id { get; set; }

        public int Col1 { get; set; }
    }
}