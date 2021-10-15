Create PROCEDURE [dbo].[GetAllAuctionIDs]
(
    -- Add the parameters for the stored procedure here
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
	@IsActive BIT = 1
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT Id FROM dbo.Auction WITH (NOLOCK)
	WHERE 
		(@StartDate IS NULL OR Created > @StartDate) 
		AND (@EndDate IS NULL OR Created < @EndDate) 
		AND (@IsActive IS NULL OR IsActive = @IsActive)
END
