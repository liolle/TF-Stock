
/** Add new product **/ 
CREATE PROCEDURE InsertProductAndTransaction
    @Quantity INT, 
    @Price INT, 
    @Name NVARCHAR(100), 
    @UserId INT 
AS
BEGIN

    IF @Quantity <=0 OR @Quantity >1000
        THROW 10, 'Invalid Quantity (Must be 0 <= Quantity <= 1000)', 2;

    DECLARE @ProductId INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO [Products] (Quantity, Price, [Name])
        VALUES (@Quantity, @Price, @Name);

        SET @ProductId = SCOPE_IDENTITY();

        INSERT INTO [Transactions] ([Type], Quantity, ProductId, UserId)
        VALUES ('Input', @Quantity, @ProductId, @UserId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; -- Re-throw the error
    END CATCH
END;

go;
/** Add existing product **/ 
CREATE PROCEDURE AddProductAndTransaction
    @Quantity INT, 
    @UserId INT,
    @ProductId INT 
AS
BEGIN
    IF @Quantity <=0 OR @Quantity >1000
        THROW 10, 'Invalid Quantity (Must be 0 <= Quantity <= 1000)', 2;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE [Products]
        SET [Quantity] = [Quantity] + @Quantity

        INSERT INTO [Transactions] ([Type], Quantity, ProductId, UserId)
        VALUES ('Input', @Quantity, @ProductId, @UserId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; -- Re-throw the error
    END CATCH
END;

go;

/** Consume Products **/
CREATE PROCEDURE OutputProductAndTransaction
    @Quantity INT, 
    @UserId INT,
    @ProductId INT 
AS
BEGIN
    IF @Quantity <=0 OR @Quantity >1000
        THROW 10, 'Invalid Quantity (Must be 0 <= Quantity <= 1000)', 2;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE [Products]
        SET [Quantity] = [Quantity] - @Quantity

        INSERT INTO [Transactions] ([Type], Quantity, ProductId, UserId)
        VALUES ('Output', @Quantity, @ProductId, @UserId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; -- Re-throw the error
    END CATCH
END;

go;

/** Inventory update **/
CREATE PROCEDURE ChangeProductAndTransaction
    @Quantity INT, 
    @UserId INT,
    @ProductId INT 
AS
BEGIN

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE [Products]
        SET [Quantity] = @Quantity

        INSERT INTO [Transactions] ([Type], Quantity, ProductId, UserId)
        VALUES ('Update', @Quantity, @ProductId, @UserId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; -- Re-throw the error
    END CATCH
END;