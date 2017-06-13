CREATE TABLE [onsite].[DistributionListProduction] (
    [DistributionListProductionID] INT IDENTITY (1, 1) NOT NULL,
    [DistributionListID]           INT NOT NULL,
    [ProductionID]                 INT NOT NULL,
    CONSTRAINT [PK_DistributionListProduction] PRIMARY KEY CLUSTERED ([DistributionListProductionID] ASC),
    CONSTRAINT [FK_DistributionListProduction_DistributionList] FOREIGN KEY ([DistributionListID]) REFERENCES [onsite].[DistributionList] ([DistributionListID]),
    CONSTRAINT [FK_DistributionListProduction_Production] FOREIGN KEY ([ProductionID]) REFERENCES [dbo].[Production] ([ProductionID])
);


GO
CREATE NONCLUSTERED INDEX [IX_DistributionListProduction_DistributionListID]
    ON [onsite].[DistributionListProduction]([DistributionListID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DistributionListProduction_ProductionID]
    ON [onsite].[DistributionListProduction]([ProductionID] ASC);

