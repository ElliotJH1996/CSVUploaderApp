CREATE PROCEDURE [dbo].[sp_GetGenreID]
	
	@Genre nvarchar(50)
	
AS


BEGIN

IF(@Genre IS NULL)
SELECT GenreID FROM Genre AS g WHERE UPPER(g.Genre) IS NULL
ELSE
SELECT GenreID FROM Genre AS g WHERE UPPER(g.Genre) = UPPER(@Genre)
END