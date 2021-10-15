CREATE TABLE [dbo].[Auction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuctionTypeId] [int] NULL,
	[Title] [nvarchar](500) NOT NULL,
	[Description] [text] NULL,
	[Restrictions] [text] NULL,
	[IsActive] [bit] NULL,
	[Amount] [decimal](18, 2) NULL,
	[Created] [datetime] NOT NULL,
	[ImageUrl] [nvarchar](2048) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Auction] ADD  CONSTRAINT [PK_Auction] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Auction] ADD  CONSTRAINT [DF_Auction_created]  DEFAULT (getutcdate()) FOR [Created]
GO
ALTER TABLE [dbo].[Auction]  WITH CHECK ADD  CONSTRAINT [FK__Auction__AuctionType] FOREIGN KEY([AuctionTypeId])
REFERENCES [dbo].[AuctionType] ([Id])
GO
ALTER TABLE [dbo].[Auction] CHECK CONSTRAINT [FK__Auction__AuctionType]
GO
INSERT INTO [dbo].[Auction]
			   ([AuctionTypeId]
			   ,[Title]
			   ,[Description]
			   ,[Restrictions]
			   ,[IsActive]
			   ,[Amount]
			   ,[Created]
			   ,[ImageUrl])
		 VALUES
			   (5
			   ,'Dontation'
			   ,''
			   ,''
			   ,1
			   ,0
			   ,GETUTCDATE()
			   ,'')
