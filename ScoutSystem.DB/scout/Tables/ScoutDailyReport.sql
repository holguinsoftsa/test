CREATE TABLE [scout].[ScoutDailyReport] (
    [EntryID]          INT            IDENTITY (1, 1) NOT NULL,
    [UserID]           NVARCHAR (128) NOT NULL,
    [ProductionID]     INT            NOT NULL,
    [Date]             DATETIME       NOT NULL,
    [DateCreated]      DATETIME       DEFAULT (getdate()) NOT NULL,
    [Task]             NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [CarNumber]        INT            DEFAULT ((0)) NOT NULL,
    [LocationsVisited] INT            DEFAULT ((0)) NOT NULL,
    [LocationOptions]  INT            DEFAULT ((0)) NOT NULL,
    [StartTime]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [EndTime]          DATETIME       DEFAULT (getdate()) NOT NULL,
    [IsDraft]          BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([EntryID] ASC),
    CONSTRAINT [FK_DailyEntry_Production] FOREIGN KEY ([ProductionID]) REFERENCES [dbo].[Production] ([ProductionID]),
    CONSTRAINT [FK_DailyEntry_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

