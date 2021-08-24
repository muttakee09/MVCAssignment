SELECT * FROM Students;
UPDATE Students SET Image=NULL WHERE Id = 12;
INSERT Courses(CourseName, CourseCredit, CourseCode) VALUES('Database', 3.00, 'CS211'),
													('OOP', 3.00, 'CS201'),
													('Complier', 3.00, 'CS233'),
													('Tethics', 2.00, 'HUM400'),
													('Sociology', 2.00, 'HUM350');