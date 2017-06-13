CREATE TABLE [onsite].[LocationReport] (
    [LocationReportID]    INT            IDENTITY (1, 1) NOT NULL,
    [LocationID]          INT            NOT NULL,
    [ProductionID]        INT            NOT NULL,
    [UnitID]              SMALLINT       NOT NULL,
    [AspUserID]           NVARCHAR (128) NOT NULL,
    [SiteStart]           TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [SiteEnd]             TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [ProductionStart]     TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [CateringArrival]     TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [CateringEnd]         TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [PoliceArrival]       TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [PoliceShift]         TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [PoliceEnd]           TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [CleaningCrewArrival] TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [CleaningCrewEnd]     TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [ProductionLunch]     TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [ProductionLunchEnd]  TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [DinnerBreak]         TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [DinnerBreakEnd]      TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [ProductionEnd]       TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [LastVehicle]         TIME (0)       DEFAULT ('00.00.00') NOT NULL,
    [Date]                DATETIME       NOT NULL,
    [DateCreated]         DATETIME       DEFAULT (getdate()) NOT NULL,
    [Comments]            NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([LocationReportID] ASC),
    CONSTRAINT [FK_LocationReport_Location] FOREIGN KEY ([LocationID]) REFERENCES [onsite].[Location] ([LocationID]),
    CONSTRAINT [FK_LocationReport_Prodiction] FOREIGN KEY ([ProductionID]) REFERENCES [dbo].[Production] ([ProductionID]),
    CONSTRAINT [FK_LocationReport_Unit] FOREIGN KEY ([UnitID]) REFERENCES [onsite].[Unit] ([UnitID]),
    CONSTRAINT [FK_LocationReport_User] FOREIGN KEY ([AspUserID]) REFERENCES [dbo].[UserInfo] ([AspUserID])
);


GO
CREATE NONCLUSTERED INDEX [IX_LocationReport_Date]
    ON [onsite].[LocationReport]([Date] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationReport_LocationID]
    ON [onsite].[LocationReport]([LocationID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationReport_ProductionID]
    ON [onsite].[LocationReport]([ProductionID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationReport_UnitID]
    ON [onsite].[LocationReport]([UnitID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationReport_UserID]
    ON [onsite].[LocationReport]([AspUserID] ASC);

