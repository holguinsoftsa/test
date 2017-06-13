CREATE TABLE [onsite].[DistributionListCategory] (
    [DistributionListCategoryID] INT IDENTITY (1, 1) NOT NULL,
    [DistributionListID]         INT NOT NULL,
    [CategoryID]                 INT NOT NULL,
    CONSTRAINT [PK_DistributionListCategory] PRIMARY KEY CLUSTERED ([DistributionListCategoryID] ASC),
    CONSTRAINT [FK_DistributionListCategory_Category] FOREIGN KEY ([CategoryID]) REFERENCES [onsite].[Category] ([CategoryID]),
    CONSTRAINT [FK_DistributionListCategory_DistributionList] FOREIGN KEY ([DistributionListID]) REFERENCES [onsite].[DistributionList] ([DistributionListID])
);


GO
CREATE NONCLUSTERED INDEX [IX_DistributionListCategory_CategoryID]
    ON [onsite].[DistributionListCategory]([CategoryID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DistributionListCategory_DistributionListID]
    ON [onsite].[DistributionListCategory]([DistributionListID] ASC);

