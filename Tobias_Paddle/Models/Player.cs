namespace Tobias_Padle.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Player> AlreadyPlayedWithPlayers { get; set; } = new List<Player>();

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
