
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/21/2016 14:47:08
-- Generated from EDMX file: C:\Users\Jens\Documents\ACLager\ACLager\Models\Model.edmx
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

IF OBJECT_ID(N'[dbo].[FK_IngredientItemType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientSet] DROP CONSTRAINT [FK_IngredientItemType];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemItemType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ItemSet] DROP CONSTRAINT [FK_ItemItemType];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemLocation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ItemSet] DROP CONSTRAINT [FK_ItemLocation];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkOrderUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkOrderSet] DROP CONSTRAINT [FK_WorkOrderUser];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkOrderItemWorkOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkOrderItemSet] DROP CONSTRAINT [FK_WorkOrderItemWorkOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkOrderItemItemType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkOrderItemSet] DROP CONSTRAINT [FK_WorkOrderItemItemType];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkOrderWasteReport]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WasteReportSet] DROP CONSTRAINT [FK_WorkOrderWasteReport];
GO
IF OBJECT_ID(N'[dbo].[FK_WasteReportItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WasteReportSet] DROP CONSTRAINT [FK_WasteReportItem];
GO
IF OBJECT_ID(N'[dbo].[FK_WasteReportUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WasteReportSet] DROP CONSTRAINT [FK_WasteReportUser];
GO
IF OBJECT_ID(N'[dbo].[FK_IngredientItemType1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IngredientSet] DROP CONSTRAINT [FK_IngredientItemType1];
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
    [IsActive] bit  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsAdmin] bit  NOT NULL,
    [PIN] smallint  NOT NULL
);
GO

-- Creating table 'LocationSet'
CREATE TABLE [dbo].[LocationSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'IngredientSet'
CREATE TABLE [dbo].[IngredientSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Amount] bigint  NOT NULL,
    [Unit] nvarchar(max)  NOT NULL,
    [IngredientForUID] bigint  NOT NULL,
    [TypeUID] bigint  NOT NULL
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
    [ItemTypeUID] bigint  NOT NULL,
    [Location_UID] bigint  NOT NULL
);
GO

-- Creating table 'ItemTypeSet'
CREATE TABLE [dbo].[ItemTypeSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [IsActive] bit  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [MinimumAmount] bigint  NOT NULL,
    [Unit] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WorkOrderSet'
CREATE TABLE [dbo].[WorkOrderSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [BatchNumber] bigint  NOT NULL,
    [DueDate] datetime  NOT NULL,
    [IsComplete] bit  NOT NULL,
    [UserUID] bigint  NULL
);
GO

-- Creating table 'WorkOrderItemSet'
CREATE TABLE [dbo].[WorkOrderItemSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Amount] bigint  NOT NULL,
    [Progress] bigint  NOT NULL,
    [WorkOrderUID] bigint  NOT NULL,
    [ItemTypeUID] bigint  NOT NULL
);
GO

-- Creating table 'LogEntrySet'
CREATE TABLE [dbo].[LogEntrySet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [LogBody] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WasteReportSet'
CREATE TABLE [dbo].[WasteReportSet] (
    [UID] bigint IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Amount] bigint  NOT NULL,
    [WorkOrderUID] bigint  NULL,
    [ItemUID] bigint  NOT NULL,
    [UserUID] bigint  NOT NULL
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

-- Creating foreign key on [IngredientForUID] in table 'IngredientSet'
ALTER TABLE [dbo].[IngredientSet]
ADD CONSTRAINT [FK_IngredientItemType]
    FOREIGN KEY ([IngredientForUID])
    REFERENCES [dbo].[ItemTypeSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientItemType'
CREATE INDEX [IX_FK_IngredientItemType]
ON [dbo].[IngredientSet]
    ([IngredientForUID]);
GO

-- Creating foreign key on [ItemTypeUID] in table 'ItemSet'
ALTER TABLE [dbo].[ItemSet]
ADD CONSTRAINT [FK_ItemItemType]
    FOREIGN KEY ([ItemTypeUID])
    REFERENCES [dbo].[ItemTypeSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemItemType'
CREATE INDEX [IX_FK_ItemItemType]
ON [dbo].[ItemSet]
    ([ItemTypeUID]);
GO

-- Creating foreign key on [Location_UID] in table 'ItemSet'
ALTER TABLE [dbo].[ItemSet]
ADD CONSTRAINT [FK_ItemLocation]
    FOREIGN KEY ([Location_UID])
    REFERENCES [dbo].[LocationSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemLocation'
CREATE INDEX [IX_FK_ItemLocation]
ON [dbo].[ItemSet]
    ([Location_UID]);
GO

-- Creating foreign key on [UserUID] in table 'WorkOrderSet'
ALTER TABLE [dbo].[WorkOrderSet]
ADD CONSTRAINT [FK_WorkOrderUser]
    FOREIGN KEY ([UserUID])
    REFERENCES [dbo].[UserSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkOrderUser'
CREATE INDEX [IX_FK_WorkOrderUser]
ON [dbo].[WorkOrderSet]
    ([UserUID]);
GO

-- Creating foreign key on [WorkOrderUID] in table 'WorkOrderItemSet'
ALTER TABLE [dbo].[WorkOrderItemSet]
ADD CONSTRAINT [FK_WorkOrderItemWorkOrder]
    FOREIGN KEY ([WorkOrderUID])
    REFERENCES [dbo].[WorkOrderSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkOrderItemWorkOrder'
CREATE INDEX [IX_FK_WorkOrderItemWorkOrder]
ON [dbo].[WorkOrderItemSet]
    ([WorkOrderUID]);
GO

-- Creating foreign key on [ItemTypeUID] in table 'WorkOrderItemSet'
ALTER TABLE [dbo].[WorkOrderItemSet]
ADD CONSTRAINT [FK_WorkOrderItemItemType]
    FOREIGN KEY ([ItemTypeUID])
    REFERENCES [dbo].[ItemTypeSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkOrderItemItemType'
CREATE INDEX [IX_FK_WorkOrderItemItemType]
ON [dbo].[WorkOrderItemSet]
    ([ItemTypeUID]);
GO

-- Creating foreign key on [WorkOrderUID] in table 'WasteReportSet'
ALTER TABLE [dbo].[WasteReportSet]
ADD CONSTRAINT [FK_WorkOrderWasteReport]
    FOREIGN KEY ([WorkOrderUID])
    REFERENCES [dbo].[WorkOrderSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkOrderWasteReport'
CREATE INDEX [IX_FK_WorkOrderWasteReport]
ON [dbo].[WasteReportSet]
    ([WorkOrderUID]);
GO

-- Creating foreign key on [ItemUID] in table 'WasteReportSet'
ALTER TABLE [dbo].[WasteReportSet]
ADD CONSTRAINT [FK_WasteReportItem]
    FOREIGN KEY ([ItemUID])
    REFERENCES [dbo].[ItemSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WasteReportItem'
CREATE INDEX [IX_FK_WasteReportItem]
ON [dbo].[WasteReportSet]
    ([ItemUID]);
GO

-- Creating foreign key on [UserUID] in table 'WasteReportSet'
ALTER TABLE [dbo].[WasteReportSet]
ADD CONSTRAINT [FK_WasteReportUser]
    FOREIGN KEY ([UserUID])
    REFERENCES [dbo].[UserSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WasteReportUser'
CREATE INDEX [IX_FK_WasteReportUser]
ON [dbo].[WasteReportSet]
    ([UserUID]);
GO

-- Creating foreign key on [TypeUID] in table 'IngredientSet'
ALTER TABLE [dbo].[IngredientSet]
ADD CONSTRAINT [FK_IngredientItemType1]
    FOREIGN KEY ([TypeUID])
    REFERENCES [dbo].[ItemTypeSet]
        ([UID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_IngredientItemType1'
CREATE INDEX [IX_FK_IngredientItemType1]
ON [dbo].[IngredientSet]
    ([TypeUID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------