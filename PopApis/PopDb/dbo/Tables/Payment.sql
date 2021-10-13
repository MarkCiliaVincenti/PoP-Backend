CREATE TABLE [dbo].[Payment] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [AuctionId]           INT            NULL,
    [CustomerId]          INT            NULL,
    [Complete]            BIT            NULL,
    [StripeInvoiceItemId] NVARCHAR (500) NULL,
    [StripeInvoiceId]     NVARCHAR (500) NULL,
    [Created]             DATETIME       CONSTRAINT [DF_Payment_Timestamp] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Payment_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id])
);

