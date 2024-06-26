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
    elif get_input == "connection":
        print(check_connection())
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
            result.update({"workout_name": split_params[0]})
            result.update({"lift_name": split_params[1]})
            result.update({"reps_num": split_params[2]})
            result.update({"weight": split_params[3]})
            result.update({"set_num": split_params[4]})
            result.update({"user_id": split_params[5]})
    return result

def save_workout(user_id, c_json_data):
    json_bytes = json.dumps(c_json_data).encode('utf-8')

    host = "127.0.0.1"
    port = 5000

    client_socket = socket.socket()
    client_socket.settimeout(5)
    
    try:
        client_socket.connect((host, port))

        message = "save_workout" + "||" + str(user_id) + "||" + str(json_bytes)

        while message != "bye":
            client_socket.send(message.encode())
            data = client_socket.recv(1024).decode()

            message = "bye"

        client_socket.close()
    except socket.error as exc:
        return "failed to save workout" + "||" + str(exc)

def get_lift(user_id, date, set_num, lift_name):
    host = "127.0.0.1"
    port = 5000

    client_socket = socket.socket()
    client_socket.settimeout(5)
    try:
        client_socket.connect((host, port))

        message = "get_lift" + "||" + str(user_id) + "||" + date + "||" + str(set_num) + "||" + str(lift_name)
        data_from_server = ""

        while message.lower().strip() != "bye":
            client_socket.send(message.encode())
            data = client_socket.recv(1024).decode()
            data_from_server = tuple_to_string(data)
            message = "bye"

        client_socket.close()
        return data_from_server
    except socket.error as exc:
        return "failed to retrieve lift" + "||" + str(exc)

def get_workout(user_id, date, workout_name):
    host = "127.0.0.1"
    port = 5000

    client_socket = socket.socket()
    client_socket.settimeout(5)
    try:
        client_socket.connect((host, port))

        message = "get_workout" + "||" + str(user_id) + "||" + date + "||" + workout_name
        data_from_server = ""

        while message.lower().strip() != "bye":
            client_socket.send(message.encode())
            data = client_socket.recv(1024).decode()
            data_from_server = tuple_to_string(data)
            message = "bye"

        client_socket.close()
        return data_from_server
    except socket.error as exc:
        return "failed to retrieve workout" + "||" + str(exc)

def check_connection():
    host = "127.0.0.1"
    port = 5000

    client_socket = socket.socket()
    client_socket.settimeout(5)

    try:
        client_socket.connect((host, port))
        message = "check_connection"
        data = ''
        while message.lower().strip() != "bye":
            client_socket.send(message.encode())
            data = client_socket.recv(1024).decode()
            data_from_server = data
            message = "bye"

        client_socket.close()
        return data
    except socket.error as exc:
        return exc

if __name__ == "__main__":
    client_program()