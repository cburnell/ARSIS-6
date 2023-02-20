import base64


class MockProcedure:
    def __init__(self):
        self.name = "Mock Procedure"
        suits_image = base64.b64encode(open("app/NASASUITS-logo.png", "rb").read())
        self.task_list = [
            ["text", "asdfasdf asdfasdf asdf sadf asdf asdf"],
            [
                "text",
                "lkashdflkasdfdffdfdfkasjfdaadsfsfasdfasdfasdfasoiueoriuwoeirtn,msnd,gmnsdf,gns,dnfgm,sdnf,msndjfksiuoqweuioqweuioxcbncxvbncxvbnxcvbnaghfasdfgsadwqyetwty32463278sdgusd",
            ],
            ["text", "asdf"],
            [
                "text",
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            ],
            ["image", suits_image],
        ]

    def get_name(self):
        return self.name

    def get_task_list(self):
        return self.task_list

    def get_task_list_encoded(self):
        to_ret = [
            str.encode(s) if type(s) is str else s
            for tup in self.task_list
            for s in tup
        ]
        return to_ret
