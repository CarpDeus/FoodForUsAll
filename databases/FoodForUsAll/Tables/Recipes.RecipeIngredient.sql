CREATE TABLE [Recipes].[RecipeIngredient](
	[Id] INT IDENTITY(1000000,1) NOT NULL,
	[RecipeId] INT NOT NULL,
	[SectionId] INT NOT NULL,
	[Quantity] [NVARCHAR](128) NULL,
	[OrderId] INT NOT NULL,
	[IngredientId] INT NOT NULL,
	[CreatedDate] [DATETIME2](7) NOT NULL DEFAULT SYSDATETIME(),
	[UpdatedDate] [DATETIME2](7) NOT NULL DEFAULT SYSDATETIME(),
 CONSTRAINT [PK_RecipesRecipeIngredient] PRIMARY KEY CLUSTERED 
(
	[RecipeId] ASC,
	[SectionId] ASC,
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]