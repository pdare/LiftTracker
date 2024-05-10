import os
import pymysql
import json
from config.definitions import ROOT_DIR

class DatabaseManager:
    def __init__(self):
        file_path = os.path.join(ROOT_DIR, "..\\passwords\\userpass.txt")
        pass_file = open(file_path, 'r')
        lines = pass_file.readlines()
        pass_file.close()
        self.username = lines[0].strip()
        self.password = lines[1].strip()

    def tuple_to_string(self, tuple):
        output = ""
        for item in tuple:
            output += str(item)
        return self.format_output_string(output)
    
    def format_output_string(self, output_str):
        result = output_str.strip('()')
        #print(result)
        return result

    def get_lift(self, user_id, date, set_num, lift_name):
        conn = pymysql.connect(
            host = 'localhost',
            user = self.username,
            password = self.password,
            db = 'LiftTracker',
        )
        name = 19752
        cur = conn.cursor()
        select_query = "SELECT * FROM workout_lifts WHERE user_id = {0} AND lift_date = '{1}' AND set_num = {2} AND lift_name = '{3}'".format(int(user_id), date, int(set_num), lift_name)
        cur.execute(select_query)
        output = cur.fetchall()
        conn.close()
        output_as_str = self.tuple_to_string(output)
        return str(output)

    def get_workout(self, user_id, date, workout_name):
        conn = pymysql.connect(
            host = 'localhost',
            user = self.username,
            password = self.password,
            db = 'LiftTracker',
        )
        name = 19752
        cur = conn.cursor()
        select_query = "SELECT * FROM workout_lifts WHERE user_id = {0} AND lift_date = '{1}' AND workout_name = '{2}'".format(int(user_id), date, workout_name)
        cur.execute(select_query)
        output = cur.fetchall()
        conn.close()
        output_as_str = self.tuple_to_string(output)
        return str(output)

    def save_workout(self, json_data, user_id):
        print("printing json input from API")
        json_dict = json_data
        date = json_dict['date']
        date_split = date.split('-')
        date_sql_format = date_split[2] + '-' + date_split[0] + '-' + date_split[1]
        workout_name = json_dict['workout']
        for key in json_dict:
            if "exercise" in key:
                print("saving {}".format(key))
                lift_name = json_dict[key]["name"]
                for num in range(json_dict[key]["number of sets"]):
                    conn = pymysql.connect(
                        host = 'localhost',
                        user = self.username,
                        password = self.password,
                        db = 'LiftTracker',
                    )
                    set_num = num + 1
                    set_str = "set {}".format(set_num)
                    reps_num = json_dict[key]["reps"][set_str]
                    weight_num = json_dict[key]["weight"][set_str]
                    insert_query = 'INSERT INTO workout_lifts VALUES ("{0}", "{1}", "{2}", {3}, {4}, {5}, {6})'.format(date_sql_format, workout_name, lift_name, int(reps_num), int(weight_num), int(set_num), int(user_id))
                    cur = conn.cursor()
                    cur.execute(insert_query)
                    conn.commit()
                    conn.close()
        print("workout saved")