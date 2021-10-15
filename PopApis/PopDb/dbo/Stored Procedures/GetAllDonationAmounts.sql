CREATE PROCEDURE [dbo].[GetAllDonationAmounts]
(
    -- Add the parameters for the stored procedure here
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT ISNULL(SUM(Payment.Amount), 0) AS Amount
	FROM dbo.Auction INNER JOIN dbo.Payment WITH (NOLOCK)
	ON Payment.AuctionId = Auction.Id
	WHERE AuctionTypeId = 5 OR AuctionTypeId = 1
END
GO
