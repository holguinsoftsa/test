CREATE TABLE [onsite].[Attachment] (
    [AttachmentID]     INT           IDENTITY (1, 1) NOT NULL,
    [IncidentReportID] INT           NOT NULL,
    [File]             VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([AttachmentID] ASC),
    CONSTRAINT [FK_Attachment_IncidentReport] FOREIGN KEY ([IncidentReportID]) REFERENCES [onsite].[IncidentReport] ([IncidentReportID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Attachment_IncidentReportID]
    ON [onsite].[Attachment]([IncidentReportID] ASC);

