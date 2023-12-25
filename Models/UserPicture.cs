namespace Composition.Models
{
    public class UserPicture
    {
        public Guid Id { get; set; }
        public string URL { get; set; } = string.Empty;

        //Relationship
        public Guid UserId { get; set; }
        public virtual required User User { get; set; }
    }
}
