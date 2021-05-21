public class Recipes
{
    public struct Recipe
    {
        public int points;
        public string recipeName;
        public eIngredients ingredient1;
        public eIngredients ingredient2;
        public eIngredients ingredient3;
    }


    public static Recipe[] RECIPES = new Recipe[]
    {
        new Recipe(){points = 300, recipeName = "Tomato Soup", ingredient1 = eIngredients.tomato, ingredient2 = eIngredients.tomato},
        new Recipe(){points = 300, recipeName = "Asparagus Meal", ingredient1 = eIngredients.asparagus, ingredient2 = eIngredients.onion},
        new Recipe(){points = 300, recipeName = "Carrot Creme Soup", ingredient1 = eIngredients.carrot, ingredient2 = eIngredients.carrot},
        new Recipe(){points = 500, recipeName = "Chicken Stew", ingredient1 = eIngredients.chicken, ingredient2 = eIngredients.onion, ingredient3 = eIngredients.carrot },
        new Recipe(){points = 500, recipeName = "Veggie Soup", ingredient1 = eIngredients.carrot, ingredient2 = eIngredients.onion, ingredient3 = eIngredients.tomato }
    };


    public enum eIngredients
    {
        empty,
        asparagus,
        carrot,
        chicken,
        onion, 
        tomato
    }
}
