CREATE PROCEDURE mst.usp_vklmt_MasterConfigs_GetByAcademicAndConfiguration
    @AcademicId INT,
    @Configuration NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM mst.vklmt_MasterConfigs
    WHERE AcademicId = @AcademicId
      AND Configuration = @Configuration
      AND IsActive = 1
    ORDER BY SortOrder;
END;