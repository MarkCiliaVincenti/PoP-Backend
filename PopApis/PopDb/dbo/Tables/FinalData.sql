CREATE TABLE [dbo].[FinalData] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [Email]               NVARCHAR (50)   NOT NULL,
    [Amount]              DECIMAL (18, 2) NULL,
    [Complete]            BIT             NULL,
    [StripeCustomerId]    NVARCHAR (255)  NULL,
    [StripeInvoiceItemId] NVARCHAR (500)  NULL,
    [StripeInvoiceId]     NVARCHAR (500)  NULL,
    [Created]             DATETIME        CONSTRAINT [DF_FinalData_Timestamp] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_FinalData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

