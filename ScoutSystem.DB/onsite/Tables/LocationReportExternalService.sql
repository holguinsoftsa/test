CREATE TABLE [onsite].[LocationReportExternalService] (
    [LocationReportExternalServiceID] INT      IDENTITY (1, 1) NOT NULL,
    [LocationReportID]                INT      NOT NULL,
    [ExternalServiceID]               INT      NOT NULL,
    [StartTime]                       TIME (0) DEFAULT ('00.00.00') NOT NULL,
    [EndTime]                         TIME (0) DEFAULT ('00.00.00') NOT NULL,
    CONSTRAINT [PK_LocalReportExternalService_LocalReportExternalServiceID] PRIMARY KEY CLUSTERED ([LocationReportExternalServiceID] ASC),
    CONSTRAINT [FK_LocalReportExternalService_ExternalService_ExternalServiceID] FOREIGN KEY ([ExternalServiceID]) REFERENCES [onsite].[ExternalService] ([ExternalServiceID]),
    CONSTRAINT [FK_LocationReportExternalService_LocationReport_LocationReportID] FOREIGN KEY ([LocationReportID]) REFERENCES [onsite].[LocationReport] ([LocationReportID])
);


GO
CREATE NONCLUSTERED INDEX [IX_LocalReportExternalService_ExternalServiceID]
    ON [onsite].[LocationReportExternalService]([ExternalServiceID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_LocationReportExternalService_LocationReportID]
    ON [onsite].[LocationReportExternalService]([LocationReportID] ASC);

