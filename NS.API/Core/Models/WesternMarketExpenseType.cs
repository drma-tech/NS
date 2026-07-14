namespace NS.API.Core.Models
{
    public enum WesternMarketExpenseType
    {
        [FieldSettings("Milk", Description = "Milk (Regular, 1 Liter)", Proportion = 0.25)]
        Milk = 0,

        [FieldSettings("White Bread", Description = "Fresh White Bread (500 g Loaf)", Proportion = 0.25)]
        WhiteBread = 1,

        [FieldSettings("Rice", Description = "White Rice (1 kg)", Proportion = 0.1)]
        Rice = 2,

        [FieldSettings("Eggs", Description = "Eggs (12, Large Size)", Proportion = 0.2)]
        Eggs = 3,

        [FieldSettings("Cheese", Description = "Local Cheese (1 kg)", Proportion = 0.1)]
        Cheese = 4,

        [FieldSettings("Chicken", Description = "Chicken Fillets (1 kg)", Proportion = 0.15)]
        Chicken = 5,

        [FieldSettings("Beef", Description = "Beef Round or Equivalent Back Leg Red Meat (1 kg)", Proportion = 0.15)]
        Beef = 6,

        [FieldSettings("Apples", Description = "Apples (1 kg)", Proportion = 0.3)]
        Apples = 7,

        [FieldSettings("Banana", Description = "Bananas (1 kg)", Proportion = 0.25)]
        Banana = 8,

        [FieldSettings("Oranges", Description = "Oranges (1 kg)", Proportion = 0.3)]
        Oranges = 9,

        [FieldSettings("Tomato", Description = "Tomatoes (1 kg)", Proportion = 0.2)]
        Tomato = 10,

        [FieldSettings("Potato", Description = "Potatoes (1 kg)", Proportion = 0.2)]
        Potato = 11,

        [FieldSettings("Onion", Description = "Onions (1 kg)", Proportion = 0.1)]
        Onion = 12,

        [FieldSettings("Lettuce", Description = "Lettuce (1 Head)", Proportion = 0.2)]
        Lettuce = 13,
    }

    public enum AsianMarketExpenseType
    {
        //[FieldSettings("Milk", Description = "Milk (Regular, 1 Liter)", Proportion = 0.25, Convert = Conversions.Gallon)]
        //Milk = 0,

        [FieldSettings("White Bread", Description = "Fresh White Bread (500 g Loaf)", Proportion = 0.1)]
        WhiteBread = 1,

        [FieldSettings("Rice", Description = "White Rice (1 kg)", Proportion = 0.25)]
        Rice = 2,

        [FieldSettings("Eggs", Description = "Eggs (12, Large Size)", Proportion = 0.2)]
        Eggs = 3,

        //[FieldSettings("Cheese", Description = "Local Cheese (1 kg)", Proportion = 0.1)]
        //Cheese = 4,

        [FieldSettings("Chicken", Description = "Chicken Fillets (1 kg)", Proportion = 0.2)]
        Chicken = 5,

        [FieldSettings("Beef", Description = "Beef Round or Equivalent Back Leg Red Meat (1 kg)", Proportion = 0.1)]
        Beef = 6,

        [FieldSettings("Apples", Description = "Apples (1 kg)", Proportion = 0.25)]
        Apples = 7,

        [FieldSettings("Banana", Description = "Bananas (1 kg)", Proportion = 0.25)]
        Banana = 8,

        [FieldSettings("Oranges", Description = "Oranges (1 kg)", Proportion = 0.15)]
        Oranges = 9,

        [FieldSettings("Tomato", Description = "Tomatoes (1 kg)", Proportion = 0.2)]
        Tomato = 10,

        [FieldSettings("Potato", Description = "Potatoes (1 kg)", Proportion = 0.2)]
        Potato = 11,

        [FieldSettings("Onion", Description = "Onions (1 kg)", Proportion = 0.1)]
        Onion = 12,

        [FieldSettings("Lettuce", Description = "Lettuce (1 Head)", Proportion = 0.1)]
        Lettuce = 13,
    }
}