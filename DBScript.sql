USE [master]
GO
/****** Object:  Database [FinalProject]    Script Date: 5/28/2024 6:44:09 PM ******/
CREATE DATABASE [FinalProject]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FinalProject', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\FinalProject.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FinalProject_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\FinalProject_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [FinalProject] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FinalProject].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FinalProject] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FinalProject] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FinalProject] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FinalProject] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FinalProject] SET ARITHABORT OFF 
GO
ALTER DATABASE [FinalProject] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FinalProject] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FinalProject] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FinalProject] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FinalProject] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FinalProject] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FinalProject] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FinalProject] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FinalProject] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FinalProject] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FinalProject] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FinalProject] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FinalProject] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FinalProject] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FinalProject] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FinalProject] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FinalProject] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FinalProject] SET RECOVERY FULL 
GO
ALTER DATABASE [FinalProject] SET  MULTI_USER 
GO
ALTER DATABASE [FinalProject] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FinalProject] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FinalProject] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FinalProject] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FinalProject] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FinalProject] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'FinalProject', N'ON'
GO
ALTER DATABASE [FinalProject] SET QUERY_STORE = ON
GO
ALTER DATABASE [FinalProject] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [FinalProject]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[AccountNo] [nchar](14) NULL,
	[AccountType] [int] NOT NULL,
	[BranchCode] [nvarchar](50) NULL,
	[CurrentBalance] [decimal](18, 2) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LookUp]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookUp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Category] [nvarchar](50) NULL,
 CONSTRAINT [PK_LookUp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [int] NULL,
	[Date] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[DepositWithdrawData]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[DepositWithdrawData] as
SELECT Amount, LookUp.Name AS [Transaction Type],Format([Transaction].Date,'dd-MMM-yyyy hh:mm') as Date,CASE WHEN Status = '6' THEN 'Pending' WHEN Status = '7' THEN 'Approved' END AS [Application Status] FROM [Transaction] JOIN LookUp ON [Transaction].Type = LookUp.Id JOIN Account ON [Transaction].AccountId = Account.AccountId
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] NOT NULL,
	[CNIC] [nchar](13) NULL,
 CONSTRAINT [PK_Customer_1] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersonInfo]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonInfo](
	[Id] [int] NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Contact] [nchar](11) NULL,
	[Date of Birth] [datetime] NULL,
	[Address] [nvarchar](max) NULL,
 CONSTRAINT [PK_PersonInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[CustomerData]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[CustomerData] as 
Select FirstName+' '+ LastName as Name, CNIC,Format([Date of Birth],'dd-MMM-yyyy') as
[Date of Birth],Contact,[Address], AccountNo,LookUp.Name as [Account Type],BranchCode,
CurrentBalance,Format(CreatedDate,'dd-MMM-yyyy') as [Created Date] from Customer 
join PersonInfo on Customer.CustomerId = PersonInfo.Id join Account
on Customer.CustomerId = Account.CustomerId join LookUp on LookUp.Id = Account.AccountType;
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[FeedbackData]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[FeedbackData] as 
Select FirstName+ ' ' + LastName as CutomerName,CNIC,Description as Feedback from Feedback 
join Customer on Feedback.CustomerId = Customer.CustomerId join PersonInfo on 
Customer.CustomerId = PersonInfo.Id;
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branch](
	[BranchId] [int] IDENTITY(1,1) NOT NULL,
	[BranchName] [nvarchar](50) NULL,
	[BranchCode] [nvarchar](50) NULL,
	[Location] [nvarchar](max) NULL,
 CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED 
(
	[BranchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[BranchCode]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[BranchCode] as 
Select BranchCode from Branch;
GO
/****** Object:  Table [dbo].[Manager]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manager](
	[ManagerId] [int] NOT NULL,
	[CNIC] [nchar](13) NULL,
	[BranchId] [int] NULL,
 CONSTRAINT [PK_Manager] PRIMARY KEY CLUSTERED 
(
	[ManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserId]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserId](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
 CONSTRAINT [PK_UserId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ManagerData]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[ManagerData] as 
Select FirstName, LastName ,BranchCode,UserId.Email,Password, CNIC,Address,Contact,
Format([Date of Birth],'dd-MMM-yyyy') as [Date of Birth] from Manager join PersonInfo 
on Manager.ManagerId = PersonInfo.Id join UserId on UserId.Id = Manager.ManagerId 
join Branch on Branch.BranchId = Manager.BranchId;
GO
/****** Object:  View [dbo].[BranchData]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create View [dbo].[BranchData] as 
Select BranchName as [Branch Name], BranchCode as [Branch Code], Location as [Branch Location] from Branch
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[Admin Id] [int] NOT NULL,
	[CNIC] [nchar](13) NULL,
 CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED 
(
	[Admin Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ATMCard]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ATMCard](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NULL,
	[Card] [nvarchar](50) NULL,
	[CardType] [int] NOT NULL,
	[Date] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ATMCard] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChangeRequest]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChangeRequest](
	[RequestId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[RequestType] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[UpdatedInfo] [nvarchar](50) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ChangeRequest] PRIMARY KEY CLUSTERED 
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstallmentOfLoan]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstallmentOfLoan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LoanId] [int] NULL,
	[PayableAmount] [decimal](18, 2) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_InstallmentOfLoan] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loan]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loan](
	[LoanId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
	[Reason] [nvarchar](50) NULL,
	[Duration] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[LoanStatus] [int] NULL,
	[PaymentStatus] [int] NULL,
 CONSTRAINT [PK_Loan] PRIMARY KEY CLUSTERED 
(
	[LoanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransferAmount]    Script Date: 5/28/2024 6:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransferAmount](
	[TransferId] [int] IDENTITY(1,1) NOT NULL,
	[AccountFrom] [int] NOT NULL,
	[AccountTo] [int] NULL,
	[Amount] [decimal](18, 2) NULL,
	[Date] [datetime] NULL,
	[Detail] [nvarchar](255) NULL,
 CONSTRAINT [PK_TransferAmount] PRIMARY KEY CLUSTERED 
(
	[TransferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (1, 4, N'34513861087389', 1, N'MBL-234', CAST(3332.00 AS Decimal(18, 2)), CAST(N'2024-05-05T14:32:39.293' AS DateTime))
INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (2, 6, N'54597913755381', 1, N'MBL-123', CAST(1998.00 AS Decimal(18, 2)), CAST(N'2024-05-05T14:35:59.613' AS DateTime))
INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (3, 7, N'92334428980170', 1, N'MBL-767', CAST(23000.00 AS Decimal(18, 2)), CAST(N'2024-05-05T22:32:23.353' AS DateTime))
INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (4, 8, N'48655466688170', 1, N'MBL-123', CAST(4400.00 AS Decimal(18, 2)), CAST(N'2024-05-05T22:36:09.113' AS DateTime))
INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (5, 10, N'92768732892706', 1, N'MBL-767', CAST(10000.00 AS Decimal(18, 2)), CAST(N'2024-05-06T22:03:51.603' AS DateTime))
INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (6, 11, N'75965348242765', 1, N'MBL-767', CAST(7199.47 AS Decimal(18, 2)), CAST(N'2024-05-07T10:37:21.650' AS DateTime))
INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (7, 24, N'79203750472144', 1, N'MBL-123', CAST(441000.90 AS Decimal(18, 2)), CAST(N'2024-05-10T03:19:27.753' AS DateTime))
INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (8, 25, N'12479506182707', 1, N'MBL-443', CAST(0.00 AS Decimal(18, 2)), CAST(N'2024-05-10T11:02:23.363' AS DateTime))
INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (9, 28, N'76892095253286', 2, N'MBL-345', CAST(4498076.00 AS Decimal(18, 2)), CAST(N'2024-05-21T10:30:19.397' AS DateTime))
INSERT [dbo].[Account] ([AccountId], [CustomerId], [AccountNo], [AccountType], [BranchCode], [CurrentBalance], [CreatedDate]) VALUES (10, 31, N'57450953879137', 1, N'MBL-343', CAST(23000.00 AS Decimal(18, 2)), CAST(N'2024-05-28T14:37:03.613' AS DateTime))
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
INSERT [dbo].[Admin] ([Admin Id], [CNIC]) VALUES (3, N'3610490507387')
GO
SET IDENTITY_INSERT [dbo].[ATMCard] ON 

INSERT [dbo].[ATMCard] ([Id], [AccountId], [Card], [CardType], [Date], [Status]) VALUES (1, 6, N'Debit Card', 11, CAST(N'2024-05-08T13:04:52.397' AS DateTime), 5)
INSERT [dbo].[ATMCard] ([Id], [AccountId], [Card], [CardType], [Date], [Status]) VALUES (2, 8, N'Debit Card', 10, CAST(N'2024-05-10T11:04:54.247' AS DateTime), 6)
SET IDENTITY_INSERT [dbo].[ATMCard] OFF
GO
SET IDENTITY_INSERT [dbo].[Branch] ON 

INSERT [dbo].[Branch] ([BranchId], [BranchName], [BranchCode], [Location]) VALUES (1, N'MBL UET ', N'MBL-123', N'UET Lahore')
INSERT [dbo].[Branch] ([BranchId], [BranchName], [BranchCode], [Location]) VALUES (2, N'MBl Khanewal', N'MBL-676', N'Khanewal')
INSERT [dbo].[Branch] ([BranchId], [BranchName], [BranchCode], [Location]) VALUES (3, N'MBL Sheikhupra', N'MBL-443', N'Sheikhupra')
INSERT [dbo].[Branch] ([BranchId], [BranchName], [BranchCode], [Location]) VALUES (4, N'MBL IslamPura', N'MBL-234', N'IslamPura')
INSERT [dbo].[Branch] ([BranchId], [BranchName], [BranchCode], [Location]) VALUES (5, N'MBL Shialkot', N'MBL-232', N'Shialkot')
INSERT [dbo].[Branch] ([BranchId], [BranchName], [BranchCode], [Location]) VALUES (6, N'MBL Multan', N'MBL-345', N'Multan City')
INSERT [dbo].[Branch] ([BranchId], [BranchName], [BranchCode], [Location]) VALUES (7, N'MBL Islamabal', N'MBL-343', N'Islamabad')
SET IDENTITY_INSERT [dbo].[Branch] OFF
GO
SET IDENTITY_INSERT [dbo].[ChangeRequest] ON 

INSERT [dbo].[ChangeRequest] ([RequestId], [CustomerId], [RequestType], [Description], [UpdatedInfo], [Status]) VALUES (1, 6, N'First_Name', N'change the firstname', N'Arslan', 6)
INSERT [dbo].[ChangeRequest] ([RequestId], [CustomerId], [RequestType], [Description], [UpdatedInfo], [Status]) VALUES (3, 8, N'Contact', N'Contact Number Change', N'03076545674', 6)
INSERT [dbo].[ChangeRequest] ([RequestId], [CustomerId], [RequestType], [Description], [UpdatedInfo], [Status]) VALUES (4, 10, N'Last_Name', N'Change my last name', N'Afzal', 5)
INSERT [dbo].[ChangeRequest] ([RequestId], [CustomerId], [RequestType], [Description], [UpdatedInfo], [Status]) VALUES (5, 11, N'DoB', N'Change My Date of Birth ', N'10-09-2001', 5)
INSERT [dbo].[ChangeRequest] ([RequestId], [CustomerId], [RequestType], [Description], [UpdatedInfo], [Status]) VALUES (6, 24, N'Contact', N'change my contact no', N'03456787676', 6)
SET IDENTITY_INSERT [dbo].[ChangeRequest] OFF
GO
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (4, N'3456765434533')
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (6, N'3610463358941')
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (7, N'34102324432@3')
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (8, N'3451232444342')
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (10, N'3412345678908')
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (11, N'3201312345676')
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (24, N'4543456787389')
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (25, N'3493945345454')
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (28, N'3510567656545')
INSERT [dbo].[Customer] ([CustomerId], [CNIC]) VALUES (31, N'3410566574757')
GO
SET IDENTITY_INSERT [dbo].[Feedback] ON 

INSERT [dbo].[Feedback] ([Id], [CustomerId], [Description]) VALUES (1, 11, N'Good')
INSERT [dbo].[Feedback] ([Id], [CustomerId], [Description]) VALUES (2, 25, N'I have experienced all the functionality of this application all are working properly')
SET IDENTITY_INSERT [dbo].[Feedback] OFF
GO
SET IDENTITY_INSERT [dbo].[InstallmentOfLoan] ON 

INSERT [dbo].[InstallmentOfLoan] ([ID], [LoanId], [PayableAmount], [Date]) VALUES (1, 1010, CAST(1200.00 AS Decimal(18, 2)), CAST(N'2024-05-08T09:57:30.747' AS DateTime))
INSERT [dbo].[InstallmentOfLoan] ([ID], [LoanId], [PayableAmount], [Date]) VALUES (2, 1010, CAST(3000.00 AS Decimal(18, 2)), CAST(N'2024-05-08T10:21:01.073' AS DateTime))
INSERT [dbo].[InstallmentOfLoan] ([ID], [LoanId], [PayableAmount], [Date]) VALUES (3, 1010, CAST(5000.00 AS Decimal(18, 2)), CAST(N'2024-05-08T13:02:09.260' AS DateTime))
INSERT [dbo].[InstallmentOfLoan] ([ID], [LoanId], [PayableAmount], [Date]) VALUES (4, 1010, CAST(1.00 AS Decimal(18, 2)), CAST(N'2024-05-08T13:15:01.407' AS DateTime))
INSERT [dbo].[InstallmentOfLoan] ([ID], [LoanId], [PayableAmount], [Date]) VALUES (5, 1010, CAST(3400.00 AS Decimal(18, 2)), CAST(N'2024-05-08T21:14:52.787' AS DateTime))
SET IDENTITY_INSERT [dbo].[InstallmentOfLoan] OFF
GO
SET IDENTITY_INSERT [dbo].[Loan] ON 

INSERT [dbo].[Loan] ([LoanId], [AccountId], [Amount], [Reason], [Duration], [Date], [LoanStatus], [PaymentStatus]) VALUES (1009, 4, CAST(50000.00 AS Decimal(18, 2)), N'Medical Issue', N'Six Month', CAST(N'2024-05-06T01:33:07.007' AS DateTime), 6, 8)
INSERT [dbo].[Loan] ([LoanId], [AccountId], [Amount], [Reason], [Duration], [Date], [LoanStatus], [PaymentStatus]) VALUES (1010, 6, CAST(130000.00 AS Decimal(18, 2)), N'Buy Property', N'Two Year', CAST(N'2024-05-07T10:43:06.303' AS DateTime), 5, 8)
INSERT [dbo].[Loan] ([LoanId], [AccountId], [Amount], [Reason], [Duration], [Date], [LoanStatus], [PaymentStatus]) VALUES (1011, 7, CAST(50000.00 AS Decimal(18, 2)), N'Financial Issue', N'Six Month', CAST(N'2024-05-10T10:45:54.323' AS DateTime), 6, 8)
SET IDENTITY_INSERT [dbo].[Loan] OFF
GO
SET IDENTITY_INSERT [dbo].[LookUp] ON 

INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (1, N'Current', N'Accountt Type')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (2, N'Saving', N'Account Type')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (3, N'Deposit', N'Transaction Type')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (4, N'Withdraw', N'Transaction Type')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (5, N'Approved', N'Status')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (6, N'Pending', N'Status')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (7, N'Paid', N'Payment Status')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (8, N'Unpaid', N'Payment Status')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (9, N'Visa Card', N'Debit Card')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (10, N'Mastercard', N'Debit Card')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (11, N'Visa Gold', N'Debit Card')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (12, N'Visa Platinum', N'Debit Card')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (13, N'Business Card', N'Credit Card')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (14, N'Fuel Card', N'Credit Card')
INSERT [dbo].[LookUp] ([Id], [Name], [Category]) VALUES (15, N'CashBack Card', N'Credit Card')
SET IDENTITY_INSERT [dbo].[LookUp] OFF
GO
INSERT [dbo].[Manager] ([ManagerId], [CNIC], [BranchId]) VALUES (9, N'3410987654545', 2)
INSERT [dbo].[Manager] ([ManagerId], [CNIC], [BranchId]) VALUES (26, N'3456787677674', 3)
INSERT [dbo].[Manager] ([ManagerId], [CNIC], [BranchId]) VALUES (27, N'3432423423423', 4)
INSERT [dbo].[Manager] ([ManagerId], [CNIC], [BranchId]) VALUES (29, N'3424565676676', 6)
INSERT [dbo].[Manager] ([ManagerId], [CNIC], [BranchId]) VALUES (30, N'4545456787676', 7)
GO
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (3, N'Muhammad Adnan', N'Amin', N'03265145770', CAST(N'2004-01-01T00:00:00.000' AS DateTime), N'Kot Muhammad Hussain Khanewal')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (4, N'Muhammad Noman', N'Amin', N'03476545321', CAST(N'2002-06-17T10:11:11.000' AS DateTime), N'Khanewal')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (6, N'Muhammad ', N'Amin', N'03047139084', CAST(N'2024-04-27T10:11:11.243' AS DateTime), N'Khanewal')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (7, N'Awais', N'Nazir', N'03054545353', CAST(N'2005-05-25T10:11:11.000' AS DateTime), N'Islamabad')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (8, N'Hamza', N'Adnan', N'03035654567', CAST(N'2024-04-27T10:11:11.243' AS DateTime), N'Lahore')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (10, N'Muhammad Arshman', N'Afzal', N'03054323432', CAST(N'2011-06-29T00:00:00.000' AS DateTime), N'Khanewal,Lahore')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (11, N'Afzal', N'Hussain', N'03054323212', CAST(N'2001-09-10T00:00:00.000' AS DateTime), N'Quetta')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (24, N'Muhammad', N'Sulman', N'03543454565', CAST(N'2024-04-27T10:11:11.243' AS DateTime), N'Lahore')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (25, N'Naeef', N'Hussain', N'03234545656', CAST(N'2024-04-27T10:11:11.243' AS DateTime), N'Lahore')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (26, N'Hassan', N'Raza', N'03054454645', CAST(N'2005-06-14T00:00:00.000' AS DateTime), N'Lahore UET')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (27, N'Muhammad', N'Nawaz', N'03456565655', CAST(N'2003-06-18T00:00:00.000' AS DateTime), N'Lahore')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (28, N'Muhammad', N'Salaman Ali', N'03076545432', CAST(N'2013-06-05T10:11:11.000' AS DateTime), N'Lahore')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (29, N'Awais', N'Ali', N'03056578778', CAST(N'2001-07-04T00:00:00.000' AS DateTime), N'Lahore')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (30, N'Hanan', N'Saeed', N'03065456765', CAST(N'2002-07-09T00:00:00.000' AS DateTime), N'Khanewal')
INSERT [dbo].[PersonInfo] ([Id], [FirstName], [LastName], [Contact], [Date of Birth], [Address]) VALUES (31, N'Adnan', N'Afzal', N'03056765676', CAST(N'2006-10-10T10:11:11.000' AS DateTime), N'Khanewal')
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 

INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (2, 2, CAST(1000.00 AS Decimal(18, 2)), N'Deposit Amount', 3, CAST(N'2024-05-05T15:44:13.463' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (3, 2, CAST(2000.00 AS Decimal(18, 2)), N'Family Issue', 3, CAST(N'2024-05-05T15:46:55.497' AS DateTime), 6)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (4, 6, CAST(2300.00 AS Decimal(18, 2)), N'Fee Issue', 3, CAST(N'2024-05-08T06:58:04.347' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (5, 6, CAST(4000.00 AS Decimal(18, 2)), N'For Investment', 3, CAST(N'2024-05-08T13:33:09.957' AS DateTime), 6)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (6, 6, CAST(5000.00 AS Decimal(18, 2)), N'withdrawl', 4, CAST(N'2024-05-08T13:44:14.413' AS DateTime), 6)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (7, 6, CAST(14000.00 AS Decimal(18, 2)), N'good', 4, CAST(N'2024-05-08T13:44:55.797' AS DateTime), 6)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (8, 6, CAST(50000.00 AS Decimal(18, 2)), N'deposit my cash', 3, CAST(N'2024-05-08T14:08:50.500' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (9, 6, CAST(3400.00 AS Decimal(18, 2)), N'Argen', 4, CAST(N'2024-05-08T21:11:56.140' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (10, 7, CAST(20000.00 AS Decimal(18, 2)), N'Deposit My Amount', 3, CAST(N'2024-05-10T03:45:51.903' AS DateTime), 6)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (11, 7, CAST(2300.00 AS Decimal(18, 2)), N'Withdraw argent', 4, CAST(N'2024-05-10T08:51:28.060' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (12, 7, CAST(1200.00 AS Decimal(18, 2)), N'Need it', 4, CAST(N'2024-05-10T10:42:17.133' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (13, 7, CAST(30000.00 AS Decimal(18, 2)), N'deposi it', 3, CAST(N'2024-05-10T10:42:37.190' AS DateTime), 6)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (14, 9, CAST(23000.00 AS Decimal(18, 2)), N'deposit my cash into account', 3, CAST(N'2024-05-21T10:31:22.580' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (15, 9, CAST(1000.00 AS Decimal(18, 2)), N'withdraw', 4, CAST(N'2024-05-21T10:41:32.500' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (16, 9, CAST(1000.00 AS Decimal(18, 2)), N'deposit', 3, CAST(N'2024-05-21T10:47:50.593' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (17, 9, CAST(1000.00 AS Decimal(18, 2)), N'withdraw my amount', 4, CAST(N'2024-05-21T10:49:24.710' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (18, 10, CAST(23000.00 AS Decimal(18, 2)), N'deposit it', 3, CAST(N'2024-05-28T14:37:47.123' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (19, 10, CAST(43400.00 AS Decimal(18, 2)), N'deposit it', 3, CAST(N'2024-05-28T14:51:18.800' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (20, 10, CAST(45000.00 AS Decimal(18, 2)), N'deposit', 3, CAST(N'2024-05-28T14:56:35.603' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (21, 10, CAST(12000.00 AS Decimal(18, 2)), N'deposit', 3, CAST(N'2024-05-28T15:00:19.047' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (22, 10, CAST(34000.00 AS Decimal(18, 2)), N'deposit it', 3, CAST(N'2024-05-28T15:02:51.373' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (23, 10, CAST(20000.00 AS Decimal(18, 2)), N'deposit', 3, CAST(N'2024-05-28T17:16:35.823' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (24, 10, CAST(23000.00 AS Decimal(18, 2)), N'deposit', 3, CAST(N'2024-05-28T17:18:25.073' AS DateTime), 5)
INSERT [dbo].[Transaction] ([TransactionId], [AccountId], [Amount], [Description], [Type], [Date], [Status]) VALUES (25, 10, CAST(23000.00 AS Decimal(18, 2)), N'deposit', 3, CAST(N'2024-05-28T17:22:32.693' AS DateTime), 5)
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO
SET IDENTITY_INSERT [dbo].[TransferAmount] ON 

INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (1, 2, 1, CAST(1000.00 AS Decimal(18, 2)), CAST(N'2024-05-05T14:46:10.347' AS DateTime), N'')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (2, 2, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(N'2024-05-05T14:47:47.673' AS DateTime), N'')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (3, 2, 1, CAST(230.00 AS Decimal(18, 2)), CAST(N'2024-05-05T14:58:24.310' AS DateTime), N'deposit')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (4, 2, 1, CAST(100.00 AS Decimal(18, 2)), CAST(N'2024-05-05T15:11:11.207' AS DateTime), N'Fee Deposit')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (5, 2, 1, CAST(2.00 AS Decimal(18, 2)), CAST(N'2024-05-05T15:21:42.580' AS DateTime), N'Education')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (6, 2, 1, CAST(3000.00 AS Decimal(18, 2)), CAST(N'2024-05-05T16:07:16.847' AS DateTime), N'Education')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (7, 6, 3, CAST(23000.00 AS Decimal(18, 2)), CAST(N'2024-05-08T13:34:06.003' AS DateTime), N'Business Transactions')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (8, 7, 5, CAST(10000.00 AS Decimal(18, 2)), CAST(N'2024-05-10T08:04:10.107' AS DateTime), N'Donation and Zakat')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (9, 7, 4, CAST(1000.00 AS Decimal(18, 2)), CAST(N'2024-05-10T08:31:33.093' AS DateTime), N'Donation and Zakat')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (10, 7, 4, CAST(3400.00 AS Decimal(18, 2)), CAST(N'2024-05-10T08:36:51.010' AS DateTime), N'Donation and Zakat')
INSERT [dbo].[TransferAmount] ([TransferId], [AccountFrom], [AccountTo], [Amount], [Date], [Detail]) VALUES (11, 7, 2, CAST(100.00 AS Decimal(18, 2)), CAST(N'2024-05-10T10:44:08.457' AS DateTime), N'Business Transactions')
SET IDENTITY_INSERT [dbo].[TransferAmount] OFF
GO
SET IDENTITY_INSERT [dbo].[UserId] ON 

INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (1, N'azlan@gmail.com', N'!23')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (2, N'azlan@gmail.com', N'!23')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (3, N'adnanamin@gmail.com', N'!@#')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (4, N'noman@gmail.com', N'!@3')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (6, N'amin@gmail.com', N'!23')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (7, N'awaisnazir@gmail.com', N')(*')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (8, N'hamza098@gmail.com', N'*()')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (9, N'saleem@gmail.com', N'DdVJ0')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (10, N'arshman@gmail.com', N'1234')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (11, N'afzal@gmail.com', N'&*(')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (24, N'naeef@gmail.com', N'12')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (25, N'nazeef@gmail.com', N'qwer')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (26, N'hassanraza@gmail.com', N'ulT6)')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (27, N'nawaz@gmail.com', N'z8ejx')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (28, N'salmanali@gmail.com', N'salman')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (29, N'awaisali@gmail.com', N'R@xNu')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (30, N'hanan@gmail.com', N'*u*B0')
INSERT [dbo].[UserId] ([Id], [Email], [Password]) VALUES (31, N'ad23@gmail.com', N'234')
SET IDENTITY_INSERT [dbo].[UserId] OFF
GO
/****** Object:  Index [IX_Account]    Script Date: 5/28/2024 6:44:10 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Account] ON [dbo].[Account]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ATMCard]    Script Date: 5/28/2024 6:44:10 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ATMCard] ON [dbo].[ATMCard]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Loan]    Script Date: 5/28/2024 6:44:10 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Loan] ON [dbo].[Loan]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Manager_1]    Script Date: 5/28/2024 6:44:10 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Manager_1] ON [dbo].[Manager]
(
	[BranchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_LookUp1] FOREIGN KEY([AccountType])
REFERENCES [dbo].[LookUp] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_LookUp1]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_UserId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[UserId] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_UserId]
GO
ALTER TABLE [dbo].[Admin]  WITH CHECK ADD  CONSTRAINT [FK_Admin_UserId] FOREIGN KEY([Admin Id])
REFERENCES [dbo].[UserId] ([Id])
GO
ALTER TABLE [dbo].[Admin] CHECK CONSTRAINT [FK_Admin_UserId]
GO
ALTER TABLE [dbo].[ATMCard]  WITH CHECK ADD  CONSTRAINT [FK_ATMCard_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[ATMCard] CHECK CONSTRAINT [FK_ATMCard_Account]
GO
ALTER TABLE [dbo].[ChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_ChangeRequest_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[ChangeRequest] CHECK CONSTRAINT [FK_ChangeRequest_Customer]
GO
ALTER TABLE [dbo].[ChangeRequest]  WITH CHECK ADD  CONSTRAINT [FK_ChangeRequest_LookUp] FOREIGN KEY([Status])
REFERENCES [dbo].[LookUp] ([Id])
GO
ALTER TABLE [dbo].[ChangeRequest] CHECK CONSTRAINT [FK_ChangeRequest_LookUp]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_UserId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[UserId] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_UserId]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK_Feedback_Customer]
GO
ALTER TABLE [dbo].[InstallmentOfLoan]  WITH CHECK ADD  CONSTRAINT [FK_InstallmentOfLoan_Loan] FOREIGN KEY([LoanId])
REFERENCES [dbo].[Loan] ([LoanId])
GO
ALTER TABLE [dbo].[InstallmentOfLoan] CHECK CONSTRAINT [FK_InstallmentOfLoan_Loan]
GO
ALTER TABLE [dbo].[Loan]  WITH CHECK ADD  CONSTRAINT [FK_Loan_Account1] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Loan] CHECK CONSTRAINT [FK_Loan_Account1]
GO
ALTER TABLE [dbo].[Loan]  WITH CHECK ADD  CONSTRAINT [FK_Loan_LookUp] FOREIGN KEY([LoanStatus])
REFERENCES [dbo].[LookUp] ([Id])
GO
ALTER TABLE [dbo].[Loan] CHECK CONSTRAINT [FK_Loan_LookUp]
GO
ALTER TABLE [dbo].[Manager]  WITH CHECK ADD  CONSTRAINT [FK_Manager_Branch] FOREIGN KEY([BranchId])
REFERENCES [dbo].[Branch] ([BranchId])
GO
ALTER TABLE [dbo].[Manager] CHECK CONSTRAINT [FK_Manager_Branch]
GO
ALTER TABLE [dbo].[Manager]  WITH CHECK ADD  CONSTRAINT [FK_Manager_UserId] FOREIGN KEY([ManagerId])
REFERENCES [dbo].[UserId] ([Id])
GO
ALTER TABLE [dbo].[Manager] CHECK CONSTRAINT [FK_Manager_UserId]
GO
ALTER TABLE [dbo].[PersonInfo]  WITH CHECK ADD  CONSTRAINT [FK_PersonInfo_UserId] FOREIGN KEY([Id])
REFERENCES [dbo].[UserId] ([Id])
GO
ALTER TABLE [dbo].[PersonInfo] CHECK CONSTRAINT [FK_PersonInfo_UserId]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Account]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_LookUp] FOREIGN KEY([Status])
REFERENCES [dbo].[LookUp] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_LookUp]
GO
ALTER TABLE [dbo].[TransferAmount]  WITH CHECK ADD  CONSTRAINT [FK_TransferAmount_Account] FOREIGN KEY([AccountTo])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[TransferAmount] CHECK CONSTRAINT [FK_TransferAmount_Account]
GO
/****** Object:  StoredProcedure [dbo].[InsertAccountInfo]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertAccountInfo]
	-- Add the parameters for the stored procedure here
	@Id INT,
    @AccountNo NVARCHAR(14),
    @Type INT,
    @Code NVARCHAR(255),
    @Balance DECIMAL(18, 2),
    @Date DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into Account (CustomerId,AccountNo,AccountType,BranchCode,CurrentBalance,CreatedDate) 
	values (@Id, @AccountNo,@Type,@Code,@Balance,@Date);
END
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoATMCard]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertIntoATMCard] 
	-- Add the parameters for the stored procedure here
	@Id INT,
    @Type VARCHAR(50),
    @CardType Int,
    @Date DATETIME,
    @Status INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into ATMCard (AccountId,Card,CardType,Date,Status) Values
	(@Id,@Type,@CardType,@Date,@Status);
END
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoFeedback]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertIntoFeedback] 
	-- Add the parameters for the stored procedure here
	@Id INT,
	@Description nvarchar(Max)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into Feedback (CustomerId,Description) values (@Id,@Description);
END
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoInstallment]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertIntoInstallment] 
	-- Add the parameters for the stored procedure here
	@LoanId INT,
    @Amount DECIMAL,
    @Date DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into InstallmentOfLoan (LoanId,PayableAmount,Date) values (@LoanId,@Amount,@Date);
END
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoPersonInfo]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertIntoPersonInfo] 
	-- Add the parameters for the stored procedure here
	@Id INT,
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255),
    @Contact NVARCHAR(11),
    @Date DATETIME,
    @Address NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into PersonInfo (Id,FirstName,LastName,Contact, [Date of Birth],Address) 
	values (@Id,@FirstName, @LastName,@Contact,@Date,@Address); SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoRequestChange]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertIntoRequestChange] 
	-- Add the parameters for the stored procedure here
  @Id INT,
  @RequestType NVARCHAR(MAX),
  @Description NVARCHAR(MAX),
  @UpdatedInfo NVARCHAR(MAX),
  @Status INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into ChangeRequest (CustomerId,RequestType,Description,UpdatedInfo,Status) 
	values (@Id,@RequestType,@Description,@UpdatedInfo,@Status);
END
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoTransferAmount]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertIntoTransferAmount] 
	-- Add the parameters for the stored procedure here
	@Acc1 INT,
    @Acc2 INT,
    @Amount DECIMAL(18, 2),
    @Detail NVARCHAR(255),
    @Date DATETIME,
	@SenderBalance decimal(18,2),
	@SenderNo NVARCHAR(14),
	@ReceiverBalance decimal(18,2),
	@ReceiverNo NVARCHAR(14)




AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRANSACTION;

    BEGIN TRY
    -- Insert statements for procedure here
	Insert into TransferAmount (AccountFrom,AccountTo,Amount,Detail,Date) 
	values (@Acc1,@Acc2,@Amount,@Detail,@Date);

	Update Account Set CurrentBalance = @SenderBalance Where AccountNo = @SenderNo;

	Update Account Set CurrentBalance = @ReceiverBalance Where AccountNo = @ReceiverNo;
	 COMMIT; 
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW; 
    END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[InsertLoan]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertLoan]
	-- Add the parameters for the stored procedure here
	@Id INT,
    @Amount DECIMAL(18, 2),
    @Reason NVARCHAR(100),
    @Duration NVARCHAR(50),
    @Date DATETIME,
    @Status INT,
	@PaymentStatus INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	BEGIN TRY
    -- Insert statements for procedure here
	Insert into Loan (AccountId,Amount,Reason,Duration,Date,LoanStatus,PaymentStatus) values 
	(@Id,@Amount,@Reason,@Duration,@Date,@Status,@PaymentStatus);
	 COMMIT; 
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW; 
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[INSERTMANAGERINFO]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[INSERTMANAGERINFO]
	-- Add the parameters for the stored procedure here
	@Email NVARCHAR(255),
    @Password NVARCHAR(255),
    @CNIC NVARCHAR(13),
	@BranchId INT,
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255),
    @Contact NVARCHAR(11),
    @Date DATETIME,
    @Address NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	BEGIN TRY
    -- Insert statements for procedure here
	Insert into UserId (Email, Password) values (@Email, @Password);
	DECLARE @Id INT = SCOPE_IDENTITY();
	Insert into Manager (ManagerId,CNIC,BranchId) values (@Id,@CNIC,@BranchId);

	Insert into PersonInfo (Id,FirstName,LastName,Contact,[Date of Birth],Address) values (@Id,@FirstName,@LastName,@Contact,@Date,@Address);
	 COMMIT; 
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW; 
    END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[INSERTNEWACCOUNTINFO]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[INSERTNEWACCOUNTINFO] 
	-- Add the parameters for the stored procedure here
	@Email NVARCHAR(255),
    @Password NVARCHAR(255),
    @CNIC NVARCHAR(13),
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255),
    @Contact NVARCHAR(11),
    @Date DATETIME,
    @Address NVARCHAR(255),
	@AccountNo NVARCHAR(14),
    @Type INT,
    @Code NVARCHAR(255),
    @Balance DECIMAL(18, 2),
    @AccountDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRANSACTION;

    BEGIN TRY
    -- Insert statements for procedure here
	Insert into UserId (Email, Password) values (@Email, @Password);
	DECLARE @Id INT = SCOPE_IDENTITY();
	Insert into Customer (CustomerId,CNIC) values (@Id, @CNIC);

	Insert into PersonInfo (Id,FirstName,LastName,Contact, [Date of Birth],Address) 
	values (@Id,@FirstName, @LastName,@Contact,@Date,@Address);

	Insert into Account (CustomerId,AccountNo,AccountType,BranchCode,CurrentBalance,CreatedDate) 
	values (@Id, @AccountNo,@Type,@Code,@Balance,@AccountDate);
	 COMMIT; 
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW; 
    END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[InsertTransactionData]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertTransactionData] 
	-- Add the parameters for the stored procedure here
	@AccountId INT,
    @Amount DECIMAL(18, 2),
    @Description NVARCHAR(255),
    @Type INT,
    @Date DATETIME,
    @Status INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Insert into [Transaction] (AccountId,Amount,Description,Type,Date,Status)
	values (@AccountId,@Amount,@Description,@Type,@Date,@Status);
END
GO
/****** Object:  StoredProcedure [dbo].[InsertTransactionDataWithdraw]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[InsertTransactionDataWithdraw] 
	-- Add the parameters for the stored procedure here
	@AccountId INT,
    @Amount DECIMAL(18, 2),
    @Description NVARCHAR(255),
    @Type INT,
    @Date DATETIME,
    @Status INT,
	@Balance Decimal(18,2),
	@No NVARCHAR(14)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	BEGIN TRY
    -- Insert statements for procedure here
	Insert into [Transaction] (AccountId,Amount,Description,Type,Date,Status)
	values (@AccountId,@Amount,@Description,@Type,@Date,@Status);

	Update Account Set CurrentBalance = @Balance Where AccountNo = @No;
	 COMMIT; 
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW; 
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[showCustomerInfoIntoGrid]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[showCustomerInfoIntoGrid] 
	-- Add the parameters for the stored procedure here
	@Code VARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select RequestId,RequestType,UpdatedInfo, FirstName,LastName,Contact,Format([Date of Birth],'dd-MMM-yyyy') as [Date of Birth]
	,Address
from Customer join PersonInfo on Customer.CustomerId = PersonInfo.Id
join Account on Account.CustomerId = Customer.CustomerId
join ChangeRequest on ChangeRequest.CustomerId = Customer.CustomerId Where Account.BranchCode = @Code;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateATMCardRequest]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateATMCardRequest] 
	-- Add the parameters for the stored procedure here
	@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update ATMCard set Status = '5' Where Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateBalance]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateBalance]
	-- Add the parameters for the stored procedure here
	@Balance DECIMAL(18, 2),
    @No NVARCHAR(14)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update Account Set CurrentBalance = @Balance Where AccountNo = @No;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCustomerChangeRequest]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCustomerChangeRequest] 
	-- Add the parameters for the stored procedure here
	@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update ChangeRequest set Status = '5' Where RequestId = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCustomerInfo]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCustomerInfo] 
	-- Add the parameters for the stored procedure here
	@Id INT,
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @DOB DATETIME,
    @Contact VARCHAR(20),
    @Address VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update PersonInfo  Set FirstName = @FirstName, LastName = @LastName,[Date of Birth] = @DOB
	, Contact = @Contact,Address= @Address from PersonInfo join ChangeRequest
	on PersonInfo.Id = ChangeRequest.CustomerId Where ChangeRequest.RequestId = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCustomerLoanRequest]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCustomerLoanRequest] 
	-- Add the parameters for the stored procedure here
	@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update Loan set LoanStatus = '5' Where LoanId = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateDepositRequest]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateDepositRequest]
	-- Add the parameters for the stored procedure here
	@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update [Transaction] set [Transaction].Status = '5' Where TransactionId = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[UPDATEMANAGERINFO]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATEMANAGERINFO] 
	-- Add the parameters for the stored procedure here
	@Id INT,
	@Email NVARCHAR(255),
    @CNIC NVARCHAR(13),
	@BranchId INT,
    @fName NVARCHAR(255),
    @lName NVARCHAR(255),
    @Contact NVARCHAR(11),
    @dob DATETIME,
    @Address NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	BEGIN TRY
    -- Insert statements for procedure here
	Update UserId set Email = @Email where Id = @Id;

	Update Manager set CNIC = @cnic ,BranchId = @BranchId where ManagerId = @Id;

	Update PersonInfo set FirstName = @fName, LastName = @lName,Contact = @Contact, [Date of Birth] = @dob, Address = @Address Where Id = @Id;
	 COMMIT; 
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW; 
    END CATCH;

END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePassword]    Script Date: 5/28/2024 6:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePassword] 
	-- Add the parameters for the stored procedure here
	@Id INT,
	@Password nvarchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update UserId Set Password = @Password Where Id = @Id;
END
GO
USE [master]
GO
ALTER DATABASE [FinalProject] SET  READ_WRITE 
GO
