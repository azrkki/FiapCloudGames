using System.Collections.Generic;

namespace FCG.Application.DTOs
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Discount { get; set; }
        public decimal Price { get; set; }
        public bool IsOnSale { get; set; }
    }

    public class GameCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
    }

    public class GameUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int? Discount { get; set; }
        public bool IsOnSale { get; set; }
    }

    public class ApplyDiscountDTO
    {
        public int Discount { get; set; }
    }

    public class UpdateSaleStatusDTO
    {
        public bool IsOnSale { get; set; }
    }
}