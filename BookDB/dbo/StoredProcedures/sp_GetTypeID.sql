﻿CREATE PROCEDURE [dbo].[sp_GetTypeID]
	
	@Type nvarchar(50)
AS



BEGIN
IF(@Type IS NULL)
SELECT TypeID FROM Type AS t WHERE UPPER(t.Type) IS NULL
ELSE
SELECT TypeID FROM Type AS t WHERE UPPER(t.Type) = UPPER(@Type)
END
