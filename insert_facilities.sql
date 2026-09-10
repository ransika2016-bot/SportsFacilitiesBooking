INSERT INTO FacilityTypes (TypeName, Description) VALUES ('Basketball Court', 'Basketball courts');
INSERT INTO FacilityTypes (TypeName, Description) VALUES ('Swimming Pool', 'Swimming pools');
INSERT INTO FacilityTypes (TypeName, Description) VALUES ('Multi-Purpose Hall', 'Multi-Purpose halls');

DECLARE @BasketballID INT = (SELECT FacilityTypeID FROM FacilityTypes WHERE TypeName = 'Basketball Court');
DECLARE @SwimmingID INT = (SELECT FacilityTypeID FROM FacilityTypes WHERE TypeName = 'Swimming Pool');
DECLARE @MultiID INT = (SELECT FacilityTypeID FROM FacilityTypes WHERE TypeName = 'Multi-Purpose Hall');
DECLARE @TennisID INT = (SELECT FacilityTypeID FROM FacilityTypes WHERE TypeName = 'Tennis Court');
DECLARE @SoccerID INT = (SELECT FacilityTypeID FROM FacilityTypes WHERE TypeName = 'Soccer Field');

INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status) VALUES ('Central Tennis Court B', @TennisID, 'Municipal Complex', 4, 'Hard court', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status) VALUES ('Mini Soccer Pitch', @SoccerID, 'Main Ground', 10, 'Small pitch', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status) VALUES ('Indoor Basketball Court', @BasketballID, 'Indoor Stadium', 10, 'Wood floor', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status) VALUES ('Outdoor Basketball Court', @BasketballID, 'Municipal Complex', 10, 'Concrete floor', 'Maintenance');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status) VALUES ('Olympic Swimming Pool', @SwimmingID, 'Aquatics Center', 50, '50m pool', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status) VALUES ('Multi-Purpose Hall A', @MultiID, 'Main Ground', 100, 'Indoor hall', 'Active');
