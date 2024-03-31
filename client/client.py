import socket
import json
import os

def client_program():
    get_input = input("enter command: ")
    if get_input == "get lifts":
        get_lifts()
    elif get_input == "send lifts":
        send_lifts()
    else:
        print("please enter a valid command")

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

        print("received from server : " + data)

        message = "bye"

    client_socket.close()

def get_lifts():
    host = "127.0.0.1"
    port = 5000

    client_socket = socket.socket()
    client_socket.connect((host, port))

    message = "get lifts"

    while message.lower().strip() != "bye":
        client_socket.send(message.encode())
        data = client_socket.recv(1024).decode()

        print("received from server : " + data)

        message = "bye"

    client_socket.close()

if __name__ == "__main__":
    client_program()