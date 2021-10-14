CREATE PROCEDURE [dbo].[GetPayments]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    SELECT
        Sub1.Id,
        Sub1.AuctionId,
        Sub1.CustomerId,
        Sub1.Complete,
        Sub1.StripeInvoiceItemId,
        Sub1.StripeInvoiceId,
        Sub1.Created,
        Sub1.Amount,
        Sub1.Description,
        Sub2.StripeCustomerId
    FROM
    (dbo.Payment AS Sub1
    INNER JOIN
    dbo.Customer AS Sub2 ON Sub1.CustomerId = Sub2.Id)
END
GO
