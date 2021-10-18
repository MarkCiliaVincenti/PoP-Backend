CREATE PROCEDURE [dbo].[DropAll]
(
	@EventId INT = NULL
)
AS
BEGIN

    DELETE FROM [dbo].[Event];
    DELETE FROM [dbo].[Payment];
    DELETE FROM [dbo].[Customer];
    DELETE FROM [dbo].[Auction];
    DELETE FROM [dbo].[AuctionBid];
    DELETE FROM [dbo].[AuctionType];
END
GO