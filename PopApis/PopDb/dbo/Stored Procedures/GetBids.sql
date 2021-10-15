CREATE PROCEDURE [dbo].[GetBids]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    SELECT * FROM dbo.AuctionBid WITH (NOLOCK)
END
GO
