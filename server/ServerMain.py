import threading
import socket
import json
from DBApi import DatabaseManager
import os
from config.definitions import ROOT_DIR
import re

server_run = True
server_listening = False

def end_server():
    global server_run
    while server_run:
        message = input("enter 'close' to stop server")
        if message == "close":
            host = "127.0.0.1"
            port = 5000

            client_socket = socket.socket()
            client_socket.connect((host, port))
            client_socket.close()
            server_run = False


def server_program():
    print("waiting for connection")
    host = "127.0.0.1"
    port = 5000

    server_socket = socket.socket()
    server_socket.bind((host, port))

    server_socket.listen(2)
    conn, address = server_socket.accept()
    print("connection from: " + str(address))

    

    while True:
        data = conn.recv(1024).decode()
        parsed_data = data.split('||')
        if not data:
            break
        match parsed_data[0]:
            case "get_lift":
                data = send_lifts(parsed_data[1], parsed_data[2], parsed_data[3], parsed_data[4])
            case "save_workout":
                json_str = str(parsed_data[2]).replace('\\', '').replace("b'", '')
                json_str = json_str[1:-2]
                json_dict = json.loads(json_str)
                receive_lifts(parsed_data[1], json_dict)
            case "check_connection":
                data = "valid connection"
            case "GetWorkout":
                get_workout(parsed_data[1], parsed_data[2])
            case _:
                print('something failed')
        conn.send(data.encode())
    conn.close()

def send_lifts(user_id, date, set_num, lift_name):
    file_path = os.path.join(ROOT_DIR, "..\\passwords\\userpass.txt")
    pass_file = open(file_path, 'r')
    lines = pass_file.readlines()
    pass_file.close()
    db_accessor = DatabaseManager(lines[0].strip(), lines[1].strip())
    return db_accessor.get_lift(user_id, date, set_num, lift_name)

def receive_lifts(user_id, json_data):
    file_path = os.path.join(ROOT_DIR, "..\\passwords\\userpass.txt")
    pass_file = open(file_path, 'r')
    lines = pass_file.readlines()
    pass_file.close()
    db_accessor = DatabaseManager(lines[0].strip(), lines[1].strip())
    db_accessor.save_workout(json_data, user_id)

def get_workout(user_id, date, workout_name):
    file_path = os.path.join(ROOT_DIR, "..\\passwords\\userpass.txt")
    pass_file = open(file_path, 'r')
    lines = pass_file.readlines()
    pass_file.close()
    db_accessor = DatabaseManager(lines[0].strip(), lines[1].strip())
    db_accessor.save_workout(json_data, user_id)

if __name__ == "__main__":
    t1 = threading.Thread(target=end_server, args=())
    t1.start()

    while server_run:
        t2 = threading.Thread(target=server_program, args=())
        t2.start()
        t2.join()

    t1.join()

    print("done")