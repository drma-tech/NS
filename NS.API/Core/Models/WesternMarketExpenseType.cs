namespace NS.API.Core.Models
{
    public enum WesternMarketExpenseType
    {
        [MarketCustom(Name = "Milk", Description = "Milk (regular), (1 liter)", Proportion = 0.25)]
        Milk = 0,

        [MarketCustom(Name = "White Bread", Description = "Loaf of Fresh White Bread (500g)", Proportion = 0.25)]
        WhiteBread = 1,

        [MarketCustom(Name = "Rice", Description = "Rice (white), (1kg)", Proportion = 0.1)]
        Rice = 2,

        [MarketCustom(Name = "Eggs", Description = "Eggs (regular) (12)", Proportion = 0.2)]
        Eggs = 3,

        [MarketCustom(Name = "Cheese", Description = "Local Cheese (1kg)", Proportion = 0.1)]
        Cheese = 4,

        [MarketCustom(Name = "Chicken", Description = "Chicken Fillets (1kg)", Proportion = 0.15)]
        Chicken = 5,

        [MarketCustom(Name = "Beef", Description = "Beef Round (1kg) (or Equivalent Back Leg Red Meat)", Proportion = 0.15)]
        Beef = 6,

        [MarketCustom(Name = "Apples", Description = "Apples (1kg)", Proportion = 0.3)]
        Apples = 7,

        [MarketCustom(Name = "Banana", Description = "Banana (1kg)", Proportion = 0.25)]
        Banana = 8,

        [MarketCustom(Name = "Oranges", Description = "Oranges (1kg)", Proportion = 0.3)]
        Oranges = 9,

        [MarketCustom(Name = "Tomato", Description = "Tomato (1kg)", Proportion = 0.2)]
        Tomato = 10,

        [MarketCustom(Name = "Potato", Description = "Potato (1kg)", Proportion = 0.2)]
        Potato = 11,

        [MarketCustom(Name = "Onion", Description = "Onion (1kg)", Proportion = 0.1)]
        Onion = 12,

        [MarketCustom(Name = "Lettuce", Description = "Lettuce (1 head)", Proportion = 0.2)]
        Lettuce = 13,
    }

    public enum AsianMarketExpenseType
    {
        //[MarketCustom(Name = "Milk", Description = "Milk (regular), (1 liter)", Proportion = 0.25, Convert = Conversions.Gallon)]
        //Milk = 0,

        [MarketCustom(Name = "White Bread", Description = "Loaf of Fresh White Bread (500g)", Proportion = 0.1)]
        WhiteBread = 1,

        [MarketCustom(Name = "Rice", Description = "Rice (white), (1kg)", Proportion = 0.25)]
        Rice = 2,

        [MarketCustom(Name = "Eggs", Description = "Eggs (regular) (12)", Proportion = 0.2)]
        Eggs = 3,

        //[MarketCustom(Name = "Cheese", Description = "Local Cheese (1kg)", Proportion = 0.1)]
        //Cheese = 4,

        [MarketCustom(Name = "Chicken", Description = "Chicken Fillets (1kg)", Proportion = 0.2)]
        Chicken = 5,

        [MarketCustom(Name = "Beef", Description = "Beef Round (1kg) (or Equivalent Back Leg Red Meat)", Proportion = 0.1)]
        Beef = 6,

        [MarketCustom(Name = "Apples", Description = "Apples (1kg)", Proportion = 0.25)]
        Apples = 7,

        [MarketCustom(Name = "Banana", Description = "Banana (1kg)", Proportion = 0.25)]
        Banana = 8,

        [MarketCustom(Name = "Oranges", Description = "Oranges (1kg)", Proportion = 0.15)]
        Oranges = 9,

        [MarketCustom(Name = "Tomato", Description = "Tomato (1kg)", Proportion = 0.2)]
        Tomato = 10,

        [MarketCustom(Name = "Potato", Description = "Potato (1kg)", Proportion = 0.2)]
        Potato = 11,

        [MarketCustom(Name = "Onion", Description = "Onion (1kg)", Proportion = 0.1)]
        Onion = 12,

        [MarketCustom(Name = "Lettuce", Description = "Lettuce (1 head)", Proportion = 0.1)]
        Lettuce = 13,
    }
}