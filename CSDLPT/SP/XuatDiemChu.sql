﻿ALTER FUNCTION  XuatDiemChu
	(@DIEM FLOAT)
	RETURNS NVARCHAR(10)
AS
BEGIN
DECLARE @RESULT NVARCHAR(10)
SET @RESULT = ''
IF (@DIEM = 0) SET @RESULT = N'Không' 
ELSE IF (@DIEM = 1) SET @RESULT = N'Một' 
ELSE IF (@DIEM = 2) SET @RESULT = N'Hai' 
ELSE IF (@DIEM = 3) SET @RESULT = N'Ba' 
ELSE IF (@DIEM = 4) SET @RESULT = N'Bốn' 
ELSE IF (@DIEM = 5) SET @RESULT = N'Năm' 
ELSE IF (@DIEM = 6) SET @RESULT = N'Sáu' 
ELSE IF (@DIEM = 7) SET @RESULT = N'Bảy' 
ELSE IF (@DIEM = 8) SET @RESULT = N'Tám' 
ELSE IF (@DIEM = 9) SET @RESULT = N'Chín' 
ELSE IF (@DIEM = 10) SET @RESULT = N'Mười'
RETURN @RESULT 
END
