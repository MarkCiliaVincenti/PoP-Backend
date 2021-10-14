


CREATE PROCEDURE [dbo].[AddOrUpdateCustomer]
(
	@CustomerId INT = NULL,
    @Email NVARCHAR(50) = NULL,
	@StripeCustomerId NVARCHAR(255) = NULL,
	@Created DATETIME = NULL
)
AS
BEGIN

   IF @CustomerId IS NULL AND @Email IS NULL
   BEGIN
	   INSERT INTO [dbo].[Customer]
			   ([Email]
			   ,[StripeCustomerId]
			   ,[Created])
		 VALUES
			   (@Email
			   ,@StripeCustomerId
			   ,COALESCE(@Created, GETUTCDATE()))
	END
	ELSE
	BEGIN
		UPDATE [dbo].[Customer]
		   SET [Email] = @Email
			  ,[StripeCustomerId] = @StripeCustomerId
			  ,[Created] = @Created
		 WHERE
			Id = @CustomerId
	END
    BEGIN
        SELECT TOP 1 Id FROM [dbo].[Customer]
        WHERE Email = @Email
    END
   END
GO
