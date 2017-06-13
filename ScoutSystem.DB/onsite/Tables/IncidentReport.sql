CREATE TABLE [onsite].[IncidentReport] (
    [IncidentReportID] INT      IDENTITY (1, 1) NOT NULL,
    [LocationReportID] INT      NOT NULL,
    [CategoryID]       INT      NOT NULL,
    [Date]             DATETIME NOT NULL,
    [DateCreated]      DATETIME DEFAULT (getdate()) NOT NULL,
    [Notes]            TEXT     DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([IncidentReportID] ASC),
    CONSTRAINT [FK_IncidentReport_CategoryID] FOREIGN KEY ([CategoryID]) REFERENCES [onsite].[Category] ([CategoryID]),
    CONSTRAINT [FK_IncidentReport_LocationReport] FOREIGN KEY ([LocationReportID]) REFERENCES [onsite].[LocationReport] ([LocationReportID])
);


GO
CREATE NONCLUSTERED INDEX [IX_IncidentReport_LocationReportID]
    ON [onsite].[IncidentReport]([LocationReportID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Table_Category]
    ON [onsite].[IncidentReport]([CategoryID] ASC);

