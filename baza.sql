USE [LoxezSpZOO]
GO
ALTER TABLE [dbo].[KoszykElement] DROP CONSTRAINT [FK_KoszykElement_KoszykNaglowekId]
GO
ALTER TABLE [dbo].[KoszykNaglowek] DROP CONSTRAINT [DF_KoszykNaglowek_DataUtworzenia]
GO
ALTER TABLE [dbo].[KoszykElement] DROP CONSTRAINT [DF_KoszykElement_DataUtworzenia]
GO
ALTER TABLE [dbo].[Dokument] DROP CONSTRAINT [DF_Dokument_DataUtworzenia]
GO
/****** Object:  Table [dbo].[KoszykNaglowek]    Script Date: 23.05.2020 16:02:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KoszykNaglowek]') AND type in (N'U'))
DROP TABLE [dbo].[KoszykNaglowek]
GO
/****** Object:  Table [dbo].[KoszykElement]    Script Date: 23.05.2020 16:02:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KoszykElement]') AND type in (N'U'))
DROP TABLE [dbo].[KoszykElement]
GO
/****** Object:  Table [dbo].[Dokument]    Script Date: 23.05.2020 16:02:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Dokument]') AND type in (N'U'))
DROP TABLE [dbo].[Dokument]
GO
/****** Object:  Table [dbo].[Dokument]    Script Date: 23.05.2020 16:02:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dokument](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Identyfikator] [int] NOT NULL,
	[Typ] [varchar](max) NOT NULL,
	[Tytul] [varchar](max) NOT NULL,
	[NazwaPliku] [varchar](max) NOT NULL,
	[Tresc] [varbinary](max) NOT NULL,
	[DataUtworzenia] [date] NOT NULL,
 CONSTRAINT [PK_Dokument] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KoszykElement]    Script Date: 23.05.2020 16:02:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KoszykElement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KoszykNaglowekId] [int] NOT NULL,
	[Nazwa] [varchar](max) NOT NULL,
	[Ilosc] [money] NOT NULL,
	[Cena] [money] NOT NULL,
	[TwTowarTwId] [int] NULL,
	[DataUtworzenia] [date] NOT NULL,
 CONSTRAINT [PK_KoszykElement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KoszykNaglowek]    Script Date: 23.05.2020 16:02:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KoszykNaglowek](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sesja] [varchar](max) NOT NULL,
	[DataUtworzenia] [date] NOT NULL,
 CONSTRAINT [PK_KoszykNaglowek] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Dokument] ADD  CONSTRAINT [DF_Dokument_DataUtworzenia]  DEFAULT (getdate()) FOR [DataUtworzenia]
GO
ALTER TABLE [dbo].[KoszykElement] ADD  CONSTRAINT [DF_KoszykElement_DataUtworzenia]  DEFAULT (getdate()) FOR [DataUtworzenia]
GO
ALTER TABLE [dbo].[KoszykNaglowek] ADD  CONSTRAINT [DF_KoszykNaglowek_DataUtworzenia]  DEFAULT (getdate()) FOR [DataUtworzenia]
GO
ALTER TABLE [dbo].[KoszykElement]  WITH NOCHECK ADD  CONSTRAINT [FK_KoszykElement_KoszykNaglowekId] FOREIGN KEY([KoszykNaglowekId])
REFERENCES [dbo].[KoszykNaglowek] ([Id])
GO
ALTER TABLE [dbo].[KoszykElement] CHECK CONSTRAINT [FK_KoszykElement_KoszykNaglowekId]
GO
