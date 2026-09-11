INSERT INTO routes (route_id, route_name)
VALUES 
    ('68', 'Knights Express'),
    ('69', 'Pegasus Express'),
    ('3', 'DT Grocery Shuttle'),
    ('10', 'Grocery Shuttle'),
    ('27', 'Health Sciences Campus'),
    ('17', 'UCF Downtown'),
    ('32', 'Rosen College Shuttle'),
    ('66', 'Waterford Lake Shuttle'),
    ('70', 'Emergency Housing TownePlace'),
    
    ('4', 'Route 1'),
    ('18', 'Route 2'),
    ('56', 'Route 3'),
    ('57', 'Route 4'),
    ('21', 'Route 5'),
    ('22', 'Route 6'),
    ('23', 'Route 7'),
    ('24', 'Route 8'),
    ('58', 'Route 9'),
    ('5', 'Route 10')
ON CONFLICT (route_id) 
DO UPDATE SET 
    route_id = EXCLUDED.route_id, 
    route_name = EXCLUDED.route_name;

INSERT INTO stops (stop_id, route_id, stop_name, sequence_order, latitude, longitude, radius_meters)
VALUES 
    (9, '18', 'College Station Apt Stop 1', 1, 28.5793,-81.2062, 30),
    (10, '18', 'College Station Apt Stop 2', 2, 28.5794294880373, -81.2071933227225, 15),
    (11, '18', 'Boardwalk Apt Stop 1', 3, 28.5865, -81.2061, 30),
    (12, '18', 'Transit Center', 4, 28.600619182856253, -81.2050703902114, 70),

    (13, '56', 'The Aves @ Twelve100', 1, 28.593691, -81.209223, 25),
    (14, '56', 'The Verge', 2, 28.587635452, -81.2090180172, 30),
    (15, '56', 'Transit Center', 3, 28.600619182856253, -81.2050703902114, 70)
    
ON CONFLICT (stop_id) 
DO UPDATE SET
    stop_id = EXCLUDED.stop_id,
    route_id = EXCLUDED.route_id,
    stop_name = EXCLUDED.stop_name,
    sequence_order = EXCLUDED.sequence_order,
    latitude = EXCLUDED.latitude,
    longitude = EXCLUDED.longitude,
    radius_meters = EXCLUDED.radius_meters;