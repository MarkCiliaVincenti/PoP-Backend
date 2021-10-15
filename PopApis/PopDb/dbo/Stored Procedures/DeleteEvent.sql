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
