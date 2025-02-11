USE tf_stock;

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName VARCHAR(100) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(200) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    CONSTRAINT CK_User_Role CHECK (Role in ('User', 'Admin')),
    CONSTRAINT U_User_UserName UNIQUE(UserName),
    CONSTRAINT DF_User_Role DEFAULT 'User' FOR Role
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Quantity INT NOT NULL,
    GTIN VARCHAR(14) NOT NULL,
    Price INT DEFAULT 0,
    Name NVARCHAR(100) NOT NULL,
    LastModified DATETIME DEFAULT GETDATE(),
    Transactions INT DEFAULT 1,
    CONSTRAINT CK_Product_Quantity CHECK(Quantity>0),
    CONSTRAINT U_Product_GTIN UNIQUE(GTIN),
    CONSTRAINT CK_Product_GTIN CHECK(
        LEN(GTIN) IN (8, 12, 13, 14) 
        AND GTIN NOT LIKE '%[^0-9]%'
    ),
    CONSTRAINT CK_Product_Price CHECK (Price >=0)
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
