CREATE PROCEDURE [dbo].[AddOrUpdateEvent]
(
	@EventId INT = NULL,
  @Name NVARCHAR(500),
	@Description TEXT = NULL,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@Goal DECIMAL(18,2) = NULL,
	@Type NVARCHAR(255) = NULL,
	@BaseAmount DECIMAL(18,2) = NULL,
	@Venue NVARCHAR(255) = NULL,
	@IsActive BIT = NULL,
	@Created DATETIME = NULL
)
AS
BEGIN
   
   IF @EventId IS NULL
   BEGIN
	   INSERT INTO [dbo].[Event]
			   ([Name]
			   ,[Description]
			   ,[StartDate]
			   ,[EndDate]
			   ,[Goal]
			   ,[Type]
			   ,[BaseAmount]
			   ,[Venue]
			   ,[IsActive]
			   ,[Created])
		 VALUES
			   (@Name
			   ,@Description
			   ,@StartDate
			   ,@EndDate
			   ,@Goal
			   ,@Type
			   ,@BaseAmount
			   ,@Venue
			   ,COALESCE(@IsActive, 1)
			   ,COALESCE(@Created, GETUTCDATE()))
	END
	ELSE
	BEGIN
		UPDATE [dbo].[Event]
		   SET [Name] = @Name
			  ,[Description] = @Description
			  ,[StartDate] = @StartDate
			  ,[EndDate] = @EndDate
			  ,[Goal] = @Goal
			  ,[Type] = @Type
			  ,[BaseAmount] = @BaseAmount
			  ,[Venue] = @Venue
			  ,[IsActive] = @IsActive
			  ,[Created] = @Created
		 WHERE 
			Id = @EventId	
	END   
   END
