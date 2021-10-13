CREATE TABLE [dbo].[Event] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (500)  NULL,
    [Description] TEXT            NULL,
    [StartDate]   DATETIME        NOT NULL,
    [EndDate]     DATETIME        NULL,
    [Goal]        DECIMAL (18, 2) NULL,
    [Type]        NVARCHAR (255)  NULL,
    [BaseAmount]  DECIMAL (18, 2) NULL,
    [Venue]       NVARCHAR (255)  NULL,
    [IsActive]    BIT             NULL,
    [Created]     DATETIME        CONSTRAINT [DF_Event_Timestamp] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC)
);

