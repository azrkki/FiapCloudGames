using System;
using System.Collections.Generic;

namespace FCG.Core.Entity
{
    public class Role : EntityBase
    {
        public string Name { get; set; }
        
        public ICollection<User> Users { get; set; } = new List<User>();
        
        public Role() { }

        public Role(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
