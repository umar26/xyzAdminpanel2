USE [AdminPanel]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 03/20/2018 21:23:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubCategory]    Script Date: 03/20/2018 21:23:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ParentCategory] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Images]    Script Date: 03/20/2018 21:23:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImagePath] [nvarchar](max) NULL,
	[ParentSubCategory] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Content]    Script Date: 03/20/2018 21:23:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Content](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TextContent] [nvarchar](max) NULL,
	[ParentSubCategory] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category_SubCategory]    Script Date: 03/20/2018 21:23:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category_SubCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Category] [int] NOT NULL,
	[SubCategory] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK__Category___Categ__1CF15040]    Script Date: 03/20/2018 21:23:37 ******/
ALTER TABLE [dbo].[Category_SubCategory]  WITH CHECK ADD FOREIGN KEY([Category])
REFERENCES [dbo].[Category] ([Id])
GO
/****** Object:  ForeignKey [FK__Category___SubCa__1DE57479]    Script Date: 03/20/2018 21:23:37 ******/
ALTER TABLE [dbo].[Category_SubCategory]  WITH CHECK ADD FOREIGN KEY([SubCategory])
REFERENCES [dbo].[SubCategory] ([Id])
GO
/****** Object:  ForeignKey [FK__Content__ParentS__1367E606]    Script Date: 03/20/2018 21:23:37 ******/
ALTER TABLE [dbo].[Content]  WITH CHECK ADD FOREIGN KEY([ParentSubCategory])
REFERENCES [dbo].[SubCategory] ([Id])
GO
/****** Object:  ForeignKey [FK__Images__ParentSu__182C9B23]    Script Date: 03/20/2018 21:23:37 ******/
ALTER TABLE [dbo].[Images]  WITH CHECK ADD FOREIGN KEY([ParentSubCategory])
REFERENCES [dbo].[SubCategory] ([Id])
GO
/****** Object:  ForeignKey [FK__SubCatego__Paren__0EA330E9]    Script Date: 03/20/2018 21:23:37 ******/
ALTER TABLE [dbo].[SubCategory]  WITH CHECK ADD FOREIGN KEY([ParentCategory])
REFERENCES [dbo].[Category] ([Id])
GO
