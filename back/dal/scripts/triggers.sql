/*Last Modified trigger */
CREATE TRIGGER OnUpdateProduct ON Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT UPDATE(LastModified) 
    /* Join Update */
    UPDATE P
    SET 
        P.LastModified = GETDATE(),
        P.Transactions = COALESCE(P.Transactions, 0) + 1
    FROM Products P
    INNER JOIN inserted I ON P.Id = I.Id;
END;