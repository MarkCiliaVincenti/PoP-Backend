

CREATE PROCEDURE [dbo].[AddOrUpdateAuctionBid]
(
	@AuctionId INT,
    @Amount DECIMAL(18,2),
	@Email NVARCHAR(100),
	@Timestamp DATETIME = NULL
)
AS
BEGIN
   IF NOT EXISTS (
		SELECT TOP 1 1 
		FROM dbo.AuctionBid WITH (NOLOCK) 
		WHERE AuctionId = @AuctionId AND Email = @Email
	)
   BEGIN
		INSERT INTO [dbo].[AuctionBid]
				   ([AuctionId]
				   ,[Amount]
				   ,[Email]
				   ,[Timestamp])
			 VALUES
				   (@AuctionId
				   ,@Amount
				   ,@Email
				   ,COALESCE(@Timestamp, GETUTCDATE()))
	END
	ELSE
	BEGIN
	 -- Update the bid amount for an existing email for an auction
		UPDATE [dbo].[AuctionBid]
		 SET [Amount] = @Amount
		,[Timestamp] = COALESCE(@Timestamp, GETUTCDATE())
		WHERE 
			AuctionId = @AuctionId	AND Email = @Email
	END 
	BEGIN
	 -- Update the bid amount for for an auction
		UPDATE [dbo].[Auction]
		 SET [Amount] = @Amount
		,[Created] = COALESCE(@Timestamp, GETUTCDATE())
		WHERE 
			Id = @AuctionId
	END 
   END