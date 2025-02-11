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