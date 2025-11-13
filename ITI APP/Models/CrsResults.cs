
namespace ITI_APP.Models
{
    public class CrsResults
    {
        public int Degree { get; set; }

        public int StudentId { get; set; }
        public int CrsId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(CrsId))]
        public virtual Course Course { get; set; }
    }
}
