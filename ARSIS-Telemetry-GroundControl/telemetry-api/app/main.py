from fastapi import FastAPI

app = FastAPI()

app.mock_heading = 0.0


@app.get("/")
async def root():
    return {"message": "Telemetry API"}


@app.get("/heading/")
async def getHeading():
    app.mock_heading += 1
    app.mock_heading %= 360
    return {"heading": app.mock_heading}


# TODO: Move this into a locations module
@app.get("/location/")
async def location():
    return {"lat": 10, "lon": 100}


# TODO: Move this into a biometrics module
@app.get("/biometrics/")
async def biometrics():
    return {"hr": 75, "o2": 0.9}
