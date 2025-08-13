namespace FCG.Core.Entity
{
    public class UserGameLibrary
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public User User { get; set; }
        public Game Game { get; set; }

        public UserGameLibrary() { }
        

        public UserGameLibrary(User user, Game game)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Game = game ?? throw new ArgumentNullException(nameof(game));
            UserId = user.Id;
            GameId = game.Id;
        }
    }
}
