IF EXISTS(SELECT 1
        FROM   INFORMATION_SCHEMA.ROUTINES
        WHERE  ROUTINE_NAME = 'sp_Test'
                AND SPECIFIC_SCHEMA = 'dbo')
BEGIN
    DROP PROCEDURE sp_Test
END
go

create procedure sp_Test 
as
select 1
