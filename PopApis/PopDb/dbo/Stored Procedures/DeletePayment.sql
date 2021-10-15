CREATE PROCEDURE [dbo].[DeletePayment]
(
    -- Add the parameters for the stored procedure here
	@PaymentId INT = NULL
)
AS
BEGIN
   DELETE FROM [dbo].[Payment]
	WHERE 
	Id = @PaymentId	
END
