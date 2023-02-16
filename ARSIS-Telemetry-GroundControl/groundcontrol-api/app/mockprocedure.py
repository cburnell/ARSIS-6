class MockProcedure:
    def __init__(self):
        self.name = "Mock Procedure"
        self.task_list = [
            ["text", "asdfasdf asdfasdf asdf sadf asdf asdf".encode()],
            [
                "text",
                "lkashdflkasdfdffdfdfkasjfdaadsfsfasdfasdfasdfasoiueoriuwoeirtn,msnd,gmnsdf,gns,dnfgm,sdnf,msndjfksiuoqweuioqweuioxcbncxvbncxvbnxcvbnaghfasdfgsadwqyetwty32463278sdgusd".encode(),
            ],
            ["text", "asdf".encode()],
            [
                "text",
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa".encode(),
            ],
        ]

    def get_name(self):
        return self.name

    def get_task_list(self):
        return self.task_list
