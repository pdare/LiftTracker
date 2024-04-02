import os
import pymysql

class DatabaseManager:
    def __init__(self, username, password):
        self.username = username
        self.password = password

    def get_lift(self):
        conn = pymysql.connect(
            host = 'localhost',
            user = self.username,
            password = self.password,
            db = 'LiftTracker',
        )
        name = 19752
        cur = conn.cursor()
        cur.execute("SELECT user_name FROM Users WHERE user_id = {}".format(name))
        output = cur.fetchall()
        print(output)
        conn.close()
        return str(output[0])

    def send_workout():
        pass