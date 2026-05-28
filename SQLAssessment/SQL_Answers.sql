SELECT * FROM dbo.Employees

SELECT * FROM dbo.Payroll

--1. Retrieve the top 3 employees with the highest total net salary in the year 2024, along with their department.
SELECT TOP 3 
E.employee_id, -- added employee_id in case there's a same name in the same department
E.name,
E.department, 
SUM(P.net_salary) as total_net_salary
FROM dbo.Employees E
INNER JOIN dbo.Payroll P ON E.employee_id = P.employee_id
WHERE YEAR(P.pay_date) = 2024 
GROUP BY E.employee_id, E.name, E.department
ORDER BY total_net_salary DESC;

--2. Show each department with total gross salary, total tax deducted, and average net salary.
SELECT E.department, 
SUM(P.gross_salary) as total_gross_salary,
SUM(P.tax_amount) as total_tax_deducted,
AVG(P.net_salary)  as average_net_salary
FROM dbo.Employees E
INNER JOIN dbo.Payroll P ON E.employee_id = P.employee_id
WHERE YEAR(P.pay_date) = 2024
GROUP BY E.department;

--3. List all employees who have not received any payroll in 2024.
SELECT
E.employee_id,
E.name,
E.department
FROM dbo.Employees E
WHERE NOT EXISTS (
	SELECT 1 
	FROM dbo.Payroll P 
	WHERE P.employee_id = E.employee_id 
	AND YEAR(P.pay_date) = 2024
);

-- 4. Return each employee’s most recent pay date and net salary.
SELECT
    E.employee_id,
    E.name,
    E.department,
    P.pay_date,
    P.net_salary
FROM dbo.Employees E
OUTER APPLY (
    SELECT TOP 1
        pay_date,
        net_salary
    FROM dbo.Payroll
    WHERE employee_id = E.employee_id
    ORDER BY pay_date DESC
) P;


-- 5. Which columns would you index in Payroll to optimize queries filtering by pay_date and aggregating by employee_id? Why?
CREATE INDEX IX_Payroll_PayDate_Employee
ON dbo.Payroll
(
    Pay_Date,
    Employee_ID
);
--Pay_Date improves filtering
--Employee_ID improves grouping
--Reduces table scans
--Helps payroll reports

-- 6. Explain why this query might be inefficient and provide an optimized alternative:
	-- SELECT * FROM Payroll WHERE YEAR(pay_date) = 2024;
   
    /* YEAR(pay_date) applies a function to every row in the pay_date column.
    SQL Server may perform an Index Scan because it needs to check all records,
    extract the year value, and compare it to 2024. This can reduce performance. */
SELECT *
FROM Payroll
WHERE Pay_Date >= '2024-01-01'
AND Pay_Date < '2025-01-01';
/*This query uses a date range directly on the pay_date column,
allowing SQL Server to use an Index Seek and search only the required records, which improves performance. */


-- 7. Rank employees by total net salary within their department using a window function.
SELECT 
X.employee_id,
X.name,
X.department,
X.total_net_salary,
RANK() OVER (PARTITION BY X.department ORDER BY X.Total_Net_Salary DESC) AS salary_rank
FROM (
    SELECT
    E.employee_id,
    E.name,
    E.department,
    SUM(P.Net_Salary) AS Total_Net_Salary
    FROM Employees E
    INNER JOIN Payroll P
        ON E.Employee_ID = P.Employee_ID
    GROUP BY
        E.employee_id,
        E.name,
        E.department
) X;