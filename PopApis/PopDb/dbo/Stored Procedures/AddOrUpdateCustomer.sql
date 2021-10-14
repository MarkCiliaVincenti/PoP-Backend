CREATE PROCEDURE [dbo].[AddOrUpdateCustomer]
(
	@CustomerId INT = NULL,
    @Email NVARCHAR(50),
	@StripeCustomerId NVARCHAR(255) = NULL,
	@Created DATETIME = NULL
)
AS
BEGIN
   IF EXISTS(Select TOP 1 Id FROM [dbo].[Customer]
        Where Id = @CustomerId
        OR Email = @Email)
    BEGIN
        UPDATE [dbo].[Customer]
		   SET [Email] = @Email
			  ,[StripeCustomerId] = @StripeCustomerId
			  ,[Created] =  GETUTCDATE()
		 WHERE
			Email = @Email
    END
    ELSE
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
    BEGIN
        SELECT TOP 1 Id FROM [dbo].[Customer]
        WHERE Email = @Email
    END
END
GO
