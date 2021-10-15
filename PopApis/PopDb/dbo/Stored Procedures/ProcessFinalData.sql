CREATE PROCEDURE [dbo].[ProcessFinalData]
(
@StartDate DATETIME = NULL,
@EndDate DATETIME = NULL
)
AS
BEGIN
-- Get payments data
INSERT INTO [dbo].[FinalData]
([Email]
,[Amount]
,[Complete]
,[StripeCustomerId]
,[StripeInvoiceItemId]
,[StripeInvoiceId]
,[Created])
SELECT ab.Email, ab.Amount, p.Complete, p.CustomerId, p.StripeInvoiceId, p.StripeInvoiceItemId, p.Created
from dbo.AuctionBid ab 
left join dbo.Auction a ON a.Id = ab.AuctionId
left join dbo.Customer c ON c.Email = ab.Email
left join dbo.Payment p on p.AuctionId = a.Id
where a.AuctionTypeId = 2
END;
