/*    ==Параметры сценариев==

    Версия исходного сервера : SQL Server 2008 R2 (10.50.6560)
    Выпуск исходного ядра СУБД : Выпуск Microsoft SQL Server Enterprise Edition
    Тип исходного ядра СУБД : Изолированный SQL Server

    Версия целевого сервера : SQL Server 2017
    Выпуск целевого ядра СУБД : Выпуск Microsoft SQL Server Standard Edition
    Тип целевого ядра СУБД : Изолированный SQL Server
*/
USE [ImageStore]
GO
/****** Object:  Table [dbo].[ChangeLocationHistory]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChangeLocationHistory](
	[ChangeLocationHistory_id] [int] IDENTITY(1,1) NOT NULL,
	[Storage_id] [int] NOT NULL,
	[Location_id] [int] NOT NULL,
	[DateChange] [datetime] NOT NULL,
 CONSTRAINT [PK_ChangeLocationHistory] PRIMARY KEY CLUSTERED 
(
	[ChangeLocationHistory_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doubles]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doubles](
	[FileDoubles_id] [int] IDENTITY(1,1) NOT NULL,
	[FileDetail_id] [int] NOT NULL,
	[TransferName] [nvarchar](255) NOT NULL,
	[Storage_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ElementConstraints]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ElementConstraints](
	[ElementConstraint_id] [int] NOT NULL,
	[FileStructElement_id] [int] NOT NULL,
	[ConstraintType] [int] NOT NULL,
	[First] [nvarchar](255) NULL,
	[Last] [nvarchar](255) NULL,
	[RegEx] [nvarchar](1000) NULL,
	[ElementValue] [nvarchar](255) NULL,
	[ConstraintName] [nvarchar](255) NULL,
 CONSTRAINT [PK__ElementC__794966B37F60ED59] PRIMARY KEY CLUSTERED 
(
	[ElementConstraint_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FileDetail]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileDetail](
	[FileDetail_id] [int] IDENTITY(1,1) NOT NULL,
	[FolderScanHistory_id] [int] NOT NULL,
	[Folder_id] [int] NOT NULL,
	[ExtensionName] [nvarchar](50) NOT NULL,
	[FileCount] [int] NOT NULL,
	[FileSize] [bigint] NOT NULL,
 CONSTRAINT [PK_FileDetail] PRIMARY KEY CLUSTERED 
(
	[FileDetail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Folder]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Folder](
	[FolderScanHistory_id] [int] NOT NULL,
	[Folder_id] [int] NOT NULL,
	[Storage_id] [int] NOT NULL,
	[Lvl] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[FolderPath] [nvarchar](255) NOT NULL,
	[TotalSubFolderCount] [int] NOT NULL,
	[DateDeleted] [datetime] NULL,
	[DateAdded] [datetime] NOT NULL,
	[ParentFolder_id] [int] NULL,
 CONSTRAINT [PK_Folder] PRIMARY KEY CLUSTERED 
(
	[FolderScanHistory_id] ASC,
	[Folder_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FolderScanHistory]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FolderScanHistory](
	[FolderScanHistory_id] [int] IDENTITY(1,1) NOT NULL,
	[DateStartScan] [datetime] NOT NULL,
	[DateEndScan] [datetime] NULL,
	[CreatorUser_id] [int] NOT NULL,
	[DeleteScannedAfterCancellation] [nvarchar](max) NULL,
 CONSTRAINT [PK_FolderScanHistory] PRIMARY KEY CLUSTERED 
(
	[FolderScanHistory_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImageConstraints]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageConstraints](
	[Transfer_id] [int] NOT NULL,
	[FileStructElement_id] [int] NOT NULL,
	[Length] [int] NULL,
	[LenghtType] [int] NULL,
	[DPI] [int] NULL,
	[Format] [int] NULL,
	[CompressionName] [nvarchar](50) NULL,
	[RegEx] [nvarchar](1000) NULL,
	[IsOnlyNum] [bit] NULL,
	[JpegQuality] [int] NULL,
 CONSTRAINT [PK_ImageConstraints] PRIMARY KEY CLUSTERED 
(
	[Transfer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoadConfigs]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoadConfigs](
	[LoadConfig_id] [int] IDENTITY(1,1) NOT NULL,
	[Project_id] [int] NOT NULL,
	[FirstNum] [int] NULL,
	[LastNum] [int] NULL,
	[EdgeBatchType] [bit] NULL,
	[EdgeLevel] [int] NULL,
	[MaxRecCount] [int] NULL,
	[GroupImgType] [int] NULL,
	[ImageTypeFilter] [nvarchar](255) NULL,
	[OrdinalFilter] [nvarchar](255) NULL,
 CONSTRAINT [PK_LoadConfigs] PRIMARY KEY CLUSTERED 
(
	[LoadConfig_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locaction]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locaction](
	[Location_id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[LocationType_id] [int] NULL,
 CONSTRAINT [PK_Locaction] PRIMARY KEY CLUSTERED 
(
	[Location_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Locaction_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocationType]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationType](
	[LocationType_id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_LocationType] PRIMARY KEY CLUSTERED 
(
	[LocationType_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_LocationType_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Permissions_id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionsName] [nvarchar](255) NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Permissions_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[Project_id] [int] NOT NULL,
	[TransferName] [nvarchar](255) NOT NULL,
	[Transfer_id] [int] NOT NULL,
	[Step_id] [int] NULL,
	[Dept] [int] NULL,
	[Dept_Name] [nvarchar](255) NULL,
	[DataSource] [nvarchar](255) NULL,
	[Project_Name] [nvarchar](255) NULL,
	[IsDefault] [bit] NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_Projects_1] PRIMARY KEY CLUSTERED 
(
	[Project_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Storage]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Storage](
	[Storage_id] [int] NOT NULL,
	[Project_id] [int] NULL,
	[Name] [nvarchar](255) NULL,
	[TotalFreeSpace] [bigint] NULL,
	[TotalSize] [bigint] NOT NULL,
	[SerialNumber] [nvarchar](255) NULL,
	[DateOfLastUpdate] [datetime] NULL,
	[DateAdded] [datetime] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[DateDeleted] [datetime] NULL,
	[StorageType_id] [int] NOT NULL,
	[Location_id] [int] NOT NULL,
	[OwnerUser_id] [int] NOT NULL,
	[CurrentHolderUser_id] [int] NOT NULL,
 CONSTRAINT [PK_Storage] PRIMARY KEY CLUSTERED 
(
	[Storage_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StorageType]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageType](
	[StorageType_id] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_StorageType] PRIMARY KEY CLUSTERED 
(
	[StorageType_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_StorageType_Name] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StorageUserHolderHistory]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageUserHolderHistory](
	[StorageUserHolderHistory_id] [int] IDENTITY(1,1) NOT NULL,
	[Storage_id] [int] NOT NULL,
	[DateChange] [datetime] NOT NULL,
	[FromUser_id] [int] NOT NULL,
	[ToUser_id] [int] NOT NULL,
 CONSTRAINT [PK_StorageUserHolderHistory] PRIMARY KEY CLUSTERED 
(
	[StorageUserHolderHistory_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transfers]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transfers](
	[Transfer_id] [int] IDENTITY(1,1) NOT NULL,
	[TransferName] [nvarchar](255) NOT NULL,
	[DateRegistration] [datetime] NULL,
	[User_id] [nvarchar](255) NULL,
	[State] [int] NULL,
	[DatePartiton] [datetime] NULL,
	[LoginPartition] [nvarchar](255) NULL,
	[CenterName] [nvarchar](255) NULL,
	[TransferType] [tinyint] NULL,
	[VersionLoad] [date] NOT NULL,
 CONSTRAINT [PK_Transfers] PRIMARY KEY CLUSTERED 
(
	[Transfer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 03.03.2019 21:49:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[User_id] [int] IDENTITY(1,1) NOT NULL,
	[Permissions_id] [int] NOT NULL,
	[Login] [nvarchar](100) NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[Password] [nvarchar](100) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[User_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_User_Login] UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_FileDetail_Folder_id]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_FileDetail_Folder_id] ON [dbo].[FileDetail]
(
	[Folder_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_FileDetail_FolderScanHistory_id]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_FileDetail_FolderScanHistory_id] ON [dbo].[FileDetail]
(
	[FolderScanHistory_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Folder_FolderScanHistory_id]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_Folder_FolderScanHistory_id] ON [dbo].[Folder]
(
	[FolderScanHistory_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Folder_ParentFolder_id]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_Folder_ParentFolder_id] ON [dbo].[Folder]
(
	[ParentFolder_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_Folder_Storage_id]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_Folder_Storage_id] ON [dbo].[Folder]
(
	[Storage_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Name]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_Name] ON [dbo].[Folder]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/****** Object:  Index [IX_FolderScanHistory_CreatorUser_id]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_FolderScanHistory_CreatorUser_id] ON [dbo].[FolderScanHistory]
(
	[CreatorUser_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Storage_CurrentHolderUser_id]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_Storage_CurrentHolderUser_id] ON [dbo].[Storage]
(
	[CurrentHolderUser_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Storage_Location_id]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_Storage_Location_id] ON [dbo].[Storage]
(
	[Location_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Storage_OwnerUser_id]    Script Date: 03.03.2019 21:49:01 ******/
