CREATE TABLE IF NOT EXISTS logs (
  uuid SERIAL PRIMARY KEY NOT NULL,
  createdAt TIMESTAMP DEFAULT now(),
  data JSON NOT NULL
);

CREATE TABLE IF NOT EXISTS users (
  id SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  createdAt TIMESTAMP DEFAULT now()
);

CREATE TABLE IF NOT EXISTS locations (
  id SERIAL PRIMARY KEY REFERENCES users,
  longitude INTEGER NOT NULL,
  latitude INTEGER NOT NULL,
  altitude INTEGER NOT NULL,
  heading INTEGER NOT NULL,
  createdAt TIMESTAMP DEFAULT now(),
  updatedAt TIMESTAMP DEFAULT now()
);

CREATE TABLE IF NOT EXISTS biometrics (
  id SERIAL PRIMARY KEY REFERENCES users,
  o2 INTEGER NOT NULL,
  battery INTEGER NOT NULL,
  heartrate INTEGER NOT NULL,
  createdAt TIMESTAMP DEFAULT now(),
  updatedAt TIMESTAMP DEFAULT now()
);

CREATE TABLE IF NOT EXISTS uia (
  id SERIAL PRIMARY KEY,
  o2 BOOLEAN NOT NULL,
  power_ BOOLEAN NOT NULL,
  comm BOOLEAN NOT NULL,
  updatedAt TIMESTAMP DEFAULT now()
);

INSERT INTO uia (o2, power_, comm) VALUES (TRUE, TRUE, TRUE);
INSERT INTO logs (createdAt, data) VALUES (now(), '{"test log": "This is a test log"}'), (now(), '{"test log": "This is another test log"}');
