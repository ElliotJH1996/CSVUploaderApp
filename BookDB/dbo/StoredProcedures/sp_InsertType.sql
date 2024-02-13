CREATE PROCEDURE [dbo].[sp_InsertType]
	
	@Type nvarchar(50),
	@refID int


AS
DECLARE
	@TypeID AS int

BEGIN
INSERT INTO [Type] (TypeID,[Type]) VALUES (@refID,@Type)
SET @TypeID = (SELECT TOP 1 id FROM [Type] AS t WHERE t.Type = @Type)
END	
	
RETURN @TypeID
