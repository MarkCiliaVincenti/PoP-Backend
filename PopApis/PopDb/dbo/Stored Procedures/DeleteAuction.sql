


CREATE PROCEDURE [dbo].[DeleteAuction]
(
	@AuctionId INT = NULL
)
AS
BEGIN
   
   DELETE FROM [dbo].[Auction]
	WHERE 
	Id = @AuctionId	
  
END
