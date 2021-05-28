using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace DbData
{
    public class RecipeRepository : IRecipeRepository
    {
        public RecipeRepository(string connectionString)
        {
            _foodForUsAllConnectionString = connectionString;
        }

        #region ingredients

        public async Task<Ingredient> GetIngredient(int ingredientId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            [Name],
                            [Description]
                        FROM Recipes.Ingredient
                        WHERE Id = @Id; ";

                cmd.Parameters.AddWithValue("@Id", ingredientId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {;
                        string name = rdr["Name"].ToString();
                        string description = rdr["Description"].ToString();
                        return new Ingredient
                        {
                            Id = ingredientId,
                            Name = name,
                            Description = description,
                        };
                    }
                }
            }

            return null;
        }

        public async Task<List<Ingredient>> GetAllIngredients()
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<Ingredient> ingredients = new List<Ingredient>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id,
                            [Name],
                            [Description]
                        FROM Recipes.Ingredient;";

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        string name = rdr["Name"].ToString();
                        string description = rdr["Description"].ToString();
                        ingredients.Add(
                            new Ingredient
                            {
                                Id = id,
                                Name = name,
                                Description = description,
                            });
                    }
                }
            }

            return ingredients;
        }

        public async Task AddIngredient(Ingredient ingredient)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  INSERT INTO Recipes.Ingredient (
                            [Name],
                            [Description]
                        )
                        OUTPUT Inserted.Id
                        VALUES (
                            @Name,
                            @Description
                        );";
                cmd.Parameters.AddWithValue("@Name", ingredient.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@Description", ingredient.Description ?? string.Empty);

                await conn.OpenAsync();

                ingredient.Id = (int)await cmd.ExecuteScalarAsync();
                
                conn.Close();
            }
        }

        public async Task<bool> DeleteIngredient(int ingredientId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<RecipeIngredient> recipeIngredients = await GetRecipeIngredientsByIngredient(ingredientId);
            if (recipeIngredients.Count > 0)
                return false;
            else
            {
                using (var conn = new SqlConnection(_foodForUsAllConnectionString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM Recipes.Ingredient WHERE Id = @Id;";

                    cmd.Parameters.AddWithValue("@Id", ingredientId);

                    await conn.OpenAsync();

                    int numberOfRowsDeleted = await cmd.ExecuteNonQueryAsync();

                    conn.Close();

                    return numberOfRowsDeleted > 0;
                }
            }
        }

        #endregion

        #region recipe detail

        public async Task UpdateRecipeName(int recipeId, string recipeName)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"UPDATE Recipes.Recipe
                        SET [Name] = @Name
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Name", recipeName);
                cmd.Parameters.AddWithValue("@Id", recipeId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async Task UpdateRecipeDescription(int recipeId, string recipeDescription)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"UPDATE Recipes.Recipe
                        SET [Description] = @Description
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Description", recipeDescription);
                cmd.Parameters.AddWithValue("@Id", recipeId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async Task<Recipe> GetRecipe(int recipeId)
        {
            List<Recipe> recipes = await GetRecipes("AND Recipe.Id = " + recipeId.ToString());

            if (recipes.Count == 1)
                return recipes.First();
            else
                return null;
        }

        public async Task<List<Recipe>> GetRecipesByAuthor(Guid authorId)
        {
            return await GetRecipes("AND Recipe.AuthorId = '" + authorId.ToString() + "'");
        }

        public async Task<List<Recipe>> GetRecipesByAuthorAndSearchByNameOrDescription(Guid authorId, string searchString)
        {
            List<Recipe> recipes = await GetRecipes("AND Recipe.AuthorId = '" + authorId.ToString() + "'");
            return recipes.Where(x => x.Name.Contains(searchString) || x.Description.Contains(searchString)).ToList();
        }

        public async Task AddRecipe(Recipe recipe)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  INSERT INTO Recipes.Recipe (
                            [Name],
                            [Description],
                            AuthorId
                        )
                        OUTPUT Inserted.Id
                        VALUES (
                            @Name,
                            @Description,
                            @AuthorId
                        );";
                cmd.Parameters.AddWithValue("@Name", recipe.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@Description", recipe.Description ?? string.Empty);
                cmd.Parameters.AddWithValue("@AuthorId", recipe.AuthorId);

                await conn.OpenAsync();

                recipe.Id = (int)await cmd.ExecuteScalarAsync();

                conn.Close();
            }

            foreach (IngredientSection ingredientSection in recipe.IngredientSections)
                await AddIngredientSection(recipe.Id, ingredientSection);

            foreach (InstructionSection instructionSection in recipe.InstructionSections)
                await AddInstructionSection(recipe.Id, instructionSection);
        }

        public async Task DeleteRecipe(int recipeId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  UPDATE Recipes.Recipe
                        SET DeletedDate = SYSDATETIME()
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", recipeId);

                await conn.OpenAsync();

                int numberOfRowsDeleted = await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async Task PermanentlyDeleteRecipe(int recipeId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            foreach (int ingredientSectionId in await GetTopLevelIngredientSectionIdsByRecipeId(recipeId))
                await DeleteIngredientSection(recipeId, ingredientSectionId);

            foreach (int instructionSectionId in await GetTopLevelInstructionSectionIdsByRecipeId(recipeId))
                await DeleteInstructionSection(recipeId, instructionSectionId);

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Recipes.Recipe WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", recipeId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        #endregion

        #region ingredient sections

        public async Task<IngredientSection> GetIngredientSection(int recipeId, int sectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
	                        OrderId,
	                        [Name]
                        FROM Recipes.[Section]
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", sectionId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        string name = rdr["Name"].ToString();
                        IngredientSection ingredientSection =
                            new IngredientSection
                            {
                                Id = sectionId,
                                OrderId = orderId,
                                Name = name,
                                RecipeIngredients = await GetRecipeIngredientsBySection(recipeId, sectionId),
                                Children = await GetIngredientSectionsByParentSection(recipeId, sectionId),
                            };

                        return ingredientSection;
                    }
                }
            }

            return null;
        }

        public async Task AddIngredientSection(int recipeId, IngredientSection ingredientSection, int? parentIngredientSectionId = null)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            if (ingredientSection.OrderId == 0)
                ingredientSection.OrderId = NextRecipeSectionOrderId(recipeId, RecipeSectionType.Ingredient);

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  INSERT INTO Recipes.[Section] (
                            [RecipeId],
                            [ParentId],
	                        [OrderId],
	                        [Name],
	                        [Type]
                        )
                        OUTPUT Inserted.Id
                        VALUES (
                            @RecipeId,
                            @ParentId,
	                        @OrderId,
	                        @Name,
	                        @Type
                        );";
                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@ParentId", parentIngredientSectionId == null ? DBNull.Value : parentIngredientSectionId.Value);
                cmd.Parameters.AddWithValue("@OrderId", ingredientSection.OrderId);
                cmd.Parameters.AddWithValue("@Name", ingredientSection.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@Type", Enum.GetName(typeof(RecipeSectionType), RecipeSectionType.Ingredient));

                await conn.OpenAsync();

                ingredientSection.Id = (int)await cmd.ExecuteScalarAsync();

                conn.Close();
            }

            foreach (RecipeIngredient recipeIngredient in ingredientSection.RecipeIngredients)
                await AddRecipeIngredient(recipeId, ingredientSection.Id, recipeIngredient);

            foreach (IngredientSection childIngredientSection in ingredientSection.Children)
                await AddIngredientSection(recipeId, childIngredientSection, ingredientSection.Id);
        }

        public async Task ChangeIngredientSectionName(int recipeId, int ingredientSectionId, string newIngredientSectionName)
        {
            await ChangeSectionName(ingredientSectionId, newIngredientSectionName);
        }

        public async Task DeleteIngredientSection(int recipeId, int ingredientSectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<int> recipeIngredientIds = await GetRecipeIngredientIdsInSection(recipeId, ingredientSectionId);

            foreach (int recipeIngredientId in recipeIngredientIds)
                await DeleteRecipeIngredient(recipeId, ingredientSectionId, recipeIngredientId);

            List<int> childIngredientSectionIds = await GetChildIngredientSectionIds(recipeId, ingredientSectionId);

            foreach (int childIngredientSectionId in childIngredientSectionIds)
                await Task.Run(() => DeleteIngredientSection(recipeId, childIngredientSectionId));

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Recipes.Section WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", ingredientSectionId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        #endregion

        #region recipe ingredients

        public async Task<List<RecipeIngredient>> GetRecipeIngredients(int recipeId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                                Id,
                                Quantity,
                                OrderId,
                                IngredientId
                            FROM Recipes.RecipeIngredient
                            WHERE RecipeId = @RecipeId;";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        string quantity = rdr["Quantity"].ToString();
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        int ingredientId = Convert.ToInt32(rdr["IngredientId"]);

                        Ingredient ingredient = await GetIngredient(ingredientId);

                        recipeIngredients.Add(
                            new RecipeIngredient
                            {
                                Id = id,
                                Quantity = quantity,
                                OrderId = orderId,
                                Ingredient = ingredient,
                            });
                    }
                }
            }

            return recipeIngredients;
        }

        public async Task<List<RecipeIngredient>> GetRecipeIngredientsByIngredient(int ingredientId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                                Id,
                                RecipeId,
                                Quantity,
                                OrderId
                            FROM Recipes.RecipeIngredient
                            WHERE IngredientId = @IngredientId;";

                cmd.Parameters.AddWithValue("@IngredientId", ingredientId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        string quantity = rdr["Quantity"].ToString();
                        int orderId = Convert.ToInt32(rdr["OrderId"]);

                        Ingredient ingredient = await GetIngredient(ingredientId);

                        recipeIngredients.Add(
                            new RecipeIngredient
                            {
                                Id = id,
                                Quantity = quantity,
                                OrderId = orderId,
                                Ingredient = ingredient,
                            });
                    }
                }
            }

            return recipeIngredients;
        }

        public async Task<RecipeIngredient> GetRecipeIngredient(int recipeId, int recipeIngredientId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id,
                            Quantity,
                            OrderId,
                            IngredientId
                        FROM Recipes.RecipeIngredient
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", recipeIngredientId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        string quantity = rdr["Quantity"].ToString();
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        int ingredientId = Convert.ToInt32(rdr["IngredientId"]);

                        Ingredient ingredient = await GetIngredient(ingredientId);

                        return new RecipeIngredient
                        {
                            Id = id,
                            Quantity = quantity,
                            OrderId = orderId,
                            Ingredient = ingredient,
                        };
                    }
                }
            }

            return null;
        }

        public async Task AddRecipeIngredient(int recipeId, int sectionId, RecipeIngredient recipeIngredient)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            if (recipeIngredient.OrderId == 0)
                recipeIngredient.OrderId = NextRecipeIngredientItemOrderId(sectionId);

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  INSERT INTO Recipes.RecipeIngredient (
                            RecipeId,
                            SectionId,
                            Quantity,
                            OrderId,
                            IngredientId
                        )
                        OUTPUT Inserted.Id
                        VALUES (
                            @RecipeId,
                            @SectionId,
                            @Quantity,
                            @OrderId,
                            @IngredientId
                        );";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);
                cmd.Parameters.AddWithValue("@Quantity", (object)recipeIngredient.Quantity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OrderId", recipeIngredient.OrderId);
                cmd.Parameters.AddWithValue("@IngredientId", recipeIngredient.Ingredient.Id);

                await conn.OpenAsync();

                recipeIngredient.Id = (int)await cmd.ExecuteScalarAsync();

                conn.Close();
            }
        }

        public async Task ChangeRecipeIngredientQuantity(int recipeId, int sectionId, int recipeIngredientId, string newQuantity)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"UPDATE Recipes.RecipeIngredient
                        SET Quantity = @Quantity
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                cmd.Parameters.AddWithValue("@Id", recipeIngredientId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async Task ChangeRecipeIngredientIngredient(int recipeId, int sectionId, int recipeIngredientId, int newIngredientId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"UPDATE Recipes.RecipeIngredient
                        SET IngredientId = @IngredientId
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@IngredientId", newIngredientId);
                cmd.Parameters.AddWithValue("@Id", recipeIngredientId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async Task DeleteRecipeIngredient(int recipeId, int sectionId, int recipeIngredientId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Recipes.RecipeIngredient WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", recipeIngredientId);

                await conn.OpenAsync();

                int numberOfRowsDeleted = await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        #endregion

        #region instruction sections

        public async Task<InstructionSection> GetInstructionSection(int recipeId, int sectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
	                        OrderId,
	                        [Name]
                        FROM Recipes.[Section]
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", sectionId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        string name = rdr["Name"].ToString();
                        InstructionSection instructionSection =
                            new InstructionSection
                            {
                                Id = sectionId,
                                OrderId = orderId,
                                Name = name,
                                RecipeInstructions = await GetRecipeInstructionsBySection(recipeId, sectionId),
                                Children = await GetInstructionSectionsByParentSection(recipeId, sectionId),
                            };

                        return instructionSection;
                    }
                }
            }

            return null;
        }

        public async Task AddInstructionSection(int recipeId, InstructionSection instructionSection, int? parentInstructionSectionId = null)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            if (instructionSection.OrderId == 0)
                instructionSection.OrderId = NextRecipeSectionOrderId(recipeId, RecipeSectionType.Instruction);

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  INSERT INTO Recipes.[Section] (
                            [RecipeId],
                            [ParentId],
	                        [OrderId],
	                        [Name],
	                        [Type]
                        )
                        OUTPUT Inserted.Id
                        VALUES (
                            @RecipeId,
                            @ParentId,
	                        @OrderId,
	                        @Name,
	                        @Type
                        );";
                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@ParentId", parentInstructionSectionId == null ? DBNull.Value : parentInstructionSectionId.Value);
                cmd.Parameters.AddWithValue("@OrderId", instructionSection.OrderId);
                cmd.Parameters.AddWithValue("@Name", instructionSection.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@Type", Enum.GetName(typeof(RecipeSectionType), RecipeSectionType.Instruction));

                await conn.OpenAsync();

                instructionSection.Id = (int)await cmd.ExecuteScalarAsync();

                conn.Close();
            }

            foreach (RecipeInstruction recipeInstruction in instructionSection.RecipeInstructions)
                await AddRecipeInstruction(recipeId, instructionSection.Id, recipeInstruction);

            foreach (InstructionSection childInstructionSection in instructionSection.Children)
                await AddInstructionSection(recipeId, childInstructionSection, instructionSection.Id);
        }

        public async Task ChangeInstructionSectionName(int recipeId, int ingredientSectionId, string newInstructionSectionName)
        {
            await ChangeSectionName(ingredientSectionId, newInstructionSectionName);
        }

        public async Task DeleteInstructionSection(int recipeId, int instructionSectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            InstructionSection instructionSection = await GetInstructionSection(recipeId, instructionSectionId);

            foreach (InstructionSection childInstructionSection in instructionSection.Children)
                await DeleteInstructionSection(recipeId, childInstructionSection.Id);

            foreach (RecipeInstruction recipeInstruction in instructionSection.RecipeInstructions)
                await Task.Run(() => DeleteRecipeInstruction(recipeId, instructionSectionId, recipeInstruction.Id));

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Recipes.Section WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", instructionSectionId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        #endregion

        #region recipe instructions

        public async Task AddRecipeInstruction(int recipeId, int sectionId, RecipeInstruction recipeInstruction)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            if (recipeInstruction.OrderId == 0)
                recipeInstruction.OrderId = NextRecipeInstructionItemOrderId(sectionId);

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  INSERT INTO Recipes.RecipeInstruction (
                            RecipeId,
                            SectionId,
                            OrderId,
                            [Description]
                        )
                        OUTPUT Inserted.Id
                        VALUES (
                            @RecipeId,
                            @SectionId,
                            @OrderId,
                            @Description
                        );";
                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);
                cmd.Parameters.AddWithValue("@OrderId", recipeInstruction.OrderId);
                cmd.Parameters.AddWithValue("@Description", recipeInstruction.Description ?? string.Empty);

                await conn.OpenAsync();

                recipeInstruction.Id = (int)await cmd.ExecuteScalarAsync();

                conn.Close();
            }
        }

        public async Task ChangeRecipeInstructionDescription(int recipeId, int sectionId, int recipeInstructionId, string newDescription)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"UPDATE Recipes.RecipeInstruction
                        SET [Description] = @Description
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Description", newDescription);
                cmd.Parameters.AddWithValue("@Id", recipeInstructionId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async Task DeleteRecipeInstruction(int recipeId, int sectionId, int recipeInstructionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Recipes.RecipeInstruction WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", recipeInstructionId);

                await conn.OpenAsync();

                int numberOfRowsDeleted = await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        #endregion

        #region private

        readonly string _foodForUsAllConnectionString;

        async Task<List<Recipe>> GetRecipes(string whereClause)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<Recipe> recipes = new List<Recipe>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id,
                            [Name],
                            [Description],
                            AuthorId
                        FROM Recipes.Recipe
                        WHERE DeletedDate IS NULL ";
                cmd.CommandText += whereClause;
                cmd.CommandText += ";";

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        string name = rdr["Name"].ToString();
                        string description = rdr["Description"].ToString();
                        Guid authorId = new Guid((string)rdr["AuthorId"]);
                        recipes.Add(
                            new Recipe
                            {
                                Id = id,
                                Name = name,
                                Description = description,
                                AuthorId = authorId,
                                IngredientSections = await GetIngredientSectionsByRecipeId(id),
                                InstructionSections = await GetInstructionSectionsByRecipeId(id),
                            });
                    }
                }
            }

            return recipes;
        }

        async Task ChangeSectionName(int sectionId, string newSectionName)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  UPDATE Recipes.[Section]
                        SET [Name] = @Name
                        WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Name", newSectionName);
                cmd.Parameters.AddWithValue("@Id", sectionId);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        async Task<List<RecipeIngredient>> GetRecipeIngredientsBySection(int recipeId, int sectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                                Id,
                                Quantity,
                                OrderId,
                                IngredientId
                            FROM Recipes.RecipeIngredient
                            WHERE RecipeId = @RecipeId
                            AND SectionId = @SectionId;";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        string quantity = rdr["Quantity"].ToString();
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        int ingredientId = Convert.ToInt32(rdr["IngredientId"]);

                        Ingredient ingredient = await GetIngredient(ingredientId);

                        recipeIngredients.Add(
                            new RecipeIngredient
                            {
                                Id = id,
                                Quantity = quantity,
                                OrderId = orderId,
                                Ingredient = ingredient,
                            });
                    }
                }
            }

            return recipeIngredients;
        }

        async Task<List<RecipeInstruction>> GetRecipeInstructionsBySection(int recipeId, int sectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<RecipeInstruction> recipeInstructions = new List<RecipeInstruction>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                                Id,
                                OrderId,
                                [Description]
                            FROM Recipes.RecipeInstruction
                            WHERE RecipeId = @RecipeId
                            AND SectionId = @SectionId;";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        string description = rdr["Description"].ToString();

                        recipeInstructions.Add(
                            new RecipeInstruction
                            {
                                Id = id,
                                OrderId = orderId,
                                Description = description,
                            });
                    }
                }
            }

            return recipeInstructions;
        }

        async Task<List<int>> GetTopLevelIngredientSectionIdsByRecipeId(int recipeId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<int> ingredientSectionIds = new List<int>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id
                        FROM Recipes.[Section]
                        WHERE RecipeId = @RecipeId
                        AND ParentId IS NULL
                        AND [Type] = 'Ingredient';";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        ingredientSectionIds.Add(id);
                    }
                }
            }

            return ingredientSectionIds;
        }

        async Task<List<IngredientSection>> GetIngredientSectionsByRecipeId(int recipeId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<IngredientSection> ingredientSections = new List<IngredientSection>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id,
	                        OrderId,
	                        [Name]
                        FROM Recipes.[Section]
                        WHERE RecipeId = @RecipeId
                        AND ParentId IS NULL
                        AND [Type] = 'Ingredient';";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        string name = rdr["Name"].ToString();

                        ingredientSections.Add(
                            new IngredientSection
                            {
                                Id = id,
                                OrderId = orderId,
                                Name = name,
                                RecipeIngredients = await GetRecipeIngredientsBySection(recipeId, id),
                                Children = await GetIngredientSectionsByParentSection(recipeId, id),
                            }
                        );
                    }
                }
            }

            return ingredientSections;
        }

        async Task<List<int>> GetRecipeIngredientIdsInSection(int recipeId, int sectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<int> childIngredientSectionIds = new List<int>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id
                        FROM Recipes.[RecipeIngredient]
                        WHERE RecipeId = @RecipeId
                        AND SectionId = @SectionId;";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@SectionId", sectionId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        childIngredientSectionIds.Add(id);
                    }
                }
            }

            return childIngredientSectionIds;
        }

        async Task<List<int>> GetChildIngredientSectionIds(int recipeId, int parentSectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<int> childIngredientSectionIds = new List<int>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id
                        FROM Recipes.[Section]
                        WHERE RecipeId = @RecipeId
                        AND ParentId IS NOT NULL
                        AND ParentId = @ParentId
                        AND [Type] = 'Ingredient';";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@ParentId", parentSectionId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        childIngredientSectionIds.Add(id);
                    }
                }
            }

            return childIngredientSectionIds;
        }

        async Task<List<IngredientSection>> GetIngredientSectionsByParentSection(int recipeId, int parentSectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<IngredientSection> ingredientSections = new List<IngredientSection>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id,
	                        OrderId,
	                        [Name]
                        FROM Recipes.[Section]
                        WHERE RecipeId = @RecipeId
                        AND ParentId IS NOT NULL
                        AND ParentId = @ParentId
                        AND [Type] = 'Ingredient';";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@ParentId", parentSectionId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        string name = rdr["Name"].ToString();

                        ingredientSections.Add(
                            new IngredientSection
                            {
                                Id = id,
                                OrderId = orderId,
                                Name = name,
                                RecipeIngredients = await GetRecipeIngredientsBySection(recipeId, id),
                                Children = await GetIngredientSectionsByParentSection(recipeId, id),
                            }
                        );
                    }
                }
            }

            return ingredientSections;
        }

        async Task<List<int>> GetTopLevelInstructionSectionIdsByRecipeId(int recipeId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<int> instructionSectionIds = new List<int>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id
                        FROM Recipes.[Section]
                        WHERE RecipeId = @RecipeId
                        AND ParentId IS NULL
                        AND [Type] = 'Instruction';";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        instructionSectionIds.Add(id);
                    }
                }
            }

            return instructionSectionIds;
        }

        async Task<List<InstructionSection>> GetInstructionSectionsByRecipeId(int recipeId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<InstructionSection> instructionSections = new List<InstructionSection>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id,
	                        OrderId,
	                        [Name]
                        FROM Recipes.[Section]
                        WHERE RecipeId = @RecipeId
                        AND ParentId IS NULL
                        AND [Type] = 'Instruction';";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        string name = rdr["Name"].ToString();

                        instructionSections.Add(
                            new InstructionSection
                            {
                                Id = id,
                                OrderId = orderId,
                                Name = name,
                                RecipeInstructions = await GetRecipeInstructionsBySection(recipeId, id),
                                Children = await GetInstructionSectionsByParentSection(recipeId, id),
                            }
                        );
                    }
                }
            }

            return instructionSections;
        }

        async Task<List<InstructionSection>> GetInstructionSectionsByParentSection(int recipeId, int parentSectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            List<InstructionSection> instructionSections = new List<InstructionSection>();

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
                            Id,
	                        OrderId,
	                        [Name]
                        FROM Recipes.[Section]
                        WHERE RecipeId = @RecipeId
                        AND ParentId IS NOT NULL
                        AND ParentId = @ParentId
                        AND [Type] = 'Instruction';";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@ParentId", parentSectionId);

                await conn.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        int id = Convert.ToInt32(rdr["Id"]);
                        int orderId = Convert.ToInt32(rdr["OrderId"]);
                        string name = rdr["Name"].ToString();

                        instructionSections.Add(
                            new InstructionSection
                            {
                                Id = id,
                                OrderId = orderId,
                                Name = name,
                                RecipeInstructions = await GetRecipeInstructionsBySection(recipeId, id),
                                Children = await GetInstructionSectionsByParentSection(recipeId, id),
                            }
                        );
                    }
                }
            }

            return instructionSections;
        }

        int NextRecipeSectionOrderId(int recipeId, RecipeSectionType sectionType)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
	                        MAX(OrderId) OrderId
                        FROM Recipes.Section
                        WHERE RecipeId = @RecipeId
                        AND [Type] = @Type;";

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);
                cmd.Parameters.AddWithValue("@Type", Enum.GetName(typeof(RecipeSectionType), sectionType));

                conn.Open();

                object result = cmd.ExecuteScalar();
                if (result.GetType() != typeof(DBNull))
                    return Convert.ToInt32(result) + 1;
                else
                    return 1;
            }
        }

        int NextRecipeIngredientItemOrderId(int sectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
	                        MAX(OrderId) OrderId
                        FROM Recipes.RecipeIngredient
                        WHERE SectionId = @SectionId;";

                cmd.Parameters.AddWithValue("@SectionId", sectionId);

                conn.Open();

                object result = cmd.ExecuteScalar();
                if (result.GetType() != typeof(DBNull))
                    return Convert.ToInt32(result) + 1;
                else
                    return 1;
            }
        }

        int NextRecipeInstructionItemOrderId(int sectionId)
        {
            if (_foodForUsAllConnectionString == null)
                throw new ArgumentException("Unable to locate the FoodForUsAllConnectionString withing the configuration file.");

            using (var conn = new SqlConnection(_foodForUsAllConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  SELECT
	                        MAX(OrderId) OrderId
                        FROM Recipes.RecipeInstruction
                        WHERE SectionId = @SectionId;";

                cmd.Parameters.AddWithValue("@SectionId", sectionId);

                conn.Open();

                object result = cmd.ExecuteScalar();
                if (result.GetType() != typeof(DBNull))
                    return Convert.ToInt32(result) + 1;
                else
                    return 1;
            }
        }

        #endregion
    }
}
