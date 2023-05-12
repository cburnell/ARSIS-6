from .create_procedure import CreateProcedure

class TurnOffLRV(CreateProcedure):
    def __init__(self):
        self.name = "CO2 error."
        self.summary = "Procedures for when EV CO2 levels are above normal."

        pull_biometrics = {
            "name": "Pull biometrics data.",
            "summary": "Retrieve the biometrics data.",
            "stepList": [
                {
                    "type": "text", 
                    "body": "Switch the power switch on the electrical compartment to OFF.",
                    "nextTask": {
                            "procedure": self.name, "task": 1
                        }
                }
            ]
        }

        check_co2 = {
            "name": "Check CO2 level.",
            "summary": "Inspect the CO2 level in your biometrics data.",
            "stepList": [
                {
                    "type": "text",
                    "body": "Navigate to the biometrics data within ARSIS 6.0 and check the CO2 values.",
                    "nextTask": {
                        "procedure": self.name, "task": 2
                    }
                }
            ]
        }

        perform_steps = {
            "name": "Examine CO2 values from biometrics.",
            "summary": "Steps to perform when CO2 values are not normal.",
            "stepList": [
                {
                    "type": "text",
                    "body": "If the CO2 level is below 500, go to the next task. Otherwise proceed to the next step.",
                    "nextTask": {
                        "procedure": self.name, "task": 3
                    }
                },
                {
                    "type": "text",
                    "body": "If the rise in the CO2 is abrupt, go to the next task. Otherwise proceed to the next step.",
                    "nextTask": {
                        "procedure": self.name, "task": 4
                    }
                },
                {
                    "type": "text",
                    "body": "Open helmet purge valve.",
                    "nextTask": None
                },
                {
                    "type": "text",
                    "body": "Monitor symptoms for CO2 poisoning:\nheadache, dizziness, weakness, upset stomach, vomiting, chest pain, and confusion.",
                    "nextTask": None
                },
                {
                    "type": "text",
                    "body": "If CO2 poisoning symptoms are not felt, go to the next task. Otherwise proceed to the next step.",
                    "nextTask": {
                        "procedure": "Terminate EVA.", "task": 0
                    }
                },
                {
                    "type": "text",
                    "body": "Minimize physical activity.",
                    "nextTask": None
                }
            ]
        }

        false_alarm = {
            "name": "CO2 levels indicate a false alarm.",
            "summary": "Steps to perform when CO2 values indicate a false alarm.",
            "stepList": [
                {
                    "type": "text",
                    "body": "The rise in CO2 levels is abrupt, the alarm was raised by a false alarm. Continue with EVA.",
                    "nextTask": None
                }
            ]
        }

        sensor_failure = {
            "name": "CO2 levels indicate a sensor failure.",
            "summary": "Steps to perform when CO2 values indicate a sensor failure.",
            "stepList": [
                {
                    "type": "text",
                    "body": "The CO2 levels are not below 500, the alarm was raised by a sensor failure. Continue with EVA.",
                    "nextTask": None
                }
            ]
        }

        self.task_list = [pull_biometrics, check_co2, perform_steps, false_alarm, sensor_failure]