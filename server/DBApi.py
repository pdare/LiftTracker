import os
import pymysql

class DatabaseManager:
    def __init__(self, username, password):
        self.username = username
        self.password = password

    def tuple_to_string(self, tuple):
        output = ""
        for item in tuple:
            output += str(item)
        return self.format_output_string(output)
    
    def format_output_string(self, output_str):
        result = output_str.strip('()')
        print(result)
        return result

    def get_lift(self, user_id, date, set_num):
        conn = pymysql.connect(
            host = 'localhost',
            user = self.username,
            password = self.password,
            db = 'LiftTracker',
        )
        name = 19752
        cur = conn.cursor()
        select_query = "SELECT * FROM workout_lifts WHERE user_id = {0} AND lift_date = '{1}' AND set_num = {2}".format(int(user_id), date, int(set_num))
        cur.execute(select_query)
        output = cur.fetchall()
        conn.close()
        output_as_str = self.tuple_to_string(output)
        return str(output)

    def send_workout():
        pass