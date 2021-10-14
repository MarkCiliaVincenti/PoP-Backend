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
    SELECT SUM(Payment.Amount) AS Amount
	FROM dbo.Auction INNER JOIN dbo.Payment WITH (NOLOCK)
	ON Payment.AuctionId = Auction.Id
	WHERE AuctionTypeId = 5
END
GO

CREATE PROCEDURE [dbo].[GetAllDonationAmounts](@param1 int, @param2 int OUTPUT)
AS EXTERNAL NAME SomeAssembly.SomeType.SomeMethod
