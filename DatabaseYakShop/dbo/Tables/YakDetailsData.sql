CREATE TABLE [dbo].[YakDetailsData]
(
	[Id]			INT		IDENTITY (1, 1) NOT NULL,
	[Name]			VARCHAR (50) NOT NULL,
	[Age]			INT NOT NULL,
	[Sex]			VARCHAR (5) NOT NULL,
	[ageLastShaved]	FLOAT NOT NULL,
	CONSTRAINT [PK_dbo.YakDetailsData] PRIMARY KEY CLUSTERED ([Id] ASC)
);