CREATE NONCLUSTERED INDEX [IX_Storage_OwnerUser_id] ON [dbo].[Storage]
(
	[OwnerUser_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Storage] ADD  CONSTRAINT [DF_Storage_TotalFreeSpace]  DEFAULT ((0)) FOR [TotalFreeSpace]
GO
ALTER TABLE [dbo].[Storage] ADD  CONSTRAINT [DF_Storage_TotalSize]  DEFAULT ((0)) FOR [TotalSize]
GO
ALTER TABLE [dbo].[Storage] ADD  CONSTRAINT [DF_Storage_DateOfLastUpdate]  DEFAULT (getdate()) FOR [DateOfLastUpdate]
GO
ALTER TABLE [dbo].[ChangeLocationHistory]  WITH CHECK ADD  CONSTRAINT [FK_ChangeLocationHistory_Location] FOREIGN KEY([Location_id])
REFERENCES [dbo].[Locaction] ([Location_id])
GO
ALTER TABLE [dbo].[ChangeLocationHistory] CHECK CONSTRAINT [FK_ChangeLocationHistory_Location]
GO
ALTER TABLE [dbo].[ChangeLocationHistory]  WITH CHECK ADD  CONSTRAINT [FK_ChangeLocationHistory_Storage] FOREIGN KEY([Storage_id])
REFERENCES [dbo].[Storage] ([Storage_id])
GO
ALTER TABLE [dbo].[ChangeLocationHistory] CHECK CONSTRAINT [FK_ChangeLocationHistory_Storage]
GO
ALTER TABLE [dbo].[Doubles]  WITH CHECK ADD  CONSTRAINT [FK_Doubles_FileDetail] FOREIGN KEY([FileDetail_id])
REFERENCES [dbo].[FileDetail] ([FileDetail_id])
GO
ALTER TABLE [dbo].[Doubles] CHECK CONSTRAINT [FK_Doubles_FileDetail]
GO
ALTER TABLE [dbo].[Doubles]  WITH CHECK ADD  CONSTRAINT [FK_Doubles_Storage] FOREIGN KEY([Storage_id])
REFERENCES [dbo].[Storage] ([Storage_id])
GO
ALTER TABLE [dbo].[Doubles] CHECK CONSTRAINT [FK_Doubles_Storage]
GO
ALTER TABLE [dbo].[FileDetail]  WITH NOCHECK ADD  CONSTRAINT [FK_FileDetail_Folder] FOREIGN KEY([FolderScanHistory_id], [Folder_id])
REFERENCES [dbo].[Folder] ([FolderScanHistory_id], [Folder_id])
GO
ALTER TABLE [dbo].[FileDetail] CHECK CONSTRAINT [FK_FileDetail_Folder]
GO
ALTER TABLE [dbo].[Folder]  WITH NOCHECK ADD  CONSTRAINT [FK_Folder_FolderScanHistory] FOREIGN KEY([FolderScanHistory_id])
REFERENCES [dbo].[FolderScanHistory] ([FolderScanHistory_id])
GO
ALTER TABLE [dbo].[Folder] CHECK CONSTRAINT [FK_Folder_FolderScanHistory]
GO
ALTER TABLE [dbo].[Folder]  WITH NOCHECK ADD  CONSTRAINT [FK_Folder_Storage] FOREIGN KEY([Storage_id])
REFERENCES [dbo].[Storage] ([Storage_id])
GO
ALTER TABLE [dbo].[Folder] CHECK CONSTRAINT [FK_Folder_Storage]
GO
ALTER TABLE [dbo].[FolderScanHistory]  WITH CHECK ADD  CONSTRAINT [FK_FolderScanHistory_User] FOREIGN KEY([CreatorUser_id])
REFERENCES [dbo].[User] ([User_id])
GO
ALTER TABLE [dbo].[FolderScanHistory] CHECK CONSTRAINT [FK_FolderScanHistory_User]
GO
ALTER TABLE [dbo].[LoadConfigs]  WITH CHECK ADD  CONSTRAINT [FK_LoadConfigs_Projects] FOREIGN KEY([Project_id])
REFERENCES [dbo].[Projects] ([Project_id])
GO
ALTER TABLE [dbo].[LoadConfigs] CHECK CONSTRAINT [FK_LoadConfigs_Projects]
GO
ALTER TABLE [dbo].[Locaction]  WITH CHECK ADD  CONSTRAINT [FK_Locaction_LocationType] FOREIGN KEY([LocationType_id])
REFERENCES [dbo].[LocationType] ([LocationType_id])
GO
ALTER TABLE [dbo].[Locaction] CHECK CONSTRAINT [FK_Locaction_LocationType]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Transfers] FOREIGN KEY([Transfer_id])
REFERENCES [dbo].[Transfers] ([Transfer_id])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Transfers]
GO
ALTER TABLE [dbo].[Storage]  WITH CHECK ADD  CONSTRAINT [FK_Storage_Locaction] FOREIGN KEY([Location_id])
REFERENCES [dbo].[Locaction] ([Location_id])
GO
ALTER TABLE [dbo].[Storage] CHECK CONSTRAINT [FK_Storage_Locaction]
GO
ALTER TABLE [dbo].[Storage]  WITH CHECK ADD  CONSTRAINT [FK_Storage_StorageType] FOREIGN KEY([Location_id])
REFERENCES [dbo].[StorageType] ([StorageType_id])
GO
ALTER TABLE [dbo].[Storage] CHECK CONSTRAINT [FK_Storage_StorageType]
GO
ALTER TABLE [dbo].[Storage]  WITH CHECK ADD  CONSTRAINT [FK_Storage_User1] FOREIGN KEY([OwnerUser_id])
REFERENCES [dbo].[User] ([User_id])
GO
ALTER TABLE [dbo].[Storage] CHECK CONSTRAINT [FK_Storage_User1]
GO
ALTER TABLE [dbo].[Storage]  WITH CHECK ADD  CONSTRAINT [FK_Storage_User2] FOREIGN KEY([CurrentHolderUser_id])
REFERENCES [dbo].[User] ([User_id])
GO
ALTER TABLE [dbo].[Storage] CHECK CONSTRAINT [FK_Storage_User2]
GO
ALTER TABLE [dbo].[StorageUserHolderHistory]  WITH CHECK ADD  CONSTRAINT [FK_StorageUserHolderHistory_User_1] FOREIGN KEY([FromUser_id])
REFERENCES [dbo].[User] ([User_id])
GO
ALTER TABLE [dbo].[StorageUserHolderHistory] CHECK CONSTRAINT [FK_StorageUserHolderHistory_User_1]
GO
ALTER TABLE [dbo].[StorageUserHolderHistory]  WITH CHECK ADD  CONSTRAINT [FK_StorageUserHolderHistory_User_2] FOREIGN KEY([ToUser_id])
REFERENCES [dbo].[User] ([User_id])
GO
ALTER TABLE [dbo].[StorageUserHolderHistory] CHECK CONSTRAINT [FK_StorageUserHolderHistory_User_2]
GO
ALTER TABLE [dbo].[Transfers]  WITH CHECK ADD  CONSTRAINT [FK_Transfers_ImageConstraints] FOREIGN KEY([Transfer_id])
REFERENCES [dbo].[ImageConstraints] ([Transfer_id])
GO
ALTER TABLE [dbo].[Transfers] CHECK CONSTRAINT [FK_Transfers_ImageConstraints]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Permissions] FOREIGN KEY([Permissions_id])
REFERENCES [dbo].[Permissions] ([Permissions_id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Permissions]
GO
