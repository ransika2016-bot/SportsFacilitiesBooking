CREATE TABLE Members (
    MemberID         INT IDENTITY(1,1) PRIMARY KEY,
    FirstName        VARCHAR(50) NOT NULL,
    LastName         VARCHAR(50) NOT NULL,
    Email            VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash     VARCHAR(255) NOT NULL,
    Phone            VARCHAR(20),
    Address          VARCHAR(255),
    RegistrationDate DATETIME DEFAULT GETDATE(),
    AccountStatus    VARCHAR(20) DEFAULT 'Active'
                     CONSTRAINT chk_member_status CHECK (AccountStatus IN ('Active', 'Inactive', 'Suspended'))
);
 

CREATE TABLE Sports (
    SportID      INT IDENTITY(1,1) PRIMARY KEY,
    SportName    VARCHAR(50) NOT NULL UNIQUE,
    Description  VARCHAR(255)
);
 

CREATE TABLE MemberSportPreferences (
    MemberID       INT NOT NULL,
    SportID        INT NOT NULL,
    PreferenceDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT pk_member_sport PRIMARY KEY (MemberID, SportID),
    CONSTRAINT fk_msp_member FOREIGN KEY (MemberID) REFERENCES Members(MemberID) ON DELETE CASCADE,
    CONSTRAINT fk_msp_sport FOREIGN KEY (SportID) REFERENCES Sports(SportID) ON DELETE CASCADE
);
 

CREATE TABLE FacilityTypes (
    FacilityTypeID INT IDENTITY(1,1) PRIMARY KEY,
    TypeName       VARCHAR(50) NOT NULL UNIQUE,
    Description    VARCHAR(255)
);
 

CREATE TABLE Facilities (
    FacilityID     INT IDENTITY(1,1) PRIMARY KEY,
    FacilityName   VARCHAR(100) NOT NULL,
    FacilityTypeID INT NOT NULL,
    Location       VARCHAR(255) NOT NULL,
    Capacity       INT,
    Description    VARCHAR(500),
    Status         VARCHAR(20) DEFAULT 'Active'
                   CONSTRAINT chk_facility_status CHECK (Status IN ('Active', 'Maintenance', 'Closed')),
    CONSTRAINT fk_facility_type FOREIGN KEY (FacilityTypeID) REFERENCES FacilityTypes(FacilityTypeID)
);
 

CREATE TABLE Bookings (
    BookingID    INT IDENTITY(1,1) PRIMARY KEY,
    MemberID     INT NOT NULL,
    FacilityID   INT NOT NULL,
    BookingDate  DATE NOT NULL,
    StartTime    DATETIME NOT NULL,
    EndTime      DATETIME NOT NULL,
    Status       VARCHAR(20) DEFAULT 'Confirmed'
                 CONSTRAINT chk_booking_status CHECK (Status IN ('Confirmed', 'Cancelled', 'Completed', 'Pending')),
    CreatedDate  DATETIME DEFAULT GETDATE(),
    Notes        VARCHAR(500),
    CONSTRAINT fk_booking_member FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
    CONSTRAINT fk_booking_facility FOREIGN KEY (FacilityID) REFERENCES Facilities(FacilityID),
    CONSTRAINT chk_booking_time CHECK (EndTime > StartTime)
);
 

CREATE TABLE Reviews (
    ReviewID     INT IDENTITY(1,1) PRIMARY KEY,
    BookingID    INT NOT NULL UNIQUE,
    MemberID     INT NOT NULL,
    FacilityID   INT NOT NULL,
    Rating       INT NOT NULL
                 CONSTRAINT chk_rating CHECK (Rating BETWEEN 1 AND 5),
    CommentText  VARCHAR(1000),
    ReviewDate   DATETIME DEFAULT GETDATE(),
    IsAnonymous  CHAR(1) DEFAULT 'N'
                 CONSTRAINT chk_anonymous CHECK (IsAnonymous IN ('Y', 'N')),
    CONSTRAINT fk_review_booking FOREIGN KEY (BookingID) REFERENCES Bookings(BookingID),
    CONSTRAINT fk_review_member FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
    CONSTRAINT fk_review_facility FOREIGN KEY (FacilityID) REFERENCES Facilities(FacilityID)
);
 

