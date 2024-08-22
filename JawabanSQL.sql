--2.1
SELECT S.City, SUM(T.Amount) AS Total_Amount
FROM Suppliers S
JOIN TransaksiOrder T ON S.SupplierCode = T.SupplierCode
GROUP BY S.City;

--2.2
SELECT s.SupplierCode, s.SupplierName, SUM(t.Amount) AS Total_Amount_Jan2019
FROM Suppliers s
JOIN TransaksiOrder t ON s.SupplierCode = t.SupplierCode
WHERE t.OrderDate BETWEEN '2019-01-01' AND '2019-01-31'
GROUP BY s.SupplierCode, s.SupplierName;

--2.3
SELECT s.SupplierCode, s.SupplierName, MAX(t.OrderDate) AS Last_Transaction_Date
FROM Suppliers s
JOIN TransaksiOrder t ON s.SupplierCode = t.SupplierCode
GROUP BY s.SupplierCode, s.SupplierName;

--2.4
SELECT t.*
FROM Suppliers s
JOIN TransaksiOrder t ON s.SupplierCode = t.SupplierCode
WHERE s.Province = 'Jawa Barat' AND t.Amount > 30000000;

--2.5
SELECT s.SupplierCode, s.SupplierName, SUM(t.Amount) AS Total_Amount
FROM Suppliers s
JOIN TransaksiOrder t ON s.SupplierCode = t.SupplierCode
GROUP BY s.SupplierCode, s.SupplierName
ORDER BY SUM(t.Amount) DESC, s.SupplierCode ASC;

