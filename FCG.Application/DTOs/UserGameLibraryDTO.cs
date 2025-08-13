namespace FCG.Application.DTOs
{
    public class UserGameLibraryDTO
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string UserName { get; set; }
        public string GameName { get; set; }
    }

    public class UserGameLibraryCreateDTO
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
    }

    public class UserGameLibraryUpdateDTO
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public int UpdateToGameId { get; set; }
    }
}