--Napisać SQL który zwraca klientów i ich wartość sprzedaży w całym poprzednim miesiącu
--Wynikiem zadania jest gotowe zapytanie SQL.

USE TestSubiektaGt
GO
--Niestety w poprzednim miesiącu brak sprzedaży :(. Koronowirus jest.
SELECT khk.kh_Symbol AS Symbol, khk.kh_Pracownik AS Pracownik, SUM(vwd.ob_WartNetto) AS WartNetto, SUM(vwd.ob_WartBrutto) AS WartBrutto FROM vwDokumenty vwd
INNER JOIN kh__Kontrahent khk ON khk.kh_Id = vwd.dok_PlatnikId
WHERE
1=1
AND vwd.dok_nrpelny LIKE N'%FS%'
AND CONVERT(VARCHAR, vwd.dok_DataWyst, 23) BETWEEN 
	CONVERT(VARCHAR, FORMAT(DATEADD(MONTH, DATEDIFF(month, 0, DATEADD(MONTH, -1, GETDATE())), 0), 'yyyy-MM-dd'), 23) AND 
	CONVERT(VARCHAR, FORMAT(EOMONTH(DATEADD(MONTH, -1, GETDATE())), 'yyyy-MM-dd'), 23)
GROUP BY vwd.dok_PlatnikId, khk.kh_Id, khk.kh_Symbol, khk.kh_Pracownik
GO
--Ale w marcu.
SELECT khk.kh_Symbol AS Symbol, khk.kh_Pracownik AS Pracownik, SUM(vwd.ob_WartNetto) AS WartNetto, SUM(vwd.ob_WartBrutto) AS WartBrutto FROM vwDokumenty vwd
INNER JOIN kh__Kontrahent khk ON khk.kh_Id = vwd.dok_PlatnikId
WHERE
1=1
AND vwd.dok_nrpelny LIKE N'%FS%'
AND CONVERT(VARCHAR, vwd.dok_DataWyst, 23) BETWEEN CONVERT(VARCHAR, '2020-03-01', 23) AND CONVERT(VARCHAR, '2020-03-31', 23)
GROUP BY vwd.dok_PlatnikId, khk.kh_Id, khk.kh_Symbol, khk.kh_Pracownik
GO
--Lub Tak w marcu.
SELECT khk.kh_Symbol AS Symbol, khk.kh_Pracownik AS Pracownik, SUM(vwd.ob_WartNetto) AS WartNetto, SUM(vwd.ob_WartBrutto) AS WartBrutto FROM vwDokumenty vwd
INNER JOIN kh__Kontrahent khk ON khk.kh_Id = vwd.dok_PlatnikId
WHERE
1=1
AND vwd.dok_nrpelny LIKE N'%FS%'
AND CONVERT(VARCHAR, vwd.dok_DataWyst, 23) BETWEEN 
	CONVERT(VARCHAR,FORMAT(DATEADD(MONTH, DATEDIFF(month, 0, DATEADD(MONTH, -2, GETDATE())), 0), 'yyyy-MM-dd'), 23) AND 
	CONVERT(VARCHAR,FORMAT(EOMONTH(DATEADD(MONTH, -2, GETDATE())), 'yyyy-MM-dd'), 23)
GROUP BY vwd.dok_PlatnikId, khk.kh_Id, khk.kh_Symbol, khk.kh_Pracownik
GO