CREATE TABLE Inquiries (
    InquiryID      INT IDENTITY(1,1) PRIMARY KEY,
    GuestName      VARCHAR(100) NOT NULL,
    GuestEmail     VARCHAR(100) NOT NULL,
    Subject        VARCHAR(200) NOT NULL,
    MessageText    VARCHAR(2000) NOT NULL,
    Category       VARCHAR(50)
                   CONSTRAINT chk_inquiry_cat CHECK (Category IN ('General', 'Membership', 'Facility', 'Technical')),
    Status         VARCHAR(20) DEFAULT 'Received'
                   CONSTRAINT chk_inquiry_status CHECK (Status IN ('Received', 'InProgress', 'Resolved')),
    SubmissionDate DATETIME DEFAULT GETDATE()
);


INSERT INTO Sports (SportName, Description) VALUES ('Tennis', 'Racket sport played on rectangular court');
INSERT INTO Sports (SportName, Description) VALUES ('Soccer', 'Team sport played with spherical ball');
INSERT INTO Sports (SportName, Description) VALUES ('Basketball', 'Team sport played on rectangular court with hoops');
INSERT INTO Sports (SportName, Description) VALUES ('Badminton', 'Racket sport with shuttlecock');
INSERT INTO Sports (SportName, Description) VALUES ('Swimming', 'Aquatic sport in pool');
INSERT INTO Sports (SportName, Description) VALUES ('Athletics', 'Track and field events');

INSERT INTO FacilityTypes (TypeName, Description) VALUES ('Tennis Court', 'Outdoor/indoor tennis courts with hard or clay surface');
INSERT INTO FacilityTypes (TypeName, Description) VALUES ('Soccer Field', 'Full-size and mini soccer fields with grass/astro turf');
INSERT INTO FacilityTypes (TypeName, Description) VALUES ('Basketball Court', 'Indoor and outdoor basketball courts');
INSERT INTO FacilityTypes (TypeName, Description) VALUES ('Swimming Pool', 'Olympic-size and learner swimming pools');
INSERT INTO FacilityTypes (TypeName, Description) VALUES ('Multi-Purpose Hall', 'Indoor hall for badminton, volleyball, and events');

INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status)
VALUES ('Central Tennis Court A', 1, 'Municipal Sports Complex, Block A', 4, 'Professional hard court with floodlights', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status)
VALUES ('Central Tennis Court B', 1, 'Municipal Sports Complex, Block A', 4, 'Clay court for practice sessions', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status)
VALUES ('Main Soccer Field', 2, 'Municipal Sports Complex, Main Ground', 22, 'Full-size grass field with spectator stands', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status)
VALUES ('Mini Soccer Pitch', 2, 'Municipal Sports Complex, East Wing', 10, '5-a-side astro turf pitch', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status)
VALUES ('Indoor Basketball Court', 3, 'Municipal Sports Complex, Block B', 10, 'Air-conditioned indoor court with scoreboard', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status)
VALUES ('Outdoor Basketball Court', 3, 'Municipal Sports Complex, West Wing', 10, 'Street basketball style court', 'Maintenance');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status)
VALUES ('Olympic Swimming Pool', 4, 'Municipal Sports Complex, Aquatic Center', 50, '50-meter Olympic pool with diving boards', 'Active');
INSERT INTO Facilities (FacilityName, FacilityTypeID, Location, Capacity, Description, Status)
VALUES ('Multi-Purpose Hall A', 5, 'Municipal Sports Complex, Block C', 30, 'Indoor hall for badminton and volleyball', 'Active');
 

