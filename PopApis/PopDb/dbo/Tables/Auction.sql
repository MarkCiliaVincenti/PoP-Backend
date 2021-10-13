CREATE TABLE [dbo].[Auction] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [AuctionTypeId] INT             NULL,
    [Title]         NVARCHAR (500)  NOT NULL,
    [Description]   TEXT            NULL,
    [Restrictions]  TEXT            NULL,
    [IsActive]      BIT             NULL,
    [Amount]        DECIMAL (18, 2) NULL,
    [Created]       DATETIME        CONSTRAINT [DF_Auction_created] DEFAULT (getutcdate()) NOT NULL,
    [ImageUrl]      NVARCHAR (2048) NULL,
    CONSTRAINT [PK_Auction] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__Auction__AuctionType] FOREIGN KEY ([AuctionTypeId]) REFERENCES [dbo].[AuctionType] ([Id])
);

