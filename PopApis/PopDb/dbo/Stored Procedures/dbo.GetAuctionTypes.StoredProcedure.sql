/****** Object:  StoredProcedure [dbo].[GetAuctionTypes]    Script Date: 10/17/2021 6:19:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAuctionTypes]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    SELECT * FROM dbo.AuctionType WITH (NOLOCK)
END
GO
