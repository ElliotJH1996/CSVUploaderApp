CREATE PROCEDURE [dbo].[sp_InsertBook]

@Title			nvarchar(100),
@ISBN10			varchar(16),
@Pages			int,
@Type			nvarchar(50),
@Genre			nvarchar(50),
@TypeID			int,
@GenreID		int,
@Authors		nvarchar(150),
@Price			decimal(19,4),
@PublishDate	nvarchar(10)

AS



BEGIN
if(@GenreID = -1)
SET @GenreID = null
if(@Pages = 0)
set @Pages = null
INSERT INTO Books (Title,[ISBN-10],Pages,[Type],Genre,Authors,Price,[Publish Date])
VALUES (@Title,@ISBN10,@Pages,@TypeID,@GenreID,@Authors,@Price,CONVERT(datetime,@PublishDate,101))
END