INSERT INTO Members (FirstName, LastName, Email, PasswordHash, Phone, Address, RegistrationDate, AccountStatus)
VALUES ('Kasun', 'Perera', 'kasun.perera@email.com', 'HASH_kasun123', '0771234567', '15 Galle Road, Colombo 03', '2024-01-15', 'Active');
INSERT INTO Members (FirstName, LastName, Email, PasswordHash, Phone, Address, RegistrationDate, AccountStatus)
VALUES ('Nimal', 'Fernando', 'nimal.f@email.com', 'HASH_nimal456', '0772345678', '22 Kandy Road, Kandy', '2024-02-10', 'Active');
INSERT INTO Members (FirstName, LastName, Email, PasswordHash, Phone, Address, RegistrationDate, AccountStatus)
VALUES ('Sunil', 'Silva', 'sunil.silva@email.com', 'HASH_sunil789', '0773456789', '8 Negombo Road, Wattala', '2024-03-05', 'Active');
INSERT INTO Members (FirstName, LastName, Email, PasswordHash, Phone, Address, RegistrationDate, AccountStatus)
VALUES ('Amara', 'Jayawardena', 'amara.j@email.com', 'HASH_amara321', '0774567890', '45 Matara Road, Galle', '2024-03-20', 'Active');
INSERT INTO Members (FirstName, LastName, Email, PasswordHash, Phone, Address, RegistrationDate, AccountStatus)
VALUES ('Dilani', 'Weerasinghe', 'dilani.w@email.com', 'HASH_dilani654', '0775678901', '12 Nugegoda, Colombo 05', '2024-04-12', 'Active');
 

INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (1, 1, '2024-01-15');
INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (1, 3, '2024-01-15');
INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (2, 2, '2024-02-10');
INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (2, 6, '2024-02-10');
INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (3, 1, '2024-03-05');
INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (3, 4, '2024-03-05');
INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (4, 3, '2024-03-20');
INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (4, 5, '2024-03-20');
INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (5, 2, '2024-04-12');
INSERT INTO MemberSportPreferences (MemberID, SportID, PreferenceDate) VALUES (5, 5, '2024-04-12');
 


INSERT INTO Bookings (MemberID, FacilityID, BookingDate, StartTime, EndTime, Status, CreatedDate, Notes)
VALUES (1, 1, '2024-06-15', '2024-06-15 08:00:00', '2024-06-15 10:00:00', 'Completed', '2024-06-10 14:30:00', 'Morning practice session');
INSERT INTO Bookings (MemberID, FacilityID, BookingDate, StartTime, EndTime, Status, CreatedDate, Notes)
VALUES (2, 3, '2024-06-16', '2024-06-16 16:00:00', '2024-06-16 18:00:00', 'Completed', '2024-06-12 09:00:00', 'Team training');
INSERT INTO Bookings (MemberID, FacilityID, BookingDate, StartTime, EndTime, Status, CreatedDate, Notes)
VALUES (3, 2, '2024-06-17', '2024-06-17 07:00:00', '2024-06-17 09:00:00', 'Confirmed', '2024-06-14 11:00:00', 'Weekend practice');
INSERT INTO Bookings (MemberID, FacilityID, BookingDate, StartTime, EndTime, Status, CreatedDate, Notes)
VALUES (4, 5, '2024-06-18', '2024-06-18 18:00:00', '2024-06-18 20:00:00', 'Confirmed', '2024-06-15 16:00:00', 'Evening match with friends');
INSERT INTO Bookings (MemberID, FacilityID, BookingDate, StartTime, EndTime, Status, CreatedDate, Notes)
VALUES (5, 7, '2024-06-19', '2024-06-19 06:00:00', '2024-06-19 08:00:00', 'Completed', '2024-06-16 20:00:00', 'Morning swim');
INSERT INTO Bookings (MemberID, FacilityID, BookingDate, StartTime, EndTime, Status, CreatedDate, Notes)
VALUES (1, 4, '2024-06-20', '2024-06-20 17:00:00', '2024-06-20 19:00:00', 'Pending', '2024-06-18 10:00:00', 'Mini soccer with colleagues');
INSERT INTO Bookings (MemberID, FacilityID, BookingDate, StartTime, EndTime, Status, CreatedDate, Notes)
VALUES (2, 8, '2024-06-21', '2024-06-21 19:00:00', '2024-06-21 21:00:00', 'Confirmed', '2024-06-17 13:00:00', 'Badminton tournament practice');
INSERT INTO Bookings (MemberID, FacilityID, BookingDate, StartTime, EndTime, Status, CreatedDate, Notes)
VALUES (3, 1, '2024-06-22', '2024-06-22 14:00:00', '2024-06-22 16:00:00', 'Cancelled', '2024-06-19 08:00:00', 'Cancelled due to rain');
 

