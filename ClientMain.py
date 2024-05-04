import socket
import json
import os
import re
from enum import Enum

class FormatType(Enum):
    GETLIFT = 1

def client_program():
    get_input = input("enter command: ")
    if get_input == "get lifts":
        get_lift(19752, "2024-04-03", 1)
    elif get_input == "send lifts":
        send_lifts(45715)
    else:
        print("please enter a valid command")

def tuple_to_string(tuple):
    output = ""
    for item in tuple:
        output += item
    return output

def format_output_string(format_type, output_str):
    result = {}
    match format_type:
        case FormatType.GETLIFT:
            date_regex = "\\(.*\\)"
            no_date_regex = "\\'.*"
            stripped_output_str = output_str.strip('()')
            stripped_output_str = stripped_output_str[:-2]
            date = re.findall(date_regex, stripped_output_str)[0]
            date = date.replace('(', "")
            date = date.replace(')', "")
            date = date.replace(',', "")
            date = date.replace(" ", "-")
            result.update({"lift_date": date})
            removed_date = re.findall(no_date_regex, stripped_output_str)
            removed_date_str = removed_date[0].replace("'", "")
            split_params = removed_date_str.split(',')
            result.update({"workout_name": split_params[0].strip()})
            result.update({"lift_name": split_params[1].strip()})
            result.update({"reps_num": int(split_params[2].strip())})
            result.update({"weight": int(split_params[3].strip())})
            result.update({"set_num": int(split_params[4].strip())})
            result.update({"user_id": int(split_params[5].strip())})
    return result

def send_lifts(user_id, c_json_data):
    #filepath = os.path.join(os.getcwd(), "Workout 06 01 03-28-2024")
    #filepath = os.path.abspath(os.path.join(os.path.dirname( __file__ ), '..', 'JSON'))
    #f = open(filepath)
    #json_data = json.load(f)
    #print(type(json_data))
    #json_bytes = json.dumps(json_data).encode('utf-8')
    json_bytes = json.dumps(c_json_data).encode('utf-8')

    host = "127.0.0.1"
    port = 5000

    client_socket = socket.socket()
    client_socket.connect((host, port))

    message = "SentLifts" + "||" + str(user_id) + "||" + str(json_bytes)

    while message != "bye":
        client_socket.send(message.encode())
        data = client_socket.recv(1024).decode()

        #print("received from server : " + data)

        message = "bye"

    client_socket.close()

def get_lift(user_id, date, set_num):
    host = "127.0.0.1"
    port = 5000

    client_socket = socket.socket()
    client_socket.connect((host, port))

    message = "GetLifts" + "||" + str(user_id) + "||" + date + "||" + str(set_num)
    data_from_server = ""

    while message.lower().strip() != "bye":
        client_socket.send(message.encode())
        data = client_socket.recv(1024).decode()
        data_from_server = tuple_to_string(data)

        #print("received from server : " + data)
        message = "bye"

    client_socket.close()
    lift_as_dict = format_output_string(FormatType.GETLIFT, data_from_server)
    print(lift_as_dict)

if __name__ == "__main__":
    client_program()