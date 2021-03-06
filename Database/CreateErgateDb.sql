USE [master]
GO
/****** Object:  Database [Ergate]    Script Date: 28.10.2016 16:38:16 ******/
CREATE DATABASE [Ergate]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Ergate', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.LOCALDB\MSSQL\DATA\Ergate.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Ergate_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.LOCALDB\MSSQL\DATA\Ergate_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Ergate] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Ergate].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Ergate] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Ergate] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Ergate] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Ergate] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Ergate] SET ARITHABORT OFF 
GO
ALTER DATABASE [Ergate] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Ergate] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Ergate] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Ergate] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Ergate] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Ergate] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Ergate] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Ergate] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Ergate] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Ergate] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Ergate] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Ergate] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Ergate] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Ergate] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Ergate] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Ergate] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Ergate] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Ergate] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Ergate] SET  MULTI_USER 
GO
ALTER DATABASE [Ergate] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Ergate] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Ergate] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Ergate] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Ergate] SET DELAYED_DURABILITY = DISABLED 
GO
USE [Ergate]
GO
/****** Object:  Table [dbo].[Assignment]    Script Date: 28.10.2016 16:38:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assignment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployerId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Location] [nvarchar](250) NULL,
	[AssignmentDateTime] [datetime] NOT NULL,
	[IsCompleted] [bit] NOT NULL,
 CONSTRAINT [PK_Assignment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Companies]    Script Date: 28.10.2016 16:38:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Address] [nvarchar](250) NOT NULL,
	[MobilePhone] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NOT NULL,
	[WebsiteUrl] [nvarchar](250) NULL,
	[BusinessHoursStart] [nvarchar](50) NULL,
	[BusinessHoursEnd] [nvarchar](50) NULL,
	[FacebookUrl] [nvarchar](250) NULL,
	[TwitterUrl] [nvarchar](250) NULL,
	[LinkedinUrl] [nvarchar](250) NULL,
	[Rating] [int] NOT NULL,
	[OwnerId] [int] NOT NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompanyEmployees]    Script Date: 28.10.2016 16:38:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyEmployees](
	[CompanyId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
 CONSTRAINT [PK_CompanyEmployees] PRIMARY KEY NONCLUSTERED 
(
	[CompanyId] ASC,
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [UX_CompanyEmployees]    Script Date: 28.10.2016 16:38:16 ******/
CREATE UNIQUE CLUSTERED INDEX [UX_CompanyEmployees] ON [dbo].[CompanyEmployees]
(
	[CompanyId] ASC,
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyServices]    Script Date: 28.10.2016 16:38:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyServices](
	[CompanyId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
 CONSTRAINT [PK_CompanyServices] PRIMARY KEY NONCLUSTERED 
(
	[CompanyId] ASC,
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [UX_CompanyServices]    Script Date: 28.10.2016 16:38:16 ******/
CREATE UNIQUE CLUSTERED INDEX [UX_CompanyServices] ON [dbo].[CompanyServices]
(
	[CompanyId] ASC,
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 28.10.2016 16:38:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 28.10.2016 16:38:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](250) NOT NULL,
	[LastName] [nvarchar](250) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[BirthDate] [datetime] NULL,
	[MobilePhone] [nvarchar](250) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Assignment]  WITH CHECK ADD  CONSTRAINT [FK_Assignment_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Assignment] CHECK CONSTRAINT [FK_Assignment_Customers]
GO
ALTER TABLE [dbo].[Assignment]  WITH CHECK ADD  CONSTRAINT [FK_Assignment_Employees] FOREIGN KEY([EmployerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Assignment] CHECK CONSTRAINT [FK_Assignment_Employees]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Users] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Users]
GO
ALTER TABLE [dbo].[CompanyEmployees]  WITH CHECK ADD  CONSTRAINT [FK_CompanyEmployees_Companies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[CompanyEmployees] CHECK CONSTRAINT [FK_CompanyEmployees_Companies]
GO
ALTER TABLE [dbo].[CompanyEmployees]  WITH CHECK ADD  CONSTRAINT [FK_CompanyEmployees_Users] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[CompanyEmployees] CHECK CONSTRAINT [FK_CompanyEmployees_Users]
GO
ALTER TABLE [dbo].[CompanyServices]  WITH CHECK ADD  CONSTRAINT [FK_CompanyServices_Companies] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[CompanyServices] CHECK CONSTRAINT [FK_CompanyServices_Companies]
GO
ALTER TABLE [dbo].[CompanyServices]  WITH CHECK ADD  CONSTRAINT [FK_CompanyServices_Services] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Services] ([Id])
GO
ALTER TABLE [dbo].[CompanyServices] CHECK CONSTRAINT [FK_CompanyServices_Services]
GO
USE [master]
GO
ALTER DATABASE [Ergate] SET  READ_WRITE 
GO
