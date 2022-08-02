DECLARE @sqlCommand NVARCHAR(1000)

DECLARE @dateTime NVARCHAR(20)

SELECT @dateTime = REPLACE(CONVERT(VARCHAR, GETDATE(),111),'/','') +

REPLACE(CONVERT(VARCHAR, GETDATE(),108),':','')

SET @sqlCommand = 'BACKUP DATABASE ' + 'StockWarehouse' +



' TO DISK = ''D:\Git\20211128\StockStrategy\SynologyDrive\Backup\' + 'StockWarehouse' + '_Full_' + @dateTime + '.BAK'''

DECLARE @OLDDATE DATETIME

SELECT @OLDDATE=GETDATE()-2

EXECUTE sp_executesql @sqlCommand

EXECUTE master.dbo.xp_delete_file 0,N'D:\Git\20211128\StockStrategy\SynologyDrive\Backup\',N'bak',@olddate

 