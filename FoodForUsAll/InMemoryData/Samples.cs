using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace InMemoryData
{
    public static class Samples
    {
        public static IEnumerable<Ingredient> Ingredients { get; private set; }
        public static IEnumerable<Recipe> Recipes { get; private set; }

        static Guid _defaultAuthor = new Guid("00000000-0000-0000-0000-000000000000");

        static Samples()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            List<Recipe> recipes = new List<Recipe>();

            Ingredient roundSteakIngredient = new() { Name = "Round Steak", Description = "" };
            Ingredient ingredient2 = new() { Name = "Bacon", Description = "Sliced bacon" };
            Ingredient ingredient3 = new() { Name = "Golden Mushroom Soup", Description = "Condensed golden mushroom soup" };
            Ingredient ingredient4 = new() { Name = "Parsley", Description = "Chopped parsley" };
            Ingredient diamondCrystalKosherSaltIngredient = new() { Name = "Diamond Crystal Kosher Salt", Description = "" };
            Ingredient groundBlackPepperIngredient = new() { Name = "Ground Black Pepper", Description = "" };
            Ingredient ingredient7 = new() { Name = "Frozen Pearl Onions", Description = "" };
            Ingredient slicedButtonMushroomsIngredient = new() { Name = "Sliced Button Mushrooms", Description = "" };
            Ingredient ingredient9 = new() { Name = "Water", Description = "" };
            Ingredient ingredient10 = new() { Name = "Egg Noodles", Description = "" };

            ingredients.Add(roundSteakIngredient);
            ingredients.Add(ingredient2);
            ingredients.Add(ingredient3);
            ingredients.Add(ingredient4);
            ingredients.Add(diamondCrystalKosherSaltIngredient);
            ingredients.Add(groundBlackPepperIngredient);
            ingredients.Add(ingredient7);
            ingredients.Add(slicedButtonMushroomsIngredient);
            ingredients.Add(ingredient9);
            ingredients.Add(ingredient10);

            List<RecipeIngredient> recipeIngredients1 = new List<RecipeIngredient>();
            recipeIngredients1.Add(new() { Ingredient = roundSteakIngredient, OrderId = 1, Quantity = "2lbs" });
            recipeIngredients1.Add(new() { Ingredient = ingredient2, OrderId = 2, Quantity = "4 slices" });
            recipeIngredients1.Add(new() { Ingredient = ingredient3, OrderId = 3, Quantity = "1 can" });
            recipeIngredients1.Add(new() { Ingredient = ingredient4, OrderId = 4, Quantity = "2tbsp" });
            recipeIngredients1.Add(new() { Ingredient = diamondCrystalKosherSaltIngredient, OrderId = 5, Quantity = "1 tbsp (2 tsp table salt)" });
            recipeIngredients1.Add(new() { Ingredient = groundBlackPepperIngredient, OrderId = 6, Quantity = "1/8 tsp" });
            recipeIngredients1.Add(new() { Ingredient = ingredient7, OrderId = 7, Quantity = "12 oz. (340g)" });
            recipeIngredients1.Add(new() { Ingredient = slicedButtonMushroomsIngredient, OrderId = 8, Quantity = "8 oz" });
            recipeIngredients1.Add(new() { Ingredient = ingredient9, OrderId = 9, Quantity = "1/4 cups" });
            recipeIngredients1.Add(new() { Ingredient = ingredient10, OrderId = 10, Quantity = "1 bag" });

            IngredientSection ingredientSection1 = new() { OrderId = 1, Name = "Main", RecipeIngredients = recipeIngredients1, };

            List<IngredientSection> ingredientSections1 = new List<IngredientSection>();
            ingredientSections1.Add(ingredientSection1);

            List<RecipeInstruction> recipeInstructions1 = new List<RecipeInstruction>();
            recipeInstructions1.Add(new RecipeInstruction { OrderId = 1, Description = "In a large saucepan cook bacon until crisp and set aside." });
            recipeInstructions1.Add(new RecipeInstruction { OrderId = 2, Description = "In the same pan with the bacon grease brown the round steak." });
            recipeInstructions1.Add(new RecipeInstruction { OrderId = 3, Description = "Add soup, water, parsley, salt and pepper; cover and simmer over low heat for 1 hour and 30 minutes." });
            recipeInstructions1.Add(new RecipeInstruction { OrderId = 4, Description = "Add mushrooms and onions; cover and simmer for an additional hour." });
            recipeInstructions1.Add(new RecipeInstruction { OrderId = 5, Description = "Cook noodles." });
            recipeInstructions1.Add(new RecipeInstruction { OrderId = 6, Description = "Serve beef sauce over the noodles and top with the bacon crumbled up into small pieces." });

            InstructionSection instructionSection1 = new InstructionSection { OrderId = 1, Name = "Main", RecipeInstructions = recipeInstructions1, };

            List<InstructionSection> instructionSections1 = new List<InstructionSection>();
            instructionSections1.Add(instructionSection1);

            recipes.Add(new Recipe
            {
                AuthorId = _defaultAuthor,
                Name = "Beef and Bacon",
                Description = "A rich beef, mushroom, onion, and bacon dish served over noodles.",
                IngredientSections = ingredientSections1,
                InstructionSections = instructionSections1,
            });

            Ingredient longGrainRiceIngredient = new() { Name = "Long Grain Rice", Description = "" };
            Ingredient cuminSeedsIngredient = new() { Name = "Cumin Seeds", Description = "" };
            Ingredient lowSodiumChickenBrothIngredient = new() { Name = "Low Sodium Chicken Broth", Description = "" };
            Ingredient groundTurmericIngredient = new() { Name = "Ground Turmeric", Description = "" };
            Ingredient indianCurryBaseIngredient = new() { Name = "Indian Curry Base", Description = "" };
            Ingredient chickenBreastsIngredient = new() { Name = "Chicken Breasts", Description = "" };
            Ingredient oliveOilIngredient = new() { Name = "Extra Virgin Olive Oil", Description = "" };
            Ingredient groundCorianderIngredient = new() { Name = "Ground Coriander", Description = "" };
            Ingredient doubleConcentratedTomatoPasteIngredient = new() { Name = "Double-Concentrated Tomato Paste", Description = "" };
            Ingredient serranoChiliesIngredient = new() { Name = "Serrano Chilies", Description = "" };
            Ingredient heavyCreamIngredient = new() { Name = "Heavy Cream", Description = "" };
            Ingredient paprikaIngredient = new() { Name = "Paprika", Description = "" };
            Ingredient whiteGranulatedSugarIngredient = new() { Name = "White Granulated Sugar", Description = "" };
            Ingredient choppedCilantroIngredient = new() { Name = "Chopped Cilantro", Description = "" };

            ingredients.Add(longGrainRiceIngredient);
            ingredients.Add(cuminSeedsIngredient);
            ingredients.Add(lowSodiumChickenBrothIngredient);
            ingredients.Add(groundTurmericIngredient);
            ingredients.Add(indianCurryBaseIngredient);
            ingredients.Add(chickenBreastsIngredient);
            ingredients.Add(oliveOilIngredient);
            ingredients.Add(groundCorianderIngredient);
            ingredients.Add(doubleConcentratedTomatoPasteIngredient);
            ingredients.Add(serranoChiliesIngredient);
            ingredients.Add(heavyCreamIngredient);
            ingredients.Add(paprikaIngredient);
            ingredients.Add(whiteGranulatedSugarIngredient);
            ingredients.Add(choppedCilantroIngredient);

            List<RecipeIngredient> recipeIngredients2 = new List<RecipeIngredient>();
            recipeIngredients2.Add(new() { Ingredient = slicedButtonMushroomsIngredient, OrderId = 1, Quantity = "8 oz (225 g)" });
            recipeIngredients2.Add(new() { Ingredient = lowSodiumChickenBrothIngredient, OrderId = 2, Quantity = "14.5oz can" });
            recipeIngredients2.Add(new() { Ingredient = groundTurmericIngredient, OrderId = 3, Quantity = "1 tsp" });
            recipeIngredients2.Add(new() { Ingredient = indianCurryBaseIngredient, OrderId = 4, Quantity = "1 portion (~600g)" });
            recipeIngredients2.Add(new() { Ingredient = chickenBreastsIngredient, OrderId = 5, Quantity = "3 (~700g)" });
            recipeIngredients2.Add(new() { Ingredient = oliveOilIngredient, OrderId = 6, Quantity = "2 tbsp." });
            recipeIngredients2.Add(new() { Ingredient = groundCorianderIngredient, OrderId = 7, Quantity = "1 tsp." });
            recipeIngredients2.Add(new() { Ingredient = groundBlackPepperIngredient, OrderId = 8, Quantity = "" });
            recipeIngredients2.Add(new() { Ingredient = doubleConcentratedTomatoPasteIngredient, OrderId = 9, Quantity = "2 Tbsp." });
            recipeIngredients2.Add(new() { Ingredient = serranoChiliesIngredient, OrderId = 10, Quantity = "2 sliced" });
            recipeIngredients2.Add(new() { Ingredient = heavyCreamIngredient, OrderId = 11, Quantity = "1/2 cup" });
            recipeIngredients2.Add(new() { Ingredient = paprikaIngredient, OrderId = 12, Quantity = "1 tsp." });
            recipeIngredients2.Add(new() { Ingredient = whiteGranulatedSugarIngredient, OrderId = 13, Quantity = "2 tsp." });
            recipeIngredients2.Add(new() { Ingredient = diamondCrystalKosherSaltIngredient, OrderId = 14, Quantity = "" });
            recipeIngredients2.Add(new() { Ingredient = choppedCilantroIngredient, OrderId = 15, Quantity = "1 tbsp." });

            List<RecipeIngredient> recipeIngredients3 = new List<RecipeIngredient>();
            recipeIngredients3.Add(new() { Ingredient = longGrainRiceIngredient, OrderId = 1, Quantity = "2 cups" });
            recipeIngredients3.Add(new() { Ingredient = oliveOilIngredient, OrderId = 2, Quantity = "1 tbsp." });
            recipeIngredients3.Add(new() { Ingredient = cuminSeedsIngredient, OrderId = 3, Quantity = "1/2 tsp." });
            recipeIngredients3.Add(new() { Ingredient = diamondCrystalKosherSaltIngredient, OrderId = 4, Quantity = "pinch" });

            IngredientSection ingredientSection2 = new() { OrderId = 1, Name = "Main", RecipeIngredients = recipeIngredients2, };
            IngredientSection ingredientSection3 = new() { OrderId = 2, Name = "Rice", RecipeIngredients = recipeIngredients3, };

            List<IngredientSection> ingredientSections2 = new List<IngredientSection>();
            ingredientSections2.Add(ingredientSection2);
            ingredientSections2.Add(ingredientSection3);

            List<RecipeInstruction> recipeInstructions2 = new List<RecipeInstruction>();
            recipeInstructions2.Add(new RecipeInstruction { OrderId = 1, Description = "In a large pot over medium-high heat, heat 2 tablespoons oil." });
            recipeInstructions2.Add(new RecipeInstruction { OrderId = 2, Description = "Add tomato paste and chilies; cook until it turns brick red stirring frequently to avoid scorching." });
            recipeInstructions2.Add(new RecipeInstruction { OrderId = 3, Description = "Mix in Indian Curry Base and cook until bubbling." });
            recipeInstructions2.Add(new RecipeInstruction { OrderId = 4, Description = "Add chicken and dry spices and sear chicken until no pink remains." });
            recipeInstructions2.Add(new RecipeInstruction { OrderId = 5, Description = "Stir in mushrooms and cook until they soften a bit." });
            recipeInstructions2.Add(new RecipeInstruction { OrderId = 6, Description = "Add broth and sugar, bring to a simmer and wait until half the liquid evaporates." });
            recipeInstructions2.Add(new RecipeInstruction { OrderId = 7, Description = "Stir in heavy cream, and season with salt and pepper. Simmer for about five more minutes." });
            recipeInstructions2.Add(new RecipeInstruction { OrderId = 8, Description = "Serve chicken mushroom sauce over rice and garnish with cilantro." });

            List<RecipeInstruction> recipeInstructions3 = new List<RecipeInstruction>();
            recipeInstructions3.Add(new RecipeInstruction { OrderId = 1, Description = "Lightly toast cumin seeds in a rice cooker. Add 1 tablespoon of oil, rinsed rice, and two cups of water. Cook through and set aside." });

            InstructionSection instructionSection2 = new InstructionSection { OrderId = 1, Name = "Main", RecipeInstructions = recipeInstructions2, };
            InstructionSection instructionSection3 = new InstructionSection { OrderId = 2, Name = "Rice", RecipeInstructions = recipeInstructions3, };

            List<InstructionSection> instructionSections2 = new List<InstructionSection>();
            instructionSections2.Add(instructionSection2);
            instructionSections2.Add(instructionSection3);

            recipes.Add(new Recipe
            {
                AuthorId = _defaultAuthor,
                Name = "Chicken Mushroom Curry",
                Description = "A declicious rendition of a Indian take out favorite.",
                IngredientSections = ingredientSections2,
                InstructionSections = instructionSections2,
            });





            Ingredient dicedTomatoesIngredient = new() { Name = "Diced Tomatoes", Description = "" };
            Ingredient chiliPowderIngredient = new() { Name = "Chili Powder", Description = "" };
            Ingredient greenPeppersIngredient = new() { Name = "Green Peppers", Description = "Green Bell Peppers" };
            Ingredient beefsteakTomatoesIngredient = new() { Name = "Beefsteak Tomatoes", Description = "Whole Beefsteak Tomatoes" };

            ingredients.Add(dicedTomatoesIngredient);
            ingredients.Add(chiliPowderIngredient);
            ingredients.Add(greenPeppersIngredient);
            ingredients.Add(beefsteakTomatoesIngredient);

            List<RecipeIngredient> recipeIngredients4 = new List<RecipeIngredient>();
            recipeIngredients4.Add(new() { Ingredient = oliveOilIngredient, OrderId = 1, Quantity = "2 tbsp." });
            recipeIngredients4.Add(new() { Ingredient = chickenBreastsIngredient, OrderId = 2, Quantity = "3 (~700g)" });
            recipeIngredients4.Add(new() { Ingredient = indianCurryBaseIngredient, OrderId = 3, Quantity = "1 portion (~600g)" });
            recipeIngredients4.Add(new() { Ingredient = serranoChiliesIngredient, OrderId = 4, Quantity = "2 sliced" });
            recipeIngredients4.Add(new() { Ingredient = dicedTomatoesIngredient, OrderId = 5, Quantity = "2 14.5oz cans" });
            recipeIngredients4.Add(new() { Ingredient = doubleConcentratedTomatoPasteIngredient, OrderId = 6, Quantity = "2 Tbsp." });
            recipeIngredients4.Add(new() { Ingredient = groundTurmericIngredient, OrderId = 7, Quantity = "1 tsp" });
            recipeIngredients4.Add(new() { Ingredient = chiliPowderIngredient, OrderId = 8, Quantity = "1 tsp" });
            recipeIngredients4.Add(new() { Ingredient = diamondCrystalKosherSaltIngredient, OrderId = 9, Quantity = "" });
            recipeIngredients4.Add(new() { Ingredient = greenPeppersIngredient, OrderId = 10, Quantity = "2" });
            recipeIngredients4.Add(new() { Ingredient = beefsteakTomatoesIngredient, OrderId = 2, Quantity = "2 cubed" });
            recipeIngredients4.Add(new() { Ingredient = choppedCilantroIngredient, OrderId = 15, Quantity = "1 tbsp." });

            List<RecipeIngredient> recipeIngredients5 = new List<RecipeIngredient>();
            recipeIngredients5.Add(new() { Ingredient = oliveOilIngredient, OrderId = 1, Quantity = "1 tbsp." });
            recipeIngredients5.Add(new() { Ingredient = longGrainRiceIngredient, OrderId = 2, Quantity = "2 cups" });
            recipeIngredients5.Add(new() { Ingredient = cuminSeedsIngredient, OrderId = 3, Quantity = "1/2 tsp." });
            recipeIngredients5.Add(new() { Ingredient = diamondCrystalKosherSaltIngredient, OrderId = 4, Quantity = "pinch" });

            IngredientSection ingredientSection4 = new() { OrderId = 1, Name = "Main", RecipeIngredients = recipeIngredients4, };
            IngredientSection ingredientSection5 = new() { OrderId = 2, Name = "Rice", RecipeIngredients = recipeIngredients5, };

            List<IngredientSection> ingredientSections3 = new List<IngredientSection>();
            ingredientSections3.Add(ingredientSection4);
            ingredientSections3.Add(ingredientSection5);

            List<RecipeInstruction> recipeInstructions4 = new List<RecipeInstruction>();
            recipeInstructions4.Add(new RecipeInstruction { OrderId = 1, Description = "In a large pot over medium-high heat, heat 2 tablespoons oil." });
            recipeInstructions4.Add(new RecipeInstruction { OrderId = 2, Description = "Add diced tomatoes, chilies and tomato paste; cook until water has cooked off and tomato turns brick red stirring frequently to avoid scorching." });
            recipeInstructions4.Add(new RecipeInstruction { OrderId = 3, Description = "Add Indian Curry Base and cook until bubbling." });
            recipeInstructions4.Add(new RecipeInstruction { OrderId = 4, Description = "Add chicken and remaining dry spices and poach chicken until no pink remains." });
            recipeInstructions4.Add(new RecipeInstruction { OrderId = 5, Description = "Stir in cubed green pepper and cubed onion. Add one cup of water. Cook until vegetables have softened slightly." });
            recipeInstructions4.Add(new RecipeInstruction { OrderId = 6, Description = "Add in cubed tomatoes and cook until all vegetables are the desired consistency." });
            recipeInstructions4.Add(new RecipeInstruction { OrderId = 7, Description = "Serve chicken mushroom sauce over rice and garnish with cilantro." });

            List<RecipeInstruction> recipeInstructions5 = new List<RecipeInstruction>();
            recipeInstructions5.Add(new RecipeInstruction { OrderId = 1, Description = "Lightly toast cumin seeds in a rice cooker. Add 1 tablespoon of oil, rinsed rice, and two cups of water. Add any additional salt for seasoning as preferred. Cook through and set aside." });

            InstructionSection instructionSection4 = new InstructionSection { OrderId = 1, Name = "Main", RecipeInstructions = recipeInstructions4, };
            InstructionSection instructionSection5 = new InstructionSection { OrderId = 2, Name = "Rice", RecipeInstructions = recipeInstructions5, };

            List<InstructionSection> instructionSections3 = new List<InstructionSection>();
            instructionSections3.Add(instructionSection4);
            instructionSections3.Add(instructionSection5);

            recipes.Add(new Recipe
            {
                AuthorId = _defaultAuthor,
                Name = "Chicken Jalfrezi",
                Description = "A declicious rendition of a Indian take out favorite.",
                IngredientSections = ingredientSections3,
                InstructionSections = instructionSections3,
            });

            Ingredients = ingredients;
            Recipes = recipes;
        }
    }
}
