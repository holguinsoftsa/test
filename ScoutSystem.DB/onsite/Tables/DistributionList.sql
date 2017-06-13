CREATE TABLE [onsite].[DistributionList] (
    [DistributionListID] INT            IDENTITY (1, 1) NOT NULL,
    [AspUserID]          NVARCHAR (128) NOT NULL,
    [Primary]            BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([DistributionListID] ASC),
    CONSTRAINT [FK_DistributionList_User] FOREIGN KEY ([AspUserID]) REFERENCES [dbo].[UserInfo] ([AspUserID])
);


GO
CREATE NONCLUSTERED INDEX [IX_DistributionList_UserID]
    ON [onsite].[DistributionList]([AspUserID] ASC);

