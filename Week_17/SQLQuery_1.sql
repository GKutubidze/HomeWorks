--Students
--1
SELECT * FROM Students WHERE YEAR(DoB) > 1990;
--2
SELECT Firstname, Lastname, Dob,Country from Students WHERE Country IN ('Georgia', 'Libya')

--3
INSERT INTO Students (Lastname, Firstname, DoB, Email, Quiz1, Quiz2, MiddleTest, FinalTest, Country)
VALUES('Doe', 'John', '1998-07-14','john@gmail.com', 90, 80, 12, 30, 'Georgia')

--4

select top (5) WITH TIES   Firstname,MiddleTest  from Students  ORDER BY MiddleTest DESC;


 --5
DELETE FROM Students OUTPUT deleted.Firstname, deleted.FinalTest WHERE FinalTest = 19;

--6

UPDATE Students SET FinalTest = 0 WHERE MiddleTest = 1;


--Persons


--1

SELECT * FROM Persons WHERE PrivateId LIKE '163%';
 

 --2

SELECT * FROM Persons WHERE Lastname=City


--3

SELECT *  FROM Persons  WHERE Country IN ('Canada', 'Monaco');


--4

SELECT Firstname,Lastname ,PersonsID FROM Persons  WHERE Email IS NULL OR Email = ''



--5

SELECT *  FROM Persons  WHERE Country IN ('Spain', 'Turkey') AND Salary BETWEEN 1000 AND 3000


--6

 SELECT WorkPlace
FROM Persons
WHERE WorkPlace LIKE '%LLC%' 
   OR WorkPlace LIKE '%PC%' 
   OR WorkPlace LIKE '%LLP%';


--7

SELECT 
    Email,
    CASE
        WHEN Email IS NULL THEN 'no email'
        WHEN (LEN(Email) - LEN(REPLACE(Email, '.', ''))) > 2 THEN 'more than 2 dots'
        ELSE 'less than 2 dots'
    END AS MAILINFO
FROM Persons;

 --8

SELECT * FROM Persons WHERE  PINcode LIKE '%51'


--9

Select Country,AVG(Salary) AS AverageSalary from Persons GROUP BY Country 




 
