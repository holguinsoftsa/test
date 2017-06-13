CREATE TABLE [onsite].[Contact] (
    [ContactID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [Phone]     NVARCHAR (20) NOT NULL,
    PRIMARY KEY CLUSTERED ([ContactID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Contact_Name]
    ON [onsite].[Contact]([Name] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Contact_Phone]
    ON [onsite].[Contact]([Phone] ASC);

