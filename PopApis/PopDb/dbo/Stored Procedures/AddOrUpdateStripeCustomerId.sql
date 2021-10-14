CREATE PROCEDURE [dbo].[AddOrUpdateStripeCustomerId]
(
	@CustomerId INT = NULL,
	@StripeCustomerId NVARCHAR(255) = NULL
)
AS
BEGIN
	IF @CustomerId IS NOT NULL AND @StripeCustomerId IS NOT NULL
	BEGIN
		UPDATE [dbo].[Customer]
		   SET [StripeCustomerId] = @StripeCustomerId
		 WHERE
			Id = @CustomerId
	END
   END
GO
