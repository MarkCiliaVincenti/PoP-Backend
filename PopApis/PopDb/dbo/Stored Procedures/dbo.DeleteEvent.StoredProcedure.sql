/****** Object:  StoredProcedure [dbo].[DeleteEvent]    Script Date: 10/17/2021 6:19:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[DeleteEvent]
(
	@EventId INT = NULL
)
AS
BEGIN
   
   DELETE FROM [dbo].[Event]
	WHERE 
	Id = @EventId	
  
END
GO
