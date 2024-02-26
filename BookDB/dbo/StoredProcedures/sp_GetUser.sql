CREATE PROCEDURE [dbo].[sp_GetUser]
	@username nvarchar(50),
	@password nvarchar(12)
AS
BEGIN
	SELECT TOP 1 Username,[Password] FROM Users WHERE Username = @userName and [Password] = @password
END 