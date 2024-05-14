-- Structure
CREATE TABLE Pallet (
  id INT PRIMARY KEY,
  name VARCHAR(50) NOT NULL
);

CREATE TABLE Box (
  id INT PRIMARY KEY,  
  barcode VARCHAR(50) NOT NULL,   
  is_opened BIT DEFAULT 0,
  pallet_id INT NULL,
  parent_box_id INT NULL,
  FOREIGN KEY (parent_box_id) REFERENCES Box(id),
  FOREIGN KEY (pallet_id) REFERENCES Pallet(id)
);

-- Add some dummy data
INSERT INTO Pallet (id, name) VALUES (1, 'Pallet1');
GO

INSERT INTO Box (id, barcode, is_opened, pallet_id, parent_box_id) VALUES (1, 'BC1', 0, 1, NULL);
GO

INSERT INTO Box (id, barcode, is_opened, pallet_id, parent_box_id) VALUES (4, 'BC4', 0, 1, NULL); 
GO

INSERT INTO Box (id, barcode, is_opened, pallet_id, parent_box_id) VALUES(2, 'BC2', 0, NULL, 1);
GO

INSERT INTO Box (id, barcode, is_opened, pallet_id, parent_box_id) VALUES(3, 'BC3', 0, NULL, 1); 
GO

INSERT INTO Box (id, barcode, is_opened, pallet_id, parent_box_id) VALUES(5, 'BC5', 0, NULL, 4); 
GO

INSERT INTO Box (id, barcode, is_opened, pallet_id, parent_box_id) VALUES(6, 'BC6', 0, NULL, 4);  
GO

INSERT INTO Box (id, barcode, is_opened, pallet_id, parent_box_id) VALUES (7, 'BC7', 0, NULL, 6);  
GO




-- This is the autoincrement PK version of structure:
-- We can insert now without pass id due to it is IDENTITY(1,1)
CREATE TABLE Pallet (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50) NOT NULL
);

CREATE TABLE Box (
  id INT IDENTITY(1,1) PRIMARY KEY,
  barcode VARCHAR(50) NOT NULL,
  is_opened BIT DEFAULT 0,
  pallet_id INT NULL,
  parent_box_id INT NULL,
  FOREIGN KEY (parent_box_id) REFERENCES Box(id),
  FOREIGN KEY (pallet_id) REFERENCES Pallet(id)
);



-- This is cascade deletion. If we need to use this version then in this case we need to not use recursive delation.
-- Apart of that, we need to test with use EF.Core and Fluent API and delete cascade behaiviair
CREATE TABLE Pallet (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50) NOT NULL
);

CREATE TABLE Box (
  id INT IDENTITY(1,1) PRIMARY KEY,
  barcode VARCHAR(50) NOT NULL,
  is_opened BIT DEFAULT 0,
  pallet_id INT NULL,
  parent_box_id INT NULL,
  FOREIGN KEY (parent_box_id) REFERENCES Box(id) ON DELETE CASCADE,
  FOREIGN KEY (pallet_id) REFERENCES Pallet(id)
);


