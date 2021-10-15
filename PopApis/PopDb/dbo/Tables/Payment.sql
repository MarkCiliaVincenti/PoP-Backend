CREATE TABLE [dbo].[Payment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuctionId] [int] NULL,
	[CustomerId] [int] NULL,
	[Complete] [bit] NULL,
	[StripeInvoiceItemId] [nvarchar](500) NULL,
	[StripeInvoiceId] [nvarchar](500) NULL,
	[Created] [datetime] NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[Description] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payment] ADD  CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payment] ADD  CONSTRAINT [DF_Payment_Timestamp]  DEFAULT (getutcdate()) FOR [Created]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_Customer]
GO