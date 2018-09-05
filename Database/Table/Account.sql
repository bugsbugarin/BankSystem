USE [Bank]
GO

/****** Object:  Table [dbo].[Account]    Script Date: 05/09/2018 3:36:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Account](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LoginName] [varchar](20) NOT NULL,
	[AccountNumber] [varchar](10) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Balance] [decimal](18, 0) NOT NULL,
	[CreateDate] [datetime] NOT NULL
) ON [PRIMARY]
GO


