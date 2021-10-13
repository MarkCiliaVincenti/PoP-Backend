CREATE TABLE [dbo].[Customer] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Email]            NVARCHAR (50)  NULL,
    [StripeCustomerId] NVARCHAR (255) NULL,
    [Created]          DATETIME       CONSTRAINT [DF_Customer_Timestamp] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC)
);

