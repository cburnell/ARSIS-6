from fastapi import FastAPI
from pydantic import BaseModel
from .mockprocedure import MockProcedure

app = FastAPI()
mock_procedure = MockProcedure()

procedures = {mock_procedure.get_name(): mock_procedure.get_task_list()}


@app.get("/")
async def root():
    return {"message": "Ground Control API"}


@app.get("/procedure/{name}")
def procedure(name: str):
    res = procedures.get(name, None)
    if res is not None:
        return {
            "name": name,
            "tasks": res,
        }
    return {"name": "Not found", "tasks": []}


class LoggingRequest(BaseModel):
    data: str


@app.put("/logger/", status_code=200)
async def gs_logger(data: LoggingRequest):
    print(data)
    return data
