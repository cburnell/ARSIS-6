from .create_procedure import CreateProcedure

class TerminateEVA(CreateProcedure):
    def __init__(self):
        self.name = "Terminate EVA."
        self.summary = "Procedures for when terminating EVA."

        perform_steps = {
            "name": "Perform steps to terminate EVA.",
            "summary": "Steps to perform when terminating EVA.",
            "stepList": [
                {
                    "type": "text", 
                    "body": "Clean up worksite, then proceed to next step.",
                    "nextTask": None
                },
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
                    "body": "Connect to the SCU, then proceed to next step.",
                    "nextTask": None
                },
                {
                    "type": "text", 
                    "body": "Confirm the UIA POWER for your EV is ON (LED ON), then proceed to next step.",
                    "nextTask": None
                },
                {
                    "type": "text", 
                    "body": "Confirm the UIA EMU for your EV is OPEN, then proceed to next step.",
                    "nextTask": None
                },
                {
                    "type": "text", 
                    "body": "If terminating due to BATT AMPS HIGH proceed to the next task. Otherwise proceed to the next step.",
                    "nextTask": {
                        "procedure": self.name, "task": 1
                    }
                },
                {
                    "type": "text",
                    "body": "Switch DCU POWER to SCU, then proceed to next step.",
                    "nextTask": None
                },
                {
                    "type": "text",
                    "body": "If SUIT PRESSURE GAUGE is below 3.3 proceed to the next task. Otherwise proceed to next step.",
                    "nextTask": {
                        "procedure": "Abort EVA.", "task": 0
                    }
                },
                {
                    "type": "text",
                    "body": "Perform step for when SUIT PRESSURE GAUGE is above 3.3.",
                    "nextTask": None
                }
            ]
        }

        batt_high = {
            "name": "Terminate due to BATT AMPS HIGH.",
            "summary": "Steps to perform when terminating due to BATT AMPS HIGH.",
            "stepList": [
                {
                    "type": "text",
                    "body": "Go to step 7a",
                    "nextTask": None
                }
            ]
        }

        self.task_list = [perform_steps, batt_high]