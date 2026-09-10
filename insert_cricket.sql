-- Insert Cricket Ground Facility Type if not exists
IF NOT EXISTS (SELECT * FROM FacilityTypes WHERE TypeName = 'Cricket Ground')
BEGIN
    INSERT INTO FacilityTypes (TypeName, Description) VALUES ('Cricket Ground', 'Professional and amateur cricket grounds');
END

DECLARE @CricketID INT = (SELECT FacilityTypeID FROM FacilityTypes WHERE TypeName = 'Cricket Ground');

-- Insert Sri Lankan Cricket Facilities
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status, ImageUrl) 
VALUES ('R. Premadasa International Cricket Stadium', @CricketID, 'Khettarama, Colombo 10', 35000, 'Largest stadium in Sri Lanka, perfect for major cricket matches under floodlights.', 'Active', 'https://images.unsplash.com/photo-1540747913346-19e32dc3e97e?q=80&w=600&auto=format&fit=crop');

INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status, ImageUrl) 
VALUES ('Galle International Stadium', @CricketID, 'Galle Fort, Galle', 18000, 'Scenic cricket ground bordered by the Indian Ocean and the historic Galle Fort.', 'Active', 'https://images.unsplash.com/photo-1531415074968-036ba1b575da?q=80&w=600&auto=format&fit=crop');

INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status, ImageUrl) 
VALUES ('Pallekele International Cricket Stadium', @CricketID, 'Pallekele, Kandy', 35000, 'Modern cricket facility located in the hill capital, Kandy.', 'Active', 'https://images.unsplash.com/photo-1624526267942-ab0f0b580898?q=80&w=600&auto=format&fit=crop');

-- Update images for the ones added previously so they don't just use fallback
UPDATE Facilities SET ImageUrl = 'https://images.unsplash.com/photo-1574629810360-7efbb1925536?q=80&w=600&auto=format&fit=crop' WHERE FacilityName = 'Olympic Swimming Pool';
UPDATE Facilities SET ImageUrl = 'https://images.unsplash.com/photo-1504450758481-7338eba7524a?q=80&w=600&auto=format&fit=crop' WHERE FacilityName LIKE '%Basketball Court%';
UPDATE Facilities SET ImageUrl = 'https://images.unsplash.com/photo-1517466787929-bc90951d0974?q=80&w=600&auto=format&fit=crop' WHERE FacilityName LIKE '%Soccer Pitch%';
