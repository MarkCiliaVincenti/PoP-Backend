CREATE PROCEDURE [dbo].[DeleteAuctionBid]
(
    @AuctionBidId INT = NULL
)
AS
BEGIN
    DELETE FROM [dbo].[AuctionBid]
	WHERE
	Id = @AuctionBidId
END
GO