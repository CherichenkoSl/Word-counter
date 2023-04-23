/* Первый запрос*/
SELECT *
FROM EMPLOYEE
WHERE salary =(SELECT MAX(salary) FROM EMPLOYEE);

/* Третий запрос*/
WITH req_1 AS (
	SELECT department_id, SUM(salary) AS total_salary
	FROM EMPLOYEE
	GROUP BY department_id
), 
	req_2 AS (
	SELECT *
	FROM req_1
	WHERE total_salary = (SELECT MAX(total_salary) FROM req_1)
)
SELECT d.id, d.name, g.total_salary
FROM req_2 AS g
LEFT JOIN DEPARTMENT as d
ON g.department_id = d.id;

/* Четвертый запрос*/
SELECT name
FROM EMPLOYEE
WHERE name like 'Р%н';