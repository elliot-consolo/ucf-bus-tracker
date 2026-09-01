CREATE TABLE IF NOT EXISTS bus_snapshots (
    snapshot_id BIGSERIAL PRIMARY KEY,
    vehicle_id VARCHAR(50) NOT NULL,
    route_id VARCHAR(50) NOT NULL,
    latitude DOUBLE PRECISION NOT NULL,
    longitude DOUBLE PRECISION NOT NULL,
    speed DOUBLE PRECISION NOT NULL,
    recorded_at TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- 2. Create an index optimized for delay and route analysis
CREATE INDEX IF NOT EXISTS ix_bus_snapshots_route_time 
ON bus_snapshots (route_id, recorded_at);