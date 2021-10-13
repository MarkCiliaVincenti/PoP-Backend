CREATE TABLE [dbo].[AuctionType] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Type] NVARCHAR (255) NULL,
    CONSTRAINT [PK_AuctionType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

