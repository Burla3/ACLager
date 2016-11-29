
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/29/2016 15:14:53
-- Generated from EDMX file: C:\Users\Mikke\Documents\GitHub\ACLager\ACLager\Models\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ACLagerDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_LocationItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ItemSet] DROP CONSTRAINT [FK_LocationItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemTypeItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ItemSet] DROP CONSTRAINT [FK_ItemTypeItem];
GO
IF OBJECT_ID(N'[dbo].[FK_WasteReportUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WasteReportSet] DROP CONSTRAINT [FK_WasteReportUser];
GO
IF OBJECT_ID(N'[dbo].[FK_WasteReportItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WasteReportSet] DROP CONSTRAINT [FK_WasteReportItem];
GO
IF OBJECT_ID(N'[dbo].[FK_WasteReportWorkOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WasteReportSet] DROP CONSTRAINT [FK_WasteReportWorkOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkOrderUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkOrderSet] DROP CONSTRAINT [FK_WorkOrderUser];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemTypeIngredient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientSet] DROP CONSTRAINT [FK_ItemTypeIngredient];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientItemType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientSet] DROP CONSTRAINT [FK_IngredientItemType];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkOrderWorkOrderItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkOrderItemSet] DROP CONSTRAINT [FK_WorkOrderWorkOrderItem];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkOrderItemItemType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkOrderItemSet] DROP CONSTRAINT [FK_WorkOrderItemItemType];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[LocationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LocationSet];
GO
IF OBJECT_ID(N'[dbo].[IngredientSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IngredientSet];
GO
IF OBJECT_ID(N'[dbo].[ItemSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemSet];
GO
IF OBJECT_ID(N'[dbo].[ItemTypeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemTypeSet];
GO
IF OBJECT_ID(N'[dbo].[WorkOrderSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkOrderSet];
GO
IF OBJECT_ID(N'[dbo].[WorkOrderItemSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkOrderItemSet];
GO
IF OBJECT_ID(N'[dbo].[LogEntrySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogEntrySet];
GO
IF OBJECT_ID(N'[dbo].[WasteReportSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WasteReportSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [PIN] smallint  NOT NULL,
    [IsAdmin] bit  NOT NULL,
    [IsActive] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL
);
GO

-- Creating table 'LocationSet'
CREATE TABLE [dbo].[LocationSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL
);
GO

-- Creating table 'IngredientSet'
CREATE TABLE [dbo].[IngredientSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Amount] bigint  NOT NULL,
    [Unit] nvarchar(max)  NOT NULL,
    [ForItemType_UID] bigint  NOT NULL,
    [ItemType_UID] bigint  NOT NULL
);
GO

-- Creating table 'ItemSet'
CREATE TABLE [dbo].[ItemSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Amount] bigint  NOT NULL,
    [ExpirationDate] datetime  NULL,
    [DeliveryDate] datetime  NOT NULL,
    [Supplier] nvarchar(max)  NOT NULL,
    [Reserved] bigint  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [Location_UID] bigint  NOT NULL,
    [ItemType_UID] bigint  NOT NULL
);
GO

-- Creating table 'ItemTypeSet'
CREATE TABLE [dbo].[ItemTypeSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [MinimumAmount] bigint  NOT NULL,
    [Unit] nvarchar(max)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [Procedure] nvarchar(max)  NULL,
    [Barcode] nvarchar(max)  NULL,
    [BatchSize] bigint  NULL,
    [Department] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WorkOrderSet'
CREATE TABLE [dbo].[WorkOrderSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [BatchNumber] bigint  NOT NULL,
    [DueDate] datetime  NOT NULL,
    [IsComplete] bit  NOT NULL,
    [ShippingInfo] nvarchar(max)  NULL,
    [CompletedByUser_UID] bigint  NULL
);
GO

-- Creating table 'WorkOrderItemSet'
CREATE TABLE [dbo].[WorkOrderItemSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Amount] bigint  NOT NULL,
    [Progress] bigint  NOT NULL,
    [WorkOrder_UID] bigint  NOT NULL,
    [ItemType_UID] bigint  NOT NULL
);
GO

-- Creating table 'LogEntrySet'
CREATE TABLE [dbo].[LogEntrySet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [LogBody] nvarchar(max)  NOT NULL,
    [ObjectData] nvarchar(max)  NOT NULL,
    [ObjectType] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WasteReportSet'
CREATE TABLE [dbo].[WasteReportSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Amount] bigint  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [User_UID] bigint  NOT NULL,
    [Item_UID] bigint  NOT NULL,
    [WorkOrder_UID] bigint  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UID] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- Creating primary key on [UID] in table 'LocationSet'
ALTER TABLE [dbo].[LocationSet]
ADD CONSTRAINT [PK_LocationSet]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- Creating primary key on [UID] in table 'IngredientSet'
ALTER TABLE [dbo].[IngredientSet]
ADD CONSTRAINT [PK_IngredientSet]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- Creating primary key on [UID] in table 'ItemSet'
ALTER TABLE [dbo].[ItemSet]
ADD CONSTRAINT [PK_ItemSet]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- Creating primary key on [UID] in table 'ItemTypeSet'
ALTER TABLE [dbo].[ItemTypeSet]
ADD CONSTRAINT [PK_ItemTypeSet]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- Creating primary key on [UID] in table 'WorkOrderSet'
ALTER TABLE [dbo].[WorkOrderSet]
ADD CONSTRAINT [PK_WorkOrderSet]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- Creating primary key on [UID] in table 'WorkOrderItemSet'
ALTER TABLE [dbo].[WorkOrderItemSet]
ADD CONSTRAINT [PK_WorkOrderItemSet]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- Creating primary key on [UID] in table 'LogEntrySet'
ALTER TABLE [dbo].[LogEntrySet]
ADD CONSTRAINT [PK_LogEntrySet]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- Creating primary key on [UID] in table 'WasteReportSet'
ALTER TABLE [dbo].[WasteReportSet]
ADD CONSTRAINT [PK_WasteReportSet]
    PRIMARY KEY CLUSTERED ([UID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Location_UID] in table 'ItemSet'
ALTER TABLE [dbo].[ItemSet]
ADD CONSTRAINT [FK_LocationItem]
    FOREIGN KEY ([Location_UID])
    REFERENCES [dbo].[LocationSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LocationItem'
CREATE INDEX [IX_FK_LocationItem]
ON [dbo].[ItemSet]
    ([Location_UID]);
GO

-- Creating foreign key on [ItemType_UID] in table 'ItemSet'
ALTER TABLE [dbo].[ItemSet]
ADD CONSTRAINT [FK_ItemTypeItem]
    FOREIGN KEY ([ItemType_UID])
    REFERENCES [dbo].[ItemTypeSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemTypeItem'
CREATE INDEX [IX_FK_ItemTypeItem]
ON [dbo].[ItemSet]
    ([ItemType_UID]);
GO

-- Creating foreign key on [User_UID] in table 'WasteReportSet'
ALTER TABLE [dbo].[WasteReportSet]
ADD CONSTRAINT [FK_WasteReportUser]
    FOREIGN KEY ([User_UID])
    REFERENCES [dbo].[UserSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WasteReportUser'
CREATE INDEX [IX_FK_WasteReportUser]
ON [dbo].[WasteReportSet]
    ([User_UID]);
GO

-- Creating foreign key on [Item_UID] in table 'WasteReportSet'
ALTER TABLE [dbo].[WasteReportSet]
ADD CONSTRAINT [FK_WasteReportItem]
    FOREIGN KEY ([Item_UID])
    REFERENCES [dbo].[ItemSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WasteReportItem'
CREATE INDEX [IX_FK_WasteReportItem]
ON [dbo].[WasteReportSet]
    ([Item_UID]);
GO

-- Creating foreign key on [WorkOrder_UID] in table 'WasteReportSet'
ALTER TABLE [dbo].[WasteReportSet]
ADD CONSTRAINT [FK_WasteReportWorkOrder]
    FOREIGN KEY ([WorkOrder_UID])
    REFERENCES [dbo].[WorkOrderSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WasteReportWorkOrder'
CREATE INDEX [IX_FK_WasteReportWorkOrder]
ON [dbo].[WasteReportSet]
    ([WorkOrder_UID]);
GO

-- Creating foreign key on [CompletedByUser_UID] in table 'WorkOrderSet'
ALTER TABLE [dbo].[WorkOrderSet]
ADD CONSTRAINT [FK_WorkOrderUser]
    FOREIGN KEY ([CompletedByUser_UID])
    REFERENCES [dbo].[UserSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkOrderUser'
CREATE INDEX [IX_FK_WorkOrderUser]
ON [dbo].[WorkOrderSet]
    ([CompletedByUser_UID]);
GO

-- Creating foreign key on [ForItemType_UID] in table 'IngredientSet'
ALTER TABLE [dbo].[IngredientSet]
ADD CONSTRAINT [FK_ItemTypeIngredient]
    FOREIGN KEY ([ForItemType_UID])
    REFERENCES [dbo].[ItemTypeSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemTypeIngredient'
CREATE INDEX [IX_FK_ItemTypeIngredient]
ON [dbo].[IngredientSet]
    ([ForItemType_UID]);
GO

-- Creating foreign key on [ItemType_UID] in table 'IngredientSet'
ALTER TABLE [dbo].[IngredientSet]
ADD CONSTRAINT [FK_IngredientItemType]
    FOREIGN KEY ([ItemType_UID])
    REFERENCES [dbo].[ItemTypeSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientItemType'
CREATE INDEX [IX_FK_IngredientItemType]
ON [dbo].[IngredientSet]
    ([ItemType_UID]);
GO

-- Creating foreign key on [WorkOrder_UID] in table 'WorkOrderItemSet'
ALTER TABLE [dbo].[WorkOrderItemSet]
ADD CONSTRAINT [FK_WorkOrderWorkOrderItem]
    FOREIGN KEY ([WorkOrder_UID])
    REFERENCES [dbo].[WorkOrderSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkOrderWorkOrderItem'
CREATE INDEX [IX_FK_WorkOrderWorkOrderItem]
ON [dbo].[WorkOrderItemSet]
    ([WorkOrder_UID]);
GO

-- Creating foreign key on [ItemType_UID] in table 'WorkOrderItemSet'
ALTER TABLE [dbo].[WorkOrderItemSet]
ADD CONSTRAINT [FK_WorkOrderItemItemType]
    FOREIGN KEY ([ItemType_UID])
    REFERENCES [dbo].[ItemTypeSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkOrderItemItemType'
CREATE INDEX [IX_FK_WorkOrderItemItemType]
ON [dbo].[WorkOrderItemSet]
    ([ItemType_UID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------