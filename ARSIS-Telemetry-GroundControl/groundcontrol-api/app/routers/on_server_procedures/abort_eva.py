from .create_procedure import CreateProcedure

class TerminateEVA(CreateProcedure):
    def __init__(self):
        self.name = "Abort EVA."
        self.summary = "Procedures for when aborting EVA."

        perform_steps = {
            "name": "Perform steps to abort EVA.",
            "summary": "Steps to perform when aborting EVA.",
            "stepList": [
                {
                    "type": "text", 
                    "body": "Navigate to airlock, then proceed to next step.",
                    "nextTask": None
                },
                {
                    "type": "text", 
                    "body": "Ingress airlock, then proceed to next step.",
                    "nextTask": None
                },
                {
                    "type": "text", 
                    "body": "Close and lock the EVA hatch, then proceed to next step.",
                    "nextTask": None
                },
                {
                    "type": "text", 
                    "body": "Go to EMER REPRESS (decal).",
                    "nextTask": None
                }
            ]
        }

        self.task_list = [perform_steps]