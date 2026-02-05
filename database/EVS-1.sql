-- Feature: EVS-1
-- Author: Adalid
-- Purpose: Create a new database
-- Create a database named EmailVerifyServiceDB
CREATE DATABASE EmailVerifyServiceDB  
GO  
  
-- Create a table named OperationLog within the EmailVerifyServiceDB database to store operation logs
CREATE TABLE EmailVerifyServiceDB.dbo.OperationLog (  
    -- Unique identifier for each operation, auto-incremented starting from 1 with an increment of 1
    OperationID INT IDENTITY(1,1) PRIMARY KEY,  
    -- Timestamp when the operation occurred
    OperationDate DATETIME,           
    -- Request data in NVARCHAR format to accommodate large amounts of text
    Request NVARCHAR(MAX),            
    -- Response data in NVARCHAR format to store results from operations
    Response NVARCHAR(MAX)  
);  
  
-- Create a table named StatusCodes within the EmailVerifyServiceDB database to store status descriptions
CREATE TABLE EmailVerifyServiceDB.dbo.StatusCodes (  
    -- Unique identifier for each status, auto-incremented starting from 1 with an increment of 1
    ID INT IDENTITY(1,1) PRIMARY KEY,  
    -- Description of the status in NVARCHAR format
    StatusDescription NVARCHAR(100) NOT NULL 
);  
  
-- Insert predefined statuses into the StatusCodes table. These are static values that represent different states or outcomes.
INSERT INTO EmailVerifyServiceDB.dbo.StatusCodes (StatusDescription)  
VALUES ('Pending'), ('Verified'), ('Expired');  
  
-- Create a table named VerifyCodes within the EmailVerifyServiceDB database to store verification codes
CREATE TABLE EmailVerifyServiceDB.dbo.VerifyCodes (  
    -- Unique identifier for each verify code, auto-incremented starting from 1 with an increment of 1
    ID INT IDENTITY(1,1) PRIMARY KEY,  
    -- Verification code itself as NVARCHAR format limited to 20 characters
    Code NVARCHAR(20) NOT NULL,           
    -- Token used for verification purposes in UNIQUEIDENTIFIER format
    Token UNIQUEIDENTIFIER NOT NULL,     
    -- TokenApp used for verification app purposes in UNIQUEIDENTIFIER format
    AppToken UNIQUEIDENTIFIER NOT NULL,           
    -- Email address associated with the verification code in NVARCHAR format
    EmailAddress NVARCHAR(100) NOT NULL,          
    -- Timestamp when the verification operation occurred
    OperationDate DATETIME NOT NULL,           
    -- Expiration timestamp for the verification code in DATETIME format
    ExpirationDate DATETIME NOT NULL,  
    -- Default status of the verification code, referencing the ID from StatusCodes table
    VerifyStatus INT DEFAULT 1  
    FOREIGN KEY (VerifyStatus) REFERENCES StatusCodes(ID)  
);