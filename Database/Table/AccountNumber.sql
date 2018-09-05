USE [Bank]
GO

/****** Object:  Table [dbo].[AccountNumber]    Script Date: 05/09/2018 3:37:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccountNumber](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[LoginName] [varchar](50) NULL
) ON [PRIMARY]
GO


