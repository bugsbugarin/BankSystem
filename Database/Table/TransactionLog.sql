USE [Bank]
GO

/****** Object:  Table [dbo].[TransactionLog]    Script Date: 05/09/2018 3:38:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TransactionLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AccountId] [bigint] NULL,
	[TransactionType] [int] NULL,
	[Amount] [decimal](18, 0) NULL,
	[DestinationAccountId] [bigint] NULL,
	[TransactionDate] [datetime] NULL
) ON [PRIMARY]
GO