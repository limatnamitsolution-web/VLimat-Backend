CREATE PROCEDURE mst.usp_vklmt_MasterConfigs_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM mst.vklmt_MasterConfigs
    WHERE Id = @Id;
END;