INSERT INTO Reviews (BookingID, MemberID, FacilityID, Rating, CommentText, ReviewDate, IsAnonymous)
VALUES (1, 1, 1, 5, 'Excellent court condition and floodlights worked perfectly for evening play.', '2024-06-16', 'N');
INSERT INTO Reviews (BookingID, MemberID, FacilityID, Rating, CommentText, ReviewDate, IsAnonymous)
VALUES (2, 2, 3, 4, 'Great field but the changing rooms need better maintenance.', '2024-06-17', 'N');
INSERT INTO Reviews (BookingID, MemberID, FacilityID, Rating, CommentText, ReviewDate, IsAnonymous)
VALUES (5, 5, 7, 5, 'Pool is very clean and well-maintained. Lifeguard was attentive.', '2024-06-20', 'Y');
 

INSERT INTO Inquiries (GuestName, GuestEmail, Subject, MessageText, Category, Status, SubmissionDate)
VALUES ('Rohan Bandara', 'rohan.b@email.com', 'Membership Fees Inquiry', 'Could you please provide information about annual membership fees and family packages?', 'Membership', 'Received', '2024-06-18 09:15:00');
INSERT INTO Inquiries (GuestName, GuestEmail, Subject, MessageText, Category, Status, SubmissionDate)
VALUES ('Samanthi Kumari', 'samanthi.k@email.com', 'Tennis Court Availability', 'Are tennis courts available for booking on Sunday mornings? What are the peak hours?', 'Facility', 'InProgress', '2024-06-17 14:20:00');
INSERT INTO Inquiries (GuestName, GuestEmail, Subject, MessageText, Category, Status, SubmissionDate)
VALUES ('Pradeep Rathnayake', 'pradeep.r@email.com', 'Website Login Issue', 'I registered yesterday but cannot log in. It says invalid password.', 'Technical', 'Received', '2024-06-19 11:45:00');
INSERT INTO Inquiries (GuestName, GuestEmail, Subject, MessageText, Category, Status, SubmissionDate)
VALUES ('Kavindi Perera', 'kavindi.p@email.com', 'Youth Sports Programs', 'Do you offer coaching programs for children aged 10-12 in basketball?', 'General', 'Resolved', '2024-06-15 16:30:00');
INSERT INTO Inquiries (GuestName, GuestEmail, Subject, MessageText, Category, Status, SubmissionDate)
VALUES ('Mahesh Senanayake', 'mahesh.s@email.com', 'Group Booking for Soccer', 'We are a company of 20 employees. Can we book the mini soccer pitch for a team building event?', 'Facility', 'Received', '2024-06-20 10:00:00');
GO

Q1: List all members with their preferred sports
SELECT
    m.MemberID,
    m.FirstName + ' ' + m.LastName AS FullName,
    m.Email,
    m.Phone,
    s.SportName AS PreferredSport
FROM Members m
JOIN MemberSportPreferences msp ON m.MemberID = msp.MemberID
JOIN Sports s ON msp.SportID = s.SportID
ORDER BY m.LastName, s.SportName;