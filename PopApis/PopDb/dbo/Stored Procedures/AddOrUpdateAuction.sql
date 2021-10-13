

CREATE PROCEDURE [dbo].[AddOrUpdateAuction]
(
	@AuctionId INT = NULL,
	@AuctionTypeId INT,
    @Title NVARCHAR(500),
	@Description TEXT = NULL,
	@Restrictions TEXT = NULL,
	@IsActive BIT = NULL,
	@Amount DECIMAL(18,2) = NULL,
	@Created DATETIME = NULL,
	@ImageUrl NVARCHAR(2048) = NULL
)
AS
BEGIN
   
   IF @AuctionId IS NULL
   BEGIN
	   INSERT INTO [dbo].[Auction]
			   ([AuctionTypeId]
			   ,[Title]
			   ,[Description]
			   ,[Restrictions]
			   ,[IsActive]
			   ,[Amount]
			   ,[Created]
			   ,[ImageUrl])
		 VALUES
			   (@AuctionTypeId
			   ,@Title
			   ,@Description
			   ,@Restrictions
			   ,COALESCE(@IsActive, 1)
			   ,@Amount
			   ,COALESCE(@Created, GETUTCDATE())
			   ,@ImageUrl)
	END
	ELSE
	BEGIN
		UPDATE [dbo].[Auction]
		   SET [AuctionTypeId] = @AuctionTypeId
			  ,[Title] = @Title
			  ,[Description] = @Description
			  ,[Restrictions] = @Restrictions
			  ,[IsActive] = @IsActive
			  ,[Amount] = @Amount
			  ,[Created] = @Created
			  ,[ImageUrl] = @ImageUrl
		 WHERE 
			Id = @AuctionId	
	END   
   END
