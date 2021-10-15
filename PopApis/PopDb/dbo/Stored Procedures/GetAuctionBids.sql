CREATE PROCEDURE [dbo].[GetAuctionBids]
(
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
)
AS
BEGIN
    SELECT * FROM dbo.AuctionBid WITH (NOLOCK)
	WHERE 
		(@StartDate IS NULL OR Timestamp > @StartDate) 
		AND (@EndDate IS NULL OR Timestamp > @EndDate) 
END
