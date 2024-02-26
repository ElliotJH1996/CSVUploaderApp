CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Username] nvarchar(50) NOT NULL,
	[Password] nvarchar(12) NOT NULL,
	[Email] nvarchar(100) NOT NULL
)
