
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/18/2013 19:30:59
-- Generated from EDMX file: C:\Users\Minardus\Documents\Visual Studio 2012\Projects\StackUnderflow\stackunderflow\MvcApplicationDatabase\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [StackOverflowDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_BelongsToQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posts] DROP CONSTRAINT [FK_BelongsToQuestion];
GO
IF OBJECT_ID(N'[dbo].[FK_BestAnswerPost_id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Questions] DROP CONSTRAINT [FK_BestAnswerPost_id];
GO
IF OBJECT_ID(N'[StackOverflowDatabaseModelStoreContainer].[FK_Tagged_Questions_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [StackOverflowDatabaseModelStoreContainer].[Tagged_Questions] DROP CONSTRAINT [FK_Tagged_Questions_ToTable];
GO
IF OBJECT_ID(N'[StackOverflowDatabaseModelStoreContainer].[FK_Tagged_Questions_ToTable_1]', 'F') IS NOT NULL
    ALTER TABLE [StackOverflowDatabaseModelStoreContainer].[Tagged_Questions] DROP CONSTRAINT [FK_Tagged_Questions_ToTable_1];
GO
IF OBJECT_ID(N'[dbo].[FK_Post_id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Post_id];
GO
IF OBJECT_ID(N'[dbo].[FK_ReplyToPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posts] DROP CONSTRAINT [FK_ReplyToPost];
GO
IF OBJECT_ID(N'[dbo].[FK_User_id]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_User_id];
GO
IF OBJECT_ID(N'[dbo].[FK_UserWhoPosted]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posts] DROP CONSTRAINT [FK_UserWhoPosted];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Posts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posts];
GO
IF OBJECT_ID(N'[dbo].[Questions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Questions];
GO
IF OBJECT_ID(N'[StackOverflowDatabaseModelStoreContainer].[Tagged_Questions]', 'U') IS NOT NULL
    DROP TABLE [StackOverflowDatabaseModelStoreContainer].[Tagged_Questions];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Comment_Id] int  NOT NULL,
    [Post_id] int  NOT NULL,
    [User_id] int  NOT NULL,
    [Content] varchar(max)  NULL,
    [DateCreated] datetime  NULL,
    [Votes] int  NULL
);
GO

-- Creating table 'Posts'
CREATE TABLE [dbo].[Posts] (
    [Post_id] int IDENTITY(1,1) NOT NULL,
    [Question_id] int  NULL,
    [ReplyToPost_id] int  NULL,
    [Content] varchar(max)  NOT NULL,
    [Votes] int  NULL,
    [DateCreated] datetime  NULL,
    [User_id] int  NULL
);
GO

-- Creating table 'Questions'
CREATE TABLE [dbo].[Questions] (
    [Question_id] int IDENTITY(1,1) NOT NULL,
    [BestAnswer_id] int  NULL,
    [DateCreated] datetime  NULL,
    [Title] varchar(150)  NOT NULL,
    [Views] int  NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [Tag_id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(25)  NOT NULL,
    [Summary] varchar(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [User_id] int IDENTITY(1,1) NOT NULL,
    [FirstName] varchar(50)  NULL,
    [LastName] varchar(50)  NULL,
    [Password] varchar(255)  NULL,
    [Email] varchar(50)  NULL,
    [DateCreated] datetime  NULL,
    [PermissionLEvel] int  NOT NULL,
    [Votes] int  NOT NULL,
    [Username] varchar(50)  NULL,
    [Address] varchar(50)  NULL,
    [Zip] varchar(6)  NULL,
    [Photo] varchar(255)  NULL
);
GO

-- Creating table 'Tagged_Questions'
CREATE TABLE [dbo].[Tagged_Questions] (
    [Questions_Question_id] int  NOT NULL,
    [Tags_Tag_id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Comment_Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Comment_Id] ASC);
GO

-- Creating primary key on [Post_id] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [PK_Posts]
    PRIMARY KEY CLUSTERED ([Post_id] ASC);
GO

-- Creating primary key on [Question_id] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [PK_Questions]
    PRIMARY KEY CLUSTERED ([Question_id] ASC);
GO

-- Creating primary key on [Tag_id] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([Tag_id] ASC);
GO

-- Creating primary key on [User_id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([User_id] ASC);
GO

-- Creating primary key on [Questions_Question_id], [Tags_Tag_id] in table 'Tagged_Questions'
ALTER TABLE [dbo].[Tagged_Questions]
ADD CONSTRAINT [PK_Tagged_Questions]
    PRIMARY KEY NONCLUSTERED ([Questions_Question_id], [Tags_Tag_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Post_id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Post_id]
    FOREIGN KEY ([Post_id])
    REFERENCES [dbo].[Posts]
        ([Post_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Post_id'
CREATE INDEX [IX_FK_Post_id]
ON [dbo].[Comments]
    ([Post_id]);
GO

-- Creating foreign key on [User_id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_User_id]
    FOREIGN KEY ([User_id])
    REFERENCES [dbo].[Users]
        ([User_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_User_id'
CREATE INDEX [IX_FK_User_id]
ON [dbo].[Comments]
    ([User_id]);
GO

-- Creating foreign key on [Question_id] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_BelongsToQuestion]
    FOREIGN KEY ([Question_id])
    REFERENCES [dbo].[Questions]
        ([Question_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BelongsToQuestion'
CREATE INDEX [IX_FK_BelongsToQuestion]
ON [dbo].[Posts]
    ([Question_id]);
GO

-- Creating foreign key on [BestAnswer_id] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [FK_BestAnswerPost_id]
    FOREIGN KEY ([BestAnswer_id])
    REFERENCES [dbo].[Posts]
        ([Post_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BestAnswerPost_id'
CREATE INDEX [IX_FK_BestAnswerPost_id]
ON [dbo].[Questions]
    ([BestAnswer_id]);
GO

-- Creating foreign key on [ReplyToPost_id] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_ReplyToPost]
    FOREIGN KEY ([ReplyToPost_id])
    REFERENCES [dbo].[Posts]
        ([Post_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ReplyToPost'
CREATE INDEX [IX_FK_ReplyToPost]
ON [dbo].[Posts]
    ([ReplyToPost_id]);
GO

-- Creating foreign key on [User_id] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_UserWhoPosted]
    FOREIGN KEY ([User_id])
    REFERENCES [dbo].[Users]
        ([User_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserWhoPosted'
CREATE INDEX [IX_FK_UserWhoPosted]
ON [dbo].[Posts]
    ([User_id]);
GO

-- Creating foreign key on [Questions_Question_id] in table 'Tagged_Questions'
ALTER TABLE [dbo].[Tagged_Questions]
ADD CONSTRAINT [FK_Tagged_Questions_Questions]
    FOREIGN KEY ([Questions_Question_id])
    REFERENCES [dbo].[Questions]
        ([Question_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tags_Tag_id] in table 'Tagged_Questions'
ALTER TABLE [dbo].[Tagged_Questions]
ADD CONSTRAINT [FK_Tagged_Questions_Tags]
    FOREIGN KEY ([Tags_Tag_id])
    REFERENCES [dbo].[Tags]
        ([Tag_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Tagged_Questions_Tags'
CREATE INDEX [IX_FK_Tagged_Questions_Tags]
ON [dbo].[Tagged_Questions]
    ([Tags_Tag_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------