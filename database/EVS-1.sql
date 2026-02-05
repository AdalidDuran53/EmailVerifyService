-- Feature: WAPB-2 
-- Author: Adalid
-- Purpose: Create a new database
CREATE DATABASE WebApiProjectBaseDB
GO

USE WebApiProjectBaseDB;
CREATE TABLE WebApiProjectBaseDB.dbo.Users (
    UserID UNIQUEIDENTIFIER PRIMARY KEY, -- UserID, must be UNIQUEIDENTIFIER
    UserName NVARCHAR(50) UNIQUE,         -- User name, must be UNIQUE
    PasswordHash NVARCHAR(MAX) NOT NULL,          -- PasswordHash, required
    PasswordSalst NVARCHAR(MAX) NOT NULL,          -- PasswordHash, required
    isDeleted BIT DEFAULT 0 -- isDeleted, DEFAULT 0 => isDeleted = false
);

CREATE TABLE WebApiProjectBaseDB.dbo.SessionLog (
    SessionID UNIQUEIDENTIFIER PRIMARY KEY, 
    UserID UNIQUEIDENTIFIER,         -- Foreign key
    InitSession DATETIME NOT NULL,          
    EndSession DATETIME 
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE WebApiProjectBaseDB.dbo.OperationLog (
    OperationID INT IDENTITY(1,1) PRIMARY KEY, 
    SessionID UNIQUEIDENTIFIER,      -- Foreign key 
    OperationDate DATETIME,         
    Request NVARCHAR(MAX),          
    Response NVARCHAR(MAX)
    FOREIGN KEY (SessionID) REFERENCES SessionLog(SessionID)
);