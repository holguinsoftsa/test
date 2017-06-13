CREATE TABLE [onsite].[IncidentReportMailingHistory] (
    [IncidentReportMailingHistoryID] INT            IDENTITY (1, 1) NOT NULL,
    [IncidentReportID]               INT            NOT NULL,
    [AspUserID]                      NVARCHAR (128) NOT NULL,
    PRIMARY KEY CLUSTERED ([IncidentReportMailingHistoryID] ASC),
    CONSTRAINT [FK_MailingHistory_IncidentReport] FOREIGN KEY ([IncidentReportID]) REFERENCES [onsite].[IncidentReport] ([IncidentReportID]) ON DELETE CASCADE,
    CONSTRAINT [FK_MailingHistory_User] FOREIGN KEY ([AspUserID]) REFERENCES [dbo].[UserInfo] ([AspUserID])
);


GO
CREATE NONCLUSTERED INDEX [IX_MailingHistory_IncidentReportID]
    ON [onsite].[IncidentReportMailingHistory]([IncidentReportID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MailingHistory_UserID]
    ON [onsite].[IncidentReportMailingHistory]([AspUserID] ASC);

