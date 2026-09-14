INSERT INTO routes (route_id, route_name, list_order)
VALUES 
    ('68', 'Knights Express', 11),
    ('69', 'Pegasus Express', 12),
    ('3', 'DT Grocery Shuttle', 16),
    ('10', 'Grocery Shuttle', 13),
    ('27', 'Health Sciences Campus', 14),
    ('17', 'UCF Downtown', 15),
    ('32', 'Rosen College Shuttle', 17),
    ('66', 'Waterford Lake Shuttle', 18),
    ('70', 'Emergency Housing TownePlace', 19),
    
    ('4', 'Route 1', 1),
    ('18', 'Route 2', 2),
    ('56', 'Route 3', 3),
    ('57', 'Route 4', 4),
    ('21', 'Route 5', 5),
    ('22', 'Route 6', 6),
    ('23', 'Route 7', 7),
    ('24', 'Route 8', 8),
    ('58', 'Route 9', 9),
    ('5', 'Route 10', 10)
ON CONFLICT (route_id) 
DO UPDATE SET 
    route_id = EXCLUDED.route_id, 
    route_name = EXCLUDED.route_name,
    list_order = EXCLUDED.list_order;

INSERT INTO stops (stop_id, route_id, stop_name, sequence_order, latitude, longitude, radius_meters)
VALUES 
    -- Route 2
    (9, '18', 'College Station Apt Stop 1', 1, 28.5793,-81.2062, 30),
    (10, '18', 'College Station Apt Stop 2', 2, 28.5794294880373, -81.2071933227225, 15),
    (11, '18', 'Boardwalk Apt Stop 1', 3, 28.5865, -81.2061, 30),
    (12, '18', 'Transit Center', 4, 28.600619182856253, -81.2050703902114, 70),

    -- Route 3
    (13, '56', 'The Aves @ Twelve100', 1, 28.593691, -81.209223, 25),
    (14, '56', 'The Verge', 2, 28.587635452, -81.2090180172, 30),
    (15, '56', 'Transit Center', 3, 28.600619182856253, -81.2050703902114, 70),

    -- Route 4
    ( 16, '57', 'Mercury Stop', 1, 28.583706592, -81.2147785423, 30),
    ( 17, '57', 'Campus Crossing Apt', 2, 28.5827, -81.2097, 30),
    ( 18, '57', 'Transit Center', 3, 28.600619182856253, -81.2050703902114, 70),
    ( 19, '57', 'The Lark', 4, 28.584981, -81.208642, 30),
    ( 20, '57', 'Lark stop 2', 5, 28.5867, -81.208553, 20),
    ( 21, '57', 'ROOK', 6, 28.5679565829, -81.2097040511, 30),

    -- Route 5
    ( 22, '21', 'Village at Science Dr', 1, 28.5817, -81.2022, 30),
    ( 23, '21', 'The Nine at Central', 2, 28.5822, -81.202, 30),
    ( 24, '21', 'Knights Landing - Southern Pine St', 3, 28.5825, -81.2059, 30),
    ( 25, '21', 'Knights Landing - Ash St', 4, 28.5826, -81.2049, 20),
    ( 26, '21', 'VERVE', 5, 28.5838063513, -81.2069675841, 20),
    ( 27, '21', 'Partnership 1', 6, 28.5862, -81.1968, 30),
    ( 28, '21', 'Research Pavilion', 7, 28.586261288222605, -81.19621260619061, 20),
    ( 29, '21', 'Research 1', 8, 28.6022830312, -81.1967234282, 70),
    ( 30, '21', 'Orlando Tech Center (Florida Inst. of Govt)', 9, 28.589, -81.1943, 20),
    ( 31, '21', 'Orlando Tech Center (Comm & Market)', 10, 28.5884, -81.1957, 20),
    ( 32, '21', 'Division of Digital Learning', 11, 28.5871, -81.1979, 20),
    ( 33, '21', 'University Towers/HR', 12, 28.5876, -81.1991, 20),

    -- Route 6
    ( 34, '22', 'Northgate Apt Stop 1', 1, 28.6145, -81.1954, 30),
    ( 35, '22', 'Northgate Apt Stop 2', 2, 28.6136, -81.1961, 15),
    ( 36, '22', 'Northgate Apt Stop 3', 3, 28.612312798804307, -81.19626800808837, 10),
    ( 37, '22', 'Tivoli Apt Stop 1', 4, 28.614, -81.2029, 30),
    ( 38, '22', 'Tivoli Apt Stop 2', 5, 28.6133052885, -81.202964373, 30),
    ( 39, '22', 'Tivoli Apt Stop 3', 6, 28.6127, -81.2014, 20),
    ( 40, '22', 'NorthView Apt', 7, 28.6126180757, -81.1909524957, 30),
    ( 41, '22', 'Research 1', 8, 28.6022896466, -81.1967199411, 70),

    -- Route 7
    ( 42, '23', 'The Pointe at Central', 1, 28.579, -81.2088, 30),
    ( 43, '23', 'Transit Center', 2, 28.600619182856253, -81.2050703902114, 70),

    -- Route 8
    ( 44, '24', 'The Station', 1, 28.6183, -81.2054, 30),
    ( 45, '24', 'Riverwind of Alafaya Apt', 2, 28.624, -81.2063, 30),
    ( 46, '24', 'Research 1', 3, 28.6022830312, -81.1967234282, 70),

    -- Route 9
    ( 47, '58', 'Plaza on University 2', 1, 28.598857, -81.210095, 30),
    ( 48, '58', 'Plaza on University 1', 2, 28.599426, -81.208459, 25),
    ( 49, '58', 'Arden Villas', 3, 28.595956167, -81.2203272295, 30),
    ( 50, '58', 'The Accolade Collegiate Village West', 4, 28.5966, -81.2145, 30),
    ( 51, '58', 'The Accolade Collegiate Village East', 5, 28.5969718088, -81.2088763506, 30),
    ( 52, '58', 'Student Union', 6, 28.602918, -81.200956, 30),

    -- Route 10
    ( 53, '5', 'Orion on Orpington Apt', 1, 28.571283166315823, -81.20010580213817, 100),
    ( 54, '5', 'The Lofts Apts Stop 1', 2, 28.570372, -81.203714, 30),
    ( 55, '5', 'The Lofts Apts Stop 2', 3, 28.570447, -81.205402, 30),
    ( 56, '5', 'Transit Center - Bay H', 4, 28.600662, -81.205422, 70),

    -- Knights Express
    ( 57, '68', 'Celeste', 1, 28.59931948027913, -81.20687272233442, 30),
    ( 58, '68', 'Visitors and Parking Information Center', 2, 28.5967, -81.2032, 30),
    ( 59, '68', 'Ferrell Commons/Student Health Services', 3, 28.5978781547, -81.2004150922, 30),
    ( 60, '68', 'RWC', 4, 28.5962989869, -81.1996148538, 30),
    ( 61, '68', 'Libra', 5, 28.595378253, -81.1968310324, 30),
    ( 62, '68', 'Research 1', 6, 28.6022830312, -81.1967234282, 70),
    ( 63, '68', 'Arena WB', 7, 28.6064610889, -81.1982753967, 30),
    ( 64, '68', 'Greek Park', 8, 28.6049250162, -81.2057013981, 30),
    ( 65, '68', 'Lake Claire Apts', 9, 28.6034227387, -81.2039674722, 30),
    ( 66, '68', 'Student Union', 10, 28.602918, -81.200956, 30),

    -- Pegasus Express
    ( 67, '69', 'Lynx Transit Center', 1, 28.6006, -81.2051, 70),
    ( 68, '69', 'Student Union', 2, 28.602918, -81.200956, 30),
    ( 69, '69', 'Lake Claire Apts', 3, 28.6034227387, -81.2039674722, 30),
    ( 70, '69', 'Greek Park', 4, 28.6049250162, -81.2057013981, 30),
    ( 71, '69', 'Arena', 5, 28.6062555314, -81.1979755092, 30),
    ( 72, '69', 'Research 1', 6, 28.6022830312, -81.1967234282, 70),
    ( 73, '69', 'Libra', 7, 28.595378253, -81.1968310324, 30),
    ( 74, '69', 'Ferrell Commons/Student Health Services', 8, 28.5978781547, -81.2004150922, 30),
    ( 75, '69', 'Visitors and Parking Information Center', 9, 28.5967, -81.2032, 30),

    -- Grocery Shuttle
    ( 76, '10', 'Market Place/Student Resource Center', 1, 28.5962740176, -81.1988141005, 30),
    ( 77, '10', 'Academic Village 1/Nike Community', 2, 28.595323, -81.196819, 30),
    ( 78, '10', 'Knights Plaza', 3, 28.606509, -81.198125, 30),
    ( 79, '10', 'Greek Park (WB)', 4, 28.6049656062, -81.2059208045, 30),
    ( 80, '10', 'Lake Claire Apts', 5, 28.6034227387, -81.2039674722, 30),
    ( 81, '10', 'Publix Supermarket', 6, 28.613018, -81.206224, 30),

    -- Health Sciences Campus
    ( 82, '27', 'Lynx Transit Center', 1, 28.6006, -81.2051, 70),
    ( 83, '27', 'Research Annex', 2, 28.5889373394, -81.1920560632, 30),
    ( 84, '27', 'Health Sciences Laureate', 3, 28.367, -81.2802, 30),
    ( 85, '27', 'University of Florida', 4, 28.3655495051, -81.2902359666, 30),

    -- UCF Downtown
    ( 86, '17', 'Transit Center - Bay A', 1, 28.600655, -81.20548, 70),
    ( 87, '17', 'Downtown', 2, 28.547456, -81.386533, 50),

    -- DT Grocery Shuttle
    ( 88, '3', 'Downtown', 1, 28.547456, -81.386533, 30),
    ( 89, '3', 'DT Publix', 2, 28.542262, -81.372889, 30),

    -- Rosen College Shuttle
    ( 90, '32', 'Rosen College', 1, 28.429, -81.4418, 50),
    ( 91, '32', 'Student Union Rosen Shuttle', 2, 28.6032, -81.2017, 100),

    -- Waterford Lakes Shuttle
    ( 92, '66', 'Market Place/Student Resource Center', 1, 28.5962740176, -81.1988141005, 30),
    ( 93, '66', 'Libra', 2, 28.595378253, -81.1968310324, 30),
    ( 94, '66', 'Addition Arena', 3, 28.6064692129, -81.1981570741, 30),
    ( 95, '66', 'Greek Park (WB)', 4, 28.6049656062, -81.2059208045, 30),
    ( 96, '66', 'H5 Parking Lot', 5, 28.6034382358, -81.2039160995, 30),
    ( 97, '66', 'Waterford Lakes', 6, 28.555256808, -81.2005871349, 30),

    -- Emergency Housing TownePlace
    ( 98, '70', 'Lynx Transit Center', 1, 28.6006, -81.2051, 70),
    ( 99, '70', 'TownePlace Suites', 2, 28.6002347293, -81.2147988745, 30)
    
ON CONFLICT (stop_id) 
DO UPDATE SET
    stop_id = EXCLUDED.stop_id,
    route_id = EXCLUDED.route_id,
    stop_name = EXCLUDED.stop_name,
    sequence_order = EXCLUDED.sequence_order,
    latitude = EXCLUDED.latitude,
    longitude = EXCLUDED.longitude,
    radius_meters = EXCLUDED.radius_meters;