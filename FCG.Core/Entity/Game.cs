using System;
using System.Collections.Generic;

namespace FCG.Core.Entity
{
    public class Game : EntityBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public decimal OriginalPrice { get; set; }
        public int Discount { get; set; }
        public bool IsOnSale { get; private set; }

        private readonly List<UserGameLibrary> _userGameLibraries = new List<UserGameLibrary>();
        public IReadOnlyCollection<UserGameLibrary> UserGameLibraries => _userGameLibraries.AsReadOnly();

        protected Game() { }

        public Game(string name, string description, decimal value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (value < 0)
                throw new ArgumentException("Value cannot be negative", nameof(value));

            Name = name;
            Description = description ?? "";
            Discount = 0;
            OriginalPrice = value;
            Price = value;
            IsOnSale = false;
        }

        public void UpdateDetails(string name, string description)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (description != null)
                Description = description;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Price cannot be negative", nameof(newPrice));

            OriginalPrice = newPrice;
            // Recalculate the discounted price if there's a discount applied
            if (Discount > 0)
            {
                Price = CalculateDiscountedPrice(Discount);
            }
            else
            {
                Price = newPrice;
            }
        }

        public void UpdateIsOnSale(bool newValue)
        {
            if (!(newValue == true || newValue == false))
                throw new ArgumentException("Value for isOnSale not valid", nameof(newValue));
            IsOnSale = newValue;
        }

        public void ApplyDiscount(int discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Discount range 0 - 100", nameof(discountPercentage));

            Price = CalculateDiscountedPrice(discountPercentage);
            Discount = discountPercentage;
            IsOnSale = discountPercentage > 0;
        }

        public void RemoveDiscount()
        {
            Discount = 0;
            IsOnSale = false;
        }

        public decimal CalculateDiscountedPrice(int discount)
        {
            if (discount <= 0)
                return OriginalPrice;
            decimal discountedPrice = OriginalPrice * ((decimal)discount / 100m);
            return OriginalPrice - discountedPrice;
        }
    }
}
