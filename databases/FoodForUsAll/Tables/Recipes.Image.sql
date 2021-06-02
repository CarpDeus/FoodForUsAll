CREATE TABLE [Recipes].[Image](
	[Id] INT IDENTITY(100000,1) NOT NULL,
	[RecipeId] INT NULL,
	[IsSampleImage] BIT NOT NULL DEFAULT(0),
	[AuthorId] UNIQUEIDENTIFIER NOT NULL,
	[IsPrimary] BIT NOT NULL,
	[IsApproved] BIT NOT NULL DEFAULT(0),
	[Name] [NVARCHAR](256) NULL,
	[Image] VARBINARY(MAX) NULL,
	[FilenameWithPath] [NVARCHAR](1024) NULL,
	[CreatedDate] [DATETIME2](7) NOT NULL DEFAULT SYSDATETIME(),
	[UpdatedDate] [DATETIME2](7) NOT NULL DEFAULT SYSDATETIME(),
	[DeletedDate] [DATETIME2](7) NULL,
 CONSTRAINT [PK_RecipesImage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]