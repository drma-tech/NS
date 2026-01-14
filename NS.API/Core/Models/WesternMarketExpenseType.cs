namespace NS.API.Core.Models
{
    public enum WesternMarketExpenseType
    {
        [MarketCustom(Name = "Milk", Description = "Milk (Regular, 1 Liter)", Proportion = 0.25)]
        Milk = 0,

        [MarketCustom(Name = "White Bread", Description = "Fresh White Bread (500 g Loaf)", Proportion = 0.25)]
        WhiteBread = 1,

        [MarketCustom(Name = "Rice", Description = "White Rice (1 kg)", Proportion = 0.1)]
        Rice = 2,

        [MarketCustom(Name = "Eggs", Description = "Eggs (12, Large Size)", Proportion = 0.2)]
        Eggs = 3,

        [MarketCustom(Name = "Cheese", Description = "Local Cheese (1 kg)", Proportion = 0.1)]
        Cheese = 4,

        [MarketCustom(Name = "Chicken", Description = "Chicken Fillets (1 kg)", Proportion = 0.15)]
        Chicken = 5,

        [MarketCustom(Name = "Beef", Description = "Beef Round or Equivalent Back Leg Red Meat (1 kg)", Proportion = 0.15)]
        Beef = 6,

        [MarketCustom(Name = "Apples", Description = "Apples (1 kg)", Proportion = 0.3)]
        Apples = 7,

        [MarketCustom(Name = "Banana", Description = "Bananas (1 kg)", Proportion = 0.25)]
        Banana = 8,

        [MarketCustom(Name = "Oranges", Description = "Oranges (1 kg)", Proportion = 0.3)]
        Oranges = 9,

        [MarketCustom(Name = "Tomato", Description = "Tomatoes (1 kg)", Proportion = 0.2)]
        Tomato = 10,

        [MarketCustom(Name = "Potato", Description = "Potatoes (1 kg)", Proportion = 0.2)]
        Potato = 11,

        [MarketCustom(Name = "Onion", Description = "Onions (1 kg)", Proportion = 0.1)]
        Onion = 12,

        [MarketCustom(Name = "Lettuce", Description = "Lettuce (1 Head)", Proportion = 0.2)]
        Lettuce = 13,
    }

    public enum AsianMarketExpenseType
    {
        //[MarketCustom(Name = "Milk", Description = "Milk (Regular, 1 Liter)", Proportion = 0.25, Convert = Conversions.Gallon)]
        //Milk = 0,

        [MarketCustom(Name = "White Bread", Description = "Fresh White Bread (500 g Loaf)", Proportion = 0.1)]
        WhiteBread = 1,

        [MarketCustom(Name = "Rice", Description = "White Rice (1 kg)", Proportion = 0.25)]
        Rice = 2,

        [MarketCustom(Name = "Eggs", Description = "Eggs (12, Large Size)", Proportion = 0.2)]
        Eggs = 3,

        //[MarketCustom(Name = "Cheese", Description = "Local Cheese (1 kg)", Proportion = 0.1)]
        //Cheese = 4,

        [MarketCustom(Name = "Chicken", Description = "Chicken Fillets (1 kg)", Proportion = 0.2)]
        Chicken = 5,

        [MarketCustom(Name = "Beef", Description = "Beef Round or Equivalent Back Leg Red Meat (1 kg)", Proportion = 0.1)]
        Beef = 6,

        [MarketCustom(Name = "Apples", Description = "Apples (1 kg)", Proportion = 0.25)]
        Apples = 7,

        [MarketCustom(Name = "Banana", Description = "Bananas (1 kg)", Proportion = 0.25)]
        Banana = 8,

        [MarketCustom(Name = "Oranges", Description = "Oranges (1 kg)", Proportion = 0.15)]
        Oranges = 9,

        [MarketCustom(Name = "Tomato", Description = "Tomatoes (1 kg)", Proportion = 0.2)]
        Tomato = 10,

        [MarketCustom(Name = "Potato", Description = "Potatoes (1 kg)", Proportion = 0.2)]
        Potato = 11,

        [MarketCustom(Name = "Onion", Description = "Onions (1 kg)", Proportion = 0.1)]
        Onion = 12,

        [MarketCustom(Name = "Lettuce", Description = "Lettuce (1 Head)", Proportion = 0.1)]
        Lettuce = 13,
    }
}