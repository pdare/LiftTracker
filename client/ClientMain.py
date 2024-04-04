import socket
import json
import os

def client_program():
    get_input = input("enter command: ")
    if get_input == "get lifts":
        get_lift(19752, "2024-04-03", 1)
    elif get_input == "send lifts":
        send_lifts()
    else:
        print("please enter a valid command")

def tuple_to_string(tuple):
    output = ""
    for item in tuple:
        output += item
    return output

def format_output_string(output_str):
        result = output_str.strip('()')
        return result

def send_lifts():
    filepath = os.path.join(os.getcwd(), "Workout 06 01 03-28-2024")
    f = open(filepath)
    json_data = json.load(f)
    json_bytes = json.dumps(json_data).encode('utf-8')

    host = "127.0.0.1"
    port = 5000

    client_socket = socket.socket()
    client_socket.connect((host, port))

    message = json_bytes
    f.close()

    while message != "bye":
        client_socket.send(message)
        data = client_socket.recv(1024).decode()

        #print("received from server : " + data)

        message = "bye"

    client_socket.close()

def get_lift(user_id, date, set_num):
    host = "127.0.0.1"
    port = 5000

    client_socket = socket.socket()
    client_socket.connect((host, port))

    message = "get lifts" + ":" + str(user_id) + ":" + date + ":" + str(set_num)
    data_from_server = ""

    while message.lower().strip() != "bye":
        client_socket.send(message.encode())
        data = client_socket.recv(1024).decode()
        data_from_server = tuple_to_string(data)

        #print("received from server : " + data)
        message = "bye"

    client_socket.close()
    lift_as_dict = format_output_string(data_from_server)
    print(lift_as_dict)

if __name__ == "__main__":
    client_program()