USE tf_stock;

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(200) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    CONSTRAINT CK_User_Role CHECK (Role in ('User', 'Admin'))
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Quantity INT NOT NULL,
    Price INT DEFAULT 0,
    Name NVARCHAR(100) NOT NULL,
    LastModified DATETIME DEFAULT GETDATE(),
    Transactions INT DEFAULT 1,
    CONSTRAINT CK_Product_Quantity CHECK(Quantity>0),
    CONSTRAINT U_Product_Name UNIQUE (Name)
);

CREATE TABLE Transactions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Type VARCHAR(50) NOT NULL,
    Date DATETIME DEFAULT GETDATE(),
    Quantity INT NOT NULL,
    ProductId INT NOT NULL,
    UserId INT NOT NULL,
    CONSTRAINT CK_Transaction_Quantity CHECK(Quantity >0),
    CONSTRAINT CK_Transaction_Type CHECK (Type IN ('Input', 'Output', 'Update')),
    CONSTRAINT FK_Transaction_Product FOREIGN KEY (ProductId) REFERENCES Products(Id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT FK_Transaction_User FOREIGN KEY (UserId) REFERENCES Users(Id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
/********* TRIGGERS *********/
go;
/*Last Modified trigger */
CREATE TRIGGER OnUpdateProduct ON Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT UPDATE(LastModified) 
    UPDATE your_table
    SET LastModified = GETDATE(), [Transaction] = COALESCE([Transaction], 0) + 1
    FROM Products
    WHERE Products.Id = inserted.Id;
END;

go;