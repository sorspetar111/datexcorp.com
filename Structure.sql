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
