/****** Object:  Table [dbo].[GetAuctionsResult]    Script Date: 10/17/2021 6:19:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GetAuctionsResult](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuctionTypeId] [int] NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Restrictions] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
 CONSTRAINT [PK_GetAuctionsResult] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
