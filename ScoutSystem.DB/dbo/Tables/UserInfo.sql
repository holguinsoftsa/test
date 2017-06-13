CREATE TABLE [dbo].[UserInfo] (
    [AspUserID]    NVARCHAR (128) NOT NULL,
    [ProductionID] INT            NOT NULL,
    [FirstName]    NVARCHAR (50)  DEFAULT ('') NOT NULL,
    [LastName]     NCHAR (50)     DEFAULT ('') NOT NULL,
    [SSO]          INT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([AspUserID] ASC),
    CONSTRAINT [FK_UserInfo_Production] FOREIGN KEY ([ProductionID]) REFERENCES [dbo].[Production] ([ProductionID])
);

