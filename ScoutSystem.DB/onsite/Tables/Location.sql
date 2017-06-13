CREATE TABLE [onsite].[Location] (
    [LocationID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Address]    NVARCHAR (50) NOT NULL,
    [ContactID]  INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([LocationID] ASC),
    CONSTRAINT [FK_Location_Contact] FOREIGN KEY ([ContactID]) REFERENCES [onsite].[Contact] ([ContactID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Location_ContactID]
    ON [onsite].[Location]([ContactID] ASC);

