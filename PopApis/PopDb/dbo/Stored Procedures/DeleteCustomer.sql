



CREATE PROCEDURE [dbo].[DeleteCustomer]
(
	@CustomerId INT = NULL
)
AS
BEGIN
   
   DELETE FROM [dbo].[Customer]
	WHERE 
	Id = @CustomerId	
  
END
