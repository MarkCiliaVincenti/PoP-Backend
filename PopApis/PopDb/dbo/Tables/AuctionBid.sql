CREATE TABLE [dbo].[AuctionBid] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [AuctionId] INT             NOT NULL,
    [Amount]    DECIMAL (18, 2) NOT NULL,
    [Email]     NVARCHAR (100)  NOT NULL,
    [Timestamp] DATETIME        CONSTRAINT [DF_AuctionBid1_Timestamp] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_AuctionBid] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__AuctionBid__Auction] FOREIGN KEY ([AuctionId]) REFERENCES [dbo].[Auction] ([Id])
);

