using FCG.Core.Entity.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCG.Core.Entity
{
    public class User : EntityBase
    {
        public string Name { get; private set; }
        private string _email;
        public string Email 
        { 
            get => _email; 
            private set => _email = value; 
        }
        
        private Password _password;
        public Password Password 
        { 
            get => _password; 
            private set => _password = value; 
        }
        
        public int RoleId { get; private set; }
        public Role Role { get; private set; }
        public char RemoveFlag { get; private set; }

        private readonly List<UserGameLibrary> _gameLibrary = new List<UserGameLibrary>();
        public IReadOnlyCollection<UserGameLibrary> GameLibrary => _gameLibrary.AsReadOnly();

        protected User() { }

        public User(string name, Email email, Password password, Role role)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
                
            Name = name;
            Email = email?.Value ?? throw new ArgumentNullException(nameof(email));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            Role = role ?? throw new ArgumentNullException(nameof(role));
            RoleId = role.Id;
            RemoveFlag = 'F';
        }

        public void UpdatePersonalInfo(string name, Email email)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;
                
            if (email != null)
                Email = email.Value;
        }
        
        public void ChangePassword(Password currentPassword, Password newPassword)
        {
            if (newPassword == null)
                throw new ArgumentNullException(nameof(newPassword));
                
            Password = newPassword;
        }
        
        public void ChangeRole(Role newRole)
        {
            if (newRole == null)
                throw new ArgumentNullException(nameof(newRole));
                
            Role = newRole;
            RoleId = newRole.Id;
        }

        public void AddGameToLibrary(Game game)
        {
            if (game == null) 
                throw new ArgumentNullException(nameof(game));
                
            if (_gameLibrary.Any(ug => ug.GameId == game.Id))
                throw new InvalidOperationException("Game already in the library");

            var userGameLibrary = new UserGameLibrary(this, game);
            _gameLibrary.Add(userGameLibrary);
        }
        
        public void RemoveGameFromLibrary(Game game)
        {
            if (game == null) 
                throw new ArgumentNullException(nameof(game));
                
            var userGame = _gameLibrary.FirstOrDefault(ug => ug.GameId == game.Id);
            if (userGame == null)
                throw new InvalidOperationException("Game not found in the library");
                
            _gameLibrary.Remove(userGame);
        }

        public void MarkForRemoval()
        {
            RemoveFlag = 'T';
        }
        
        public void RestoreAccount()
        {
            RemoveFlag = 'F';
        }
    }
}