﻿CREATE TABLE [dbo].[Genre]
(
	[Id] INT NOT NULL UNIQUE IDENTITY PRIMARY KEY, 
	[GenreID] INT NULL UNIQUE,
    [Genre] NVARCHAR(MAX) NULL
)
