CREATE TABLE [onsite].[ExternalService] (
    [ExternalServiceID] INT        IDENTITY (1, 1) NOT NULL,
    [Name]              NCHAR (50) NOT NULL,
    CONSTRAINT [PK_ExternalService_ExternalServiceID] PRIMARY KEY CLUSTERED ([ExternalServiceID] ASC)
);

