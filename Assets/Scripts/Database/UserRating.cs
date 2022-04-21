using SQLite4Unity3d;

namespace MuseumApp
{
    [Table("UserRating")]
    public class UserRating
    {
        [PrimaryKey][AutoIncrement] public int Id { get; set; }
        public string Username { get; set; }
        public string AttractionId { get; set; }
        public int Rating { get; set; }
    }
}