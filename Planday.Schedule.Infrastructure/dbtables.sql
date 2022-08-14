CREATE TABLE Employee (
   Id INTEGER PRIMARY KEY AUTOINCREMENT,
   Name text NOT NULL
);

CREATE TABLE Shift (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	EmployeeId INTEGER,
	Start TEXT NOT NULL, --SQLite doesn't support DateTime types
	End TEXT NOT NULL, --SQLite doesn't support DateTime types
	FOREIGN KEY(EmployeeId) REFERENCES Employee(Id)
);

INSERT INTO Employee (Name) VALUES ('John Doe');
INSERT INTO Employee (Name) VALUES ('Jane Doe');

INSERT INTO Shift (EmployeeId, Start, End)
VALUES (1, '2022-06-17 12:00:00.000', '2022-06-17 17:00:00.000');
INSERT INTO Shift (EmployeeId, Start, End)
VALUES (2, '2022-06-17 09:00:00.000', '2022-06-17 15:00:00.000');