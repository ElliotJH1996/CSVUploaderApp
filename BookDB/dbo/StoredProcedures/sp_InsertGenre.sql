CREATE PROCEDURE [dbo].[sp_InsertGenre]
	
	@Genre nvarchar(50),
	@refID int

AS
DECLARE
@GenreID AS int

BEGIN
INSERT INTO Genre(GenreID,Genre) VALUES (@refID,@Genre)
END
IF(@Genre IS NULL)
BEGIN
SET @GenreID = (SELECT TOP 1 id FROM Genre AS g WHERE UPPER(g.Genre) IS NULL)
END
ELSE
BEGIN
SET @GenreID = (SELECT TOP 1 id FROM Genre AS g WHERE UPPER(g.Genre) = UPPER(@Genre))
END

	
RETURN @GenreID
