from .create_procedure import CreateProcedure

class TurnOffLRV(CreateProcedure):
    def __init__(self):
        self.name = "Turn off lunar roving vehicle."
        self.summary = "Procedures to turn off the lunar roving vehicle."

        task_1 = {
            "name": "Power switch.",
            "summary": "Steps to use the power switch to turn off the lunar roving vehicle.",
            "stepList": [
                {
                    "type": "text", 
                    "body": "Switch the power switch on the electrical compartment to OFF.",
                    "nextTask": None
                }
            ]
        }

        self.task_list = [task_1]